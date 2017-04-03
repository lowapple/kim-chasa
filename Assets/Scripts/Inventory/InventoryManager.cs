using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class InventoryManager : MonoBehaviour
    {
        public GameObject m_ItemInventory;
        public GameObject m_CharacterInventory;

        public KeyCode key_Item = KeyCode.I;
        public KeyCode key_Character = KeyCode.C;

        private void Start()
        {
            m_CharacterInventory.SetActive(false);
            m_ItemInventory.SetActive(false);
        }

        [HideInInspector]
        public bool isItemOpen = false;
        [HideInInspector]
        public bool isCharacterOpen = false;

        private void Update()
        {
            if (Input.GetKeyDown(key_Item))
            {
                SceneManager.instance.optionUIManager.isDontOpen = true;

                if (isItemOpen)
                    ItemClose();
                else
                    ItemOpen();
            }

            if (Input.GetKeyDown(key_Character))
            {
                SceneManager.instance.optionUIManager.isDontOpen = true;

                if (isCharacterOpen)
                    CharacterClose();
                else
                    CharacterOpen();
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if (isItemOpen)
                    ItemClose();
                if (isCharacterOpen)
                    CharacterClose();
            }
        }

        private void CharacterOpen()
        {
            SceneManager.instance.soundManager.PlayEffect("Inventory");
            SceneManager.instance.ShowCursor();
            m_CharacterInventory.SetActive(true);
            m_CharacterInventory.GetComponent<Animator>().Play("CO");
            isCharacterOpen = true;
        }
        private void CharacterClose()
        {
            SceneManager.instance.soundManager.PlayEffect("Inventory");
            if (!isItemOpen)
                SceneManager.instance.HideCursor();
            m_CharacterInventory.GetComponent<Animator>().Play("CC");
            isCharacterOpen = false;
        }
        private void ItemOpen()
        {
            SceneManager.instance.soundManager.PlayEffect("Inventory");
            SceneManager.instance.ShowCursor();
            m_ItemInventory.SetActive(true);
            m_ItemInventory.GetComponent<Animator>().Play("IO");
            isItemOpen = true;
        }
        private void ItemClose()
        {
            SceneManager.instance.soundManager.PlayEffect("Inventory");
            if (!isCharacterOpen)
                SceneManager.instance.HideCursor();
            m_ItemInventory.GetComponent<Animator>().Play("IC");
            isItemOpen = false;
        }

        private void LateUpdate()
        {
            if (isCharacterOpen || isItemOpen)
                SceneManager.instance.optionUIManager.isDontOpen = true;
            else
                SceneManager.instance.optionUIManager.isDontOpen = false;
        }
    }
}