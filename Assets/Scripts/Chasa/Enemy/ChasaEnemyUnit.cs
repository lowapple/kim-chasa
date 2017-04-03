using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

namespace Chasa
{
    public class ChasaEnemyUnit : ChasaUnit
    {
        [HideInInspector]
        public ChasaEnemyBoss chasaBoss;
        [HideInInspector]
        public ChasaEnemyCombat chasaCombat;
        [HideInInspector]
        public ChasaEnemyAI chasaAI;
        [HideInInspector]
        public ChasaEnemySight chasaSight;
        [HideInInspector]
        public ChasaCharacter chasaCharacter;
        [HideInInspector]
        public MissionRequest missionRequest;
        public ChasaEnemyWeapon chasaWeapon;
        [HideInInspector]
        public ChasaEnemyWeaponChange chasaWeaponChange;

        private WeaponTrail[] weaponTrails;

        [HideInInspector]
        public Transform bloodParticle;

        // 보스 같이 여러가지 기술을 쓸 경우
        public bool ImBoss = false;
        // 시네마
        public bool ImRealBoss = false;

        public void Awake()
        {
            bloodParticle = transform.Find("BloodParticle");
            chasaCombat = GetComponent<ChasaEnemyCombat>();
            chasaAI = GetComponent<ChasaEnemyAI>();
            if (ImBoss)
            {
                chasaBoss = GetComponent<ChasaEnemyBoss>();
                chasaWeaponChange = GetComponent<ChasaEnemyWeaponChange>();
            }
            chasaWeapon = GetComponent<ChasaEnemyWeapon>();
            chasaSight = GetComponent<ChasaEnemySight>();
            chasaAI = GetComponent<ChasaEnemyAI>();
            chasaCharacter = GetComponent<ChasaCharacter>();
            chasaCharacter.IsEnemy = true;
            missionRequest = GetComponent<MissionRequest>();
            weaponTrails = GetComponentsInChildren<WeaponTrail>();
            for (int i = 0; i < weaponTrails.Length; i++)
            {
                weaponTrails[i].Deactivate();
            }
        }

        public void OnDisable()
        {
            chasaCharacter.enabled = false;
            chasaCombat.enabled = false;
            chasaAI.enabled = false;
            chasaAI.agent.enabled = false;
            chasaSight.enabled = false;
            for (int i = 0; i < weaponTrails.Length; i++)
            {
                weaponTrails[i].Deactivate();
            }
            var enemyErase = SceneManager.instance.poolObjects.GetObject("EnemyErase");
            if(enemyErase != null)
            {
                enemyErase.SetActive(true);
                enemyErase.transform.position = transform.position + new Vector3(0, 0.4f, 0);
                SceneManager.instance.poolObjects.LifeTime(enemyErase.gameObject, 0.75f);
            }
        }

        public void Alive()
        {
            chasaCharacter.enabled = true;
            chasaCombat.enabled = true;
            chasaCombat.IsAttackKey = true;
            chasaAI.enabled = true;
            chasaAI.IsAttack = false;
            chasaAI.IsDamage = false;
            chasaAI.agent.enabled = true;
            chasaSight.enabled = true;
            for (int i = 0; i < weaponTrails.Length; i++)
            {
                weaponTrails[i].Activate();
            }
        }

        // 플레이어에게 데미지를 입었을 때
        public override void Damage(int power, bool stun = false)
        {
            this.health -= power;

            if (this.health <= 0)
            {
                health = 0;
                if (!ImRealBoss)
                {
                    missionRequest.Request();
                    gameObject.SetActive(false);
                }
                return;
            }

            // 공격중이라면 중간에 맞지 않는다.
            if (!chasaAI.IsAttack)
            {
                if (Random.Range(0, 2) == 0)
                    this.GetComponent<Animator>().Play("Impact1");
                else
                    this.GetComponent<Animator>().Play("Impact2");
                SetDamageCallback(DamageFunction);
            }

            base.Damage(power);
        }

        private void DamageFunction()
        {

        }
       
        public override void HitBoxEnter(Collider coll)
        {
        }

        public override void HitBoxExit(Collider coll)
        {
        }
    }
}