using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField]
    private float movementSpeed = 5f;
    private Vector3 movementVelocity;
    private float verticalVelocity;
    [SerializeField]
    private float gravity = -9.8f;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float cameraYAngle = -45; //the y angle of the game camera

    [SerializeField]
    private PlayerInput playerInput;

    private void Awake(){
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate() {
        CalculatePlayerMovement();
        CalculateGravity();

        characterController.Move(movementVelocity);
    }

    private void CalculateGravity(){

        if(characterController.isGrounded == false){
            verticalVelocity = gravity;
        }else{
            verticalVelocity = gravity * .3f; // even if the character is grounded, apply 30% of the gravity to prevent it from floating
        }

        
        animator.SetBool("IsGrounded", characterController.isGrounded);

        movementVelocity += verticalVelocity * Vector3.up * Time.deltaTime;
    }

    private void CalculatePlayerMovement(){
        movementVelocity.Set(playerInput.horizontalInput, 0f, playerInput.verticalInput);
        movementVelocity.Normalize();
        movementVelocity = Quaternion.Euler(0, cameraYAngle, 0) * movementVelocity;
        animator.SetFloat("Speed", movementVelocity.magnitude);

        movementVelocity *= movementSpeed * Time.deltaTime;

        if(movementVelocity != Vector3.zero){
            transform.rotation = Quaternion.LookRotation(movementVelocity);
        }
    }
}
