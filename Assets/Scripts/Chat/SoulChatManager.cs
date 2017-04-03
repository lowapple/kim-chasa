using UnityEngine.UI;
using UnityEngine;
using System.Collections;

namespace Chasa
{
    public class SoulChatManager : MonoBehaviour
    {
        private string currentTalk;
        private string showTalk;
        private int currentTalkIdx;
        private int currentCharacterIdx;

        // -----------------

        public Text ChatName;
        public Text ChatText;

        // ----------- OK
        public Text ONE;
        // ----------- OK_NO
        public Text TWO_1;
        public Text TWO_2;

        private SoulChat soulChat;

        // 영혼의 퀘스트를 클리어하면 임시적으로 사용함
        private bool isClear = false;
        private bool isOKSelection = false;
        private bool isNOSelection = false;
        private bool isSkip = false;
        private bool isChatActive = false;

        private void Start()
        {
            ChatEnd();
            SceneManager.instance.HideCursor();
        }

        public void ChatStart(SoulChat soulChat)
        {
            if (!isChatActive && soulChat.isAlive)
            {
                SceneManager.instance.HideCharacterSoul();
                SceneManager.instance.ShowCursor();

                gameObject.SetActive(true);

                isClear = false;
                isSkip = false;

                isChatActive = true;

                this.soulChat = soulChat;

                if (SceneManager.instance.character.chasaControl != null)
                    SceneManager.instance.character.chasaControl.enabled = false;
                if (SceneManager.instance.cam != null)
                    SceneManager.instance.cam.enabled = false;
                if (SceneManager.instance.character.chasaCombat != null)
                    SceneManager.instance.character.chasaCombat.enabled = false;
                if (this.soulChat != null)
                    this.soulChat.soulCamera.gameObject.SetActive(true);
                SceneManager.instance.character.chasaControl.gameObject.GetComponent<Animator>().SetFloat("Forward", 0.0f);
                SceneManager.instance.character.chasaControl.gameObject.GetComponent<Animator>().SetFloat("Turn", 0.0f);
                ChatName.text = soulChat.soul_name;
                if (SceneManager.instance.others != null)
                    SceneManager.instance.others.SetActive(false);
                var fightQuestType = soulChat.GetComponent<FightSoul>();
                var collectionQuestType = soulChat.GetComponent<FindSoul>();
                try
                {
                    if (fightQuestType != null)
                    {
                        if (MissionGenerator.instance.GetMission(fightQuestType.fight_mission).clear &&
                            MissionGenerator.instance.GetMission(fightQuestType.fight_mission) != null)
                        {
                            isClear = true;
                            ChatTalkStart(soulChat.mission_clear_chats);
                            return;
                        }
                    }
                    else if (collectionQuestType != null)
                    {
                        if (MissionGenerator.instance.GetMission(collectionQuestType.find_mission).clear &&
                            MissionGenerator.instance.GetMission(collectionQuestType.find_mission) != null)
                        {
                            isClear = true;
                            ChatTalkStart(soulChat.mission_clear_chats);
                            return;
                        }
                    }
                    ChatTalkStart(soulChat.soul_chats);
                }
                catch
                {
                    ChatTalkStart(soulChat.soul_chats);
                }
            }
        }

        public void ChatEnd()
        {
            try
            {
                SceneManager.instance.ShowCharacterSoul();
                SceneManager.instance.HideCursor();

                isChatActive = false;

                gameObject.SetActive(false);

                if (SceneManager.instance.character != null)
                {
                    if (SceneManager.instance.character.chasaControl != null)
                        SceneManager.instance.character.chasaControl.enabled = true;
                    if (SceneManager.instance.character.chasaCombat != null)
                        SceneManager.instance.character.chasaCombat.enabled = true;
                }
                if (soulChat != null)
                    soulChat.soulCamera.gameObject.SetActive(false);
                if (SceneManager.instance.cam != null)
                    SceneManager.instance.cam.enabled = true;

                isOKSelection = false;
                isNOSelection = false;
                if (SceneManager.instance.others != null)
                    SceneManager.instance.others.SetActive(true);

                if (isClear)
                    Invoke("SoulClear", 0.5f);


                if (soulChat != null && !soulChat.isTalk)
                {
                    soulChat.isTalk = true;
                    SceneManager.instance.character.PlusSoul(10);
                }
            }
            catch
            {
                Debug.Log("Error");
            }
        }

