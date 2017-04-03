using UnityEngine;

namespace Chasa
{
    public class SoulChat : MonoBehaviour
    {
        public string soul_name;
        public string[] soul_chats;
        public float nextCharTime = 0.0f;
        public float nextTalkTime = 0.0f;
        public SoulChatType soulChatType;
        public Camera soulCamera;
        [HideInInspector]
        public bool isTalk = false;

        public string selection1 = "수락";
        public string selection2 = "거절";

        // 선택지에서 선택후 나오는 대사
        public string[] ok_chats;
        public string[] no_chats;

        public string[] mission_clear_chats;

        private GameObject character;

        [System.Serializable]
        public class MissionSendMessage
        {
            public GameObject gameObject;
            public string sendMessage;
        }

        public MissionSendMessage[] ok_mission_send;
        public MissionSendMessage[] mission_clear_send;

        public void OnMissionOKSend()
        {
            for (int i = 0; i < ok_mission_send.Length; i++)
                ok_mission_send[i].gameObject.SendMessage(ok_mission_send[i].sendMessage);
        }
        public void OnMissionClearSend()
        {
            for (int i = 0; i < mission_clear_send.Length; i++)
                mission_clear_send[i].gameObject.SendMessage(mission_clear_send[i].sendMessage);
        }

        [HideInInspector]
        public bool isAlive = true;

        private void Start()
        {
            character = GameObject.Find("Player");
            if (soulCamera == null)
                soulCamera = transform.Find("Camera").GetComponent<Camera>();
        }

        public void LateUpdate()
        {
            if (character != null)
            {
                Vector3 position = character.transform.position;
                position.y = transform.position.y;
                transform.LookAt(position);
            }
        }

        public void Erase()
        {
            if (isAlive)
                SceneManager.instance.soundManager.PlayEffect("GlassBroken");
            isAlive = false;

            gameObject.SetActive(false);

            var eraseParticle = SceneManager.instance.poolObjects.GetObject("FillerErase");
            if (eraseParticle != null)
            {
                eraseParticle.SetActive(true);
                eraseParticle.transform.position = transform.position;
                SceneManager.instance.poolObjects.LifeTime(eraseParticle.gameObject, eraseParticle.GetComponent<ParticleSystem>().main.duration);
            }

            var fight = GetComponent<FightSoul>();
            var find = GetComponent<FindSoul>();

            if (fight != null)
                MissionGenerator.instance.MissionClear(fight.fight_mission);
            else if (find != null)
                MissionGenerator.instance.MissionClear(find.find_mission);
        }
    }
}