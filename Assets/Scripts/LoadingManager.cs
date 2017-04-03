using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class LoadingManager : MonoBehaviour
    {
        public static LoadingManager instance;
        public static string loadScene;

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(this);
                return;
            }

            instance = this;

            DontDestroyOnLoad(this);
        }

        private void OnApplicationQuit()
        {
            instance = null;
        }

        public void LoadScene(string scene)
        {
            loadScene = scene;

            SceneManager.instance.optionUIManager.enabled = false;
            SceneManager.instance.pressF.SetActive(false);
            SceneManager.instance.character.chasaCharacter.m_Rigidbody.useGravity = false;
            SceneManager.instance.character.chasaCharacter.m_Rigidbody.isKinematic = true;
            SceneManager.instance.character.chasaCharacter.enabled = false;
            SceneManager.instance.character.chasaCombat.enabled = false;
            SceneManager.instance.character.chasaControl.enabled = false;
            SceneManager.instance.character.enabled = false;
            SceneManager.instance.HideCharacterSoul();
            SceneManager.instance.poolObjects.HideObject();

            UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");

            StartCoroutine("SceneLoadUpdate");
        }

        public void ReLoad()
        {
            LoadScene(loadScene);
            SceneManager.instance.character.Alive();
        }

        IEnumerator SceneLoadUpdate()
        {
            yield return new WaitForSeconds(3.0f);

            AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(loadScene);

            while (!async.isDone)
            {
                yield return null;
            }

            SceneManager.instance.optionUIManager.enabled = true;
            SceneManager.instance.pressF.SetActive(false);
            SceneManager.instance.character.chasaCharacter.m_Rigidbody.useGravity = true;
            SceneManager.instance.character.chasaCharacter.m_Rigidbody.isKinematic = false;
            SceneManager.instance.character.chasaCharacter.enabled = true;
            SceneManager.instance.character.chasaCombat.enabled = true;
            SceneManager.instance.character.chasaControl.enabled = true;
            SceneManager.instance.character.enabled = true;
            SceneManager.instance.character.chasaCharacter.m_Animator.Play("Alive");
            SceneManager.instance.ShowCharacterSoul();
        }
    }
}