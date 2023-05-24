using System.Collections.Generic;
using UnityEngine;

namespace DPScript.Loading
{
    [CreateAssetMenu(menuName = "DPScript Load/Battle Object/Object Material Definition")]
    public class DPS_ObjectMat : ScriptableObject
    {
        public string name;
        public List<Material> materials;
    }
}