using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class StoreWeapon : MonoBehaviour
    {
        public string weapon_name;
        public string weapon_inventory_name;
        public string weapon_comment;
        public int weapon_cost;

        [HideInInspector]
        public int item_num;

        public Vector3 camera_position;

        private void Start()
        {
            camera_position = transform.Find("ItemPos").transform.position;
        }
    }
}