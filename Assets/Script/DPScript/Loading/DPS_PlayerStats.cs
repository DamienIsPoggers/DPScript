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
        [Tooltip("Either scale by -1 or rotate 180 degrees when facing left.")]
        public ObjectDirType directionType = ObjectDirType.scale;
        [Tooltip("If true, the mesh child will always look at the camera with the Y rotation")]
        public bool faceCamera = true;
        public bool usesMeshes = false;
        public float weightMultiplier = 1;
        public string startState = "CmnStand";
        [Tooltip("Names of meshes loaded")]
        public List<string> meshNames = new List<string>();
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
        [Header("Col box attributes")]
        [Tooltip("When true it will generate a collision box in script with these parameters. If a col box is found in a col entry, it wont create one")]
        public bool generateColBox = false;
        public Vector3 standingColPos = Vector3.zero;
        public Vector3 standingColSize = Vector3.zero;
        public Vector3 crouchingColPos = Vector3.zero;
        public Vector3 crouchingColSize = Vector3.zero;
        public Vector3 airealColPos = Vector3.zero;
        public Vector3 airealColSize = Vector3.zero;
    }
}