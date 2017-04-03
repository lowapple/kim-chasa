using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Chasa
{
    // 게임 화면에서 보임
    // 현재 미션을 표시 한다.
    public class MissionUI : MonoBehaviour
    {
        public Text missionTitle;

        private string missionTitleString;

        [SerializeField]
        private RectTransform missionHints;
        [SerializeField]
        private string pool_missionHint_name;

        private Dictionary<string, MissionGenerator.MissionHint> d_missionHints;
        private Dictionary<string, Text> d_missionText;

        private void Start()
        {
            d_missionText = new Dictionary<string, Text>();
            d_missionHints = new Dictionary<string, MissionGenerator.MissionHint>();
        }

        public void RemoveMission()
        {
            missionTitle.text = "";

            if (d_missionText.Count > 0)
            {
                foreach (var key in d_missionText.Keys)
                {
                    d_missionText[key].gameObject.SetActive(false);
                }
                d_missionText.Clear();
                d_missionHints.Clear();
            }
        }

        public void SetMission(string missionTitle, MissionGenerator.MissionHint[] missionHints)
        {
            if (d_missionText.Count > 0)
            {
                foreach (var key in d_missionText.Keys)
                {
                    d_missionText[key].gameObject.SetActive(false);
                }
                d_missionText.Clear();
                d_missionHints.Clear();
            }

            missionTitleString = missionTitle;
            this.missionTitle.text = "◈ " + missionTitle;
            for (int i = 0; i < missionHints.Length; i++)
            {
                var tempPool = SceneManager.instance.poolObjects.GetObject(pool_missionHint_name);
                if (tempPool != null)
                {
                    var tempHint = tempPool.GetComponent<Text>();

                    tempHint.gameObject.SetActive(true);
                    tempHint.transform.SetParent(this.missionHints.transform);
                    tempHint.transform.localScale = new Vector3(0.25f, 0.55f, 1);
                    tempHint.transform.localPosition = Vector3.zero;
                    tempHint.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    tempHint.text = "ㄴ" + missionHints[i].mission_hint;

                    if (missionHints[i].mission_hint.Length > 15)
                        tempHint.fontSize = 21;
                    else
                        tempHint.fontSize = 25;

                    if (missionHints[i].mission_current_count >= missionHints[i].mission_target_count)
                    {
                        tempHint.text = " (완료)";
                        missionHints[i].clear = true;
                    }
                    else
                    {
                        tempHint.text += missionHints[i].mission_current_count.ToString() + "/" + missionHints[i].mission_target_count.ToString();
                        missionHints[i].clear = false;
                    }

                    d_missionText.Add(missionHints[i].mission_hint, tempHint);
                    d_missionHints.Add(missionHints[i].mission_hint, missionHints[i]);
                }
            }
            IsClear();
        }

        public void Request(string mission_hint_name)
        {
            // 현재 미션과 다른 미션
            if (d_missionHints.ContainsKey(mission_hint_name))
            {
                d_missionHints[mission_hint_name].mission_current_count += 1;
                d_missionText[mission_hint_name].text = "ㄴ" + mission_hint_name;

                if (d_missionHints[mission_hint_name].mission_current_count >= d_missionHints[mission_hint_name].mission_target_count)
                {
                    d_missionText[mission_hint_name].text = " (완료)";
                    d_missionHints[mission_hint_name].clear = true;
                }
                else
                {
                    d_missionText[mission_hint_name].text += " " + d_missionHints[mission_hint_name].mission_current_count + "/" + d_missionHints[mission_hint_name].mission_target_count;
                    d_missionHints[mission_hint_name].clear = false;
                }

                IsClear();
                MissionRefresh();
            }
            else
            {
                if (MissionGenerator.instance == null)
                    return;

                var hint = MissionGenerator.instance.Hint2MissionHint(mission_hint_name);

                if (hint == null)
                    return;

                hint.mission_current_count += 1;

                if (hint.mission_current_count >= hint.mission_target_count)
                    hint.clear = true;
                else
                    hint.clear = false;

                MissionRefresh();
            }
        }


        public void MissionRefresh()
        {
            foreach (var MGKey in MissionGenerator.instance.missionDictionary.Keys)
            {
                bool isClear = true;
                for (int i = 0; i < MissionGenerator.instance.missionDictionary[MGKey].mission_hints.Length; i++)
                {
                    if (!MissionGenerator.instance.missionDictionary[MGKey].mission_hints[i].clear)
                        isClear = false;
                }
                if (isClear)
                    MissionGenerator.instance.missionDictionary[MGKey].clear = true;
            }
        }

        // 모든 Hint를 클리어 했는가?
        public bool IsClear()
        {
            bool isClear = true;
            foreach(var key in d_missionHints.Keys)
            {
                if (!d_missionHints[key].clear)
                    isClear = false;
            }

            MissionGenerator.instance.GetMission(missionTitleString).clear = true;
            return isClear;
        }
    }
}