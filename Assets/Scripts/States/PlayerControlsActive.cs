﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsActive : ByTheTale.StateMachine.State
{
    public PlayerControls playerControls { get { return machine as PlayerControls; } }

    public override void Enter()
    {
        base.Enter();
    }

    public override void PhysicsExecute()
    {
        base.PhysicsExecute();

        CheckEnemyPlayerHit(playerControls.gameObject);
    }

    void CheckEnemyPlayerHit(GameObject player)
    {

        PlayerControls pc = player.GetComponent<PlayerControls>();

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(pc.attackPoint.position, pc.attackRange);


        //check to see if you hit the enemy player in your frame
        foreach (Collider2D collider in hitColliders)
        {
            //is it a player hitbox?
            if (collider.CompareTag("HitBox")
                //is it not yourself?
                && !collider.transform.parent.gameObject.name.Equals(pc.gameObject.name))
            {
                //given they are parrying
                if (collider.transform.parent.gameObject.GetComponent<PlayerControls>().IsCurrentState<PlayerControlsParryStance>())
                {
                    
                    //they can still be hit if they are facing the other way
                    if (pc.AreTheyFacingMe(collider.transform.parent.gameObject) == false)
                    {
                        HitEnemyPlayer(collider, pc);

                        //ignore any other hitboxes
                        break;
                    }

                    //make the other player parry
                    collider.transform.parent.gameObject.GetComponent<PlayerControls>().ChangeState<PlayerControlsParrying>();

                    //hit yourself
                    pc.ChangeState<PlayerControlsHit>();

                    break;
                }
                //theyre not parrying
                else
                {
                    HitEnemyPlayer(collider, pc);

                    //ignore any other hitboxes
                    break;
                }
            }

        }

    }

    void HitEnemyPlayer(Collider2D collider, PlayerControls pc)
    {
        

        //hit the other player
        collider.transform.parent.GetComponent<PlayerControls>().ChangeState<PlayerControlsHit>();

        //make sparks appear
        playerControls.StartHitSparks();

        //make them face you if they are not already
        if (pc.AreTheyFacingMe(collider.transform.parent.gameObject) == false)
        {
            collider.transform.parent.GetComponent<PlayerControls>().FlipDirectionFacing();
        }

        //is the enemy also attacking? if so, trade
        if (collider.transform.parent.GetComponent<PlayerControls>().IsCurrentState<PlayerControlsActive>())
        {
            playerControls.otherPlayer.GetComponent<PlayerControls>().StartHitSparks();
            playerControls.ChangeState<PlayerControlsHit>();
        }
        else //if theyre not attacking then dont knock them back
        {
            //knock back the other player
            collider.transform.parent.GetComponent<PlayerControls>().rb.velocity =
                new Vector2(
                    (new Vector2(collider.transform.parent.position.x, 0) //the x of the enemy
                    - new Vector2(pc.transform.position.x, 0)).normalized.x, //minus the x of the player
                    1f) //the up direction
                * pc.knockbackForce;
        }

    }
}