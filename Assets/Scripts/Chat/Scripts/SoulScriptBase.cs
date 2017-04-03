using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class SoulScriptBase : MonoBehaviour
    {
        public static SoulScriptBase currentTargetSoul;

        public delegate void Script();

        public Script script = null;
        
        [System.Serializable]
        public class ActiveObject
        {
            public GameObject gameObject;
            public bool active;
        }
        public ActiveObject[] activeObjects;

        [System.Serializable]
        public class ActiveAnimation
        {
            public Animator animator;
            public string animation_name;
        }
        public ActiveAnimation[] activeAnimations;
        
        public void CurrentSoul()
        {
            currentTargetSoul = this;
        }

        public void Active()
        {
            for (int i = 0; i < activeObjects.Length; i++)
                activeObjects[i].gameObject.SetActive(activeObjects[i].active);
            for (int i = 0; i < activeAnimations.Length; i++)
                activeAnimations[i].animator.Play(activeAnimations[i].animation_name);
        }
    }
}