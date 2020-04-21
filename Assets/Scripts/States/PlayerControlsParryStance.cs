using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PlayerControlsParryStance : ByTheTale.StateMachine.State
{
    public PlayerControls playerControls { get { return machine as PlayerControls; } }

    public override void Enter()
    {
        base.Enter();

        //start parry animation
        playerControls.anim.SetTrigger("EnterParryStance");

        //lock player movement
        playerControls.lockPlayerMove = true;
        playerControls.lockPlayerFacing = true;
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        
    }
}