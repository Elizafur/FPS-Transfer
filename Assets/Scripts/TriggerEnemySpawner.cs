#define DEBUG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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
                int randomOffset = Random.Range(-9, 9);
                Vector3 spawnPosition = new Vector3(randomOffset + transform.position.x, transform.position.y, transform.position.z - randomOffset);

                if (Physics.CheckSphere(spawnPosition, 2))
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
                    Instantiate(Prefab, spawnPosition, Quaternion.Euler(0, 0, 0));
                }
            }

            yield return null;
        }

        for (int i = 0; i < Amount; ++i)                //Delayed Spawn
        {
            int randomOffset = Random.Range(-15, 15);
            Vector3 spawnPosition = new Vector3(randomOffset + transform.position.x, transform.position.y, transform.position.z - randomOffset);

            if (Physics.CheckSphere(spawnPosition, 2))
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
                Instantiate(Prefab, spawnPosition, Quaternion.Euler(0, 0, 0));
            }

            yield return new WaitForSeconds(TimeBetween);
        }
        yield return null;
    }


    
}
