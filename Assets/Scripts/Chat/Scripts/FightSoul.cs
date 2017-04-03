using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class FightSoul : SoulScriptBase
    {
        public string fight_mission;

        // 생성한 유닛에게 전달 할 request 이름
        public string unit_request;

        private void Awake()
        {
            script = OK;
        }

        public void OK()
        {
            if (fight_mission.CompareTo("") == 0)
                return;
            
            MissionGenerator.instance.Active(fight_mission);
            MissionGenerator.instance.MissionStart(fight_mission);
        }
    }
}