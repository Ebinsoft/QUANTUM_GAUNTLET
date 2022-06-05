using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Vector3 currentMovement;
    private CharacterController characterController;

    void Awake() {
        characterController = GetComponent<CharacterController>();
        currentMovement = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
