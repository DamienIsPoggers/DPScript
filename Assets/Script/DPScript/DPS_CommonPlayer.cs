using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPScript
{
    public class DPS_CommonPlayer : MonoBehaviour
    {
        Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();
        [SerializeField]
        List<GameObject> effectList = new List<GameObject>();
        [SerializeField]
        List<string> effectNames = new List<string>();

        [SerializeField]
        List<AudioClip> soundList = new List<AudioClip>();
        [SerializeField]
        List<string> soundNames = new List<string>();

        void Start()
        {
            Battle_Manager.Instance.commonPlayer = this;

            for (int i = 0; i < effectList.Count; i++)
                effects.Add(effectNames[i], effectList[i]);
        }

        public void spawnEffect(string name, Vector3 offset, uint destroyTime, GameWorldObject parent)
        {
            if (!effects.ContainsKey(name))
                return;

            Transform effect = parent.GetComponent<DPS_EffectManager>().effectSpawner.transform;
            parent.GetComponent<DPS_EffectManager>().effectsSpawned.Add(Instantiate(effects[name], effect.position + offset, 
                effect.rotation, effect));
            parent.GetComponent<DPS_EffectManager>().effectsCountdown.Add(destroyTime);
        }
    }
}