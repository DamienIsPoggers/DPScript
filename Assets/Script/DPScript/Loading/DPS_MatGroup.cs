using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPScript.Loading
{
    [CreateAssetMenu(menuName = "DPScript Load/Battle Object/Material Group")]
    public class DPS_MatGroup : ScriptableObject
    {
        public List<Material> materials;
        public List<string> names;
    }
}
