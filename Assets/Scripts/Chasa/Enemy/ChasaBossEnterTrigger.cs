using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class ChasaBossEnterTrigger : MonoBehaviour
    {
        public string bossRoomName;

        private void OnTriggerEnter(Collider other)
        {
            LoadingManager.instance.LoadScene(bossRoomName);
        }
    }
}