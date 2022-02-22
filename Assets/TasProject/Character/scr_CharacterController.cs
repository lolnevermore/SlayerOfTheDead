using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static scr_Models;

public class scr_CharacterController : MonoBehaviour
{

    private DefaultInput defaultInput;
    public Vector2 input_Movement;
    public Vector2 input_View;

    private Vector3 newCameraRotation;


    [Header("References")]
    public Transform cameraHolder;



    [Header("Settings")]
    public PlayerSettingsModel playerSettings;
    public float viewClamYMin = -70;
    public float viewClamYMax = 80;



    private void Awake()
    {
        defaultInput = new DefaultInput();


        defaultInput.Character.Movement.performed += e => input_Movement = e.ReadValue<Vector2>();
        defaultInput.Character.View.performed += e => input_View = e.ReadValue<Vector2>();

        defaultInput.Enable();

        newCameraRotation = cameraHolder.localRotation.eulerAngles; 


    }


    private void Update() {
        CalculateView();
        CalculateMovement();
    }

    private void CalculateView() {

        newCameraRotation.x += playerSettings.ViewYSensitivity * input_View.y * Time.deltaTime;

        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClamYMin, viewClamYMax);


        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);

    }

    private void CalculateMovement() {

    }

}
