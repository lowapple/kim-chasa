using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Chasa
{
    [CustomEditor(typeof(ChasaPlayerCombat))]
    public class ChasaCombatEditorPlayer : Editor
    {
        private void OnSceneGUI()
        {
            ChasaPlayerCombat combat = (ChasaPlayerCombat)target;

            Handles.color = Color.red;
            Handles.DrawWireArc(combat.transform.position, Vector3.up, Vector3.forward, 360, combat.attackRange);
        }
    }
}