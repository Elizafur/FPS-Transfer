using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmDroneAttackAI : TurretController  
{
    [SerializeField]
    public LayerMask IgnoreRaycast;

    void Start()    
    {
        timer = GetComponent<Timer>();
    }

    void Awake()
    {
        StartCoroutine("scanForPlayer");
        muzzleflashLight.enabled = false;
    }

    public void shootAtPlayer()    {
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

    public bool checkLOS(Transform targ, out RaycastHit rayHit)
    {
        //Todo: Add arc of fire and scanning for player instead of knowing where the player is all the time.
        //direction
        Ray sight = new Ray(gun.position, targ.position - gun.position);



        //Check if target in LOS
        if (Physics.Raycast(sight, out rayHit, 200f, ~IgnoreRaycast))
        {
            if (rayHit.collider.tag == "Player")
            {
                seesPlayer = true;
                return true;
            }

            if (rayHit.collider.tag != "Player" && rayHit.collider.tag != "Enemy")
            {
                seesPlayer = false;
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
            if (!seesPlayer)
                yield return new WaitForSeconds(scanDelay);

            lookAtPlayer();
            yield return 0;
            
        }
        yield return null;
        Destroy(this.gameObject);
    }

    void lookAtPlayer()
    {
        gun.LookAt(target.transform.position, Vector3.up);
        gun.Rotate(new Vector3(0,-90,10));  //TODO: this is a hacky shit way to do it because my model's transform is fucked up.
    }
}
