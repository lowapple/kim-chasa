using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chasa
{
    public class MissionDoor : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                SceneManager.instance.pressF.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                SceneManager.instance.pressF.SetActive(false);
            }
        }

        public void Update()
        {
            if (SceneManager.instance.pressF.activeSelf)
            {
                if(Input.GetKeyDown(KeyCode.F))
                    LoadingManager.instance.LoadScene("BlueCityGeoje");
            }
        }
    }
}