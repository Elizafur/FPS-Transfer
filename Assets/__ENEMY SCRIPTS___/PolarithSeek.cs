using Polarith.AI.Move;
using UnityEngine;

namespace UnityMovementAI
{
    public class PolarithSeek : MonoBehaviour
    {
        public Transform    target;

        SteeringBasics      steeringBasics;

        bool                shouldFlee;

        public float        fleeRangeMin = -5f;
        public float        fleeRangeMax =  5f;


        void Start()
        {
            steeringBasics = GetComponent<SteeringBasics>();
        }

        void FixedUpdate()
        {
            RaycastHit rh;
            Vector3 accel = steeringBasics.Seek(target.position);

            if (Physics.Raycast(transform.position, 
                                target.position - transform.position, 
                                out rh))  {
                                    
                if (rh.distance < 20 
                 && rh.collider.CompareTag("Player"))
                {
                    float yPosition = target.position.y + Random.Range(-10, 10);

                    accel = steeringBasics.Seek(
                        new Vector3(target.position.x + Random.Range(fleeRangeMin, fleeRangeMax), 
                                    yPosition,
                                    target.position.z + Random.Range(fleeRangeMin, fleeRangeMax)));
                }

                if (rh.distance < 5)
                {
                    
                }
                
            }
            

            

            steeringBasics.Steer(accel);
            steeringBasics.LookWhereYoureGoing();
        }
    }
}