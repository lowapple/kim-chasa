using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class FindSoul : SoulScriptBase
    {
        [System.Serializable]
        public class FindItem
        {
            public string item_name;
            public GameObject item;
            public string request_name;
        }

        public FindItem[] findItems;

        public string find_mission;

        private void Awake()
        {
            script = OK;
        }

        public void OK()
        {
            if (find_mission.CompareTo("") == 0)
                return;

            for (int i = 0; i < findItems.Length; i++)
            {
                findItems[i].item.GetComponent<Chasa.FindItem>().item_name = findItems[i].item_name;
                findItems[i].item.GetComponent<Chasa.FindItem>().mission_request.request_name = findItems[i].request_name;
            }

            MissionGenerator.instance.Active(find_mission);
        }
    }
}