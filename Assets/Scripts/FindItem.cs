using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chasa
{
    public class FindItem : MonoBehaviour
    {
        private bool item_active = false;

        [HideInInspector] public MissionRequest mission_request;

        public string item_name;
        
        private void Awake()
        {
            mission_request = GetComponent<MissionRequest>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                item_active = true;
                SceneManager.instance.pressF.gameObject.SetActive(true);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                item_active = false;
                SceneManager.instance.pressF.gameObject.SetActive(false);
            }
        }

        public void Update()
        {
            if (item_active)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    mission_request.Request();

                    SceneManager.instance.getItemText.text = "[" + item_name + "]" + "\n획득";

                    StartCoroutine("GetItemUpdate");
                }
            }
        }

        public IEnumerator GetItemUpdate()
        {
            SceneManager.instance.pressF.SetActive(false);
            SceneManager.instance.getItemText.transform.parent.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            SceneManager.instance.getItemText.transform.parent.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
