using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPScript.Loading
{
    [CreateAssetMenu(menuName = "DPScript/Battle Object/Player stats")]
    public class DPS_PlayerStats : ScriptableObject
    {
        [Header("Misc Attributes")]
        public string characterId = "";
        public int health = 1;
        [Tooltip("If true, the mesh child will always look at the camera with the Y rotation")]
        public bool faceCamera = true;
        public bool usesMeshes = false;
        public float weightMultiplier = 1;
        public string startState = "CmnStand";
        [Header("Movement Attributes")]
        public int forwardWalkingSpeed = 4000;
        public int backwardsWalkingSpeed = -3000;
        public int initalDashingSpeed = 6000;
        public int dashingAccelerationRate = 400;
        public int dashingMaxSpeed = 8200;
        [Header("Aerial Attributes")]
        public int jumpingHeight = 15000;
        public int forwardJumpSpeed = 5000;
        public int backwardsJumpSpeed = -5000;
        public int defualtGravity = -900;
        public int airJumpCount = 1;
        public int forwardAirDashCount = 1;
        public int backwardsAirDashCount = 1;
    }
}