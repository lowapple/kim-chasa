using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Chasa
{
    public class PoolObjectsManager : MonoBehaviour
    {
        [System.Serializable]
        public class PoolObject
        {
            public string object_name;
            public GameObject object_pf;
            public int object_num;
        }

        public PoolObject[] objects;

        public void Start()
        {
            Create();
        }

        public void Create()
        {
            for (int i = 0; i < objects.Length; i++)
            {
                SceneManager.instance.poolObjects.CreateObject(objects[i].object_name, objects[i].object_pf, objects[i].object_num);
            }
        }
    }
}