using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileTurretController : TurretController
{
    
    [Header("Patrol Waypoints")][SerializeField]
    private AIPatrolPoints patrolPoints;

    private AIState state;


}
