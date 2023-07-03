using System.Collections.Generic;
using UnityEngine;

namespace DPScript.Loading
{
    [CreateAssetMenu(menuName = "DPScript/Battle Object/Voice Lines")]
    public class DPS_VoiceLoad : ScriptableObject
    {
        public DPS_VoiceLanguages lang;
        public List<AudioClip> voiceLines = new List<AudioClip>();
    }
}