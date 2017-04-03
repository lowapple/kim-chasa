using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    // Unit의 Transform과 TextBox를 넣는다.
    public class GlobalChatManager : MonoBehaviour
    {
        private static GlobalChatManager manager;
        public static GlobalChatManager GetInstance
        {
            get
            {
                return manager;
            }
        }

        [HideInInspector]
        public Camera mainCamera;
        public List<KeyValuePair<Transform, RectTransform>> targets = new List<KeyValuePair<Transform, RectTransform>>();

        private void Awake()
        {
            manager = this;
            mainCamera = Camera.main;
        }

        // 위치, 아이템
        public void AddChat(Transform target, RectTransform item)
        {
            item.transform.SetParent(transform);
            targets.Add(new KeyValuePair<Transform, RectTransform>(target, item));
        }

        void Update()
        {
            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].Value.position = RectTransformUtility.WorldToScreenPoint(mainCamera, targets[i].Key.position);
            }
        }
    }
}