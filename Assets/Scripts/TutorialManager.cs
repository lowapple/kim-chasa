using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chasa
{
    public class TutorialManager : MonoBehaviour
    {
        [System.Serializable]
        public class BasicTutorial
        {
            public string tutorialTitle;
            public Sprite tutorialImage;
            public string[] tutorialComments;
        }

        [System.Serializable]
        public class Tutorial
        {
            [HideInInspector]
            public int currentIdx;

            public string tutorialTitle;
            public BasicTutorial[] tutorials;

            public bool isShow = false;
        }

        public Tutorial[] tutorialList;
        public Dictionary<string, Tutorial> tutorials;

        public Text rightKey;
        public Text leftKey;


        [SerializeField]
        private GameObject tutorialUI;
        [SerializeField]
        private Text tutorialTitle;
        [SerializeField]
        private Text tutorialComment;
        [SerializeField]
        private Image tutorialImage;

        private Tutorial currentTutorial;

        public bool isActive = false;

        public void Awake()
        {
            tutorials = new Dictionary<string, Tutorial>();
            foreach (var tutorial in tutorialList)
                tutorials.Add(tutorial.tutorialTitle, tutorial);
        }

        private void Start()
        {
            tutorialUI.gameObject.SetActive(false);
        }

        public void ShowTutorial(string title)
        {
            tutorialUI.gameObject.SetActive(true);

            if (tutorials.ContainsKey(title))
            {
                currentTutorial = tutorials[title];
                if (currentTutorial.isShow)
                {
                    currentTutorial = null;
                    tutorialUI.gameObject.SetActive(false);
                    return;
                }

                isActive = true;

                currentTutorial.isShow = true;
                currentTutorial.currentIdx = 0;
                tutorialUI.SetActive(true);

                leftKey.gameObject.SetActive(false);
                rightKey.gameObject.SetActive(false);

                if (currentTutorial.tutorials.Length > 1)
                    rightKey.gameObject.SetActive(true);

                tutorialTitle.text = currentTutorial.tutorials[currentTutorial.currentIdx].tutorialTitle;
                tutorialComment.text = "";
                for (int i = 0; i < currentTutorial.tutorials[currentTutorial.currentIdx].tutorialComments.Length; i++)
                {
                    if (i != 0)
                        tutorialComment.text += "\n";
                    tutorialComment.text += currentTutorial.tutorials[currentTutorial.currentIdx].tutorialComments[i];
                }
                tutorialImage.sprite = currentTutorial.tutorials[currentTutorial.currentIdx].tutorialImage;

                SceneManager.instance.optionUIManager.isDontOpen = true;
                SceneManager.instance.ShowCursor();
            }
        }

        public void Right()
        {
            if (currentTutorial.currentIdx < currentTutorial.tutorials.Length - 1)
            {
                currentTutorial.currentIdx += 1;
                if (currentTutorial.currentIdx >= currentTutorial.tutorials.Length - 1)
                    rightKey.gameObject.SetActive(false);

                tutorialTitle.text = currentTutorial.tutorials[currentTutorial.currentIdx].tutorialTitle;
                tutorialComment.text = "";
                for (int i = 0; i < currentTutorial.tutorials[currentTutorial.currentIdx].tutorialComments.Length; i++)
                {
                    if (i != 0)
                        tutorialComment.text += "\n";
                    tutorialComment.text += currentTutorial.tutorials[currentTutorial.currentIdx].tutorialComments[i];
                }
                tutorialImage.sprite = currentTutorial.tutorials[currentTutorial.currentIdx].tutorialImage;

                leftKey.gameObject.SetActive(true);
            }
            else
                rightKey.gameObject.SetActive(false);
        }

        public void Left()
        {
            if (currentTutorial.currentIdx > 0)
            {
                currentTutorial.currentIdx -= 1;
                if (currentTutorial.currentIdx <= 0)
                    leftKey.gameObject.SetActive(false);

                tutorialTitle.text = currentTutorial.tutorials[currentTutorial.currentIdx].tutorialTitle;
                tutorialComment.text = "";
                for (int i = 0; i < currentTutorial.tutorials[currentTutorial.currentIdx].tutorialComments.Length; i++)
                {
                    if (i != 0)
                        tutorialComment.text += "\n";
                    tutorialComment.text += currentTutorial.tutorials[currentTutorial.currentIdx].tutorialComments[i];
                }
                tutorialImage.sprite = currentTutorial.tutorials[currentTutorial.currentIdx].tutorialImage;

                rightKey.gameObject.SetActive(true);
            }
            else
                leftKey.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isActive)
                {
                    isActive = false;
                    tutorialUI.gameObject.SetActive(false);
                    SceneManager.instance.HideCursor();
                }
            }
        }

        private void FixedUpdate()
        {
            SceneManager.instance.optionUIManager.isDontOpen = isActive;
        }
    }
}