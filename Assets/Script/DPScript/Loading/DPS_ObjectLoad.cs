using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPScript.Loading
{
    [CreateAssetMenu(menuName = "DPScript Load/Battle Object/Main Load")]
    public class DPS_ObjectLoad : ScriptableObject
    {
        public List<TextAsset> scriptLoad = new List<TextAsset>(), cmnScriptLoad = new List<TextAsset>(),
            colLoad = new List<TextAsset>();
        public GameObject prefab;
        public DPS_MatLoad materials;
    }
}