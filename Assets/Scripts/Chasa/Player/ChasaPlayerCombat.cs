using UnityEngine;
using System.Collections;

namespace Chasa
{
    public class ChasaPlayerCombat : MonoBehaviour
    {
        [HideInInspector]
        public ChasaPlayerUnit chasaUnit;

        private FanManager fanManger;

        public string pool_attack_name;
        public Transform rightHand;
        public string pool_defence_name;
        public Transform leftHand;

        public bool defence = false;

        [HideInInspector]
        public int attackCount = 0;
        public bool isAttack = false;
        public bool isAttackKey = true;

        public bool isCounterAttack = false;
        public bool isCounterAttackReady = false;
        private float rollAnimTime = 0.0f;

        [Range(1, 10)]
        public float attackRange = 0.0f;
        // 구르기
        public bool isRoll = false;

        private void Start()
        {
            chasaUnit = GetComponent<ChasaPlayerUnit>();
            fanManger = GetComponent<FanManager>();
        }

        void Update()
        {
            if (chasaUnit.chasaCharacter == null)
                return;

            if (SceneManager.instance.optionUIManager.isActive)
                return;

            if (SceneManager.instance.isCursor)
                return;

            bool isGround = chasaUnit.chasaCharacter.m_Animator.GetBool("OnGround");

            // Attack
            if (Input.GetMouseButtonDown(0) && isGround && !defence)
            {
                if (isAttackKey)
                {
                    if (attackCount < 3.0f)
                    {
                        if (chasaUnit.stemina > 25)
                            chasaUnit.stemina -= 25;
                        else
                            return;

                        if (chasaUnit.stemina < 0)
                        {
                            chasaUnit.stemina = 0;
                            isAttackKey = true;
                        }
                        else
                        {
                            isAttackKey = false;
                            attackCount += 1;
                            isAttack = true;
                        }

                        if (chasaUnit.steminabar != null)
                            chasaUnit.steminabar.fillAmount = chasaUnit.stemina / 100f;

                        chasaUnit.chasaCharacter.m_Animator.SetTrigger("Attack");
                    }
                }
            }

            // Block
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (chasaUnit.chasaCharacter.m_IsGrounded && !isAttack && !chasaUnit.isStun)
                {
                    defence = true;
                    chasaUnit.chasaCharacter.m_Animator.SetBool("Block", true);
                    chasaUnit.chasaControl.enabled = false;

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (isCounterAttackReady)
                        {
                            // 카운터 공격
                            if (!isCounterAttack)
                            {
                                isCounterAttack = true;
                                StopCoroutine("CounterAttackReady");
                                StartCoroutine("CounterAttackReady", 0.01f);
                                chasaUnit.chasaCharacter.m_Animator.Play("Attack4");
                            }
                        }
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                if (defence)
                {
                    defence = false;
                    chasaUnit.chasaCharacter.m_Animator.SetBool("Block", false);
                    chasaUnit.chasaControl.enabled = true;
                    attackCount = 0;

                    isAttackKey = true;
                    isAttack = false;
                }
            }

            // 카운터 공격을 하지 않을 때
            if (!isCounterAttack)
            {
                chasaUnit.chasaCharacter.m_Animator.SetFloat("AttackCount", (float)attackCount);
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (!isRoll && !isAttack)
                {
                    isRoll = true;
                    isAttackKey = true;
                    attackCount = 0;
                    isAttack = false;
                    defence = false;
                    fanManger.Close();

                    chasaUnit.chasaCharacter.m_Animator.SetTrigger("Roll");

                    SceneManager.instance.soundManager.PlayEffect("Run");

                    StopCoroutine("RollReady");
                    StartCoroutine("RollReady");
                }
            }
        }

        // 애니메이션에서 실제 공격
        public void Attack()
        {
            bool distEnemy = false;

            chasaUnit.enemys.ForEach(p =>
            {
                ChasaEnemyUnit enemy = p.GetComponent<ChasaEnemyUnit>();

                if (enemy != null)
                {
                    float dist = Vector3.Distance(transform.position, p.transform.position);

                    if (dist < attackRange)
                    {
                        if (p.health > 0)
                        {
                            if (pool_attack_name != "")
                            {
                                // 파티클 생성
                                var bloodParticle = enemy.bloodParticle;
                                if (bloodParticle != null)
                                {
                                    GameObject tempAttack = SceneManager.instance.poolObjects.GetObject(pool_attack_name);
                                    if (tempAttack != null)
                                    {
                                        tempAttack.SetActive(true);
                                        tempAttack.transform.position = bloodParticle.position;
                                        SceneManager.instance.poolObjects.LifeTime(tempAttack, tempAttack.GetComponent<ParticleSystem>().main.duration);
                                    }

                                    Time.timeScale = 0.05f;

                                    StartCoroutine("FightShake", 0.003f);
                                }

                                distEnemy = true;

                                // Effect Sound
                                SceneManager.instance.soundManager.PlayEffect("Hit1");

                                p.Damage(chasaUnit.power);
                            }
                        }
                    }
                }
            });

            if (distEnemy)
            {
                Shaker.GetInstance.shake(0.1f, 0.5f, 0.8f);
            }
        }

        // NextAnimationReset
        public void AttackActiveTrigger()
        {
            isAttackKey = true;
        }

        // attack Count를 초기화 해준다.
        public void LastAttackTrigger()
        {
            attackCount = 0;
            isAttack = false;
        }

        public void AttackReset()
        {
            AttackActiveTrigger();
            LastAttackTrigger();
        }

        // 타격감 테스트
        public IEnumerator FightShake(float time)
        {
            yield return new WaitForSeconds(time);
            Time.timeScale = 1.0f;
        }

        public IEnumerator CounterAttackReady(float time)
        {
            yield return new WaitForSeconds(time);
            isCounterAttack = false;
        }

        public void SoundPlay(string sound)
        {
            SceneManager.instance.soundManager.PlayEffect(sound);
        }

        public IEnumerator RollReady()
        {
            var anim_list = chasaUnit.chasaCharacter.m_Animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < anim_list.Length; i++)
            {
                if (anim_list[i].name.CompareTo("Roll") == 0)
                {
                    rollAnimTime = anim_list[i].length;
                    break;
                }
            }
            yield return new WaitForSeconds(rollAnimTime + 0.85f);
            isRoll = false;
        }
    }
}