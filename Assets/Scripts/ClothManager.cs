using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class ClothManager : MonoBehaviour
    {
        public static ClothManager instance;

        [System.Serializable]
        public class ClothTexture
        {
            public string clothName;
            public Material body;
            public Material leg;
            public Material arm;
        }

        [System.Serializable]
        public class ClothHat
        {
            public string hatName;
            public GameObject hatObject;
        }

        public SkinnedMeshRenderer body;
        public SkinnedMeshRenderer leg;
        public SkinnedMeshRenderer[] arms;

        public ClothTexture[] clothTextures;
        public ClothHat[] clothHats;

        private void Start()
        {
            // ChangeCloth("GJW");
            // ChangeHat("Basic");
            instance = this;
        }

        private void OnDestroy()
        {
            instance = null;
        }

        public void ChangeCloth(string clothName)
        {
            for (int i = 0; i < clothTextures.Length; i++)
            {
                if (clothTextures[i].clothName.CompareTo(clothName) == 0)
                {
                    body.material = clothTextures[i].body;
                    leg.material = clothTextures[i].leg;
                    for (int j = 0; j < arms.Length; j++)
                        arms[j].material = clothTextures[i].arm;
                    break;
                }
            }
        }
        public void ChangeHat(string hatName)
        {
            ClothHat temp = null;
            for (int i = 0; i < clothHats.Length; i++)
            {
                if (clothHats[i].hatName.CompareTo(hatName) == 0)
                {
                    temp = clothHats[i];
                }
                clothHats[i].hatObject.gameObject.SetActive(false);
            }
            temp.hatObject.gameObject.SetActive(true);
        }
    }
}