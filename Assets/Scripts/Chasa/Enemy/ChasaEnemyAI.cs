using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Chasa
{
    public class ChasaEnemyAI : MonoBehaviour
    {
        [HideInInspector]
        public ChasaEnemyUnit chasaUnit;
        public NavMeshAgent agent { get; private set; }

        [HideInInspector]
        public Transform target;

        [Range(0, 4)]
        public float toDist = 0.0f;

        // 플레이어 발견시 IsUse = true
        // 플레이어랑 전투시 IsCombat = true
        // 플레이어랑 전투시 IsUse = false

        public bool IsMove = true;
        public bool IsDamage = false;

        public bool IsAttack = false;
        public float attackTime = 0.0f;

        public bool IsUse = false;
        public bool IsCombat = false;
        public bool IsSlash = false;

        // 슬래시 공격이 있을 경우
        public bool IsSlashCheck = false;
        public float slashTime = 0.0f;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            chasaUnit = GetComponent<ChasaEnemyUnit>();

            agent.updateRotation = false;
            agent.updatePosition = true;

            target = GameObject.Find("Player").transform;

            chasaUnit.SetDamageCallback(DamageFunction);
        }

        private void Start()
        {
            if(!chasaUnit.chasaSight.IsSee)
                chasaUnit.chasaSight.TargetFindStart();

            StopCoroutine("Updater");
            StartCoroutine("Updater");
            StopCoroutine("LookUpdate");
            StartCoroutine("LookUpdate");
        }

        private void OnEnable()
        {
            if (chasaUnit.chasaSight == null)
                return;
            if (!chasaUnit.chasaSight.IsSee)
                chasaUnit.chasaSight.TargetFindStart();

            StartCoroutine("Updater");
            StartCoroutine("LookUpdate");
        }

        public void Update()
        {
            transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
        }

        IEnumerator LookUpdate()
        {
            while (true)
            {
                if (IsUse && chasaUnit.chasaSight.IsSee)
                {
                    LookTarget();
                }
                yield return null;
            }
        }

        private void DamageFunction()
        {
            StopCoroutine("Damage");
            IsDamage = true;
            StartCoroutine("Damage");
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        IEnumerator MoveUpdate(float time)
        {
            yield return new WaitForSeconds(time);
            IsMove = true;
        }

        IEnumerator FindPlayer()
        {
            while (true)
            {
                if (IsCombat)
                {
                    IsUse = true;
                    IsCombat = false;
                    IsMove = true;
                }
                yield return new WaitForSeconds(2.0f);
            }
        }

        // 공격 대기
        IEnumerator Attack()
        {
            if (IsAttack)
            {
                attackTime = chasaUnit.chasaCharacter.m_Animator.GetCurrentAnimatorStateInfo(0).length;
                yield return new WaitForSeconds(attackTime + chasaUnit.chasaCombat.currentAttackData.attack_delay_time);
                IsAttack = false;
            }
        }
        // 데미지 대기
        IEnumerator Damage()
        {
            yield return new WaitForSeconds(0.5f);
            IsDamage = false;
        }
        // 슬래시 대기
        IEnumerator SlashUpdate()
        {
            attackTime = chasaUnit.chasaCharacter.m_Animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(attackTime + slashTime);
            IsSlash = false;
        }

        IEnumerator Updater()
        {
            while (true)
            {
                yield return null;

                if (chasaUnit.health > 0 && IsMove)
                {
                    float dist = Vector3.Distance(target.transform.position, transform.position);

                    // 플레이어 발견
                    if (chasaUnit.chasaSight.IsSee && !IsSlash)
                    {
                        chasaUnit.chasaSight.TargetFindStop();
                        IsUse = true;
                    }

                    // 발견시 플레이어를 향해서 이동한다.
                    if (IsUse && chasaUnit.chasaSight.IsSee)
                    {
                        // currentIsUseTime = 0.0f;

                        if (dist < toDist)
                        {
                            IsUse = false;
                            IsCombat = true;
                            WalkStop();
                        }
                        else
                        {
                            if (IsMove)
                            {
                                try
                                {
                                    if(agent.isOnNavMesh)
                                        agent.SetDestination(target.position);
                                }
                                catch
                                {
                                    Debug.Log("Agent Exception");
                                }
                                chasaUnit.chasaCharacter.Move(agent.desiredVelocity, false, false);
                            }

                            // 슬래시 범위 안에 있으면
                            if (dist < 30 && IsSlashCheck && dist > 2.5f)
                            {
                                if (!IsSlash && chasaUnit.chasaSight.IsSee)
                                {
                                    IsSlash = true;
                                    IsMove = false;
                                    WalkStop();
                                    chasaUnit.chasaCombat.EnemySlash();
                                    StartCoroutine("SlashUpdate");
                                    StopCoroutine("MoveUpdate");
                                    StartCoroutine("MoveUpdate", attackTime);
                                }
                            }
                        }
                    }

                    // 전투시
                    if (IsCombat)
                    {
                        if (dist < toDist && !IsDamage)
                        {
                            if (!IsAttack)
                            {
                                IsAttack = true;
                                IsMove = false;

                                WalkStop();

                                chasaUnit.chasaCombat.EnemyAttack();

                                LookTarget();

                                StopCoroutine("Attack");
                                StartCoroutine("Attack");

                                StopCoroutine("MoveUpdate");
                                StartCoroutine("MoveUpdate", attackTime);
                            }
                        }
                        // 플레이어가 멀어졌을 때
                        else
                        {
                            IsCombat = false;
                            IsUse = true;
                        }
                    }
                }
            }
        }

        public void LookTarget()
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
            transform.LookAt(targetPosition);
            //transform.localRotation = Quaternion.Euler(new Vector3(0, transform.localRotation.eulerAngles.y, 0));
        }

        private void WalkStop()
        {
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(transform.position);
                chasaUnit.chasaCharacter.m_Animator.SetFloat("Forward", 0.0f);
                chasaUnit.chasaCharacter.Move(Vector3.zero, false, false);
            }
        }
    }
}