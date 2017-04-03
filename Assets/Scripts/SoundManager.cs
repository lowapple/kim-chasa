using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class SoundManager : MonoBehaviour
    {
        [System.Serializable]
        public class Sound
        {
            public string name;
            public AudioClip soundClip;
        }
        [HideInInspector]
        public AudioSource bgmAudio;
        [HideInInspector]
        public AudioSource effectAudio;

        public Sound[] sounds;
        public Dictionary<string, AudioClip> soundsDictionary;

        public void Awake()
        {
            soundsDictionary = new Dictionary<string, AudioClip>();
            bgmAudio = gameObject.AddComponent<AudioSource>();
            effectAudio = gameObject.AddComponent<AudioSource>();
            for (int i = 0; i < sounds.Length; i++)
                soundsDictionary.Add(sounds[i].name, sounds[i].soundClip);
        }

        public void PlayEffect(string soundName, float volume = 1.0f)
        {
            if (soundsDictionary.ContainsKey(soundName))
                effectAudio.PlayOneShot(soundsDictionary[soundName], volume * SceneManager.instance.effectScale);
        }

        public void PlayBGM(string soundName)
        {
            bgmAudio.clip = soundsDictionary[soundName];
            bgmAudio.loop = true;
            bgmAudio.Play();
        }

        private void LateUpdate()
        {
            bgmAudio.volume = 0.3f * SceneManager.instance.bgmScale;
        }
    }
}