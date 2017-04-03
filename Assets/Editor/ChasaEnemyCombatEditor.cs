using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Chasa
{
    [CustomEditor(typeof(ChasaEnemyCombat))]
    public class ChasaEnemyCombatEditor : Editor
    {
        private void OnSceneGUI()
        {
            ChasaEnemyCombat combat = (ChasaEnemyCombat)target;

            Handles.color = Color.blue;
            Handles.DrawWireArc(combat.transform.position, Vector3.up, Vector3.forward, 360, combat.attackRange);

            Vector3 sightAngleA = combat.DirFromAngle(-combat.attackAngle / 2, false);
            Vector3 sightAngleB = combat.DirFromAngle(combat.attackAngle / 2, false);

            Handles.DrawLine(combat.transform.position, combat.transform.position + sightAngleA * combat.attackRange);
            Handles.DrawLine(combat.transform.position, combat.transform.position + sightAngleB * combat.attackRange);
        }
    }
}