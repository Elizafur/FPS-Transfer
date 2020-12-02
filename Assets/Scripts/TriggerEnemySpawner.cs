#define DEBUG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Lean.Pool;

public class TriggerEnemySpawner : MonoBehaviour
{

    [ShowInInspector, PreviewField, Required, AssetsOnly, LabelText("Prefab"),  HorizontalGroup("Split")]
    public GameObject Prefab;

    [LabelText("Amount"), VerticalGroup("Split/Properties")]
    public int Amount;

    [LabelText("TimeBetween"), VerticalGroup("Split/Properties"), MinValue(0)]
    public int TimeBetween = 0;

    [LabelText("Trigger?"), VerticalGroup("Split/Properties")]
    public bool DoTrigger = true;

    void Awake()
    {
        if (!DoTrigger)
            StartCoroutine(SpawnEnemies());
    }


    public void DoActivateTrigger()     //Trigger called
    {
        if (DoTrigger)
            StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        int timesRetried = 0;
        if (TimeBetween == 0)                           //Instant Spawn, Random Positions
        {
            
            for (int i = 0; i < Amount; ++i)  
            {
                int randomOffsetX = Random.Range(-5, 5);
                int randomOffsetZ = Random.Range(-5, 5);
                Vector3 spawnPosition = new Vector3(randomOffsetX + transform.position.x, transform.position.y, transform.position.z - randomOffsetZ);

                if (Physics.CheckBox(spawnPosition, new Vector3(.5f, .5f, .5f)))
                {
                    --i;                //retry spawning
                    ++timesRetried;
                    #if DEBUG
                        print("Tried to Spawn, Times retried: " + timesRetried);
                    #endif

                    if (timesRetried > 5)
                    {
                    #if DEBUG
                        print("Failed to Spawn: " + i);
                    #endif
                        ++i;            //Undo retry, try to spawn next prefab
                        timesRetried = 0;
                    }
                }
                else
                {
                    #if DEBUG
                        print("Spawned: " + i);
                    #endif
                    if (timesRetried != 0)  //reset retries if we had to retry at all
                    {
                        timesRetried = 0;
                        ++i;
                    }
                    LeanPool.Spawn(Prefab, spawnPosition, Quaternion.Euler(0, 0, 0));
                }
            }

            yield break;
        }

        for (int i = 0; i < Amount; ++i)                //Delayed Spawn
        {
            int randomOffsetX = Random.Range(-5, 5);
            int randomOffsetZ = Random.Range(-5, 5);
            Vector3 spawnPosition = new Vector3(randomOffsetX + transform.position.x, transform.position.y, transform.position.z - randomOffsetZ);

            if (Physics.CheckSphere(spawnPosition, .5f))
            {
                --i;
                ++timesRetried;

                if (timesRetried > 5)
                {
                    ++i;
                    timesRetried = 0;
                }
            }
            else
            {
                if (timesRetried != 0)
                {
                    timesRetried = 0;
                    ++i;
                }
                LeanPool.Spawn(Prefab, spawnPosition, Quaternion.Euler(0, 0, 0));
            }

            yield return new WaitForSeconds(TimeBetween);
        }
        yield return null;
    }


    
}
