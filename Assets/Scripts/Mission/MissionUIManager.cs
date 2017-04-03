using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

namespace Chasa
{
    // 게임 매뉴에서 보임
    public class MissionUIManager : MonoBehaviour
    {
        public Text missionTitle;

        public Text missionComment;
        public Text missionTarget;

        public GridLayoutGroup parents;

        private Dictionary<string, GameObject> missions = new Dictionary<string, GameObject>();

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        // 미션 추가
        public void AddMission(string mission_name)
        {
            var missionTempItem = SceneManager.instance.poolObjects.GetObject("MissionContents");
            if (missionTempItem != null)
            {
                var missionItem = missionTempItem.GetComponent<MissionItem>();
                if (missionItem != null)
                {
                    missionItem.gameObject.SetActive(true);
                    missionItem.transform.SetParent(parents.transform);
                    missionItem.transform.localScale = Vector3.one;
                    missionItem.transform.localPosition = Vector3.one;
                    missionItem.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    missionItem.mission_name = mission_name;
                    missionItem.transform.Find("MissionName").GetComponent<Text>().text = mission_name;

                    parents.CalculateLayoutInputVertical();

                    missions.Add(mission_name, missionItem.gameObject);
                }
            }
        }

        public void RemoveMission(string mission_name)
        {
            if (missions.ContainsKey(mission_name))
            {
                missions[mission_name].SetActive(false);
                missions.Remove(mission_name);
            }
        }

        public void SetMission(string mission_name, string[] mission_contents, MissionGenerator.MissionHint[] mission_target)
        {
            missionTitle.text = mission_name;
            StringBuilder commentBuilder = new StringBuilder();
            for (int i = 0; i < mission_contents.Length; i++)
            {
                commentBuilder.Append(mission_contents[i]);
                commentBuilder.Append("\n");
            }
            missionComment.text = commentBuilder.ToString();
            // Mission Hint를 Mission Target에 작성하기
            StringBuilder targetBuilder = new StringBuilder();
            for(int i = 0; i < mission_target.Length; i++)
            {
                targetBuilder.Append(mission_target[i].mission_hint);
                targetBuilder.Append("\n");
            }
            missionTarget.text = targetBuilder.ToString();
        }
    }
}