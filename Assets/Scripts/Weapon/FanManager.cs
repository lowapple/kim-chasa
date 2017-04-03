using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa {
    public class FanManager : MonoBehaviour
    {
        public GameObject fan_open;
        public GameObject fan_close;

        public void Open()
        {
            fan_open.SetActive(true);
            fan_close.SetActive(false);
        }

        public void Close()
        {
            fan_open.SetActive(false);
            fan_close.SetActive(true);
        }
    }
}