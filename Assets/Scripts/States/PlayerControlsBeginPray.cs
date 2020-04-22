using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsBeginPray : ByTheTale.StateMachine.State
{
    public PlayerControls playerControls { get { return machine as PlayerControls; } }

    public override void Enter()
    {
        base.Enter();

        //start the pray animation
        playerControls.anim.SetTrigger("BeginPraying");

        //face the plant if not already
        if (!playerControls.plant.GetComponent<Plant>().AreTheyFacingMe(playerControls.gameObject))
        {
            playerControls.FlipDirectionFacing();
        }

        //lock player movement and facing
        playerControls.lockPlayerFacing = true;
        playerControls.lockPlayerMove = true;
    }

    public override void Execute()
    {
        base.Execute();
        
    }

    public override void Exit()
    {
        base.Exit();

        playerControls.anim.ResetTrigger("BeginPraying");
    }
}
