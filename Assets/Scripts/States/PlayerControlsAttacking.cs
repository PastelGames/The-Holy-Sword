using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsAttacking : ByTheTale.StateMachine.State
{
    public PlayerControls playerControls { get { return machine as PlayerControls; } }

    public override void Enter()
    {
        base.Enter();

        //start the attack animation
        playerControls.anim.SetTrigger("Attack");
        //lock the players movement
        playerControls.lockPlayerMove = true;
        //lock players ability to face directions
        playerControls.lockPlayerFacing = true;
    }

    public override void Execute()
    {
        base.Execute();
    }
}
