using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPScript
{
    [CreateAssetMenu]
    public class DPS_ObjectLoad : ScriptableObject
    {
        public List<TextAsset> scriptLoad = new List<TextAsset>(), cmnScriptLoad = new List<TextAsset>(),
            colLoad = new List<TextAsset>();
        public List<GameObject> prefabLoad = new List<GameObject>();
    }
}