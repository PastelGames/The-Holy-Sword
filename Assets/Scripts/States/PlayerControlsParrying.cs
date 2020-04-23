using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PlayerControlsParrying: ByTheTale.StateMachine.State
{
    public PlayerControls playerControls { get { return machine as PlayerControls; } }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("began parry");
        playerControls.anim.SetTrigger("BeginParry");

        //start hit sparks
        playerControls.StartHitSparks();
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("ended parry");
        playerControls.anim.ResetTrigger("BeginParry");
    }
}