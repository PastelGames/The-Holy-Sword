using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsHit : ByTheTale.StateMachine.State
{
    public PlayerControls playerControls { get { return machine as PlayerControls; } }

    public override void Enter()
    {
        base.Enter();

        //lock the players movement and facing
        playerControls.lockPlayerMove = true;
        playerControls.lockPlayerFacing = true;
        //start hurt animation
        playerControls.anim.SetTrigger("Hurt");

        //disable hitboxes
        playerControls.DisableHitboxes();
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();

        //make the player invincible for a short period to allow them to move around
        playerControls.StartInvincibility();
    }
}