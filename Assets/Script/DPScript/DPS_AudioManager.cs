using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPScript
{
    public class DPS_AudioManager : MonoBehaviour
    {
        [SerializeField]
        List<AudioClip> soundList = new List<AudioClip>(), voiceList = new List<AudioClip>();
        [SerializeField]
        List<string> soundNames = new List<string>(), voiceNames = new List<string>();
        public AudioSource voiceParent;
        public Transform soundsParent;

        void Start()
        {
            voiceParent = GetComponentInChildren<AudioSource>();
            soundsParent = transform.Find("Sounds");
        }

        public void playVoiceLine(string id)
        {
            if (!voiceNames.Contains(id))
                return;
            playVoiceLine(voiceNames.FindIndex(i => i.Contains(id)));
        }

        public void playVoiceLine(int id)
        {
            if (voiceList.Count <= id)
                return;
            voiceParent.clip = voiceList[id];
            voiceParent.Play();
        }
        public void playSoundEffect(string id)
        {
            if (!soundNames.Contains(id))
                return;
            playSoundEffect(soundNames.FindIndex(i => i.Contains(id)));
        }

        public void playSoundEffect(int id)
        {
            if (soundList.Count <= id)
                return;
            GameObject sound = new GameObject();
            sound.transform.parent = soundsParent;
            sound.AddComponent<AudioSource>();
            sound.GetComponent<AudioSource>().clip = soundList[id];
            sound.GetComponent<AudioSource>().Play();
            Destroy(sound, sound.GetComponent<AudioSource>().clip.length);
        }
    }
}