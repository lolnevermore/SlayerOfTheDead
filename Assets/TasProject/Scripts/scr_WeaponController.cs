using UnityEngine;
using static scr_Models;
using System.Collections.Generic;
using System.Linq;

public class scr_WeaponController : MonoBehaviour
{
    private scr_CharacterController characterController;

    [Header("References")]
    public Animator weaponAnimator;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;


    [Header("Settings")]
    public WeaponSettingsModel settings;


    bool isInitialised;

    private bool isGroundedTrigger;
    public float fallingDelay;


    [Header("Weapon Breating (DS reference xd)")]
    public Transform weaponSwayObject;

    public float swayAmountA = 1;
    public float swayAmountB = 2;
    public float swayScale = 600;
    public float swayLerpSpeed = 14;

    public float swayTime;
    public Vector3 swayPosition;


    Vector3 newWeaponRotation;
    Vector3 newWeaponRotationVelocity;

    Vector3 targetWeaponRotation;
    Vector3 targetWeaponRotationVelocity;

    Vector3 newWeaponMovementRotation;
    Vector3 newWeaponMovementRotationVelocity;

    Vector3 targetWeaponMovementRotation;
    Vector3 targetWeaponMovementRotationVelocity;



    public bool isAimingIn;


    [Header("Sights")]
    public Transform sightTarget;
    public float sightOffset;
    public float aimingInTime;
    private Vector3 weaponSwayPosition;
    private Vector3 weaponSwayPositionVelocity;


    [Header("Shooting")]
    public float rateOfFire;
    private float currentFireRate;
    public List<WeaponFireType> allowedFireTypes;
    public WeaponFireType currentFireType;
    public bool isShooting;

    [Header("Gun")]
    public float bulletDamage = 10f;
    public float bulletRange = 100f;

    public Camera fpsCam;




    #region - Initialise - 
    public void Initialise(scr_CharacterController CharacterController)
    {
        characterController = CharacterController;
        isInitialised = true;
    }

    #endregion


    #region - Start / Update - 

    private void Start()
    {

        newWeaponRotation = transform.localRotation.eulerAngles;

        currentFireType = allowedFireTypes.First();
    }

    private void Update()
    {
        if (!isInitialised)
        {
            return;
        }

        CalculateWeaponRotation();
        SetWeaponAnimations();
        CalculateAimingIn();
        CalculateWeaponSway();
        CalculateShooting();



    }

    #endregion


    #region - Shooting - 

    private void CalculateShooting()
    {
        if (isShooting)
        {
            Shoot();
            if (currentFireType == WeaponFireType.SemiAuto)
                isShooting = false;

        }


    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawn);

        RaycastHit hit;
        if  (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, bulletRange))
        {
            Debug.Log(hit.transform.name);

            scr_Target target = hit.transform.GetComponent<scr_Target>();
            
            if (target !=null)
            {
                target.takeDamage(bulletDamage);
            }
        }


    }

    #endregion


    #region - Aiming In - 

    private void CalculateAimingIn()
    {
        var targetPositon = transform.position;

        if (isAimingIn)
        {
            targetPositon = characterController.cameraHolder.transform.position + (weaponSwayObject.transform.position - sightTarget.position) + (characterController.cameraHolder.transform.forward * sightOffset);
        }


        weaponSwayPosition = weaponSwayObject.transform.position;
        weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPositon, ref weaponSwayPositionVelocity, aimingInTime);
        weaponSwayObject.transform.position = weaponSwayPosition + swayPosition;

    }

    #endregion


    #region - Jumping - 

    public void TriggerJump()
    {
        isGroundedTrigger = false;
        weaponAnimator.SetTrigger("Jump");
    }

    #endregion


    #region - Rotation - 

    private void CalculateWeaponRotation()
    {







        targetWeaponRotation.y += (isAimingIn ? settings.SwayAmount / 3 : settings.SwayAmount) * (settings.SwayXInverted ? -characterController.input_View.x : characterController.input_View.x) * Time.deltaTime;
        targetWeaponRotation.x += (isAimingIn ? settings.SwayAmount / 3 : settings.SwayAmount) * (settings.SwayYInverted ? characterController.input_View.y : -characterController.input_View.y) * Time.deltaTime;

        targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -settings.SwayClampX, settings.SwayClampX);
        targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -settings.SwayClampY, settings.SwayClampY);
        targetWeaponRotation.z = isAimingIn ? 0 : targetWeaponRotation.y;

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, settings.SwayResetSmoothing);
        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, settings.SwaySmoothing);



        targetWeaponMovementRotation.z = (isAimingIn ? settings.MovementSwayX / 3 : settings.MovementSwayX) * (settings.MovementSwayXInverted ? -characterController.input_Movement.x : characterController.input_Movement.x);
        targetWeaponMovementRotation.x = (isAimingIn ? settings.MovementSwayY / 3 : settings.MovementSwayY) * (settings.MovementSwayYInverted ? -characterController.input_Movement.y : characterController.input_Movement.y);

        targetWeaponMovementRotation = Vector3.SmoothDamp(targetWeaponMovementRotation, Vector3.zero, ref targetWeaponMovementRotationVelocity, settings.SwayResetSmoothing);
        newWeaponMovementRotation = Vector3.SmoothDamp(newWeaponMovementRotation, targetWeaponMovementRotation, ref newWeaponMovementRotationVelocity, settings.SwaySmoothing);


        transform.localRotation = Quaternion.Euler(newWeaponRotation + newWeaponMovementRotation);






    }

    #endregion


    #region - Animations - 

    private void SetWeaponAnimations()
    {

        if (isGroundedTrigger)
        {
            fallingDelay = 0;
        }


        else
        {
            fallingDelay += Time.deltaTime;
        }


        if (characterController.isGrounded && !isGroundedTrigger && fallingDelay > 0.1f)
        {

            weaponAnimator.SetTrigger("Land");
            isGroundedTrigger = true;
        }

        else if (!characterController.isGrounded && isGroundedTrigger)
        {

            weaponAnimator.SetTrigger("Falling");
            isGroundedTrigger = false;
        }

        weaponAnimator.SetBool("isSprinting", characterController.isSprinting);

        weaponAnimator.SetFloat("weaponAnimationSpeed", characterController.weaponAnimationSpeed);
    }

    #endregion


    #region - Sway - 

    private void CalculateWeaponSway()
    {
        var targetPosition = LissajousCurve(swayTime, swayAmountA, swayAmountB) / (isAimingIn ? swayScale * 3 : swayScale);

        swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * swayLerpSpeed);
        swayTime += Time.deltaTime;

        if (swayTime > 6.3f)
        {
            swayTime = 0;
        }


    }



    // Taken from Youtube (not my work)
    private Vector3 LissajousCurve(float Time, float A, float B)
    {
        return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time + Mathf.PI));
    }

    #endregion

}
