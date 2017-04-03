using System.Collections;
using UnityEngine;

namespace Chasa
{
    // 모든 공격방식은 bool 형으로 컨트롤 한다.
    public class ChasaEnemyCombat : MonoBehaviour
    {
        [System.Serializable]
        public class AttackData
        {
            public string attack_name;
            public string attack_particle_name;
            public bool attack_particle_always;
            public float attack_delay_time;
            public int attack_power;
        }

        [HideInInspector]
        public ChasaEnemyUnit chasaUnit;
        [HideInInspector]
        public ChasaPlayerUnit target;

        public bool IsAttackKey = true;
        [Range(0, 5)]
        public float attackRange = 0.0f;
        [Range(0, 180)]
        public float attackAngle = 0.0f;
        [SerializeField]
        private bool IsRandom = false;

        public AttackData[] attack_datas;

        [HideInInspector]
        public AttackData currentAttackData;

        private void Awake()
        {
            chasaUnit = GetComponent<ChasaEnemyUnit>();
            target = GameObject.Find("Player").GetComponent<ChasaPlayerUnit>();
        }

        public void SoundPlay(string sound)
        {
            SceneManager.instance.soundManager.PlayEffect(sound);
        }

        public void EnemyAttack()
        {
            if (!chasaUnit.ImBoss)
            {
                if (IsAttackKey)
                {
                    // 일반 몬스터는 공격이 하나밖에 없다.
                    chasaUnit.chasaCharacter.m_Animator.SetTrigger("Attack1");

                    if (attack_datas.Length > 0)
                        currentAttackData = attack_datas[0];
                    else
                        Debug.Log("Empty Data");
                }
            }
            else
            {
                string attackName = chasaUnit.chasaBoss.AttackActive(chasaUnit.chasaCharacter.m_Animator, IsRandom);
                for (int i = 0; i < attack_datas.Length; i++)
                {
                    if (attack_datas[i].attack_name.Equals(attackName))
                    {
                        currentAttackData = attack_datas[i];
                        break;
                    }
                }
            }

            chasaUnit.power = currentAttackData.attack_power;
        }

        public void EnemySlash()
        {
            chasaUnit.chasaCharacter.m_Animator.SetTrigger("Slash");
        }

        public void SlashAttack()
        {
            SceneManager.instance.soundManager.PlayEffect("Electric");
            chasaUnit.chasaBoss.AttackSlash();
        }

        public void Attack()
        {
            // 특수 공격이 포함될 수 있다.
            if (chasaUnit.ImBoss)
            {
                BossAttack();
            }
            else
            {
                NormalAttack();
            }
        }

        private void BossAttack()
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            GameObject tempAttackParticle = null;

            // 파티클
            for (int i = 0; i < attack_datas.Length; i++)
            {
                if (attack_datas[i].attack_name.Equals(chasaUnit.chasaBoss.currentAttackName))
                {
                    tempAttackParticle = SceneManager.instance.poolObjects.GetObject(attack_datas[i].attack_particle_name);
                    break;
                }
            }

            // 항상 파티클이 나옴
            if (currentAttackData.attack_particle_always)
            {
                if (tempAttackParticle != null && !target.chasaCombat.isRoll)
                {
                    ParticlePlay(tempAttackParticle);
                }
            }

            // 패턴별로 공격하는 방법이 다르다.
            if (chasaUnit.chasaBoss.attack_patern != null)
            {
                if (chasaUnit.chasaBoss.attack_patern.attack_type == ChasaEnemyBoss.AttackType.NORMAL_ATTACK)
                {
                    // 공격
                    if (dist < attackRange)
                    {
                        // 너무 가까우면 각도 검색이 안됨
                        if (dist > 1f)
                        {
                            if (AngleInTarget())
                            {
                                if (!currentAttackData.attack_particle_always)
                                {
                                    if (tempAttackParticle != null && !target.chasaCombat.isRoll)
                                    {
                                        ParticlePlay(tempAttackParticle);
                                    }
                                }
                                target.Damage(chasaUnit.power);
                            }
                        }
                        else
                        {
                            if (!currentAttackData.attack_particle_always)
                            {
                                if (tempAttackParticle != null && !target.chasaCombat.isRoll)
                                {
                                    ParticlePlay(tempAttackParticle);
                                }
                            }
                            target.Damage(chasaUnit.power);
                        }
                    }
                }
                else if (chasaUnit.chasaBoss.attack_patern.attack_type == ChasaEnemyBoss.AttackType.STUN)
                {
                    // 스턴 공격
                    if (dist < attackRange && AngleInTarget())
                    {
                        if (!currentAttackData.attack_particle_always)
                        {
                            if (tempAttackParticle != null && !target.chasaCombat.isRoll)
                            {
                                ParticlePlay(tempAttackParticle);
                            }
                        }
                        target.Damage(chasaUnit.power, true);
                    }
                }
            }
        }

        private void NormalAttack()
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            GameObject tempAttackParticle = null;

            if (attack_datas.Length > 0)
            {
                tempAttackParticle = SceneManager.instance.poolObjects.GetObject(attack_datas[0].attack_particle_name);
            }

            if (currentAttackData.attack_particle_always)
            {
                if (tempAttackParticle != null && !target.chasaCombat.isRoll)
                {
                    ParticlePlay(tempAttackParticle);
                }
            }

            if (dist < attackRange && AngleInTarget())
            {
                if (!currentAttackData.attack_particle_always)
                {
                    if (tempAttackParticle != null && !target.chasaCombat.isRoll)
                    {
                        ParticlePlay(tempAttackParticle);
                    }
                }

                target.Damage(chasaUnit.power);
            }
        }

        public Vector3 DirFromAngle(float angleInDgree, bool angleInGlobal)
        {
            if (!angleInGlobal)
            {
                angleInDgree += transform.eulerAngles.y;
            }

            return new Vector3(Mathf.Sin(angleInDgree * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDgree * Mathf.Deg2Rad));
        }

        bool AngleInTarget()
        {
            Vector3 currentPosition = transform.position + new Vector3(0, 0.5f, 0);
            Vector3 targetPosition = target.transform.position;
            targetPosition.y = currentPosition.y;

            Vector3 dirToTarget = (targetPosition - currentPosition).normalized;

            float angle = Vector3.Angle(transform.forward, dirToTarget);

            if (angle < attackAngle / 2)
                return true;
            else
                return false;
        }

        public void ParticlePlay(GameObject targetParticle)
        {
            if (targetParticle != null && !target.chasaCombat.isRoll)
            {
                targetParticle.SetActive(true);
                targetParticle.transform.position = SceneManager.instance.character.chasaCombat.leftHand.position;
                var particle = targetParticle.GetComponent<ParticleSystem>();
                if(particle != null)
                {
                    particle.Play();
                    SceneManager.instance.poolObjects.LifeTime(targetParticle, particle.main.duration);
                }
                else
                {
                    var particles = targetParticle.GetComponentsInChildren<ParticleSystem>();
                    float duration = 0.0f;
                    for(int i = 0; i < particles.Length; i++)
                    {
                        particles[i].Play();
                        if (particles[i].main.duration > duration)
                            duration = particles[i].main.duration;
                    }
                    SceneManager.instance.poolObjects.LifeTime(targetParticle, duration);
                }
            }
        }
    }
}