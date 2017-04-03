using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class BlueCityGeojeSceneManager : MonoBehaviour
    {
        public string background_music_name;

        private void Start()
        {
            SceneManager.instance.playerSoul.gameObject.SetActive(true);
            SceneManager.instance.soundManager.PlayBGM(background_music_name);

            SceneManager.instance.tutorialManager.ShowTutorial("윤준서");
        }
    }
}