using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class StoreCloth : MonoBehaviour
    {
        public string cloth_name;
        public string cloth_inventory_name;
        public string cloth_comment;
        public int cloth_cost;
        [HideInInspector]
        public int item_num;
        public Vector3 camera_position;

        private void Start()
        {
            camera_position = transform.Find("ItemPos").transform.position;
        }
    }
}