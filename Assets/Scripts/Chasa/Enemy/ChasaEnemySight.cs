using UnityEngine;
using System.Collections;

namespace Chasa
{
    public class ChasaEnemySight : MonoBehaviour
    {
        public float radius;
        [Range(0, 360)]
        public float angle;

        public LayerMask targetMask;
        public LayerMask obstacleMask;

        [HideInInspector]
        public Transform target = null;

        private void Awake()
        {
            target = GameObject.Find("Player").transform;
        }

        public void TargetFindStart()
        {
            StartCoroutine("FindTarget");
        }

        public void TargetFindStop()
        {
            StopCoroutine("FindTarget");
        }

        public bool IsSee = false;

        IEnumerator FindTarget()
        {
            while (true)
            {
                if (FindVisibleTargets())
                {
                    IsSee = true;
                    break;
                }
                else
                {
                    IsSee = false;
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        bool FindVisibleTargets()
        {
            Vector3 currentPosition = transform.position + new Vector3(0, 0.5f, 0);
            if (target == null)
                target = GameObject.Find("Player").transform;
            Vector3 targetPosition = target.position;
            targetPosition.y = currentPosition.y;

            Vector3 dirToTarget = (targetPosition - currentPosition).normalized;

            float distAngle = Vector3.Angle(transform.forward, dirToTarget);

            if (distAngle < angle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if(radius >= distToTarget)
                {
                    if (!Physics.Raycast(currentPosition, dirToTarget, distToTarget, obstacleMask))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public Vector3 DirFromAngle(float angleInDgree, bool angleInGlobal)
        {
            if (!angleInGlobal)
            {
                angleInDgree += transform.eulerAngles.y;
            }

            return new Vector3(Mathf.Sin(angleInDgree * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDgree * Mathf.Deg2Rad));
        }
    }
}