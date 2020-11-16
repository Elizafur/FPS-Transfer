using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Status { idle, walking, crouching, sprinting, sliding, climbingLadder, wallRunning, vaulting, grabbedLedge, climbingLedge, surfaceSwimming, underwaterSwimming, hook, slowWalk }
public class StatusEvent : UnityEvent<Status, Func<IKData>> { }
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    public Status status;
    public LayerMask collisionLayer; //Default
    public float crouchHeight = 1f;
    public PlayerInfo info;
    [SerializeField]
    private float sprintTime = 6f;
    [SerializeField]
    private float sprintReserve = 4f;
    [SerializeField]
    private float sprintMinimum = 2f;

    [SerializeField]
    private PlayerInventoryHandler inventory;

    new CameraMovement camera;
    PlayerMovement movement;
    PlayerInput playerInput;
    AnimateLean animateLean;
    AnimateCameraLevel animateCamLevel;

    bool canInteract;
    bool forceSprintReserve = false;
    
    float crouchCamAdjust;
    float stamina;

    public StatusEvent onStatusChange;
    List<MovementType> movements;
    WallrunMovement wallrun;
    SurfaceSwimmingMovement swimming;

    public void ChangeStatus(Status s)
    {
        if (status == s) return;
        status = s;
        if (onStatusChange != null)
            onStatusChange.Invoke(status, null);
    }
    public void ChangeStatus(Status s, Func<IKData> call)
    {
        if (status == s) return;
        status = s;
        if (onStatusChange != null)
            onStatusChange.Invoke(status, call);
    }

    public void AddToStatusChange(UnityAction<Status, Func<IKData>> action)
    {
        if(onStatusChange == null)
            onStatusChange = new StatusEvent();

        onStatusChange.AddListener(action);
    }

    public void AddMovementType(MovementType move)
    {
        if (movements == null) movements = new List<MovementType>();
        move.SetPlayerComponents(movement, playerInput);

        if ((move as WallrunMovement) != null) //If this move type is a Wallrunning
            wallrun = (move as WallrunMovement);
        else if ((move as SurfaceSwimmingMovement) != null) //If this move type is a Surface Swimming
            swimming = (move as SurfaceSwimmingMovement);

        movements.Add(move);
    }

    public SurfaceSwimmingMovement GetSwimmingMovement()
    {
        return swimming;
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        movement = GetComponent<PlayerMovement>();
        movement.AddToReset(() => { status = Status.walking; });

        camera = GetComponentInChildren<CameraMovement>();

        if (GetComponentInChildren<AnimateLean>())
            animateLean = GetComponentInChildren<AnimateLean>();
        if (GetComponentInChildren<AnimateCameraLevel>())
            animateCamLevel = GetComponentInChildren<AnimateCameraLevel>();

        info = new PlayerInfo(movement.controller.radius, movement.controller.height);
        crouchCamAdjust = (crouchHeight - info.height) / 2f;
        stamina = sprintTime;
    }

    public bool gamePaused = false;
    /******************************* UPDATE ******************************/
    void Update()
    {
        if (playerInput.inventory)  {
            Time.timeScale = (Time.timeScale == 1) ? 0 : 1;
            gamePaused = !gamePaused;
        }
        
        if (gamePaused) return;

        //Updates
        UpdateInteraction();
        CheckShouldHook();
        UpdateMovingStatus();

        //Checks
        CheckCrouching();
        foreach (MovementType moveType in movements)
        {
            if(moveType.enabled)
                moveType.Check(canInteract);
        }

        //Misc
        UpdateLean();
        UpdateCamLevel();
    }

    float maxDistance = 100f;
    Vector3 grapplePoint = new Vector3();
    void CheckShouldHook()  
    {
        if (status == Status.hook)  
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, grapplePoint-transform.position, out hit, 2f)) {
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere); //test visualization
                g.transform.position = grapplePoint;
                g.transform.localScale = new Vector3(1,1,1);
                g.SetActive(true);
                movement.Move(new Vector3(0, -1, 0), 2f, 9.8f);
                ChangeStatus(Status.walking);
            }
        }

        if (playerInput.hook)   
        {
            if (isCrouching())
                Uncrouch();
            
            RaycastHit hit;
            LayerMask l = LayerMask.GetMask("Player");

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance, ~l)) {
                grapplePoint = hit.point;
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                g.transform.position = grapplePoint;
                g.transform.localScale = new Vector3(1,1,1);
                g.SetActive(true);

                ChangeStatus(Status.hook);
                movement.grounded = false;

                Vector3 direction = hit.point - transform.position;
                float distance = Vector3.Distance(hit.point, transform.position);
                float speed = 2f;
                float forceTime = 2f;
                movement.ForceMove(direction, speed, forceTime, false);

            }
            
        }
    }

    void UpdateInteraction()
    {
        if ((int)status >= 5)
            canInteract = false;
        else if (!canInteract)
        {
            if (movement.grounded || movement.moveDirection.y < 0)
                canInteract = true;
        }
    }

    void UpdateMovingStatus()
    {
        if (status == Status.sprinting && stamina > 0)
            stamina -= Time.deltaTime;
        else if (stamina < sprintTime)
            stamina += Time.deltaTime;

        if ((int)status <= 1 || isSprinting())
        {
            if (playerInput.input.magnitude > 0.02f)
                ChangeStatus((shouldSprint()) ? Status.sprinting : Status.walking);
            else
                ChangeStatus(Status.idle);
        }
    }

    public bool shouldSprint()
    {
        bool sprint = false;
        sprint = (playerInput.run && playerInput.input.y > 0);
        if (status != Status.sliding)
        {
            if (!isSprinting()) //If we want to sprint
            {
                if (forceSprintReserve && stamina < sprintReserve)
                    return false;
                else if (!forceSprintReserve && stamina < sprintMinimum)
                    return false;
            }
            if (stamina <= 0)
            {
                forceSprintReserve = true;
                return false;
            }
        }
        if (sprint)
            forceSprintReserve = false;
        return sprint;
    }

    void UpdateLean()
    {
        if (animateLean == null) return;
        Vector2 lean = Vector2.zero;
        if (status == Status.wallRunning)
            lean.x = getWallrunDir();
        if (status == Status.sliding)
            lean.y = -1;
        else if (status == Status.climbingLedge || status == Status.vaulting)
            lean.y = 1;
        animateLean.SetLean(lean);
    }

    void UpdateCamLevel()
    {
        if (animateCamLevel == null) return;

        float level = 0f;
        if(status == Status.crouching || status == Status.sliding || status == Status.vaulting || status == Status.climbingLedge || status == Status.underwaterSwimming)
            level = crouchCamAdjust;
        animateCamLevel.UpdateLevel(level);
    }

    int getWallrunDir()
    {
        int wallDir = 0;
        if (wallrun != null)
            wallDir = wallrun.getWallDir();
        return wallDir;
    }
    /*********************************************************************/


    /******************************** MOVE *******************************/
    void FixedUpdate()
    {
        foreach (MovementType moveType in movements)
        {
            if (status == moveType.changeTo)
            {
                moveType.Movement();
                return;
            }
        }

        DefaultMovement();
    }

    void DefaultMovement()
    {
        if (isSprinting() && isCrouching())
            Uncrouch();

        movement.Move(playerInput.input, isSprinting(), isCrouching());
        if (movement.grounded && playerInput.Jump())
        {
            if (status == Status.crouching)
            {
                if (!Uncrouch()) 
                    return;
            }

            movement.Jump(Vector3.up, 1f);
            playerInput.ResetJump();
        }
    }

    public bool isSprinting()
    {
        return (status == Status.sprinting && movement.grounded);
    }

    public bool isWalking()
    {
        if (status == Status.walking || status == Status.crouching)
            return (movement.controller.velocity.magnitude > 0f && movement.grounded);
        else
            return false;
    }
    public bool isCrouching()
    {
        return (status == Status.crouching);
    }
   
    void CheckCrouching()
    {
        if (!movement.grounded || (int)status > 2) return;

        if (playerInput.run)
        {
            Uncrouch();
            return;
        }

        if (playerInput.crouch)
        {
            if (status != Status.crouching)
                Crouch(true);
            else
                Uncrouch();
        }
    }

    public void Crouch(bool setStatus)
    {
        movement.controller.height = crouchHeight;
        if(setStatus) ChangeStatus(Status.crouching);
    }

    public bool Uncrouch()
    {
        Vector3 bottom = transform.position - (Vector3.up * ((crouchHeight / 2) - info.radius));
        bool isBlocked = Physics.SphereCast(bottom, info.radius, Vector3.up, out var hit, info.height - info.radius, collisionLayer);
        if (isBlocked) return false; //If we have something above us, do nothing and return
        movement.controller.height = info.height;
        ChangeStatus(Status.walking);
        return true;
    }

    public bool hasObjectInfront(float dis, LayerMask layer)
    {
        Vector3 top = transform.position + (transform.forward * 0.25f);
        Vector3 bottom = top - (transform.up * info.halfheight);

        return (Physics.CapsuleCastAll(top, bottom, 0.25f, transform.forward, dis, layer).Length >= 1);
    }

    public bool hasWallToSide(int dir, LayerMask layer)
    {
        //Check for ladder in front of player
        Vector3 top = transform.position + (transform.right * 0.25f * dir);
        Vector3 bottom = top - (transform.up * info.radius);
        top += (transform.up * info.radius);

        return (Physics.CapsuleCastAll(top, bottom, 0.25f, transform.right * dir, 0.05f, layer).Length >= 1);
    }
}

public class PlayerInfo
{
    public float rayDistance;
    public float radius;
    public float height;
    public float halfradius;
    public float halfheight;

    public PlayerInfo(float r, float h)
    {
        radius = r; height = h;
        halfradius = r / 2f; halfheight = h / 2f;
        rayDistance =  halfheight + radius + .175f;
    }
}
