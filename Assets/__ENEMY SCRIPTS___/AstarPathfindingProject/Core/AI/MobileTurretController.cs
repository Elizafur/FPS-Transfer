using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileTurretController : MonoBehaviour
{
    [SerializeField]
    GameObject          target;
    [SerializeField]
    private Transform   gun;
    private Ray         sight;

    [SerializeField]
    private float       reactionTime;
    [SerializeField]
    private float       fireDelay;
    [SerializeField]
    private float       scanDelay;

    private bool        isFiring = false;
    private bool        seesPlayer = false;

    private Vector3     lastSeenLocation = Vector3.forward;

    private bool        isDead = false;

    [System.Serializable]
	public class prefabs
	{  
		[Header("Prefabs")]
		public Transform bulletPrefab;
		public Transform casingPrefab;
	}
    public prefabs Prefabs;

    [System.Serializable]
	public class spawnpoints
	{  
		[Header("Spawnpoints")]
		//Array holding casing spawn points 
		//Casing spawn point array
		public Transform casingSpawnPoint;
		//Bullet prefab spawn from this point
		public Transform bulletSpawnPoint;
	}
    public spawnpoints Spawnpoints;
	
    [Tooltip("How much force is applied to the bullet when shooting.")]
	public float bulletForce = 400;

    public bool enableMuzzleflash = true;
	public ParticleSystem muzzleParticles;
	public bool enableSparks = true;
	public ParticleSystem sparkParticles;
	public int minSparkEmission = 1;
	public int maxSparkEmission = 7;

    [Header("Muzzleflash Light Settings")]
	public Light muzzleflashLight;
	public float lightDuration = 0.02f;

    [SerializeField]
    private AudioSource audio;
    private bool soundHasPlayed = false;
    
    [SerializeField]
    public float MoveSpeed;
    [SerializeField]
    public float RotationSpeed;
    //[SerializeField]
    //public AIMContext Context;

    void Start()    
    {
    }

    void Awake()
    {
        StartCoroutine("scanForPlayer");
        muzzleflashLight.enabled = false;
    }

    void move()
    {
        /* Rotate the character
            Vector3 targetDirection = Context.DecidedDirection;
            float step = RotationSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            // Calculate rotation distance
            float speedMultiplier = 1.0f;
            if (Vector3.Angle(targetDirection, transform.forward) > 50.0f)
                speedMultiplier = 0.0f;
            {

            }*/
    }


    bool checkLOS(Transform targ, out RaycastHit rayHit)
    {
        //Todo: Add arc of fire and scanning for player instead of knowing where the player is all the time.
        //direction
        sight = new Ray(gun.position, targ.position - gun.position);

        //Check if target in LOS
        if (Physics.Raycast(sight, out rayHit, 200f))
        {
            if (rayHit.collider.tag == "Player")
            {
                return true;
            }

            if (rayHit.collider.tag != "Player" && rayHit.collider.tag != "Enemy")
            {
                return false;
            }

        }


        return false;
    }

    IEnumerator scanForPlayer()
    {
        RaycastHit rayHit;
        CapsuleCollider t = target.GetComponent<CapsuleCollider>();
        while (!isDead)
        {

            bool canSeePlayer = (checkLOS(t.transform, out rayHit));
            
            if (canSeePlayer)
            {
                shootAtPlayer();
                yield return new WaitForSeconds(fireDelay);
            }

            yield return new WaitForSeconds(scanDelay);

            
        }
        yield return null;
        Destroy(this.gameObject);
    }

    void shootAtPlayer()    {
        gun.LookAt(target.transform.position);
        isFiring = true;
        
        muzzleParticles.Emit (1);
				
        audio.PlayOneShot(audio.clip);

        //Light flash start
        StartCoroutine(MuzzleFlashLight());
        sparkParticles.Emit (Random.Range (1, 6));

        //Spawn bullet at bullet spawnpoint
        var bullet = (Transform)Instantiate (
            Prefabs.bulletPrefab,
            Spawnpoints.bulletSpawnPoint.transform.position,
            Spawnpoints.bulletSpawnPoint.transform.rotation);

        //Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = 
        bullet.transform.forward * bulletForce;

        //Spawn casing prefab at spawnpoint
        Instantiate (Prefabs.casingPrefab, 
            Spawnpoints.casingSpawnPoint.transform.position, 
            Spawnpoints.casingSpawnPoint.transform.rotation);
        
    }

    private IEnumerator MuzzleFlashLight () 
	{
		muzzleflashLight.enabled = true;
		yield return new WaitForSeconds (lightDuration);
		muzzleflashLight.enabled = false;
	}
}
