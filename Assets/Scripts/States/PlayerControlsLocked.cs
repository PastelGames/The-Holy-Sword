using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsLocked : ByTheTale.StateMachine.State
{
    public PlayerControls playerControls { get { return machine as PlayerControls; } }

    public override void Enter()
    {
        base.Enter();

        playerControls.lockPlayerMove = true;
        playerControls.lockPlayerFacing = true;
    }

    public override void Execute()
    {
        base.Execute();
    }
}
