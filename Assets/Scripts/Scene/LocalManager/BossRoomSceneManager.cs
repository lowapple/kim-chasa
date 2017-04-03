using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class BossRoomSceneManager : MonoBehaviour
    {
        public ChasaEnemyUnit boss;

        public void Start()
        {
            SceneManager.instance.soundManager.PlayBGM("Background3");
        }

        public void PlayAnimation(string animationName)
        {
            SceneManager.instance.others.SetActive(false);
            SceneManager.instance.HideCharacterSoul();

            boss.chasaCombat.enabled = false;
            boss.chasaAI.enabled = false;
            boss.chasaAI.agent.enabled = false;
            boss.chasaSight.enabled = false;

            boss.chasaCharacter.m_Animator.Play(animationName);
        }

        public void StopAnimation()
        {
            SceneManager.instance.others.SetActive(true);
            SceneManager.instance.ShowCharacterSoul();

            if (boss.health < 1)
                SceneManager.instance.gameStateManager.Win();
            else
                boss.chasaCharacter.m_Animator.SetTrigger("Action");
        }
    }
}