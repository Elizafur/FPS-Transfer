#define DEBUG

using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class RunAwayFromTarget : Action
{
   // The transform that the object is moving towards
   public SharedTransform target;

   private NavMeshAgent agent;

   public int toDistance;

   public override void OnAwake()
   {
       agent = GetComponent<NavMeshAgent>();
   }

   public override TaskStatus OnUpdate()  
   {

       float distance = Vector3.Distance(target.Value.position, transform.position);
       Debug.Log(distance);
       if (distance > toDistance)
       {
           return TaskStatus.Success;
       }
       
       Vector3 directionToTarget = (transform.position - target.Value.position).normalized;
       Vector3 moveTowards = directionToTarget * -1;

       int RandomOffsetX;
       int RandomOffsetZ;
       for (int i = 0; i < 5; ++i) //Try 5 locations to check if they are free
       {
           RandomOffsetX = Random.Range(-5, 5);
           RandomOffsetZ = Random.Range(-5, 5);
           Vector3 destination = new Vector3(RandomOffsetX + moveTowards.x, moveTowards.y, RandomOffsetZ + moveTowards.z);

           if (!Physics.CheckSphere(destination, 2f)) // SPACE IS FREE
           {
               agent.destination = destination;
               break;
           }

       }

       return TaskStatus.Running;



       return TaskStatus.Running;     
   }
}