        public void ChatSelectionStart()
        {
            if (!isClear)
            {
                switch (soulChat.soulChatType)
                {
                    case SoulChatType.ONE:
                        {
                            ONE.gameObject.SetActive(true);
                            ONE.text = soulChat.selection1;
                            TWO_1.gameObject.SetActive(false);
                            TWO_2.gameObject.SetActive(false);
                        }
                        break;
                    case SoulChatType.TWO:
                        {
                            ONE.gameObject.SetActive(false);
                            TWO_1.gameObject.SetActive(true);
                            TWO_1.text = soulChat.selection1;
                            TWO_2.gameObject.SetActive(true);
                            TWO_2.text = soulChat.selection2;
                        }
                        break;
                    case SoulChatType.NONE:
                        {
                            if (!SceneManager.instance.pressF.activeSelf)
                                SceneManager.instance.pressF.SetActive(true);
                            ChatEnd();
                        }
                        break;
                }
            }
        }

        public void ChatSelectionEnd()
        {
            ONE.gameObject.SetActive(false);
            TWO_1.gameObject.SetActive(false);
            TWO_2.gameObject.SetActive(false);
        }

        public void ChatTalkStart(string[] chats)
        {
            currentTalkIdx = 0;
            currentCharacterIdx = 0;

            StopCoroutine("ChatEnumerator");
            StartCoroutine("ChatEnumerator", chats);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isSkip)
                    isSkip = true;
            }
        }

        IEnumerator ChatEnumerator(string[] chats)
        {
            while (currentTalkIdx != chats.Length)
            {
                showTalk = "";
                currentTalk = chats[currentTalkIdx];
                while (currentCharacterIdx != currentTalk.Length)
                {
                    showTalk += currentTalk[currentCharacterIdx];
                    currentCharacterIdx += 1;
                    ChatText.text = showTalk;
                    if (!isSkip)
                        yield return new WaitForSeconds(soulChat.nextCharTime);
                    else
                    {
                        isSkip = false;
                        showTalk = currentTalk;
                        ChatText.text = showTalk;
                        break;
                    }
                }
                currentCharacterIdx = 0;
                currentTalkIdx += 1;
                yield return new WaitForSeconds(soulChat.nextTalkTime);
            }

            if (isClear)
                Invoke("ChatEnd", 0.5f);
            else
            {
                if (!isOKSelection && !isNOSelection)
                    ChatSelectionStart();
                else
                    Invoke("SoulScriptActive", 0.5f);
            }
        }

        public void Ok()
        {
            SceneManager.instance.soundManager.PlayEffect("Button");
            soulChat.OnMissionOKSend();
            isOKSelection = true;
            ChatSelectionEnd();
            ChatTalkStart(soulChat.ok_chats);
        }

        public void No()
        {
            SceneManager.instance.soundManager.PlayEffect("Button");
            isNOSelection = true;
            ChatSelectionEnd();
            ChatTalkStart(soulChat.no_chats);
        }

        private void SoulScriptActive()
        {
            if (isOKSelection)
            {
                SoulScriptBase soulScript = soulChat.GetComponent<SoulScriptBase>();
                if (soulScript != null)
                {
                    soulScript.CurrentSoul();
                    soulScript.script();
                }
                ChatEnd();
            }
            else if (isNOSelection)
            {
                SoulScriptBase soulScript = soulChat.GetComponent<SoulScriptBase>();
                if (soulScript != null)
                {
                    soulScript.CurrentSoul();
                    soulScript.script();
                }
                ChatEnd();
            }
        }

        private void SoulClear()
        {
            if (soulChat != null)
            {
                soulChat.OnMissionClearSend();
                soulChat.Erase();
            }
        }
    }
}