using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsStartup : ByTheTale.StateMachine.State
{
    public PlayerControls playerControls { get { return machine as PlayerControls; } }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();
    }
}