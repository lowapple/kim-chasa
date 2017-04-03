using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class Spawn : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.instance.CharacterMove(transform);
        }
    }
}