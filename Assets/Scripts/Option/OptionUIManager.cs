using UnityEngine;
using UnityEngine.UI;

namespace Chasa
{
    public class OptionUIManager : MonoBehaviour
    {
        public GameObject OptionObject;
        public GameObject m_Option;
        public GameObject m_Mission;

        [HideInInspector]
        public bool isActive = false;

        private bool isOption = false;
        private bool isMission = false;

        // 다른 매니저에서 관리
        // Inventory, Tutorial
        [HideInInspector]
        public bool isDontOpen = false;

        public Dropdown screenSize;

        private void Update()
        {
            if (SceneManager.instance.isOnotherScene)
                return;
            if (isDontOpen)
                return;
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isActive)
                {
                    isActive = true;
                    Time.timeScale = 0.0f;
                    OptionObject.SetActive(true);
                    SceneManager.instance.ShowCursor();
                }
                else
                {
                    if (!isOption && !isMission)
                    {
                        GameResume();
                    }
                    else if (isOption)
                    {
                        GameOptionOff();
                    }
                    else if (isMission)
                    {
                        GameMissionOff();
                    }
                }
            }
        }

        public void GameResume()
        {
            isActive = false;

            SceneManager.instance.HideCursor();

            Time.timeScale = 1.0f;

            OptionObject.SetActive(false);

            isOption = false;
            isMission = false;
        }

        public void GameExit()
        {
            Time.timeScale = 1.0f;
            isActive = false;
            SceneManager.instance.HideCursor();
            OptionObject.SetActive(false);
            isOption = false;
            isMission = false;
            LoadingManager.instance.LoadScene("Home");
        }

        public void GameOption()
        {
            OptionObject.SetActive(false);
            m_Option.SetActive(true);
            isOption = true;
            isMission = false;
        }
        public void GameOptionOff()
        {
            OptionObject.SetActive(true);
            m_Option.SetActive(false);
            isOption = false;
            isMission = false;
        }

        public void GameMission()
        {
            OptionObject.SetActive(false);
            m_Mission.SetActive(true);
            isOption = false;
            isMission = true;
        }
        public void GameMissionOff()
        {
            OptionObject.SetActive(true);
            m_Mission.SetActive(false);
            isOption = false;
            isMission = false;
        }

        // Function
        public void FullScreen()
        {
            if (Screen.fullScreen)
                Screen.fullScreen = false;
            else
                Screen.fullScreen = true;
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
        }
    }
}