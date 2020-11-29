#define DEBUG

using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class MoveTowards : Action
{
   // The speed of the object
   public float speed = 0; 
   // The transform that the object is moving towards
   public SharedTransform target;

   public CharacterController character;

   public override void OnAwake()
   {
      character = GetComponent<CharacterController>();
   }

   public override TaskStatus OnUpdate()
   {
      // Return a task status of success once we've reached the target
      if (Vector3.SqrMagnitude(transform.position - target.Value.position) < 0.1f) {
         return TaskStatus.Success;
      }
      // We haven't reached the target yet so keep moving towards it
      Vector3 _New = Vector3.MoveTowards(transform.position, target.Value.position, speed * Time.deltaTime);
      _Move(_New);
      #if DEBUG
         Debug.Log("Moving");
         Debug.Log(_New);
         Debug.Log(transform.position);
      #endif
      return TaskStatus.Running;
   }

   private void _Move(Vector3 Movdir)
   {
      character.SimpleMove(new Vector3(1,0,0));
   }
}