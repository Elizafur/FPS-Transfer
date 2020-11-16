/*using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public class BugController : MonoBehaviour
{
    private Transform targetLoc;
    private Seeker seek;
    public bool seeking = false;
    
    
    private CharacterController character;
    private Animator anim;

    
    private Path path;
    public float speed = 2;
    private float nextWaypointDistance = 3;
    private int currentWaypoint = 0;
    public bool endOfPath;
    public GameObject player;

    public bool animating = false;

    // Start is called before the first frame update
    void Start()
    {
        
        seek = GetComponent<Seeker>();
        character = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        targetLoc = player.transform;
        searchPlayer();
    }

    bool LOS()
    {
        RaycastHit hit = new RaycastHit();
        var rayDirection = player.transform.position - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out hit, 5))
        {
            if (hit.transform == player.transform)
            {
                // enemy can see the player!
                //TODO RAYCAST LIMIT RANGE TO MELEE!!!
                print("Player in Melee Range");

                return true;
            }
            else
            {
                // there is something obstructing the view
                return false;
            }
        }
        return false;
    }

    void Update()
    {
        bool shouldAttack = LOS();       

        if (path == null) return;

        if (seeking)
        {
            endOfPath = false;
            float distanceToWaypoint;

            while (true)
            {
                distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
                if (distanceToWaypoint < nextWaypointDistance)
                {
                    // Check if there is another waypoint or if we have reached the end of the path
                    if (currentWaypoint + 1 < path.vectorPath.Count)
                    {
                        currentWaypoint++;
                    }
                    else
                    {
                        endOfPath = true;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            var speedFactor = endOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            // Multiply the direction by our desired speed to get a velocity
            Vector3 velocity = dir * speed * speedFactor;

            transform.rotation = Quaternion.LookRotation(dir);
            
            // Move the agent using the CharacterController component
            // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
            character.SimpleMove(velocity);
        }
        else
        {
            
        }
    }
    
    void FixedUpdate()
    {
        searchPlayer();
    }

    public void startSeek(Transform t)
    {
        seek.StartPath(transform.position, t.position, OnPathComplete);
        seeking = true;
        
    }

    public void searchPlayer()
    {
        Transform t = player.transform;
        if (targetLoc != t)
        {
            targetLoc = t;
            stopSeek();
            startSeek(targetLoc);
        }
        if (!seeking)
        {
            targetLoc = t;
            startSeek(targetLoc);
        }
    }

    public void stopSeek()
    {
        seek.CancelCurrentPathRequest();
        seeking = false;

    }

    private void OnPathComplete(Path p)
    {
        Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

        if (!p.error)
        {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
            anim.SetBool("Walk Forward In Place", true);
            

        }
        
    }

}
*/