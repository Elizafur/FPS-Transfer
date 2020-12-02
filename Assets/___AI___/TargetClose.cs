using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class TargetClose : Conditional
{

    public int minimumDistance;
    public SharedTransform target;

    public override TaskStatus OnUpdate()
    {
        if (Vector3.Distance(target.Value.position, transform.position) > minimumDistance)
            return TaskStatus.Failure;
        return TaskStatus.Success;
    }
}
