using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class StoreItem : MonoBehaviour
    {
        public string item_name;
        public string item_inventory_name;

        public string item_comment;

        public int item_cost;

        public InventoryType item_type;

        public int plus_health;
        public int plus_steamina;
        public int plus_power;
        public int plus_defence;
        public Sprite itemImage;

        [HideInInspector]
        public int item_num;

        public Vector3 camera_position;

        public void Start()
        {
            camera_position = transform.Find("ItemPos").transform.position;
        }
    }
}