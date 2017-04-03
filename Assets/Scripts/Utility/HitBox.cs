using UnityEngine;

namespace Chasa
{
    public class HitBox : MonoBehaviour
    {
        public static int hitBoxIdx;
        [HideInInspector]
        public ChasaUnit ChasaUnit;

        public string cullingTag;

        private void Start()
        {
            ChasaUnit = transform.parent.GetComponent<ChasaUnit>();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag(cullingTag))
                return;
            ChasaUnit.HitBoxEnter(other);
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag(cullingTag))
                return;
            ChasaUnit.HitBoxExit(other);
        }
    }
}