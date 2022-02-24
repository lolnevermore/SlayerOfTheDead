using System;
using System.Collections.Generic;
using UnityEngine;

public static class scr_Models 
{
    #region - player -


    [Serializable]


    public class PlayerSettingsModel
    {
        [Header("View Settings")]
        public float ViewXSensitivity;
        public float ViewYSensitivity;

        public bool ViewXInverted;
        public bool ViewYInverted;



        [Header("Movement")]
        public float WalkingForwardSpeed;
        public float WalkingBackwardsSpeed;
        public float WalkingStrafeSpeed;

        [Header("Movement + Running")]
        public float RunningForwardSpeed;
        public float RunningStrafeSpeed;


        [Header("Jumping")]
        public float JumpingHeight;
        public float JumpingFalloff;

    }

    #endregion







    #region - Weapons - 


    [Serializable]
    public class WeaponSettingsModel

    {
        [Header("Sway")]
        public float    SwayAmount;
        public bool     SwayYInverted;
        public bool     SwayXInverted;
        public float    SwaySmoothing;
        public float    SwayResetSmoothing;
        public float    SwayClampX;
        public float    SwayClampY;



    }




    #endregion












}

