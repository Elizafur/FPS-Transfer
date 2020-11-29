using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityMovementAI
{
    public class EnemySpawner : MonoBehaviour
    {
        public Transform Template;
        public int NumberOfObjects = 10;
        public bool RandomizeOrientation = false;
        public bool ShouldSpawnInsant = false;
        public float SecondsInterval = 1;

        public MovementAIRigidbody[] thingsToAvoid;

        [System.NonSerialized]
        public List<MovementAIRigidbody> objs = new List<MovementAIRigidbody>();

        private int intCounter = 0;

        void Start()
        {
            MovementAIRigidbody rb = Template.GetComponent<MovementAIRigidbody>();
            /* Manually set up the MovementAIRigidbody since the given obj can be a prefab */
            rb.SetUp();

            StartCoroutine(SpawnUnits());
            
        }

        IEnumerator SpawnUnits()    
        {

            
            /* Create the objects */
            for (; intCounter < NumberOfObjects; intCounter++)
            {
                /* Try to place the objects multiple times before giving up */
                for (int j = 0; j < 10; j++)
                {
                    if (TryToCreateObject())
                    {
                        break;
                    }
                }

                yield return new WaitForSeconds(SecondsInterval);

            }

            yield return null;
        }

        bool TryToCreateObject()
        {
            Vector3 pos = this.transform.position;

            if (CanPlaceObject(pos))
            {
                Transform t = Instantiate(Template, pos, Quaternion.identity) as Transform;

                t.localScale = new Vector3(Template.localScale.x, Template.localScale.y, Template.localScale.z);
                
                if (RandomizeOrientation)  
                {
                    t.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                }


                objs.Add(t.GetComponent<MovementAIRigidbody>());

                return true;
            }

            return false;
        }

        bool CanPlaceObject(Vector3 pos)
        {
            /* Make sure it does not overlap with any thing to avoid */
            for (int i = 0; i < thingsToAvoid.Length; i++)
            {
                float dist = Vector3.Distance(thingsToAvoid[i].Position, pos);

                if (dist < thingsToAvoid[i].Radius)
                {
                    return false;
                }
            }

            /* Make sure it does not overlap with any existing object */
            foreach (MovementAIRigidbody o in objs)
            {
                float dist = Vector3.Distance(o.Position, pos);

                if (dist < o.Radius)
                {
                    return false;
                }
            }

            return true;
        }
    }
}