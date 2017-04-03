using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class MissionRequest : MonoBehaviour
    {
        public string request_name;
        public void Request()
        {
            if (request_name == "")
                return;

            SceneManager.instance.missionUI.Request(request_name);
        }
    }
}