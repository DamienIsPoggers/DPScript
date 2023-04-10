using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPScript
{
    public class DPS_EffectManager : MonoBehaviour
    {
        Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();
        [SerializeField]
        List<GameObject> effectList = new List<GameObject>();
        [SerializeField]
        List<string> effectNames = new List<string>();

        List<GameObject> effectsSpawned = new List<GameObject>();
        List<uint> effectsCountdown = new List<uint>();

        void Start()
        {
            for (int i = 0; i < effectList.Count; i++)
                effects.Add(effectNames[i], effectList[i]);
        }

        void Update()
        {
            for(int i = 0; i < effectsSpawned.Count; i++)
            {
                effectsCountdown[i]--;
                if (effectsCountdown[i] <= 0)
                {
                    Destroy(effectsSpawned[i]);
                    effectsSpawned.RemoveAt(i);
                    effectsCountdown.RemoveAt(i);
                    i--;
                }
            }
        }

        public void spawnEffect(string name, Vector3 offset, uint destroyTime)
        {
            if (!effects.ContainsKey(name))
                return;

            effectsSpawned.Add(Instantiate(effects[name], transform.position + offset, transform.rotation, transform));
            effectsCountdown.Add(destroyTime);
        }
    }
}