using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PlayerControlsBlocking : ByTheTale.StateMachine.State
{
    public PlayerControls playerControls { get { return machine as PlayerControls; } }

    public override void Enter()
    {
        base.Enter();

        //start blocking animation
        playerControls.anim.SetBool("Blocking", true);

        //lock player movement
        playerControls.lockPlayerMove = true;
    }

    public override void Execute()
    {
        base.Execute();

        //if they are a bot
        if (playerControls.isBot)
        {
            //if they recover then leave this state
            if (playerControls.otherPlayer.GetComponent<PlayerControls>().IsCurrentState<PlayerControlsFree>())
            {
                playerControls.ChangeState<PlayerControlsFree>();
            }
        }
        //if they are not a bot
        else
        {
            //end block
            if (Input.GetKeyUp(playerControls.controlsManager.GetKey(playerControls.PlayerID, ControlKeys.Block)))
            {
                playerControls.ChangeState<PlayerControlsFree>();
            }
        }
        
    }

    public override void Exit()
    {
        base.Exit();

        //stop blocking animation
        playerControls.anim.SetBool("Blocking", false);

    }
}