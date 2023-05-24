using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPScript.Loading
{
    [CreateAssetMenu(menuName = "DPScript Load/BGM List")]
    public class DPS_BGMList : ScriptableObject
    {
        public List<string> names;
        public List<AudioClip> song;
    }
}