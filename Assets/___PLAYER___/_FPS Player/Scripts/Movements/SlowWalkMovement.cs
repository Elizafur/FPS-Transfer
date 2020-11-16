using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Vector3 input = transform.forward; //TOOD: need to get real player input
        movement.Move(input, moveSpeed, 0f);
    }

    public override void Check(bool canInteract)
    {
        if (playerInput.slowWalk)
            player.ChangeStatus(changeTo, IK);
        else if (movement.grounded && player.isCrouching())
            player.ChangeStatus(Status.crouching, IK);
        else if (movement.grounded && movement.controller.velocity.x > 0)
            player.ChangeStatus(Status.walking, IK);
        else if (playerInput.run)
            player.ChangeStatus(Status.sprinting, IK);
        else
            player.ChangeStatus(Status.idle, IK);
    }   

    public override IKData IK()
    {
        IKData data = new IKData();
        return data;
    }
}
