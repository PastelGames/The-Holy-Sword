using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsActive : ByTheTale.StateMachine.State
{
    public PlayerControls playerControls { get { return machine as PlayerControls; } }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();

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
                //given they are blocking
                if (collider.transform.parent.gameObject.GetComponent<PlayerControls>().IsCurrentState<PlayerControlsBlocking>())
                {
                    //TODO give player feedback so they dont just think they missed

                    //they can still be hit if they are facing the other way
                    if (pc.AreTheyFacingMe(collider.transform.parent.gameObject) == false)
                    {
                        HitEnemyPlayer(collider, pc);

                        //ignore any other hitboxes
                        break;
                    }
                }
                //theyre not blocking
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

        //knock back the other player
        collider.transform.parent.GetComponent<PlayerControls>().rb.velocity =
            new Vector2(
                (new Vector2(collider.transform.parent.position.x, 0) //the x of the enemy
                - new Vector2(pc.transform.position.x, 0)).normalized.x, //minus the x of the player
                1f) //the up direction
            * pc.knockbackForce;

        //make them face you if they are not already
        if (pc.AreTheyFacingMe(collider.transform.parent.gameObject) == false)
        {
            collider.transform.parent.GetComponent<PlayerControls>().FlipDirectionFacing();
        }

        //is the enemy also attacking? if so, trade
        if (collider.transform.parent.GetComponent<PlayerControls>().IsCurrentState<PlayerControlsActive>())
        {
            CheckEnemyPlayerHit(collider.transform.parent.gameObject);
        }

    }
}