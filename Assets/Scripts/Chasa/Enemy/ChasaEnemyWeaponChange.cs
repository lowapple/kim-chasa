using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    [RequireComponent(typeof(ChasaEnemyWeapon))]
    public class ChasaEnemyWeaponChange : MonoBehaviour
    {
        private ChasaEnemyUnit chasaUnit;

        [System.Serializable]
        public class WeaponChangeData
        {
            public string weaponChangeAnimationName;
            public string weaponName;
        }

        public WeaponChangeData[] weaponChangeDatas;

        public WeaponChangeData currentWeapon;
        private WeaponChangeData changeWeapon;

        private void Start()
        {
            chasaUnit = GetComponent<ChasaEnemyUnit>();
        }

        public void FirstWeapon()
        {
            currentWeapon = null;
            changeWeapon = weaponChangeDatas[0];
            WeaponChange();
        }
        
        private void WeaponChange()
        {
            // 무기가 존재한다면
            if (currentWeapon != null && changeWeapon != null)
            {
                chasaUnit.chasaCharacter.m_Animator.SetTrigger(currentWeapon.weaponChangeAnimationName);
            }
            else if(currentWeapon == null && changeWeapon != null)
            {
                currentWeapon = changeWeapon;
                changeWeapon = null;
                chasaUnit.chasaCharacter.m_Animator.SetTrigger(currentWeapon.weaponChangeAnimationName);
            }
        }
        
        // 애니메이션에서 무기를 바꾼다.
        public void CurrentWeaponChange()
        {
            if (changeWeapon == null && currentWeapon != null)
            {
                chasaUnit.chasaWeapon.ChangeWeapon(currentWeapon.weaponName);
                chasaUnit.chasaWeapon.OnWeapon();
            }
            else if(changeWeapon != null && currentWeapon != null)
            {
                currentWeapon = null;
                chasaUnit.chasaWeapon.OffWeapon();
                WeaponChange();
            }
        }
    }
}