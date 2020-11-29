using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Stupid implementation
public class SlowWalkMovement : MovementType
{

    float moveSpeed;

    public override void SetPlayerComponents(PlayerMovement move, PlayerInput input)
    {
        base.SetPlayerComponents(move, input);
        moveSpeed = movement.walkSpeed / 2;
    }

    public override void Movement()
    {
        Vector2 i = playerInput.input;
        Vector3 dir = new Vector3(i.y, 0, i.x);

        movement.Move(dir, moveSpeed, 1f);
    }

    public override void Check(bool canInteract)
    {
        if (!canInteract || !movement.grounded)
            return;

        if (playerInput.slowWalk)
            player.ChangeStatus(changeTo, IK);
        else
            player.ChangeStatus(Status.walking, IK);
    }   

    public override IKData IK()
    {
        IKData data = new IKData();
        return data;
    }
}
*/