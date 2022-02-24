using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static scr_Models;

public class scr_CharacterController : MonoBehaviour
{

    private CharacterController characterController;


    private DefaultInput defaultInput;
    public Vector2 input_Movement;
    public Vector2 input_View;

    private Vector3 newCameraRotation;
    private Vector3 newCharacterRotation;


    [Header("References")]
    public Transform cameraHolder;



    [Header("Settings")]
    public PlayerSettingsModel playerSettings;
    public float viewClamYMin   = -70;
    public float viewClamYMax   = 80;

    [Header("Gravity")]
    public float    gravityAmount;
    public float    gravityMin;
    private float   playerGravity;


    public Vector3  jumpingForce;
    private Vector3 jumpingForceVelocity;


    private bool isSprinting;

    [Header("Weapon")]
    public scr_WeaponController currentWeapon;

    public float weaponAnimationSpeed;







    private void Awake()
    {
        defaultInput = new DefaultInput();


        defaultInput.Character.Movement.performed += e => input_Movement = e.ReadValue<Vector2>();
        defaultInput.Character.View.performed += e => input_View = e.ReadValue<Vector2>();
        defaultInput.Character.Jump.performed += e => Jump();
        defaultInput.Character.Sprint.performed += e => ToggleSprint();


        defaultInput.Enable();

        newCameraRotation = cameraHolder.localRotation.eulerAngles;
        newCharacterRotation = transform.localRotation.eulerAngles;

        characterController = GetComponent<CharacterController>();

        if (currentWeapon)
        {
            currentWeapon.Initialise(this);
        }


    }


    private void Update() {
        CalculateView();
        CalculateMovement();
        CalculateJump();
    }

    private void CalculateView() {

        newCharacterRotation.y += playerSettings.ViewXSensitivity * input_View.x * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(newCharacterRotation);



        newCameraRotation.x += playerSettings.ViewYSensitivity * input_View.y * Time.deltaTime;
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClamYMin, viewClamYMax);


        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);

    }

    private void CalculateMovement() {


        if (input_Movement.y <= 0.2f)
        {
            isSprinting = false;
        }


        var verticalSpeed = playerSettings.WalkingForwardSpeed;
        var horizontalSpeed = playerSettings.WalkingStrafeSpeed;


        if (isSprinting)
        {
            verticalSpeed   = playerSettings.RunningForwardSpeed;
            horizontalSpeed = playerSettings.RunningStrafeSpeed;
        }


        var newMovementSpeed = new Vector3 (horizontalSpeed * input_Movement.x * Time.deltaTime, 0, verticalSpeed * input_Movement.y * Time.deltaTime);
        newMovementSpeed = transform.TransformDirection(newMovementSpeed);



        if (playerGravity > gravityMin) 
        {
            playerGravity -= gravityAmount * Time.deltaTime;
        }

        if (playerGravity < -0.1 && characterController.isGrounded)
        {
            playerGravity = -0.1f;
        }

     

        newMovementSpeed.y  += playerGravity;
        newMovementSpeed    += jumpingForce * Time.deltaTime;

        characterController.Move(newMovementSpeed);
    }


    private void CalculateJump()
    {
        jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity, playerSettings.JumpingFalloff);
    }


    private void Jump()
    {

        if (!characterController.isGrounded)
        {
            return;
        }



        jumpingForce = Vector3.up * playerSettings.JumpingHeight;
        playerGravity = 0;

    }

    private void ToggleSprint()
    {


        if (input_Movement.y <= 0.2f)
        {
            isSprinting = false;
            return;
        }

        isSprinting = !isSprinting;



    }

}
