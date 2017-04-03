using UnityEngine;
using UnityEngine.UI;

namespace Chasa
{
    public class StoreUI : MonoBehaviour
    {
        #region ITEM
        public GameObject item_canvas;
        public Image item_image;
        public Text item_name;
        public Text item_comment;
        public Text item_status_health;
        public Text item_status_stemina;
        public Text item_status_power;
        public Text item_status_defence;
        public GameObject item_purchse;
        public Text item_purchase_name;
        public Text item_purchase_count;
        public Text item_cost_count;
        public Text item_notification;
        private bool item_visible = false;
        private bool item_purchase_visible = false;
        private string[] item_status = { "체력 : ", "스테미나 : ", "공격력 : ", "방어력 : " };
        private int n_buy = 0;
        private int n_cost_sum = 0;
        #endregion

        #region WEAPON
        public GameObject weapon_canvas;
        public Text weapon_name;
        public Text weapon_comment;
        public Text weapon_cost;
        public Text weapon_purchase_name;
        private bool weapon_purchase_visible = false;
        private bool weapon_visible = false;
        #endregion

        #region CLOTH
        public GameObject cloth_canvas;
        public Text cloth_name;
        public Text cloth_comment;
        public Text cloth_cost;
        public Text cloth_purchase_name;
        private bool cloth_purchase_visible = false;
        private bool cloth_visible = false;
        #endregion

        public GameObject key;
        public GameObject storeKey;
        public GameObject itemKey;

        public void OK()
        {
            if(StoreManager.instnace != null)
                StoreManager.instnace.OK();
        }
        public void NO()
        {
            if(StoreManager.instnace != null)
                StoreManager.instnace.NO();
        }
        public void Right()
        {
            if (StoreManager.instnace != null)
                StoreManager.instnace.RightKey();
        }
        public void Left()
        {
            if (StoreManager.instnace != null)
                StoreManager.instnace.LeftKey();
        }
    }
}