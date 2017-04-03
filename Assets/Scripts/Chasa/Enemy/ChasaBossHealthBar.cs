using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chasa
{
    public class ChasaBossHealthBar : MonoBehaviour
    {
        private ChasaEnemyUnit chasaUnit;

        [SerializeField]
        private Animator animator;

        public int maxHealth;
        public Image healthBar;
        public bool isAlive = false;

        private void Start()
        {
            chasaUnit = GetComponent<ChasaEnemyUnit>();
            maxHealth = (int)chasaUnit.health;
        }

        private void LateUpdate()
        {
            healthBar.fillAmount = chasaUnit.health / maxHealth;

            if (chasaUnit.health <= 0)
            {
                healthBar.fillAmount = 0;

                SceneManager.instance.character.health = 100;

                if (!isAlive)
                {
                    isAlive = true;
                    animator.Play("BossRoomCinemaClose");
                }
            }
        }
    }
}