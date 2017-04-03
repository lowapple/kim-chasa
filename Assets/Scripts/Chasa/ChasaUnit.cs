using UnityEngine;
using System.Collections.Generic;

namespace Chasa
{
    public class ChasaUnit : MonoBehaviour
    {
        public delegate void DamageCallback();

        public float health;
        public int power;
        public int soul;

        DamageCallback damageCallback;

        public virtual void Damage(int power, bool stun = false)
        {
            SceneManager.instance.soundManager.PlayEffect("Hit1");

            damageCallback();
        }

        public void SetDamageCallback(DamageCallback callback)
        {
            damageCallback = callback;
        }

        public virtual void HitBoxEnter(Collider coll) { }
        public virtual void HitBoxExit(Collider coll) { }
    }
}