using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PlayerControlsFree : ByTheTale.StateMachine.State
{
    public PlayerControls playerControls { get { return machine as PlayerControls; } }
    

    public override void Enter()
    {
        base.Enter();

        //unlock players movement and direction facing
        playerControls.lockPlayerMove = false;
        playerControls.lockPlayerFacing = false;

        playerControls.anim.ResetTrigger("EndPraying");

        

    }

    public override void Execute()
    {
        base.Execute();

        bool enemyAffectRateHigher;

        //if they are a bot
        if (playerControls.isBot)
        {
            //if they are being attacked then parry

            //are they attacking me
            if (playerControls.otherPlayer.GetComponent<PlayerControls>().IsCurrentState<PlayerControlsAttacking>()
                //are they close enough
                && Vector2.Distance(playerControls.otherPlayer.transform.position, playerControls.transform.position)
                <= playerControls.b_forceParryRange
                //are they facing me?
                && playerControls.AreTheyFacingMe(playerControls.otherPlayer))
            {
                //if im not facing them
                if (!playerControls.otherPlayer.GetComponent<PlayerControls>().AreTheyFacingMe(playerControls.gameObject))
                {
                    //face them
                    playerControls.FlipDirectionFacing();
                }

                //parry
                playerControls.ChangeState<PlayerControlsParryStance>();
            }

            //if enemy affect rate is higher go hit them
            if (playerControls.PlayerID == 0)
            {
                if (playerControls.plant.GetComponent<Plant>().fillAffectRate > playerControls.plant.GetComponent<Plant>().drainAffectRate)
                {
                    enemyAffectRateHigher = true;
                }
                else
                {
                    enemyAffectRateHigher = false;
                }
            }
            else
            {
                if (playerControls.plant.GetComponent<Plant>().drainAffectRate > playerControls.plant.GetComponent<Plant>().fillAffectRate)
                {
                    enemyAffectRateHigher = true;
                }
                else
                {
                    enemyAffectRateHigher = false;
                }
            }

            //if the enemy affect rate is higher then go hit them
            if (enemyAffectRateHigher == true)
            {
                //if you are not within the hit range move towards them
                if (Vector2.Distance(playerControls.otherPlayer.transform.position, playerControls.transform.position) >= .5f) //hit range
                {
                    //if they are on the your right then move right
                    if (!playerControls.GetComponent<PlayerControls>().AreTheyOnMyLeft(playerControls.otherPlayer))
                    {
                        playerControls.movementValue = 1f;
                    }
                    else //if they are on the left of the you then move left
                    {
                        playerControls.movementValue = -1f;
                    }
                }
                else //if you are in the hit range
                {
                    //if im not facing the player, face the player
                    if (!playerControls.otherPlayer.GetComponent<PlayerControls>().AreTheyFacingMe(playerControls.gameObject)) playerControls.FlipDirectionFacing();

                    Debug.Log(!playerControls.otherPlayer.GetComponent<PlayerControls>().AreTheyFacingMe(playerControls.gameObject));

                    playerControls.ChangeState<PlayerControlsAttacking>();
                }
            }
            else //if the enemy affect rate is not higher
            {
                //if they are too close
                if (Vector2.Distance(playerControls.otherPlayer.transform.position, playerControls.transform.position) <= 2f)
                {
                        //if they are on the right of the plant then move to the left
                        if (!playerControls.plant.GetComponent<Plant>().AreTheyOnMyLeft(playerControls.otherPlayer))
                        {
                            playerControls.movementValue = -1f;
                        }
                        else //if they are on the left of the plant then move to the right
                        {
                            playerControls.movementValue = 1f;
                        }
                    
                }
                else //if they are not too close then dont move
                {
                    playerControls.movementValue = 0f;

                    //face the plant
                    if (!playerControls.plant.GetComponent<Plant>().AreTheyFacingMe(playerControls.gameObject))
                    {
                        playerControls.FlipDirectionFacing();
                    }

                    //begin praying
                    playerControls.ChangeState<PlayerControlsBeginPray>();

                }
            }
            
        }
        else //if they are a player
        {
            //begin attack
            if (Input.GetKey(playerControls.controlsManager.GetKey(playerControls.PlayerID, ControlKeys.Attack)))
            {
                playerControls.ChangeState<PlayerControlsAttacking>();
            }

            //begin parry
            if (Input.GetKey(playerControls.controlsManager.GetKey(playerControls.PlayerID, ControlKeys.Block)))
            {
                playerControls.ChangeState<PlayerControlsParryStance>();
            }

            //begin pray
            if (Input.GetKey(playerControls.controlsManager.GetKey(playerControls.PlayerID, ControlKeys.Pray)))
            {
                playerControls.ChangeState<PlayerControlsBeginPray>();
            }
        }
        
        //if airborne no x movements and lock facing
        playerControls.lockPlayerMove = !playerControls.grounded;
        playerControls.lockPlayerFacing = !playerControls.grounded;
        

        
    }
}
