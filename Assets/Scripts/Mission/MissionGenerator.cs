using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class MissionGenerator : MonoBehaviour
    {
        public static MissionGenerator instance;

        [System.Serializable]
        public class MissionHint
        {
            public string mission_hint;
            public int mission_target_count;
            [HideInInspector]
            public int mission_current_count;
            [HideInInspector]
            public bool clear = false;
        }

        [System.Serializable]
        public class MissionData
        {
            // 나중에 활성화 한다.
            // 추가 퀘스트등
            public string mission_name;
            public bool active = false;
            public MissionType mission_type;
            public string[] mission_comments;
            public MissionHint[] mission_hints;
            [HideInInspector]
            public bool clear = false;
            public int mission_clear_soul;
        }

        // 보여주기용, 사용 후 삭제
        public MissionData[] missionDatas;
        public static MissionPoolManager.MissionItem currentMission;
        // 실제 데이터
        public Dictionary<string, MissionData> missionDictionary;

        private void Awake()
        {
            missionDictionary = new Dictionary<string, MissionData>();
            instance = this;
        }

        private void OnDestroy()
        {
            instance = null;
        }

        public void Start()
        {
            Create();
        }

        public void Create()
        {
            for (int i = 0; i < missionDatas.Length; i++)
            {
                missionDictionary.Add(missionDatas[i].mission_name, missionDatas[i]);
                if (missionDatas[i].active)
                {
                    try
                    {
                        missionDatas[i].clear = false;
                        SceneManager.instance.missionUIManager.AddMission(missionDatas[i].mission_name);
                    }
                    catch
                    {
                        Debug.Log("MissionUIManager is Null");
                    }
                }
            }
        }

        // 미션을 클리어함
        public void MissionClear(string mission_name)
        {
            // 미션 제거
            Remove(mission_name);
        }

        // 미션을 시작한다.
        // 현재 표시중인 Mission UI를 이것으로 바꾼다.
        public void MissionStart(string mission_name)
        {
            if (currentMission != null)
                MissionPoolManager.instance.StopMission(mission_name);

            currentMission = MissionPoolManager.instance.StartMission(mission_name);

            SceneManager.instance.missionUI.SetMission(mission_name, missionDictionary[mission_name].mission_hints);
        }

        // 활성화하여 Mission UI에 추가한다.
        public void Active(string mission_name)
        {
            if (missionDictionary.ContainsKey(mission_name))
            {
                if (missionDictionary[mission_name].active)
                    return;
                else
                {
                    missionDictionary[mission_name].active = true;
                    SceneManager.instance.missionUIManager.AddMission(mission_name);
                }
            }
        }
        
        // Mission UI에서 삭제한다.
        // 미션 클리어 시
        public void Remove(string mission_name)
        {
            if (missionDictionary.ContainsKey(mission_name))
            {
                SceneManager.instance.character.PlusSoul(missionDictionary[mission_name].mission_clear_soul);
                missionDictionary.Remove(mission_name);
                SceneManager.instance.missionUI.RemoveMission();
                SceneManager.instance.missionUIManager.RemoveMission(mission_name);
            }
            else
                return;
        }

        // Misssion을 받아온다.
        public MissionData GetMission(string mission_name)
        {
            if (missionDictionary != null)
            {
                if (missionDictionary.ContainsKey(mission_name))
                {
                    return missionDictionary[mission_name];
                }
                else
                    return null;
            }
            return null;
        }

        public MissionHint Hint2MissionHint(string mission_hint)
        {
            var keys = missionDictionary.Keys;
            foreach(var key in keys)
            {
                var value = missionDictionary[key];
                for(int i = 0; i < value.mission_hints.Length; i++)
                {
                    if(value.mission_hints[i].mission_hint.CompareTo(mission_hint) == 0)
                    {
                        return value.mission_hints[i];
                    }
                }
            }
            return null;
        }
    }
}