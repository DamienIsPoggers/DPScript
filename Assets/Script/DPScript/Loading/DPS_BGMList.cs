using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPScript.Loading
{
    [CreateAssetMenu(menuName = "DPScript/BGM List")]
    public class DPS_BGMList : ScriptableObject
    {
        public List<string> names;
        public List<AudioClip> song;
    }
}