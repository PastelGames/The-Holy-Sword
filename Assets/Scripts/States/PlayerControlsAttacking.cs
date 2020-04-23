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

        //if the player is not facing the other player, face them
        if (!playerControls.otherPlayer.GetComponent<PlayerControls>().AreTheyFacingMe(playerControls.gameObject))
        {
            playerControls.FlipDirectionFacing();
        }

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
