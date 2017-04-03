using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chasa
{
    public class CharacterInventory : MonoBehaviour
    {
        public Text health;
        public Text stemina;
        public Text power;
        public Text defence;
        
        public void LateUpdate()
        {
            health.text = "체력 : " + SceneManager.instance.character.health;
            stemina.text = "스테미너 : " + SceneManager.instance.character.stemina;
            power.text = "공격력 : " + SceneManager.instance.character.power;
            defence.text = "방어력 : " + SceneManager.instance.character.defence;
        }
    }
}