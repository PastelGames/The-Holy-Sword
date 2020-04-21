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

        //lock player movement and facing
        playerControls.lockPlayerFacing = true;
        playerControls.lockPlayerMove = true;
    }

    public override void Execute()
    {
        base.Execute();
        
    }
}
