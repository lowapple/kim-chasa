using UnityEngine;
using Chasa;

namespace Chasa
{
    public class ChasaSoulChatControl : MonoBehaviour
    {
        private bool inSideSoul = false;
        private SoulChat soulChat;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Soul"))
                inSideSoul = true;
            else
                return;

            SceneManager.instance.pressF.SetActive(true);

            soulChat = other.transform.GetComponent<SoulChat>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("Soul"))
                inSideSoul = false;
            else
                return;

            soulChat = null;

            SceneManager.instance.pressF.SetActive(false);

            if (SceneManager.instance.soulChatManager != null)
            {
                SceneManager.instance.soulChatManager.ChatSelectionEnd();
                SceneManager.instance.soulChatManager.ChatEnd();
            }
        }

        private void Update()
        {
            if (inSideSoul)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    SceneManager.instance.soulChatManager.ChatStart(soulChat);
                    SceneManager.instance.pressF.SetActive(false);
                }
            }
        }
    }
}