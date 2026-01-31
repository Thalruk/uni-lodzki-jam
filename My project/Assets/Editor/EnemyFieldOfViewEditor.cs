using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyFieldOfView))]
public class EnemyFieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        EnemyFieldOfView fow = (EnemyFieldOfView)target;
        Handles.color = Color.white;

        Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.up, 360, fow.radius);

        Vector3 viewAngleA = fow.DirFromAngle(-fow.angle / 2);
        Vector3 viewAngleB = fow.DirFromAngle(fow.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.radius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.radius);

        if (fow.playerInView && fow.player != null)
        {
            Handles.color = Color.red;
            Handles.DrawLine(fow.transform.position, fow.player.transform.position);
        }
    }
}