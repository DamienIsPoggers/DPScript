using System.Collections.Generic;
using UnityEngine;

namespace DPScript.Loading
{
    [CreateAssetMenu(menuName = "DPScript/Battle Object/Main Load")]
    public class DPS_ObjectLoad : ScriptableObject
    {
        public List<TextAsset> scriptLoad = new List<TextAsset>(), cmnScriptLoad = new List<TextAsset>(),
            colLoad = new List<TextAsset>();
        public GameObject prefab;
        public DPS_MatLoad materials;
        public DPS_PlayerStats stats;
        public List<DPS_VoiceLoad> voices = new List<DPS_VoiceLoad>();
        public AudioClip announcerLine;
    }
}