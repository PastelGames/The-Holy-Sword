using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class PlayerControls : ByTheTale.StateMachine.MachineBehaviour
{
    public int PlayerID;
    public ControlsManager controlsManager;

    public Rigidbody2D rb;
    public float moveSpeed;
    float dirX;
    SpriteRenderer sr;
    public Animator anim;
    public bool lockPlayerMove; //bool used to lock the player position
    public Transform attackPoint;
    public float attackRange;
    public BoxCollider2D bc;
    public float knockbackForce;
    public bool lockPlayerFacing;
    public bool grounded = false;
    List<Collider2D> hitboxes;
    public GameObject plant;
    public bool isBot;
    public float b_forceParryRange;
    public GameObject parrySparks;

    public GameObject otherPlayer;

    public float movementValue;

    public override void AddStates()
    {
        AddState<PlayerControlsFree>();
        AddState<PlayerControlsAttacking>();
        AddState<PlayerControlsActive>();
        AddState<PlayerControlsAttackRecovery>();
        AddState<PlayerControlsHit>();
        AddState<PlayerControlsStartup>();
        AddState<PlayerControlsParryStance>();
        AddState<PlayerControlsBeginPray>();
        AddState<PlayerControlsPraying>();
        AddState<PlayerControlsEndPray>();
        AddState<PlayerControlsLocked>();
        AddState<PlayerControlsParrying>();

        SetInitialState<PlayerControlsLocked>();
    }

    // Start is called before the first frame update
    void Awake()
    { 
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
        controlsManager = FindObjectOfType<ControlsManager>();
        //get all colliders attached to player
        hitboxes = new List<Collider2D>(GetComponentsInChildren<BoxCollider2D>());
        //get the hitboxes
        List<Collider2D> acceptableColliders = new List<Collider2D>();
        foreach (BoxCollider2D bc in hitboxes)
        {
            if (bc.gameObject.CompareTag("HitBox"))
            {
                acceptableColliders.Add(bc);
            }
        }
        hitboxes = acceptableColliders;

        //get the other player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(players.Length);
        foreach (GameObject player in players)
        {
            //if their player ids dont match then it is the other player
            if (PlayerID != player.GetComponent<PlayerControls>().PlayerID)
            {
                otherPlayer = player;
            }
        }
    }

    // Update is called once per frame
    public override void LateUpdate()
    {
        base.LateUpdate();

        //if they are a bot
        if (isBot)
        {

        }
        else
        {
            movementValue = GetLeftRightMovement();
        }

        dirX = movementValue * moveSpeed;
        FaceForward();
        SetMovingAnim();
        
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        //if the player is touching the ground they are grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        //if the player is not touching the ground they are not grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

    float GetLeftRightMovement()
    {
        if (Input.GetKey(controlsManager.GetKey(PlayerID, ControlKeys.Leftkey)))
        {
            return -1f;
        }
        else if (Input.GetKey(controlsManager.GetKey(PlayerID, ControlKeys.RightKey)))
        {
            return 1f;
        }
        else
        {
            return 0f;
        }
          
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        //set the velocity of the player
        if (!lockPlayerMove)
        {
            rb.velocity = Vector3.ClampMagnitude(new Vector2(dirX + rb.velocity.x, rb.velocity.y), moveSpeed);
        }

        //if the player is not on the ground they cannot be hit
        if (grounded)
        {
            
        }
        else
        {
            DisableHitboxes();
        }
    }

    public void EnableHitboxes()
    {
        foreach (Collider2D hitbox in hitboxes)
        {
            hitbox.enabled = true;
        }
    }

    public void DisableHitboxes()
    {
        foreach(Collider2D hitbox in hitboxes)
        {
            hitbox.enabled = false;
        }
    }

    //sets the animation bool moving to true if moving
    void SetMovingAnim()
    {
        if (Mathf.Abs(rb.velocity.x) > 0.1) anim.SetBool("Moving", true);
        else anim.SetBool("Moving", false);
    }

    //make the player face the right direction based on inputs
    void FaceForward()
    {
        if (!lockPlayerFacing)
        {
            if (movementValue > 0.1)
            {
                transform.localScale = new Vector3(-1f, transform.localScale.y);
            }
            else if (movementValue < -0.1)
            {
                transform.localScale = new Vector3(1f, transform.localScale.y);
            }
        }
    }

    public void SetStateStartup()
    {
        if (IsCurrentState<PlayerControlsAttacking>())
        {
            ChangeState<PlayerControlsStartup>();
        }
    }

    public void SetStateActive()
    {
        if (IsCurrentState<PlayerControlsStartup>())
        {
            ChangeState<PlayerControlsActive>();
        }
    }

    public void SetStateRecovery()
    {
        if (IsCurrentState<PlayerControlsActive>())
        {
            ChangeState<PlayerControlsAttackRecovery>();
        }
    }


    //at the end of the attack animation
    public void OnAttackAnimationFinished()
    {
        if (IsCurrentState<PlayerControlsAttackRecovery>())
        {
            ChangeState<PlayerControlsFree>();
        }
    }

    //at the end of the hit animation
    public void OnHitAnimationFinished()
    {
        if(IsCurrentState<PlayerControlsHit>())
        {
            ChangeState<PlayerControlsFree>();
        }

    }

    //at the end of the begin pray animation
    public void OnBeginPrayAnimationFinished()
    {
        if (IsCurrentState<PlayerControlsBeginPray>())
        {
            ChangeState<PlayerControlsPraying>();
        }
    }

    public void OnEndPrayAnimationFinished()
    {
        if(IsCurrentState<PlayerControlsEndPray>())
        {
            ChangeState<PlayerControlsFree>();
        }
    }

    public void OnParryStanceAnimationFinished()
    {
        if (IsCurrentState<PlayerControlsParryStance>())
        {
            anim.SetTrigger("ExitParryStance");
            ChangeState<PlayerControlsFree>();
        }
    }

    public void OnParryingAnimationFinished()
    {
        if (IsCurrentState<PlayerControlsParrying>())
        {
            ChangeState<PlayerControlsFree>();
        }
    }

    public void StartParrySparks()
    {
        parrySparks.GetComponent<ParticleSystem>().Play();
    }


    public bool AreTheyFacingMe(GameObject otherPlayer)
    {
        //if the other player is on your left
        if (AreTheyOnMyLeft(otherPlayer) == true)
        {
            Debug.Log("They are on my left");
            //if the other player is facing right
            if (IsThisPlayerFacingLeft(otherPlayer) == false)
            {
                //they are indeed facing you
                return true;
            }
        }
        //if they are on your right
        else
        {
            Debug.Log("They are on my right");
            //if they other player is facing left
            if (IsThisPlayerFacingLeft(otherPlayer) == true)
            {
                //they are indeed facing you
                return true;
            }
        }
        //if none of these conditions are true, then they are not facing you
        return false;
    }

    //is the other player to the left of you?
    public bool AreTheyOnMyLeft(GameObject otherPlayer)
    {
        if (transform.position.x - otherPlayer.transform.position.x >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    //get whether the player is facing left or not
    public bool IsThisPlayerFacingLeft(GameObject player)
    {
        if (player.transform.localScale.x == 1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void FlipDirectionFacing()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);
    }


    //shows the sphere for the collider for hitting
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    IEnumerator Invincible()
    {
        yield return new WaitForSeconds(.1f);
        EnableHitboxes();
    }

    public void StartInvincibility()
    {
        StartCoroutine(Invincible());
    }

}
