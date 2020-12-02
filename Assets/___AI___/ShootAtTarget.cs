#define DEBUG

using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class ShootAtTarget : Action
{
   // The speed of the object
   public float speed = 0; 
   // The transform that the object is moving towards
   public SharedTransform target;
   // The weapon to shoot from
   public SharedTransform weapon;

   private NavMeshAgent agent;

   public override void OnAwake()
   {
      agent     = GetComponent<NavMeshAgent>();
      agent.speed = speed;
   }

   public override TaskStatus OnUpdate()  
   {
       Vector3 raycastTarget = target.Value.transform.position;
       int randomness = Random.Range(0, 30);


       Vector3 dir = raycastTarget - transform.position;
       Ray R = new Ray(weapon.Value.transform.position, dir);
       if (Physics.Raycast(R, 50, LayerMask.GetMask("Player")))
       {
           //do Damage play animation etc todo
           return TaskStatus.Running;
       }


       return TaskStatus.Success;

       
   }
}
