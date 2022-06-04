using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState currentState;

    // One for each concrete state
    PlayerIdleState IdleState = new PlayerIdleState();
    PlayerMovingState MovingState = new PlayerMovingState();
    PlayerJumpingState JumpingState = new PlayerJumpingState();
    PlayerFallingState FallingState = new PlayerFallingState();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
