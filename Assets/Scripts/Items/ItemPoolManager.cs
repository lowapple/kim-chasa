using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Chasa
{
    public class ItemPoolManager : MonoBehaviour
    {
        public enum ItemType
        {
            HEALTH,
            STEMINA,
            POWER,
            DEFENCE
        }

        public enum SkinType
        {
            WEAPON,
            CLOTH
        }

        public enum ClothType
        {
            None,
            Hat,
            Body
        }

        [Serializable]
        public class Item
        {
            public string item_name;
            public Sprite item_image;
        }

        [Serializable]
        public class ToolItem : Item
        {
            public float plus_state;
            public ItemType item_type;
        }

        [Serializable]
        public class SkinItem : Item
        {
            public SkinType skin_type;
            public ClothType cloth_type;
        }

        [SerializeField]
        private ToolItem[] items;
        public static Dictionary<string, ToolItem> itemDictionary;

        [SerializeField]
        private SkinItem[] skins;
        public static Dictionary<string, SkinItem> skinDictionary;

        private void Awake()
        {
            itemDictionary = new Dictionary<string, ToolItem>();

            skinDictionary = new Dictionary<string, SkinItem>();

            for (int i = 0; i < items.Length; i++)
            {
                itemDictionary.Add(items[i].item_name, items[i]);
            }
            for (int i = 0; i < skins.Length; i++)
            {
                skinDictionary.Add(skins[i].item_name, skins[i]);
            }
        }
    }
}