using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    // Mission Item
    // Mission UI에서 사용

    public class MissionItem : MonoBehaviour
    {
        public string mission_name;
        
        // [Button]
        // 미션 UI에 적용하고
        // 현재 미션을 알려준다.
        public void MissionClick()
        {
            var missionData = MissionGenerator.instance.GetMission(mission_name);
            // Mission UI 갱신
            // OptionMission
            SceneManager.instance.missionUIManager.SetMission(mission_name, missionData.mission_comments, missionData.mission_hints);
            // GameMission
            SceneManager.instance.missionUI.SetMission(mission_name, missionData.mission_hints);
        }
    }
}