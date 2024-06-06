using System;
using Runtime.Controllers.Enemy;
using UnityEditor;
using UnityEngine;
namespace NotRunTime
{
    
    [CustomEditor(typeof(EnemyFieldOfViewController))]
    public class EnemyFieldOfViewEditor : Editor
    {
        private void OnSceneGUI()
        {
            EnemyFieldOfViewController fov = (EnemyFieldOfViewController)target;
            Handles.color = Color.white;
            Handles.DrawWireArc(fov.transform.position,Vector3.up,Vector3.forward,360,fov.radius);


            Vector3 viewAngel01 = DirectionFromAngele(fov.transform.eulerAngles.y, -fov.angle / 2);
            Vector3 viewAngle02 = DirectionFromAngele(fov.transform.eulerAngles.y, fov.angle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(fov.transform.position,fov.transform.position + viewAngel01 * fov.radius);
            Handles.DrawLine(fov.transform.position,fov.transform.position + viewAngle02 * fov.radius);

            if (fov.canSeePlayer)
            {
                Handles.color = Color.green;
                Handles.DrawLine(fov.transform.position, fov.player.transform.position);
            }
        }

        private Vector3 DirectionFromAngele(float eulerY,float angleInDegrees)
        {
            angleInDegrees += eulerY;
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));

        }
    }
}