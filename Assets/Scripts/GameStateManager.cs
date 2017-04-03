using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chasa
{
    public class GameStateManager : MonoBehaviour
    {
        public Text gameState;
        public Text gameComment;

        public void Win()
        {
            SceneManager.instance.poolObjects.HideObject();
            gameState.transform.parent.gameObject.SetActive(true);
            gameState.text = "영혼 수거 완료";
            gameComment.text = "윤준서의 영혼을 저승으로 보냈습니다.";
            StartCoroutine("SceneChange", true);
        }

        public void Lose()
        {
            SceneManager.instance.poolObjects.HideObject();
            gameState.transform.parent.gameObject.SetActive(true);
            gameState.text = "사망";
            gameComment.text = "영혼 수거를 위해 다시 시작합니다.";
            StartCoroutine("SceneChange", false);
        }

        IEnumerator SceneChange(bool isClear)
        {
            SceneManager.instance.HideCharacterSoul();

            yield return new WaitForSeconds(4.0f);

            if (isClear)
            {
                LoadingManager.instance.LoadScene("Home");
            }
            else
            {
                LoadingManager.instance.ReLoad();
            }

            gameState.transform.parent.gameObject.SetActive(false);
        }
    }
}