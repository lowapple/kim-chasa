using UnityEngine;

namespace Chasa
{
    public class Slash : MonoBehaviour
    {
        ChasaPlayerUnit player;
        public WeaponTrail[] weaponTrails;

        public void Awake()
        {
            player = GameObject.Find("Player").GetComponent<ChasaPlayerUnit>();
            weaponTrails = GetComponentsInChildren<WeaponTrail>();
        }

        public void SlashActive()
        {
            for (int i = 0; i < weaponTrails.Length; i++)
            {
                weaponTrails[i].Activate();
            }
            GetComponent<Animator>().Play("Slash");
        }

        public void OnDisable()
        {
            for (int i = 0; i < weaponTrails.Length; i++)
            {
                weaponTrails[i].Deactivate();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                player.Damage(5, true);
            }
        }
    }
}