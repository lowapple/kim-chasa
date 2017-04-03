using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chasa
{
    public class HelpSoul : SoulScriptBase
    {
        private void Awake()
        {
            script = OK;
        }

        public void OK()
        {
            gameObject.SetActive(false);
            gameObject.GetComponent<MissionRequest>().Request();
        }
    }
}