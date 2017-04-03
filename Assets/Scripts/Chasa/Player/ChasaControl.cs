using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

namespace Chasa
{
    public class ChasaControl : MonoBehaviour
    {
        private ChasaPlayerUnit chasaUnit;
        private Vector3 m_CamForward;
        private Vector3 m_Move;
        private bool m_Jump;

        public Transform m_Cam;

        [HideInInspector]
        public bool active_doubleJump = false;

        public Transform jump_position;
        public string pool_doubleJump_name;

        private bool isJump = false;
        public bool isAttackDelay = false;

        private void Start()
        {
            chasaUnit = GetComponent<ChasaPlayerUnit>();
        }

        private void Update()
        {
            if (chasaUnit.health <= 0)
                return;

            if (!chasaUnit.chasaCombat.isAttack)
                if (!m_Jump)
                    m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");

            if (Input.GetButtonDown("Jump") && !isAttackDelay && m_Jump)
            {
                if (!chasaUnit.chasaCharacter.IsDoubleJumpReady)
                {
                    active_doubleJump = true;
                    chasaUnit.chasaCharacter.IsDoubleJumpReady = true;

                    if (!isJump)
                    {
                        isJump = true;

                        GameObject tempJump = SceneManager.instance.poolObjects.GetObject(pool_doubleJump_name);
                        if (tempJump != null)
                        {
                            tempJump.transform.position = jump_position.transform.position;
                            tempJump.SetActive(true);
                            tempJump.GetComponent<ParticleSystem>().Play();
                            SceneManager.instance.poolObjects.LifeTime(tempJump, tempJump.GetComponent<ParticleSystem>().main.duration);
                        }
                    }
                }
            }

            CheckGroundStatus();
        }

        private void FixedUpdate()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = (v * m_CamForward + h * m_Cam.right) * 0.5f;

            bool run = false;
            if (Input.GetKey(KeyCode.LeftShift))
                run = true;

            chasaUnit.chasaCharacter.Move(m_Move, run, m_Jump);
            chasaUnit.chasaCharacter.DoubleJump();
            m_Jump = false;
        }

        void CheckGroundStatus()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, 0.3f))
            {
                isJump = false;
            }
        }

        public void AttackDelayStart()
        {
            isAttackDelay = true;

            StopCoroutine("AttackDelayUpdate");
            StartCoroutine("AttackDelayUpdate");
        }

        IEnumerator AttackDelayUpdate()
        {
            yield return new WaitForSeconds(0.4f);
            isAttackDelay = false;
        }
    }
}