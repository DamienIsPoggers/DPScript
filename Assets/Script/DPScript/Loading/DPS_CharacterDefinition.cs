using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPScript.Loading
{
    [CreateAssetMenu(menuName = "DPScript/Character Definition")]
    public class DPS_CharacterDefinition : ScriptableObject
    {
        public string LoadString;
        public string Name_Short;
        public string Name_Long;
    }
}