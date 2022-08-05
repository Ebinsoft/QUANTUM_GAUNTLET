using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreditsPlayer : MonoBehaviour
{
    private PlayerInput playerInput;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        onEnable();
    }

    public void onStart(InputAction.CallbackContext context)
    {
        playerInput.actions["Start"].started -= onStart;
        GameManager.instance.TransitionToScene("MainMenu");
    }

    void onEnable()
    {
        playerInput.actions["Start"].started += onStart;
    }
}