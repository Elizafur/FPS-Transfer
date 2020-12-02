#define DEBUG

using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class MoveTowards : Action
{
   // The speed of the object
   public float speed = 0; 
   // The transform that the object is moving towards
   public SharedTransform target;
   public float distance;

   private NavMeshAgent agent;

   public override void OnAwake()
   {
      agent     = GetComponent<NavMeshAgent>();
      agent.speed = speed;
   }

   public override TaskStatus OnUpdate()
   {
      // Return a task status of success once we've reached the target
      if (Vector3.Distance(transform.position, target.Value.position) < distance) {
         return TaskStatus.Success;
      }

         // We haven't reached the target yet so keep moving towards it
      _Move();
      
      return TaskStatus.Running;
   }

   private void _Move()
   {    
      agent.destination = target.Value.position; 
   }
}