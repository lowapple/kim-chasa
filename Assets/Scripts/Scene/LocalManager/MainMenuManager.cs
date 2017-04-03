using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chasa
{
    public class MainMenuManager : MonoBehaviour
    {
        public GameObject MainMenuCamera;
        public GameObject MainMenuCanvas;

        public GameObject OptionUI;
        public GameObject MenuUI;
        public GameObject ChasaKim;

        public Slider bgmSlider;
        public Slider effectSlider;

        public bool isStart = false;

        public void Start()
        {
            SceneManager.instance.character.chasaCombat.enabled = false;
            SceneManager.instance.character.chasaControl.enabled = false;
            SceneManager.instance.character.chasaCharacter.enabled = false;
            SceneManager.instance.soundManager.PlayBGM("Background1");
            SceneManager.instance.ShowCursor();
            SceneManager.instance.HideCharacterSoul();

            OptionUI.gameObject.SetActive(false);
            MenuUI.gameObject.SetActive(true);
            ChasaKim.gameObject.SetActive(true);

            SceneManager.instance.isOnotherScene = true;
        }

        public void NewGame()
        {
            SceneManager.instance.soundManager.PlayEffect("Button");
            SceneManager.instance.HideCursor();
            SceneManager.instance.playerSoul.gameObject.SetActive(true);

            MainMenuCamera.gameObject.SetActive(false);
            OptionUI.gameObject.SetActive(false);
            MenuUI.gameObject.SetActive(false);
            ChasaKim.gameObject.SetActive(false);

            SceneManager.instance.character.chasaCombat.enabled = true;
            SceneManager.instance.character.chasaControl.enabled = true;
            SceneManager.instance.character.chasaCharacter.enabled = true;
            SceneManager.instance.ShowCharacterSoul();

            SceneManager.instance.isOnotherScene = false;

            SceneManager.instance.tutorialManager.ShowTutorial("메인");

            isStart = true;
        }

        public void Option()
        {
            SceneManager.instance.soundManager.PlayEffect("Button");

            MenuUI.gameObject.SetActive(false);
            OptionUI.gameObject.SetActive(true);
        }

        public void Exit()
        {
            SceneManager.instance.soundManager.PlayEffect("Button");
            Application.Quit();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (OptionUI.activeSelf)
                {
                    OptionUI.SetActive(false);
                    MenuUI.SetActive(true);
                }
            }
        }

        private void LateUpdate()
        {
            if (!isStart)
                SceneManager.instance.HideCharacterSoul();

            if (OptionUI.activeSelf)
            {
                SceneManager.instance.bgmSlider.value = bgmSlider.value;
                SceneManager.instance.effectSlider.value = effectSlider.value;
            }
        }

        public void ScreenSize(Dropdown drop)
        {
            switch (drop.value)
            {
                case 0:
                    Screen.SetResolution(1920, 1080, Screen.fullScreen);
                    break;
                case 1:
                    Screen.SetResolution(1280, 720, Screen.fullScreen);
                    break;
                case 2:
                    Screen.SetResolution(640, 360, Screen.fullScreen);
                    break;
            }

            SceneManager.instance.optionUIManager.screenSize.value = drop.value;
        }
    }
}