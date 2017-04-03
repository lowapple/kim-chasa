using UnityEngine;
using UnityEditor;

namespace Chasa
{
    [CustomEditor(typeof(ChasaEnemyAI))]
    public class ChasaEnemyAIEditor : Editor
    {
        private void OnSceneGUI()
        {
            ChasaEnemyAI ai = (ChasaEnemyAI)target;

            Handles.color = Color.red;
            Handles.DrawWireArc(ai.transform.position, Vector3.up, Vector3.forward, 360, ai.toDist);
        }
    }
}