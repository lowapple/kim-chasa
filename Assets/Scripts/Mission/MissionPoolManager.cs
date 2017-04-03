using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Chasa
{
    public class MissionPoolManager : MonoBehaviour
    {
        public static MissionPoolManager instance;

        [Serializable]
        public class MissionUnit
        {
            public string unit_name;
            public int health;
            public int soul;
            public Transform position;
            [HideInInspector]
            public GameObject unit;
        }

        [Serializable]
        public class MissionItem
        {
            public string mission_name;
            public MissionUnit[] mission_units;
        }

        public MissionItem[] missionItems;
        public Dictionary<string, MissionItem> missionDictionary;

        private void Awake()
        {
            instance = this;
        }

        private void OnDestroy()
        {
            instance = null;
        }

        private void Start()
        {
            missionDictionary = new Dictionary<string, MissionItem>();

            for (int i = 0; i < missionItems.Length; i++)
                missionDictionary.Add(missionItems[i].mission_name, missionItems[i]);
        }

        public MissionItem StartMission(string mission_name)
        {
            if (missionDictionary.ContainsKey(mission_name))
            {
                var mission_item = missionDictionary[mission_name];

                for (int i = 0; i < mission_item.mission_units.Length; i++)
                {
                    var unit_position = mission_item.mission_units[i].position.position;
                    var unit_rotation = mission_item.mission_units[i].position.localRotation.eulerAngles;

                    var mission_type = MissionGenerator.instance.GetMission(mission_name).mission_type;
                    switch (mission_type)
                    {
                        case MissionType.REMOVAL:
                            {
                                var unit = SceneManager.instance.poolObjects.GetObject(mission_item.mission_units[i].unit_name).GetComponent<ChasaEnemyUnit>();
                                if (unit != null)
                                {
                                    unit.gameObject.SetActive(true);

                                    unit.transform.position = unit_position;
                                    unit.transform.localRotation = Quaternion.Euler(unit_rotation);

                                    unit.missionRequest.request_name = SoulScriptBase.currentTargetSoul.GetComponent<FightSoul>().unit_request;
                                    unit.health = mission_item.mission_units[i].health;
                                    unit.soul = mission_item.mission_units[i].soul;
                                    mission_item.mission_units[i].unit = unit.gameObject;

                                    unit.Alive();
                                }
                            }
                            break;
                    }
                }
                return missionDictionary[mission_name];
            }
            return null;
        }

        public bool StopMission(string mission_name)
        {
            if (missionDictionary.ContainsKey(mission_name))
            {
                var mission_item = missionDictionary[mission_name];
                for (int i = 0; i < mission_item.mission_units.Length; i++)
                {
                    try
                    {
                        // mission_item.mission_units[i].unit.gameObject.SetActive(false);
                        mission_item.mission_units[i].unit = null;
                    }
                    catch
                    {
                        Debug.Log("Exception");
                    }
                }
                return true;
            }
            return false;
        }
    }
}