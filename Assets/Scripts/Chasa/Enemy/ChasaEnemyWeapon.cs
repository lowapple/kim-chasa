using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class ChasaEnemyWeapon : MonoBehaviour
    {
        public Transform right;

        [System.Serializable]
        public class EnemyWeapon
        {
            [HideInInspector]
            public Transform body;
            public Transform weapon;
            [HideInInspector]
            public Vector3 basicPosition;
            [HideInInspector]
            public Vector3 basicRotation;
            public Vector3 onWeaponPosition;
            public Vector3 onWeaponRotation;
            [HideInInspector]
            public WeaponTrail[] weaponTrails;
            [HideInInspector]
            public string weaponName;
        }

        public EnemyWeapon[] weapons;
        public EnemyWeapon currentWeapon;

        private void Start()
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].body = weapons[i].weapon.transform.parent;
                weapons[i].basicPosition = weapons[i].weapon.transform.localPosition;
                weapons[i].basicRotation = weapons[i].weapon.transform.localRotation.eulerAngles;
                weapons[i].weaponTrails = weapons[i].weapon.GetComponentsInChildren<WeaponTrail>();
                weapons[i].weaponName = weapons[i].weapon.transform.name;
            }
        }

        public void ChangeWeapon(string weaponName)
        {
            for(int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i].weaponName.Equals(weaponName))
                {
                    currentWeapon = weapons[i];
                }
            }
        }

        public void OnTrail()
        {
            for (int i = 0; i < currentWeapon.weaponTrails.Length; i++)
            {
                currentWeapon.weaponTrails[i].Activate();
            }
        }

        public void OnWeapon()
        {
            currentWeapon.weapon.transform.parent = right.transform;
            currentWeapon.weapon.transform.localPosition = currentWeapon.onWeaponPosition;
            currentWeapon.weapon.transform.localRotation = Quaternion.Euler(currentWeapon.onWeaponRotation);

            OnTrail();
        }

        public void OffTrail()
        {
            for (int i = 0; i < currentWeapon.weaponTrails.Length; i++)
            {
                currentWeapon.weaponTrails[i].Deactivate();
            }
        }

        public void OffWeapon()
        {
            currentWeapon.weapon.transform.parent = currentWeapon.body.transform;
            currentWeapon.weapon.transform.localPosition = currentWeapon.basicPosition;
            currentWeapon.weapon.transform.localRotation = Quaternion.Euler(currentWeapon.basicRotation);

            OffTrail();
        }
    }
}