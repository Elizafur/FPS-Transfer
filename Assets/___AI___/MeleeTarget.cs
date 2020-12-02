#define DEBUG


using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class MeleeTarget : Action
{
   // The transform that the object is moving towards
   public SharedTransform target;

   private NavMeshAgent agent;

   public override void OnAwake()
   {
   }

   public override TaskStatus OnUpdate()  
   {
       return TaskStatus.Success;     
   }
}
