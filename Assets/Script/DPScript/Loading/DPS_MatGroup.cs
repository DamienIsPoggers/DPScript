using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPScript.Loading
{
    [CreateAssetMenu(menuName = "DPScript/Battle Object/Material Group")]
    public class DPS_MatGroup : ScriptableObject
    {
        public List<DPS_ObjectMat> materials;
        public List<Material> spriteMaterials;
    }
}
