using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class PoolObjects : MonoBehaviour
    {
        private Dictionary<string, GameObject[]> pool_objects;

        private void Awake()
        {
            pool_objects = new Dictionary<string, GameObject[]>();
        }

        public void HideObject()
        {
            var keys = pool_objects.Keys;
            foreach (var key in keys)
            {
                for (int i = 0; i < pool_objects[key].Length; i++)
                {
                    pool_objects[key][i].gameObject.SetActive(false);
                }
            }
        }

        public GameObject GetObject(string object_name)
        {
            // 해당 오브젝트가 존재하는지 찾아본다.
            if (pool_objects.ContainsKey(object_name))
            {
                GameObject[] tempObjects = pool_objects[object_name];
                for (int i = 0; i < tempObjects.Length; i++)
                {
                    if(tempObjects[i] != null)
                        if (!tempObjects[i].activeSelf)
                            return tempObjects[i];
                }
            }
            return null;
        }

        // 오브젝트를 생성한다.
        public void CreateObject(string object_name, GameObject object_pf, int object_num)
        {
            GameObject parent = GameObject.Find("PoolObjects");
            GameObject tempParent = new GameObject();
            tempParent.name = object_name;
            tempParent.transform.parent = parent.transform;

            GameObject[] tempObjects = new GameObject[object_num];
            for (int i = 0; i < object_num; i++)
            {
                tempObjects[i] = Instantiate(object_pf);
                tempObjects[i].SetActive(false);
                tempObjects[i].name = object_name + " : " + i.ToString();
                tempObjects[i].transform.SetParent(tempParent.transform);
            }

            pool_objects.Add(object_name, tempObjects);
        }

        public void LifeTime(GameObject _object, float sec)
        {
            StartCoroutine(LifeTimeUpdate(_object, sec));
        }

        IEnumerator LifeTimeUpdate(GameObject _object, float sec)
        {
            yield return new WaitForSeconds(sec);

            _object.SetActive(false);
        }
    }
}