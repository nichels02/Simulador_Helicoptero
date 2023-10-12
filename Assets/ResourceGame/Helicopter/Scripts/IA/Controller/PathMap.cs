using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace IA
{
    public class PathMap : MonoBehaviour
    {
        public List<Transform> checkpoints = new List<Transform>();
        public int index = 0;
        public float radioNode;
        public Color GizmoColor = Color.red;
        public bool pathFollow;
        public bool IsDrawGizmo = true;
        public float radiusShpereGizmos=5;
        private void Start()
        {
            
        }
        public Transform currentCheckPoint()
        {
            if(checkpoints.Count > 0)
            return checkpoints[index];
            return null;
        }
        public void GoToGoal(Transform player, float radio)
        {
            radioNode = radio;
            if ((player.position - currentCheckPoint().position).magnitude < radioNode)
            {
                GetCheckPoints();
            }

        }
        Transform GetCheckPoints()
        {
            index++;
            index = index%checkpoints.Count;
            return checkpoints[index];
        }

        //private void OnDrawGizmos()
        //{
        //    if (!IsDrawGizmo) return;

            
        //    if (checkpoints.Count == 0) return;
            
        //    for (int i = 0; i < checkpoints.Count-1; i++)
        //    {
        //        if (i==index)
        //            Gizmos.color = Color.yellow;
        //        else
        //            Gizmos.color = Color.blue;

        //        Gizmos.DrawWireSphere(checkpoints[i].position, radioNode);
        //        Gizmos.DrawSphere(checkpoints[i+1].position, radiusShpereGizmos);
        //        Gizmos.DrawLine(checkpoints[i].position, checkpoints[i + 1].position);
        //    }
        //}
    }
}