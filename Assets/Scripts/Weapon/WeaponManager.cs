using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class WeaponManager : MonoBehaviour
    {
        public static WeaponManager instance;

        public bool isWeapon = false;

        public Weapon basicWeapon;

        [HideInInspector]
        public Weapon currentWeapon;

        [HideInInspector]
        public Weapon[] weapons;

        private void Start()
        {
            instance = this;

            weapons = GetComponentsInChildren<Weapon>();

            currentWeapon = basicWeapon;

            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].gameObject.SetActive(false);
            }

            currentWeapon.gameObject.SetActive(true);

            Invoke("WeaponTrailOff", 0.5f);
        }

        public void WeaponActive(string weapon_name)
        {
            isWeapon = true;
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i].weapon_name.CompareTo(weapon_name) == 0)
                {
                    GetComponent<FanManager>().enabled = false;

                    currentWeapon.gameObject.SetActive(false);
                    currentWeapon = weapons[i];
                    currentWeapon.gameObject.SetActive(true);
                    break;
                }
            }
        }

        public void WeaponDisable()
        {
            isWeapon = false;

            currentWeapon.gameObject.SetActive(false);
            WeaponActive("Fan");
            GetComponent<FanManager>().enabled = false;
        }

        public void WeaponTrailOn()
        {
            currentWeapon.WeaponTrailOn();
        }
        public void WeaponTrailOff()
        {
            currentWeapon.WeaponTrailOff();
        }
    }
}