using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace Chasa
{
    public class StoreManager : MonoBehaviour
    {
        private Vector3 original_camera_position;
        public static StoreManager instnace;
        public Camera store_camera;

        public enum StoreType
        {
            Item,
            Weapon,
            Cloth,
            None
        }

        #region ITEM
        public Transform item_camera_position;
        private bool item_visible = false;
        private bool item_purchase_visible = false;
        private string[] item_status = { "체력 : ", "스테미나 : ", "공격력 : ", "방어력 : " };
        private int n_buy = 0;
        private int n_cost_sum = 0;
        #endregion

        #region WEAPON
        public Transform weapon_camera_position;
        private bool weapon_purchase_visible = false;
        private bool weapon_visible = false;
        #endregion

        #region CLOTH
        public Transform cloth_camera_position;
        private bool cloth_purchase_visible = false;
        private bool cloth_visible = false;
        #endregion

        // 나중에 BaseStore로 상속형태로 만들기
        private StoreItem current_item;
        private StoreWeapon current_weapon;
        private StoreCloth current_cloth;

        public List<StoreItem> store_items;
        public List<StoreWeapon> store_weapons;
        public List<StoreCloth> store_clothes;

        // =====================

        public bool is_shop_open;

        private StoreType currentStoreType;
        private StoreType tempStoreType;

        private bool is_shop_open_ready = false;

        private void Awake()
        {
            instnace = this;
        }

        private void Start()
        {
            SceneManager.instance.storeUI.item_canvas.SetActive(false);
            SceneManager.instance.storeUI.item_notification.transform.parent.gameObject.SetActive(false);
            SceneManager.instance.storeUI.weapon_canvas.SetActive(false);
            SceneManager.instance.storeUI.weapon_purchase_name.transform.parent.gameObject.SetActive(false);
            SceneManager.instance.storeUI.cloth_canvas.SetActive(false);
            SceneManager.instance.storeUI.cloth_purchase_name.transform.parent.gameObject.SetActive(false);
            SceneManager.instance.storeUI.key.gameObject.SetActive(false);
            SceneManager.instance.storeUI.storeKey.gameObject.SetActive(true);
            SceneManager.instance.storeUI.itemKey.gameObject.SetActive(false);

            store_camera.gameObject.SetActive(false);

            var items = GetComponentsInChildren<StoreItem>();
            for (int i = 0; i < items.Length; i++)
            {
                items[i].item_num = i;
                store_items.Add(items[i]);
            }

            var weapons = GetComponentsInChildren<StoreWeapon>();
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].item_num = i;
                store_weapons.Add(weapons[i]);
            }

            var clothes = GetComponentsInChildren<StoreCloth>();
            for (int i = 0; i < clothes.Length; i++)
            {
                clothes[i].item_num = i;
                store_clothes.Add(clothes[i]);
            }

            original_camera_position = store_camera.transform.position;

            currentStoreType = StoreType.None;
            tempStoreType = StoreType.Item;

            CountReset();
            CostCalculator();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (is_shop_open_ready)
                {
                    if (!is_shop_open)
                    {
                        is_shop_open = true;
                        store_camera.gameObject.SetActive(true);
                        SceneManager.instance.ShowCursor();
                        SceneManager.instance.others.SetActive(false);
                        SceneManager.instance.character.chasaCharacter.Stop();
                        SceneManager.instance.character.chasaControl.enabled = false;
                        SceneManager.instance.character.chasaCombat.enabled = false;
                        SceneManager.instance.storeUI.key.gameObject.SetActive(true);
                        CountReset();
                    }
                }
            }

            if (is_shop_open)
            {
                try
                {
                    // 오른쪽
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        RightKey();
                    }

                    // 왼쪽
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        LeftKey();
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (currentStoreType == StoreType.None)
                            currentStoreType = tempStoreType;
                        
                        if(currentStoreType != StoreType.None)
                        {
                            SceneManager.instance.storeUI.key.gameObject.SetActive(true);
                            SceneManager.instance.storeUI.storeKey.gameObject.SetActive(false);
                            SceneManager.instance.storeUI.itemKey.gameObject.SetActive(true);
                        }

                        switch (currentStoreType)
                        {
                            case StoreType.Item:
                                {
                                    if (item_visible)
                                    {
                                        if (!item_purchase_visible)
                                        {
                                            item_purchase_visible = true;
                                            BuyItem();
                                        }
                                        else
                                            OK();
                                    }
                                    else
                                    {
                                        RightKey();
                                    }
                                }
                                break;
                            case StoreType.Weapon:
                                {
                                    if (weapon_visible)
                                    {
                                        if (!weapon_purchase_visible)
                                        {
                                            weapon_purchase_visible = true; BuyItem();
                                        }
                                        else
                                            OK();
                                    }
                                    else
                                    {
                                        RightKey();
                                    }
                                }
                                break;
                            case StoreType.Cloth:
                                {
                                    if (cloth_visible)
                                    {
                                        if (!cloth_purchase_visible)
                                        {
                                            cloth_purchase_visible = true;
                                            BuyItem();
                                        }
                                        else
                                            OK();
                                    }
                                    else
                                    {
                                        RightKey();
                                    }
                                }
                                break;
                        }
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        RaycastHit hit;
                        Physics.Raycast(store_camera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity);
                        if (hit.transform != null)
                        {
                            if (hit.transform.CompareTag("Item"))
                            {
                                switch (currentStoreType)
                                {
                                    case StoreType.Item:
                                        {
                                            #region ITEM
                                            var storeItem = hit.transform.GetComponent<StoreItem>();
                                            MoveCamera();
                                            if (item_visible)
                                            {
                                                if (current_item != null)
                                                {
                                                    if (current_item.transform == storeItem.transform)
                                                    {
                                                        if (!item_purchase_visible)
                                                            item_purchase_visible = true;
                                                    }
                                                    else
                                                    {
                                                        current_item = storeItem;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                item_visible = true;
                                                current_item = storeItem;
                                            }
                                            #endregion
                                        }
                                        break;
                                    case StoreType.Weapon:
                                        {
                                            #region WEAPON
                                            var storeWeapon = hit.transform.GetComponent<StoreWeapon>();
                                            MoveCamera();
                                            if (weapon_visible)
                                            {
                                                if (storeWeapon != null)
                                                {
                                                    if (current_weapon.transform == storeWeapon.transform)
                                                    {
                                                        if (!weapon_purchase_visible)
                                                            weapon_purchase_visible = true;
                                                    }
                                                    else
                                                    {
                                                        current_weapon = storeWeapon;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                weapon_visible = true;
                                                current_weapon = storeWeapon;
                                            }
                                            #endregion
                                        }
                                        break;
                                    case StoreType.Cloth:
                                        {
                                            #region CLOTH
                                            var storeCloth = hit.transform.GetComponent<StoreCloth>();
                                            MoveCamera();
                                            if (cloth_visible)
                                            {
                                                if (storeCloth != null)
                                                {
                                                    if (current_cloth.transform == storeCloth.transform)
                                                    {
                                                        if (!cloth_purchase_visible)
                                                            cloth_purchase_visible = true;
                                                    }
                                                    else
                                                    {
                                                        current_cloth = storeCloth;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                cloth_visible = true;
                                                current_cloth = storeCloth;
                                            }
                                            #endregion
                                        }
                                        break;
                                }
                            }
                            ShowItem();
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.S))
                    {
                        Back();
                    }
                }
                catch
                {
                    Debug.Log("Store Exception");
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                is_shop_open_ready = true;
                SceneManager.instance.pressF.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                is_shop_open_ready = false;
                SceneManager.instance.pressF.gameObject.SetActive(false);
            }
        }

        private void MoveCamera()
        {
            switch (currentStoreType)
            {
                case StoreType.Item:
                    {
                        if (current_item != null)
                            iTween.MoveTo(store_camera.gameObject, current_item.camera_position, 0.7f);
                    }
                    break;
                case StoreType.Weapon:
                    {
                        if (current_weapon != null)
                            iTween.MoveTo(store_camera.gameObject, current_weapon.camera_position, 0.7f);
                    }
                    break;
                case StoreType.Cloth:
                    {
                        if (current_cloth != null)
                            iTween.MoveTo(store_camera.gameObject, current_cloth.camera_position, 0.7f);
                    }
                    break;
                case StoreType.None:
                    {
                        switch (tempStoreType)
                        {
                            case StoreType.Item:
                                iTween.MoveTo(store_camera.gameObject, item_camera_position.position, 0.7f);
                                break;
                            case StoreType.Weapon:
                                iTween.MoveTo(store_camera.gameObject, weapon_camera_position.position, 0.7f);
                                break;
                            case StoreType.Cloth:
                                iTween.MoveTo(store_camera.gameObject, cloth_camera_position.position, 0.7f);
                                break;
                        }
                    }
                    break;
            }
        }

        private void LateUpdate()
        {
            SceneManager.instance.storeUI.item_canvas.SetActive(item_visible);
            SceneManager.instance.storeUI.item_purchse.SetActive(item_purchase_visible);

            SceneManager.instance.storeUI.weapon_canvas.SetActive(weapon_visible);
            SceneManager.instance.storeUI.weapon_purchase_name.transform.parent.gameObject.SetActive(weapon_purchase_visible);

            SceneManager.instance.storeUI.cloth_canvas.SetActive(cloth_visible);
            SceneManager.instance.storeUI.cloth_purchase_name.transform.parent.gameObject.SetActive(cloth_purchase_visible);
        }

        public void OK()
        {
            SceneManager.instance.soundManager.PlayEffect("Button");

            switch (currentStoreType)
            {
                case StoreType.Item:
                    {
                        if (current_item != null)
                        {
                            if (n_buy != 0)
                            {
                                if (n_cost_sum <= SceneManager.instance.character.soul)
                                {
                                    SceneManager.instance.character.soul -= n_cost_sum;

                                    for (int i = 1; i <= n_buy; i++)
                                    {
                                        SceneManager.instance.itemManager.AddItem(current_item.item_inventory_name);
                                        item_purchase_visible = false;
                                    }

                                    SceneManager.instance.soundManager.PlayEffect("StoreItemBuy");
                                    StartCoroutine("StoreNotification", "구매 성공");
                                    CountReset();
                                }
                                else
                                {
                                    StartCoroutine("StoreNotification", "구매 실패(돈 부족)");
                                }
                            }
                            else
                            {
                                StartCoroutine("StoreNotification", "개수를 선택해 주세요");
                            }
                        }
                    }
                    break;
                case StoreType.Weapon:
                    {
                        if (current_weapon != null)
                        {
                            if (current_weapon.weapon_cost <= SceneManager.instance.character.soul)
                            {
                                SceneManager.instance.character.soul -= current_weapon.weapon_cost;
                                SceneManager.instance.itemManager.AddSKin(current_weapon.weapon_inventory_name);
                                weapon_purchase_visible = false;

                                SceneManager.instance.soundManager.PlayEffect("StoreItemBuy");
                                StartCoroutine("StoreNotification", "구매 성공");
                            }
                            else
                            {
                                StartCoroutine("StoreNotification", "구매 실패(돈 부족)");
                            }
                        }
                    }
                    break;
                case StoreType.Cloth:
                    {
                        if (current_cloth != null)
                        {
                            if (current_cloth.cloth_cost <= SceneManager.instance.character.soul)
                            {
                                SceneManager.instance.character.soul -= current_cloth.cloth_cost;
                                SceneManager.instance.itemManager.AddSKin(current_cloth.cloth_inventory_name);
                                cloth_purchase_visible = false;

                                SceneManager.instance.soundManager.PlayEffect("StoreItemBuy");
                                StartCoroutine("StoreNotification", "구매 성공");
                            }
                            else
                            {
                                StartCoroutine("StoreNotification", "구매 실패(돈 부족)");
                            }
                        }
                    }
                    break;
            }
        }

        public void NO()
        {
            SceneManager.instance.soundManager.PlayEffect("Button");

            switch (currentStoreType)
            {
                case StoreType.Item:
                    {
                        if (item_purchase_visible)
                            item_purchase_visible = false;
                        else if (item_visible)
                            item_visible = false;
                        CountReset();
                        CostCalculator();
                    }
                    break;
                case StoreType.Weapon:
                    {
                        if (weapon_purchase_visible)
                            weapon_purchase_visible = false;
                        else if (weapon_visible)
                            weapon_visible = false;
                    }
                    break;
                case StoreType.Cloth:
                    {
                        if (cloth_purchase_visible)
                            cloth_purchase_visible = false;
                        else if (cloth_visible)
                            cloth_visible = false;
                    }
                    break;
            }
        }

        public void RightKey()
        {
            try
            {
                switch (currentStoreType)
                {
                    case StoreType.Item:
                        {
                            #region ITEM
                            if (!item_purchase_visible)
                            {
                                if (!item_visible)
                                {
                                    item_visible = true;
                                    current_item = store_items[0];
                                    MoveCamera();
                                }
                                else
                                {
                                    try
                                    {
                                        current_item = store_items[current_item.item_num + 1];
                                    }
                                    catch
                                    {
                                        current_item = store_items[store_items.Count - 1];
                                    }
                                    MoveCamera();
                                }
                                ShowItem();
                            }
                            else
                                CountUp();
                            #endregion
                        }
                        break;
                    case StoreType.Weapon:
                        {
                            #region WEAPON
                            if (!weapon_purchase_visible)
                            {
                                if (!weapon_visible)
                                {
                                    weapon_visible = true;
                                    current_weapon = store_weapons[0];
                                    MoveCamera();
                                }
                                else
                                {
                                    try
                                    {
                                        current_weapon = store_weapons[current_weapon.item_num + 1];
                                    }
                                    catch
                                    {
                                        current_weapon = store_weapons[store_weapons.Count - 1];
                                    }
                                    MoveCamera();
                                }
                                ShowItem();
                            }
                            else
                                CountUp();
                            #endregion
                        }
                        break;
                    case StoreType.Cloth:
                        {
                            #region CLOTH
                            if (!cloth_purchase_visible)
                            {
                                if (!cloth_visible)
                                {
                                    cloth_visible = true;
                                    current_cloth = store_clothes[0];
                                    MoveCamera();
                                }
                                else
                                {
                                    try
                                    {
                                        current_cloth = store_clothes[current_cloth.item_num + 1];
                                    }
                                    catch
                                    {
                                        current_cloth = store_clothes[store_clothes.Count - 1];
                                    }
                                    MoveCamera();
                                }
                                ShowItem();
                            }
                            else
                                CountUp();
                            #endregion
                        }
                        break;
                    case StoreType.None:
                        {
                            switch (tempStoreType)
                            {
                                case StoreType.Item:
                                    tempStoreType = StoreType.Weapon;
                                    break;
                                case StoreType.Weapon:
                                    tempStoreType = StoreType.Cloth;
                                    break;
                                case StoreType.Cloth:
                                    tempStoreType = StoreType.Item;
                                    break;
                            }
                            MoveCamera();
                        }
                        break;
                }
            }
            catch
            {
                Debug.Log("Store Exception");
            }
        }

        public void LeftKey()
        {
            try
            {
                switch (currentStoreType)
                {
                    case StoreType.Item:
                        {
                            #region ITEM
                            if (!item_purchase_visible)
                            {
                                if (!item_visible)
                                {
                                    item_visible = true;
                                    current_item = store_items[0];
                                    MoveCamera();
                                }
                                else
                                {
                                    try
                                    {
                                        current_item = store_items[current_item.item_num - 1];
                                    }
                                    catch
                                    {
                                        current_item = store_items[0];
                                    }
                                    MoveCamera();
                                }
                                ShowItem();
                            }
                            else
                                CountDown();
                            #endregion
                        }
                        break;
                    case StoreType.Weapon:
                        {
                            #region WEAPON
                            if (!weapon_purchase_visible)
                            {
                                if (!weapon_visible)
                                {
                                    weapon_visible = true;
                                    current_weapon = store_weapons[0];
                                    MoveCamera();
                                }
                                else
                                {
                                    try
                                    {
                                        current_weapon = store_weapons[current_weapon.item_num - 1];
                                    }
                                    catch
                                    {
                                        current_weapon = store_weapons[0];
                                    }
                                    MoveCamera();
                                }
                                ShowItem();
                            }
                            else
                                CountDown();
                            #endregion
                        }
                        break;
                    case StoreType.Cloth:
                        {
                            #region CLOTH
                            if (!cloth_purchase_visible)
                            {
                                if (!cloth_visible)
                                {
                                    cloth_visible = true;
                                    current_cloth = store_clothes[0];
                                    MoveCamera();
                                }
                                else
                                {
                                    try
                                    {
                                        current_cloth = store_clothes[current_cloth.item_num - 1];
                                    }
                                    catch
                                    {
                                        current_cloth = store_clothes[0];
                                    }
                                    MoveCamera();
                                }
                                ShowItem();
                            }
                            else
                                CountUp();
                            #endregion
                        }
                        break;
                    case StoreType.None:
                        {
                            switch (tempStoreType)
                            {
                                case StoreType.Item:
                                    tempStoreType = StoreType.Cloth;
                                    break;
                                case StoreType.Weapon:
                                    tempStoreType = StoreType.Item;
                                    break;
                                case StoreType.Cloth:
                                    tempStoreType = StoreType.Weapon;
                                    break;
                            }
                            MoveCamera();
                        }
                        break;
                }
            }
            catch
            {
                Debug.Log("Store Exception");
            }
        }

        public void CountReset()
        {
            n_buy = 0;
            SceneManager.instance.storeUI.item_purchase_count.text = n_buy.ToString();
        }

        public void CostCalculator()
        {
            if (current_item != null)
            {
                n_cost_sum = 0;
                for (int i = 0; i < n_buy; i++)
                {
                    n_cost_sum += current_item.item_cost;
                }
                SceneManager.instance.storeUI.item_cost_count.text = n_cost_sum.ToString() + " 전";
            }
        }

        public void CountUp()
        {
            SceneManager.instance.soundManager.PlayEffect("Button");
            n_buy += 1;
            if (n_buy > 20)
                n_buy = 20;
            SceneManager.instance.storeUI.item_purchase_count.text = n_buy.ToString();
            CostCalculator();
        }

        public void CountDown()
        {
            SceneManager.instance.soundManager.PlayEffect("Button");
            n_buy -= 1;
            if (n_buy < 0)
                n_buy = 0;
            SceneManager.instance.storeUI.item_purchase_count.text = n_buy.ToString();
            CostCalculator();
        }

        private void BuyItem()
        {
            switch (currentStoreType)
            {
                case StoreType.Item:
                    {
                        if (current_item != null)
                        {
                            item_purchase_visible = true;
                            SceneManager.instance.storeUI.item_purchase_name.text = "[" + current_item.item_name + "]";
                        }
                    }
                    break;
                case StoreType.Weapon:
                    {
                        if (current_weapon != null)
                        {
                            weapon_purchase_visible = true;
                            SceneManager.instance.storeUI.weapon_purchase_name.text = "[" + current_weapon.weapon_name + "]";
                        }
                    }
                    break;
                case StoreType.Cloth:
                    {
                        if (current_cloth != null)
                        {
                            cloth_purchase_visible = true;
                            SceneManager.instance.storeUI.cloth_purchase_name.text = "[" + current_cloth.cloth_name + "]";
                        }
                    }
                    break;
            }
        }

        public void ShowItem()
        {
            switch (currentStoreType)
            {
                case StoreType.Item:
                    {
                        if (current_item != null)
                        {
                            SceneManager.instance.storeUI.item_name.text = current_item.item_name;
                            SceneManager.instance.storeUI.item_comment.text = current_item.item_comment;
                            SceneManager.instance.storeUI.item_status_health.text = item_status[0] + "+" + current_item.plus_health;
                            SceneManager.instance.storeUI.item_status_stemina.text = item_status[1] + "+" + current_item.plus_steamina;
                            SceneManager.instance.storeUI.item_status_power.text = item_status[2] + "+" + current_item.plus_steamina;
                            SceneManager.instance.storeUI.item_status_defence.text = item_status[3] + "+" + current_item.plus_defence;
                            SceneManager.instance.storeUI.item_image.sprite = current_item.itemImage;
                        }
                    }
                    break;
                case StoreType.Weapon:
                    {
                        if (current_weapon != null)
                        {
                            SceneManager.instance.storeUI.weapon_name.text = current_weapon.weapon_name;
                            SceneManager.instance.storeUI.weapon_comment.text = current_weapon.weapon_comment;
                            SceneManager.instance.storeUI.weapon_cost.text = "가격 : " +  current_weapon.weapon_cost.ToString() + "전";
                        }
                    }
                    break;
                case StoreType.Cloth:
                    {
                        if (current_cloth != null)
                        {
                            SceneManager.instance.storeUI.cloth_name.text = current_cloth.cloth_name;
                            SceneManager.instance.storeUI.cloth_comment.text = current_cloth.cloth_comment;
                            SceneManager.instance.storeUI.cloth_cost.text = "가격 : " + current_cloth.cloth_cost.ToString() + "전";
                        }
                    }
                    break;
            }
        }

        public IEnumerator StoreNotification(string notifi)
        {
            SceneManager.instance.storeUI.item_notification.transform.parent.gameObject.SetActive(true);
            SceneManager.instance.storeUI.item_notification.text = notifi;
            yield return new WaitForSeconds(0.7f);
            SceneManager.instance.storeUI.item_notification.transform.parent.gameObject.SetActive(false);
        }

        public void Back()
        {
            switch (currentStoreType)
            {
                case StoreType.Item:
                    {
                        if (item_purchase_visible)
                        {
                            item_purchase_visible = false;
                        }
                        else if (item_visible)
                        {
                            item_visible = false;
                            currentStoreType = StoreType.None;
                            tempStoreType = StoreType.Item;
                            HideItemKey();
                            iTween.MoveTo(store_camera.gameObject, original_camera_position, 0.8f);
                        }
                    }
                    break;
                case StoreType.Weapon:
                    {
                        if (weapon_purchase_visible)
                        {
                            weapon_purchase_visible = false;
                        }
                        else if (weapon_visible)
                        {
                            weapon_visible = false;
                            currentStoreType = StoreType.None;
                            tempStoreType = StoreType.Item;
                            HideItemKey();
                            iTween.MoveTo(store_camera.gameObject, original_camera_position, 0.8f);
                        }
                    }
                    break;
                case StoreType.Cloth:
                    {
                        if (cloth_purchase_visible)
                        {
                            cloth_purchase_visible = false;
                        }
                        else if (cloth_visible)
                        {
                            cloth_visible = false;
                            currentStoreType = StoreType.None;
                            tempStoreType = StoreType.Item;
                            HideItemKey();
                            iTween.MoveTo(store_camera.gameObject, original_camera_position, 0.8f);
                        }
                    }
                    break;
                case StoreType.None:
                    {
                        is_shop_open = false;
                        SceneManager.instance.HideCursor();
                        SceneManager.instance.others.SetActive(true);
                        store_camera.gameObject.SetActive(false);
                        SceneManager.instance.character.chasaControl.enabled = true;
                        SceneManager.instance.character.chasaCombat.enabled = true;
                        SceneManager.instance.storeUI.key.gameObject.SetActive(false);
                    }
                    break;
            }
        }

        public void HideItemKey()
        {
            SceneManager.instance.storeUI.key.gameObject.SetActive(true);
            SceneManager.instance.storeUI.storeKey.gameObject.SetActive(true);
            SceneManager.instance.storeUI.itemKey.gameObject.SetActive(false);
        }
    }
}