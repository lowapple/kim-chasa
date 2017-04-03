using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class SoundPlayer : MonoBehaviour
    {
        public void PlayEffect(string soundName)
        {
            SceneManager.instance.soundManager.PlayEffect(soundName);
        }
        public void PlayBGM(string soundName)
        {
            SceneManager.instance.soundManager.PlayBGM(soundName);
        }
    }
}