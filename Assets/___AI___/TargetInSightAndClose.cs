using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsTargetCloseAndInSight : Action
{
    public GameObject target;

    public float angle;

    public float closeDistance;

    public override TaskStatus OnUpdate()
    {
        Vector3 dir = (target.transform.position - gameObject.transform.position);
        if (dir.sqrMagnitude > closeDistance * closeDistance)
            return TaskStatus.Failure;
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position + new Vector3(0, 0.1f, 0), dir, out hit))
        {
            bool x = hit.collider.gameObject == target && Vector3.Angle(dir, gameObject.transform.forward) < angle * 0.5f;
            if (x)
                return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}