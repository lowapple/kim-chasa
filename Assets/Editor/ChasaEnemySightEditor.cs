using UnityEngine;
using UnityEditor;

namespace Chasa
{
    [CustomEditor(typeof(ChasaEnemySight))]
    public class ChasaEnemySightEditor : Editor
    {
        private void OnSceneGUI()
        {
            ChasaEnemySight sight = (ChasaEnemySight)target;

            Handles.color = Color.white;
            Handles.DrawWireArc(sight.transform.position, Vector3.up, Vector3.forward, 360, sight.radius);

            Vector3 sightAngleA = sight.DirFromAngle(-sight.angle / 2, false);
            Vector3 sightAngleB = sight.DirFromAngle(sight.angle / 2, false);

            Handles.DrawLine(sight.transform.position, sight.transform.position + sightAngleA* sight.radius);
            Handles.DrawLine(sight.transform.position, sight.transform.position + sightAngleB* sight.radius);
        }
    }
}