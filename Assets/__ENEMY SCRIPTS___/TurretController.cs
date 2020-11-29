using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TurretController : MonoBehaviour
{
    [ShowInInspector, PreviewField, Required, TabGroup("Prefabs")]
    public GameObject   target;
    [ShowInInspector, PreviewField, Required, TabGroup("Prefabs")]
    public Transform    gun;
    private Ray         sight;

    [ShowInInspector, TabGroup("Variables")]
    public float       reactionTime;
    [ShowInInspector, TabGroup("Variables")]
    public float       fireDelay;
    [ShowInInspector, TabGroup("Variables")]
    public float       scanDelay;
    [HideInInspector]
    public bool        isFiring = false;
    [HideInInspector]
    public bool        seesPlayer = false;

    private Vector3    lastSeenLocation = Vector3.forward;
    [HideInInspector]
    public  bool       isDead = false;
    [HideInInspector]
    public  Timer      timer;

    [System.Serializable]
	public class prefabs
	{  
		[Header("Prefabs")]
		public Transform bulletPrefab;
		public Transform casingPrefab;
	}
    [ShowInInspector, TabGroup("Prefabs")]
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
    [ShowInInspector, TabGroup("Prefabs")]
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
    public  AudioSource audio;
    private bool soundHasPlayed = false;
    

    void Start()    
    {
        timer = GetComponent<Timer>();
    }

    void Awake()
    {
        StartCoroutine("scanForPlayer");
        muzzleflashLight.enabled = false;
    }


    public bool checkLOS(Transform targ, out RaycastHit rayHit)
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

    IEnumerator lookAtPlayer()
    {
        gun.LookAt(target.transform.position);
        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator scanForPlayer()
    {
        RaycastHit rayHit;
        CapsuleCollider t = target.GetComponent<CapsuleCollider>();
        bool startedCoroutine = false;
        while (!isDead)
        {

            bool canSeePlayer = (checkLOS(t.transform, out rayHit));
            
            if (canSeePlayer)
            {
                shootAtPlayer();
                if (!startedCoroutine) 
                {
                    StartCoroutine(lookAtPlayer());
                    startedCoroutine = true;
                }
                yield return new WaitForSeconds(fireDelay);
            }
            else
            {
                StopCoroutine(lookAtPlayer());
                startedCoroutine = false;
            }

            yield return new WaitForSeconds(scanDelay);
        }
        Destroy(this.gameObject);
        yield return null;

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

    public IEnumerator MuzzleFlashLight () 
	{
		muzzleflashLight.enabled = true;
		yield return new WaitForSeconds (lightDuration);
		muzzleflashLight.enabled = false;
	}
}
