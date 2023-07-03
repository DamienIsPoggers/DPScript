using System.Collections.Generic;
using UnityEngine;

namespace DPScript.Loading
{
    [CreateAssetMenu(menuName = "DPScript/Battle Object/Object Material Definition")]
    public class DPS_ObjectMat : ScriptableObject
    {
        public new string name;
        public List<Material> materials;
    }
}