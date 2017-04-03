using UnityEngine;

namespace Chasa
{
    public class ChasaCharacter : MonoBehaviour
    {
        [SerializeField]
        float m_MovingTurnSpeed = 360;
        [SerializeField]
        float m_StationaryTurnSpeed = 180;
        [SerializeField]
        float m_JumpPower = 12f;
        [SerializeField]
        float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
        [SerializeField]
        float m_MoveSpeedMultiplier = 1f;
        [SerializeField]
        float m_AnimSpeedMultiplier = 1f;
        [SerializeField]
        float m_GroundCheckDistance = 0.1f;

        public Rigidbody m_Rigidbody;
        [HideInInspector]
        public Animator m_Animator;
        [HideInInspector]
        public bool m_IsGrounded;
        float m_OrigGroundCheckDistance;
        const float k_Half = 0.5f;
        float m_TurnAmount;
        float m_ForwardAmount;
        Vector3 m_GroundNormal;
        float m_CapsuleHeight;
        Vector3 m_CapsuleCenter;
        CapsuleCollider m_Capsule;
        bool m_Crouching;

        public float moveSpeed;
        public bool IsDoubleJumpReady;
        public bool IsEnemy = false;
        private bool IsFootStep = false;

        [SerializeField]
        private Vector3 movePosition;

        void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            m_CapsuleHeight = m_Capsule.height;
            m_CapsuleCenter = m_Capsule.center;
            m_OrigGroundCheckDistance = m_GroundCheckDistance;
        }

        public void SetSpeed(float speed)
        {
            moveSpeed = speed;
        }

        public void Move(Vector3 move, bool run, bool jump)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired
            // direction.
            if (run)
                move *= 2;
            movePosition = move;
            if (move.magnitude > 1f)
                move.Normalize();
            move = transform.InverseTransformDirection(move);

            CheckGroundStatus();

            move = Vector3.ProjectOnPlane(move, m_GroundNormal);
            m_TurnAmount = Mathf.Atan2(move.x, move.z);
            m_ForwardAmount = move.z;

            ApplyExtraTurnRotation();

            // control and velocity handling is different when grounded and airborne:
            if (m_IsGrounded)
            {
                HandleGroundedMovement(jump);
            }
            PreventStandingInLowHeadroom();

            // send input and other state parameters to the animator
            UpdateAnimator(move);
        }

        public void Stop()
        {
            m_Animator.SetFloat("Forward", 0.0f);
            m_Animator.SetFloat("Turn", 0.0f);
            m_Animator.SetFloat("Jump", 0.0f);
        }

        void ScaleCapsuleForCrouching(bool crouch)
        {
            if (m_IsGrounded && crouch)
            {
                if (m_Crouching)
                    return;
                m_Capsule.height = m_Capsule.height / 2f;
                m_Capsule.center = m_Capsule.center / 2f;
                m_Crouching = true;
            }
            else
            {
                Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
                float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
                if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    m_Crouching = true;
                    return;
                }
                m_Capsule.height = m_CapsuleHeight;
                m_Capsule.center = m_CapsuleCenter;
                m_Crouching = false;
            }
        }

        void PreventStandingInLowHeadroom()
        {
            if (!m_Crouching)
            {
                Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
                float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
                if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    m_Crouching = true;
                }
            }
        }


        void UpdateAnimator(Vector3 move)
        {
            m_Animator.SetBool("OnGround", m_IsGrounded);

            if (!m_IsGrounded)
            {
                if (!IsEnemy)
                    m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
            }
            else
            {
                m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
                if (!IsEnemy)
                    m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
            }

            float runCycle = Mathf.Repeat(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);

            float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;

            if (m_IsGrounded)
            {
                if (jumpLeg != 0.0f)
                {
                    if (jumpLeg > 0.4f || jumpLeg < 0.4f)
                    {
                        if (!SceneManager.instance.character.chasaCombat.isRoll)
                        {
                            if (!IsFootStep)
                            {
                                IsFootStep = true;

                                if (IsEnemy)
                                {
                                    float dist = Vector3.Distance(transform.position, SceneManager.instance.character.transform.position);
                                    if(dist < 6)
                                        SceneManager.instance.soundManager.PlayEffect("Walk", 1f);
                                }
                                else
                                    SceneManager.instance.soundManager.PlayEffect("Walk", 1f);
                            }
                            else
                            {
                                if (jumpLeg < -0.4f && m_Animator.GetFloat("JumpLeg") > 0.4f ||
                                    jumpLeg > 0.4f && m_Animator.GetFloat("JumpLeg") < -0.4f)
                                {
                                    IsFootStep = false;
                                }
                            }
                        }
                    }
                }
                m_Animator.SetFloat("JumpLeg", jumpLeg);
            }

            if (m_IsGrounded && move.magnitude > 0)
            {
                m_Animator.speed = m_AnimSpeedMultiplier;
            }
            else
            {
                m_Animator.speed = 1;
            }
        }

        // 더블점프에서 호출 & 땅에서 호출
        private bool IsActiveDoubleJump = false;
        public void DoubleJump()
        {
            if (IsDoubleJumpReady && !IsActiveDoubleJump)
            {
                IsActiveDoubleJump = true;
                IsDoubleJumpReady = false;
                m_Rigidbody.drag = 0.5f;
                m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower * 1.8f, m_Rigidbody.velocity.z);
                m_Animator.SetTrigger("Double");
            }
        }

        void HandleGroundedMovement(bool jump)
        {
            if (jump && m_IsGrounded)
            {
                // jump!
                m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
                m_IsGrounded = false;
                m_Animator.applyRootMotion = false;
            }
        }

        void ApplyExtraTurnRotation()
        {
            //if (m_IsGrounded)
            //{
            float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
            transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
            //}
        }


        public void OnAnimatorMove()
        {
            if (Time.deltaTime > 0)
            {
                if (m_IsGrounded)
                {
                    Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;
                    v.y = m_Rigidbody.velocity.y;
                    m_Rigidbody.velocity = v;
                }
                else
                {
                    if (!IsEnemy)
                        m_Rigidbody.MovePosition(transform.position + (movePosition * (moveSpeed * 0.5f)));
                }
            }
        }

        void CheckGroundStatus()
        {
            RaycastHit hitInfo;
#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
            {
                IsDoubleJumpReady = false;
                m_GroundNormal = hitInfo.normal;
                m_IsGrounded = true;
                m_Animator.applyRootMotion = true;
                IsActiveDoubleJump = false;
            }
            else
            {
                m_IsGrounded = false;
                m_GroundNormal = Vector3.up;
                m_Animator.applyRootMotion = false;
            }
        }
    }
}