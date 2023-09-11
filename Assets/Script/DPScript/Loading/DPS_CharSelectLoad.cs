using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPScript.Loading
{
    [CreateAssetMenu(menuName = "DPScript/Battle Object/Character Select Load")]
    public class DPS_CharSelectLoad : ScriptableObject
    {
        public GameObject prefab;
        public DPS_MatLoad mats;
        public bool faceCamera = false;
        public bool isSprite = false;
        public List<AudioClip> idleLines = new List<AudioClip>(), selectedLines = new List<AudioClip>();
        public AudioClip announcerLine;
    }
}