using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class ItemUIManager : MonoBehaviour
    {
        public static ItemUIManager instance;

        public Item[] items;
        public Item[] targetItem = new Item[4];

        private void Start()
        {
            instance = this;
        }

        public void Update()
        {
            int tempKey = 5;
            if (Input.GetKeyDown(KeyCode.Alpha1))
                tempKey = 0;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                tempKey = 1;
            if (Input.GetKeyDown(KeyCode.Alpha3))
                tempKey = 2;
            if (Input.GetKeyDown(KeyCode.Alpha4))
                tempKey = 3;

            if (tempKey != 5)
            {
                if (targetItem[tempKey] != null)
                {
                    targetItem[tempKey].Use();
                    Change(tempKey);
                }
            }
        }

        public void SetItem(int n, Item item)
        {
            if (item.isEmpty)
                return;

            items[n].isEmpty = false;
            items[n].itemImage.sprite = item.itemImage.sprite;
            items[n].itemImage.color = Color.white;
            items[n].nitemCount = item.nitemCount;
            items[n].itemImage.gameObject.SetActive(true);
            items[n].itemCount.gameObject.SetActive(true);

            targetItem[n] = item;
        }
        
        public Item GetItem(int n)
        {
            return items[n];
        }

        public void Change()
        {
            for (int i = 0; i < 4; i++)
                Change(i);
        }

        public void Change(int n)
        {
            if (targetItem[n] != null)
            {
                if (targetItem[n].isEmpty)
                {
                    items[n].isEmpty = true;
                    items[n].itemImage.gameObject.SetActive(false);
                    items[n].itemCount.gameObject.SetActive(false);
                }
            }
        }
    }
}