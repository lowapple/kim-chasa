using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class ChasaEnemyBoss : MonoBehaviour
    {
        public enum AttackType
        {
            NORMAL_ATTACK,
            STUN
        }

        [System.Serializable]
        public class AttackPattern
        {
            public string object_name;
            public AttackType attack_type;
        }

        [System.Serializable]
        public class SummonsUnit
        {
            public string object_name;
            public Transform object_position;
        }

        [System.Serializable]
        public class SummonsPattern
        {
            public SummonsUnit[] units;
            public float delayTime = 0.0f;
        }

        public AttackPattern[] attack_patterns;
        public SummonsPattern summons_patterns;

        [HideInInspector]
        public string currentAttackName = "";
        [HideInInspector]
        public AttackPattern attack_patern;

        private void Start()
        {
            StartCoroutine("SummonsUpdate", summons_patterns.delayTime);
        }

        public string AttackActive(Animator animator, bool random = false)
        {
            if (random && attack_patterns.Length > 0)
            {
                int rd = Random.Range(0, attack_patterns.Length);
                var attackName = attack_patterns[rd].object_name;
                attack_patern = attack_patterns[rd];
                animator.SetTrigger(attackName);
                currentAttackName = attackName;
                return attack_patterns[rd].object_name;
            }
            else
            {
                attack_patern = null;
                animator.SetTrigger("Attack1");
                currentAttackName = "Attack1";
                return "Attack1";
            }
        }

        // 우선 단일 공격
        public void AttackSlash()
        {
            GameObject tempSlashAttack = SceneManager.instance.poolObjects.GetObject("Slash");
            if (tempSlashAttack != null)
            {
                tempSlashAttack.gameObject.SetActive(true);
                Vector3 pos = transform.position;
                pos.y = 0.5f;
                tempSlashAttack.transform.position = pos;
                Vector3 euler = transform.localRotation.eulerAngles;
                euler.x = 0;
                euler.z = 0;
                tempSlashAttack.transform.rotation = Quaternion.Euler(euler);
                tempSlashAttack.GetComponent<Slash>().SlashActive();
                SceneManager.instance.poolObjects.LifeTime(tempSlashAttack, 2.0f);
            }
        }

        IEnumerator SummonsUpdate(float time)
        {
            while (true)
            {
                yield return new WaitForSeconds(time);
                Summons();
            }
        }

        private void Summons()
        {
            for (int j = 0; j < summons_patterns.units.Length; j++)
            {
                var summonsUnit = summons_patterns.units[j];
                var unit = SceneManager.instance.poolObjects.GetObject(summonsUnit.object_name).GetComponent<ChasaEnemyUnit>();
                if (unit != null)
                {
                    unit.gameObject.SetActive(true);
                    unit.transform.position = summons_patterns.units[j].object_position.position;
                    unit.transform.localRotation = summons_patterns.units[j].object_position.localRotation;
                    unit.missionRequest.request_name = "";
                    unit.health = 80;
                    unit.soul = 0;
                    unit.Alive();
                    unit.chasaSight.IsSee = true;
                    unit.chasaAI.IsMove = true;
                    StartCoroutine("IsSeeUpdate", unit.chasaSight);
                }
            }
        }

        IEnumerator IsSeeUpdate(ChasaEnemySight sight)
        {
            yield return null;
            yield return null;
            sight.IsSee = true;
        }
    }
}