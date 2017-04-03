using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Chasa
{
    public class ChasaPlayerUnit : ChasaUnit
    {
        public float stemina;
        public float defence;

        [HideInInspector]
        public Image healthbar;
        [HideInInspector]
        public Image steminabar;

        [HideInInspector]
        public ChasaPlayerCombat chasaCombat;
        [HideInInspector]
        public ChasaControl chasaControl;
        [HideInInspector]
        public ChasaCharacter chasaCharacter;

        public List<ChasaUnit> enemys = new List<ChasaUnit>();
        
        [SerializeField]
        private Image damageImage;
        [SerializeField]
        private Text plusCoinText;

        private bool isAlive = false;
        public bool isStun = false;

        private void Awake()
        {
            chasaCombat = GetComponent<ChasaPlayerCombat>();
            chasaControl = GetComponent<ChasaControl>();
            chasaCharacter = GetComponent<ChasaCharacter>();

            var healthbar = GameObject.Find("PlayerHealth");
            if (healthbar != null)
                this.healthbar = healthbar.GetComponent<Image>();
            var steminabar = GameObject.Find("PlayerStemina");
            if (steminabar != null)
                this.steminabar = steminabar.GetComponent<Image>();
            
            StartCoroutine("CoinUpdate");
        }

        public override void Damage(int power, bool stun = false)
        {
            if (health <= 0)
                return;
            
            if (chasaCombat.isRoll)
                return;

            if (stun)
            {
                StunEffect();
                KnockBack();
            }

            // 방어시 근접은 스테미나 조금 소비
            if (chasaCombat.defence)
            {
                Shaker.GetInstance.shake(0.1f, 0.5f, 0.8f);

                if (stemina >= 15)
                {
                    SceneManager.instance.soundManager.PlayEffect("Defence");

                    stemina -= 15;

                    chasaCharacter.m_Animator.Play("BlockStart");

                    StopCoroutine("AttackCounterReady");
                    Time.timeScale = 0.03f;
                    chasaCombat.isCounterAttackReady = true;
                    StartCoroutine("AttackCounterReady", 0.01f);

                    DefenceEffect();
                    
                    chasaCharacter.m_Animator.SetBool("Block", false);
                    chasaControl.enabled = true;
                    chasaCombat.isAttackKey = true;
                    chasaCombat.attackCount = 0;
                    chasaCombat.isAttack = false;
                    
                    return;
                }
                else
                {
                    stun = true;
                    KnockBack();
                }
            }

            health -= power;
            health += defence;

            StopCoroutine("DamageUpdate");
            StartCoroutine("DamageUpdate");

            Shaker.GetInstance.shake(0.05f, 0.3f, 0.2f);

            chasaCombat.isAttackKey = true;
            chasaCombat.attackCount = 0;
            chasaCombat.isAttack = false;

            if (!stun)
            {
                if (Random.Range(0, 2) == 0)
                    chasaCharacter.m_Animator.Play("Impact1");
                else
                    chasaCharacter.m_Animator.Play("Impact2");
            }

            SetDamageCallback(DamageFunction);

            base.Damage(power);
        }

        private void DamageFunction()
        {

        }

        public void PlusSoul(int plusSoul)
        {
            soul += plusSoul;
            StartCoroutine("PlusSoulUpdate", plusSoul);
        }

        IEnumerator PlusSoulUpdate(int plusSoul)
        {
            plusCoinText.gameObject.SetActive(true);
            plusCoinText.text = "+" + plusSoul.ToString();
            yield return new WaitForSeconds(2.5f);
            plusCoinText.gameObject.SetActive(false);
        }
        
        public void Alive()
        {
            health = 100;
            stemina = 100;
            power = 20;
            defence = 0;
            isAlive = false;
        }

        private void StunEffect()
        {
            GameObject stunAttack = SceneManager.instance.poolObjects.GetObject("Stun");
            if (stunAttack != null)
            {
                stunAttack.SetActive(true);
                stunAttack.transform.position = chasaCombat.leftHand.transform.position;
                stunAttack.GetComponent<ParticleSystem>().Play();
                stunAttack.transform.parent = transform;
                SceneManager.instance.poolObjects.LifeTime(stunAttack, stunAttack.GetComponent<ParticleSystem>().main.duration);

                chasaControl.enabled = false;
                chasaCombat.enabled = false;
                isStun = true;

                StartCoroutine("StunAttack");
            }
        }

        private void DefenceEffect()
        {
            GameObject tempAttack = SceneManager.instance.poolObjects.GetObject(chasaCombat.pool_defence_name);
            if (tempAttack != null)
            {
                tempAttack.SetActive(true);
                tempAttack.transform.position = chasaCombat.leftHand.transform.position;
                tempAttack.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
                tempAttack.GetComponent<ParticleSystem>().Play();
                SceneManager.instance.poolObjects.LifeTime(tempAttack, tempAttack.GetComponent<ParticleSystem>().main.duration);
            }
        }

        private void KnockBack()
        {
            chasaCharacter.m_Animator.SetTrigger("KnockBack");
        }

        // 히트박스
        public override void HitBoxEnter(Collider coll)
        {
            bool find = false;

            HitBox hitBox = coll.GetComponent<HitBox>();

            if (hitBox != null)
            {
                enemys.ForEach(p =>
                {
                    if (p == null)
                        enemys.Remove(p);
                    else
                    {
                        if (p.gameObject == hitBox.ChasaUnit.gameObject)
                        {
                            find = true;
                        }
                    }
                });

                if (!find)
                {
                    enemys.Add(hitBox.ChasaUnit);
                }
            }
        }

        public override void HitBoxExit(Collider coll)
        {

        }

        private void Update()
        {
            if (stemina < 100f)
            {
                stemina += Time.deltaTime * 20;
                if (stemina >= 100f)
                    stemina = 100f;
                if (steminabar != null)
                    steminabar.fillAmount = stemina / 100f;
            }
        }

        public void LateUpdate()
        {
            if (steminabar != null)
                steminabar.fillAmount = stemina / 100f;
            if (healthbar != null)
                healthbar.fillAmount = health / 100f;

            if (health <= 0)
            {
                if (!isAlive)
                {
                    isAlive = true;
                    
                    chasaCharacter.m_Animator.SetTrigger("Death");
                    chasaControl.enabled = false;
                    chasaCharacter.enabled = false;
                    chasaCombat.enabled = false;

                    SceneManager.instance.gameStateManager.Lose();
                }
                return;
            }
        }

        public IEnumerator AttackCounterReady(float time)
        {
            yield return new WaitForSeconds(time);
            Time.timeScale = 1.0f;
            chasaCombat.isCounterAttackReady = false;
        }

        public IEnumerator StunAttack()
        {
            yield return new WaitForSeconds(2.0f);

            chasaControl.enabled = true;
            chasaCombat.enabled = true;
            isStun = false;
        }

        public IEnumerator CoinUpdate()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.2f);
                SceneManager.instance.playerSoulText.text = soul.ToString();
            }
        }
        
        public IEnumerator DamageUpdate()
        {
            damageImage.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.1f);
            damageImage.color = new Color(1, 1, 1, 0);
        }
    }
}