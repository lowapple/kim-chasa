using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chasa
{
    public class Item : MonoBehaviour
    {
        private Camera uiCamera;
        private ChasaPlayerUnit chasaUnit;

        [HideInInspector]
        public string itemName;

        [HideInInspector]
        public Item uiItem = null;
        public Image itemImage;
        public Text itemCount;
        public int nitemCount;

        // 비어있는지 여부
        public bool isEmpty = false;
        public bool isUIItem = false;

        [HideInInspector]
        public int itemIdx = 0;

        public InventoryType itemType;

        private void Start()
        {
            uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
            chasaUnit = GameObject.Find("Player").GetComponent<ChasaPlayerUnit>();
            
            if (isEmpty || nitemCount <= 0)
            {
                isEmpty = true;
                itemImage.gameObject.SetActive(false);
                itemCount.gameObject.SetActive(false);
            }
        }

        void Update()
        {
            if (isUIItem)
                return;

            Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity);

            if (hit.collider != null && hit.transform == transform)
            {
                if (Input.GetMouseButtonDown(1))
                    Use();
                KeyUpdate();
            }
        }

        public void KeyUpdate()
        {
            if (SceneManager.instance.itemManager.inventoryType == InventoryType.SKIN)
                return;

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ItemUIManager.instance.SetItem(0, this);
                uiItem = ItemUIManager.instance.GetItem(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ItemUIManager.instance.SetItem(1, this);
                uiItem = ItemUIManager.instance.GetItem(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ItemUIManager.instance.SetItem(2, this);
                uiItem = ItemUIManager.instance.GetItem(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ItemUIManager.instance.SetItem(3, this);
                uiItem = ItemUIManager.instance.GetItem(3);
            }
        }

        // 아이템을 사용한다.
        public void Use()
        {
            switch (itemType)
            {
                case InventoryType.TOOL:
                    // 개수가 있다.
                    if (nitemCount > 0)
                    {
                        SceneManager.instance.soundManager.PlayEffect("Potion");

                        nitemCount -= 1;
                        // UI에 장착된것도 같이 깎음
                        if (uiItem != null)
                            uiItem.nitemCount -= 1;

                        var item = ItemPoolManager.itemDictionary[itemName];
                        switch (item.item_type)
                        {
                            case ItemPoolManager.ItemType.HEALTH:
                                chasaUnit.health += item.plus_state;
                                break;
                            case ItemPoolManager.ItemType.STEMINA:
                                chasaUnit.stemina += item.plus_state;
                                break;
                            case ItemPoolManager.ItemType.POWER:
                                chasaUnit.power += (int)item.plus_state;
                                break;
                            case ItemPoolManager.ItemType.DEFENCE:
                                chasaUnit.defence += (int)item.plus_state;
                                break;
                        }

                        if (nitemCount <= 0)
                        {
                            isEmpty = true;
                            itemImage.gameObject.SetActive(false);
                            itemCount.gameObject.SetActive(false);
                            ItemUIManager.instance.Change();
                            uiItem = null;
                            itemName = "";
                        }
                    }
                    break;
                case InventoryType.SKIN:
                    if (nitemCount > 0)
                    {
                        var item = ItemPoolManager.skinDictionary[itemName];
                        if (item != null)
                        {
                            if (item.skin_type == ItemPoolManager.SkinType.WEAPON)
                            {
                                WeaponManager.instance.WeaponActive(itemName);
                                SceneManager.instance.soundManager.PlayEffect("Metal");
                            }
                            else
                            {
                                if (item.cloth_type == ItemPoolManager.ClothType.Hat)
                                    ClothManager.instance.ChangeHat(item.item_name);
                                else
                                    ClothManager.instance.ChangeCloth(item.item_name);

                                SceneManager.instance.soundManager.PlayEffect("Inventory");
                            }
                        }
                    }
                    break;
            }
        }

        private void LateUpdate()
        {
            if (itemCount != null)
                itemCount.text = nitemCount.ToString();
        }
    }
}