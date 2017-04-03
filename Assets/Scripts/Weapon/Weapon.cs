using UnityEngine;

namespace Chasa
{
    public class Weapon : MonoBehaviour
    {
        [HideInInspector]
        public string weapon_name;

        [HideInInspector]
        public WeaponTrail[] weapon_trails;

        private void Start()
        {
            weapon_name = transform.name;
            weapon_trails = GetComponentsInChildren<WeaponTrail>();
        }

        public void WeaponTrailOn()
        {
            for(int i = 0; i < weapon_trails.Length; i++)
            {
                weapon_trails[i].Activate();
            }
        }
        public void WeaponTrailOff()
        {
            for (int i = 0; i < weapon_trails.Length; i++)
            {
                weapon_trails[i].Deactivate();
            }
        }
    }
}