using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PlayerControlsPraying : ByTheTale.StateMachine.State
{
    public PlayerControls playerControls { get { return machine as PlayerControls; } }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();

        //stop praying after they've reached maximum drain/fill speed
        if (playerControls.plant.GetComponent<Plant>().drainAffectRate == playerControls.plant.GetComponent<Plant>().maxAffectRate)
        {
            if (playerControls.PlayerID == 0)
            {
                playerControls.ChangeState<PlayerControlsEndPray>();
            }
        }
        else
        if (playerControls.plant.GetComponent<Plant>().fillAffectRate == playerControls.plant.GetComponent<Plant>().maxAffectRate)
        {
            if (playerControls.PlayerID == 1)
            {
                playerControls.ChangeState<PlayerControlsEndPray>();
            }
        }

        //if bot player
        if (playerControls.isBot)
        {
            //if enemy affect rate is higher go hit them
            if (playerControls.PlayerID == 0)
            {
                if (playerControls.plant.GetComponent<Plant>().fillAffectRate > playerControls.plant.GetComponent<Plant>().drainAffectRate)
                {
                    playerControls.ChangeState<PlayerControlsEndPray>();
                }
            }
            else
            {
                if (playerControls.plant.GetComponent<Plant>().drainAffectRate > playerControls.plant.GetComponent<Plant>().fillAffectRate)
                {
                    playerControls.ChangeState<PlayerControlsEndPray>();
                }
            }
        }
        else //if not a bot player
        {
            //are they still holding down the pray button?
            if (Input.GetKey(playerControls.controlsManager.GetKey(playerControls.PlayerID, ControlKeys.Pray)))
            {

            }
            else
            {
                playerControls.ChangeState<PlayerControlsEndPray>();
            }
        }

        //if you are player 2 then fill up the plants health
        if (playerControls.PlayerID == 1)
        {
            playerControls.plant.GetComponent<Plant>().fill(playerControls.gameObject);
        }
        //if you are player 1 then drain the health
        else if (playerControls.PlayerID == 0)
        {
            playerControls.plant.GetComponent<Plant>().drain(playerControls.gameObject);

        }
    }

    public override void Exit()
    {
        base.Exit();

        //end praying animation
        playerControls.anim.SetTrigger("EndPraying");

        //reset the drain/fill rate
        //for player 2
        if (playerControls.PlayerID == 1)
        {
            playerControls.plant.GetComponent<Plant>().fillAffectRate = playerControls.plant.GetComponent<Plant>().minAffectRate;
        }
        //for player 1
        else if (playerControls.PlayerID == 0)
        {
            playerControls.plant.GetComponent<Plant>().drainAffectRate = playerControls.plant.GetComponent<Plant>().minAffectRate;

        }
    }
}