using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPScript.Loading
{
    [CreateAssetMenu(menuName = "DPScript Load/Battle Object/Material Load")]
    public class DPS_MatLoad : ScriptableObject
    {
        public List<DPS_MatGroup> mats;
    }
}
