using UnityEngine;
using System.Collections.Generic;
namespace Chasa
{
    public class ItemManager : MonoBehaviour
    {
        private Camera uiCamera;

        // 현재 이동 아이템
        [SerializeField]
        private Item tempItem;

        private Item currentItem;

        public GameObject tool;
        public GameObject weapon;

        [HideInInspector]
        public List<Item> toolItems;

        [HideInInspector]
        public List<Item> weaponItems;

        public InventoryType inventoryType;

        public void Awake()
        {
            uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
            tempItem.gameObject.SetActive(false);

            this.toolItems = new List<Item>();
            this.weaponItems = new List<Item>();

            tool.SetActive(true);
            var toolItems = tool.GetComponentsInChildren<Item>();
            for (int i = 0; i < toolItems.Length; i++)
            {
                toolItems[i].itemIdx = i;
                this.toolItems.Add(toolItems[i]);
            }
            weapon.SetActive(false);
            var weaponItems = weapon.GetComponentsInChildren<Item>();
            for (int i = 0; i < weaponItems.Length; i++)
            {
                weaponItems[i].itemIdx = i;
                this.weaponItems.Add(weaponItems[i]);
            }

            // ------
            // ToolBoxClick();
        }

        private void Start()
        {
            AddItem("Health");
            AddItem("Stemina");
            AddItem("Power");
            AddItem("Defence");

            AddSKin("Fan");

            // AddSKin("Laser_O");
            // AddSKin("Laser_G");
            // AddSKin("Laser_P");
            // AddSKin("FireSword");
            // 
            // AddSKin("WhiteHat");
            // AddSKin("WhiteBody");
            // AddSKin("GJWHat");
            // AddSKin("GJWBOdy");
        }

        public void AddItem(string itemName)
        {
            if (ItemPoolManager.itemDictionary.ContainsKey(itemName))
            {
                var item = ItemPoolManager.itemDictionary[itemName];
                if (item != null)
                {
                    bool isItem = false;
                    toolItems.ForEach(p =>
                    {
                        if (p.itemName.CompareTo(item.item_name) == 0)
                        {
                            isItem = true;
                            p.nitemCount += 1;
                            p.itemType = InventoryType.TOOL;
                        }
                    });

                    if (!isItem)
                    {
                        toolItems.ForEach(p =>
                        {
                            if (!isItem)
                            {
                                if (p.isEmpty)
                                {
                                    isItem = true;
                                    p.isEmpty = false;
                                    p.itemImage.sprite = item.item_image;
                                    p.itemImage.gameObject.SetActive(true);
                                    p.itemName = item.item_name;
                                    p.itemCount.gameObject.SetActive(true);
                                    p.nitemCount = 1;
                                    p.itemType = InventoryType.TOOL;
                                }
                            }
                        });
                    }
                }
            }
        }

        public void AddSKin(string weaponName)
        {
            if (ItemPoolManager.skinDictionary.ContainsKey(weaponName))
            {
                var item = ItemPoolManager.skinDictionary[weaponName];
                if (item != null)
                {
                    bool isItem = false;
                    for (int i = 0; i < weaponItems.Count; i++)
                    {
                        if(weaponItems[i].itemName.CompareTo(item.item_name) == 0)
                        {
                            isItem = true;
                            weaponItems[i].nitemCount += 1;
                            weaponItems[i].itemType = InventoryType.SKIN;
                        }
                    }

                    if (!isItem)
                    {
                        for(int i = 0; i < weaponItems.Count; i++)
                        {
                            if (weaponItems[i].isEmpty)
                            {
                                weaponItems[i].isEmpty = false;
                                weaponItems[i].itemImage.sprite = item.item_image;
                                weaponItems[i].itemImage.gameObject.SetActive(true);
                                weaponItems[i].itemName = item.item_name;
                                weaponItems[i].itemCount.gameObject.SetActive(true);
                                weaponItems[i].nitemCount = 1;
                                weaponItems[i].itemType = InventoryType.SKIN;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void ToolBoxClick()
        {
            SceneManager.instance.soundManager.PlayEffect("Button");
            tool.SetActive(true);
            weapon.SetActive(false);
            inventoryType = InventoryType.TOOL;
        }

        public void WeaponBoxClick()
        {
            SceneManager.instance.soundManager.PlayEffect("Button");
            tool.SetActive(false);
            weapon.SetActive(true);
            inventoryType = InventoryType.SKIN;
        }

        private void Update()
        {
            Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Vector3 pos = Input.mousePosition;
            pos.x -= Screen.width / 2;
            pos.y -= Screen.height / 2;
            pos.z = 0f;
            tempItem.transform.localPosition = pos;

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.transform != null)
                    {
                        currentItem = hit.transform.GetComponent<Item>();

                        if (currentItem != null)
                        {
                            if (!currentItem.isEmpty)
                            {
                                tempItem.gameObject.SetActive(true);
                                tempItem.itemImage.sprite = currentItem.itemImage.sprite;
                                tempItem.nitemCount = currentItem.nitemCount;
                                tempItem.itemName = currentItem.itemName;

                                currentItem.itemImage.gameObject.SetActive(false);
                                currentItem.itemCount.gameObject.SetActive(false);
                            }
                            else
                            {
                                currentItem = null;
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                Physics.Raycast(ray, out hit, Mathf.Infinity);

                if (hit.collider != null)
                {
                    // 이동에 성공하면 이미지와 숫자를 초기화 한다.
                    if (currentItem != null)
                    {
                        Item changeItem = hit.transform.GetComponent<Item>();

                        if (changeItem.isEmpty)
                        {
                            currentItem.itemImage.color = new Color(1, 1, 1, 0);
                            currentItem.isEmpty = true;
                            currentItem.itemImage.gameObject.SetActive(false);
                            currentItem.itemCount.gameObject.SetActive(false);
                            //
                            changeItem.itemImage.gameObject.SetActive(true);
                            changeItem.itemCount.gameObject.SetActive(true);
                            changeItem.isEmpty = false;
                            changeItem.itemImage.sprite = tempItem.itemImage.sprite;
                            changeItem.itemImage.color = new Color(1, 1, 1, 1);
                            changeItem.nitemCount = tempItem.nitemCount;
                            changeItem.itemName = tempItem.itemName;
                        }
                        else
                        {
                            tempItem.itemImage.sprite = changeItem.itemImage.sprite;
                            tempItem.nitemCount = changeItem.nitemCount;
                            tempItem.itemName = changeItem.itemName;

                            changeItem.itemImage.sprite = currentItem.itemImage.sprite;
                            changeItem.itemImage.color = new Color(1, 1, 1, 1);
                            changeItem.itemImage.gameObject.SetActive(true);
                            changeItem.itemCount.gameObject.SetActive(true);
                            changeItem.nitemCount = currentItem.nitemCount;
                            changeItem.itemName = currentItem.itemName;
                            //
                            currentItem.itemImage.sprite = tempItem.itemImage.sprite;
                            currentItem.itemImage.color = new Color(1, 1, 1, 1);
                            currentItem.itemImage.gameObject.SetActive(true);
                            currentItem.itemCount.gameObject.SetActive(true);
                            currentItem.nitemCount = tempItem.nitemCount;
                            currentItem.itemName = tempItem.itemName;
                        }
                    }
                }
                else
                {
                    if (currentItem != null)
                    {
                        currentItem.itemImage.gameObject.SetActive(true);
                        currentItem.itemCount.gameObject.SetActive(true);
                    }
                }

                currentItem = null;
                tempItem.gameObject.SetActive(false);
            }
        }
    }
}