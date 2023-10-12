using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IA
{
    
    public class AIEye : MonoBehaviour
    {
        public float distanceView = 10f;
        public float Distance {
            get {
                if (ViewPlayer != null)
                    return Direction.magnitude;
                return -1;
            } 
        }
        public Vector3 Direction {
            get {
                if (ViewPlayer != null)
                    return (ViewPlayer.position - transform.position);
                return Vector3.zero;
            }
        }
        [Range(0,180)]
        public float angle = 30f;
        Mesh mesh;
        int count = 0;
        public float height = 1.0f;
        public Color meshColor = Color.red;
        public Transform ViewPlayer;
        public bool IsDrawGizmo = false;
        // Start is called before the first frame update
        void Start()
        {
            
            
        }
        public bool IsInSight(GameObject obj)
        {
            Vector3 origin = transform.position;
            Vector3 dest = obj.transform.position;
            Vector3 direcction = (dest - origin);
            
            direcction.y = 0;

            float deltaAngle = Vector3.Angle(direcction, transform.forward);

            if (deltaAngle > angle)
                return false;
            
            //if (Physics.Linecast(origin, dest, occlusionlayers))
            //{
            //    return false;
            //}
            return true;
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                if (IsInSight(other.gameObject))
                    ViewPlayer = other.transform;
                else
                    ViewPlayer = null;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                ViewPlayer = null;
            }
        }
        Mesh CreateWedgeMesh()
        {
            Mesh mesh = new Mesh();
            int segments = 10;
            int numTriangles = (segments * 4) + 4;
            int numVertices = numTriangles * 3;
            Vector3[] vertices = new Vector3[numVertices];
            int[] triangles = new int[numVertices];

            Vector3 bottomCenter = Vector3.zero;
            Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distanceView;
            Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distanceView;

            Vector3 topCenter = bottomCenter + Vector3.up * height;
            Vector3 topLeft = bottomLeft + Vector3.up * height;
            Vector3 topRight = bottomRight + Vector3.up * height;

            int vert = 0;

            // left side
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomLeft;
            vertices[vert++] = topLeft;

            vertices[vert++] = topLeft;
            vertices[vert++] = topCenter;
            vertices[vert++] = bottomCenter;

            // right side
            vertices[vert++] = bottomCenter;
            vertices[vert++] = topCenter;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomCenter;

            float currentAngle = -angle;
            float deltaAngle = (angle * 2) / segments;
            for (int i = 0; i < segments; ++i)
            {
                bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distanceView;
                bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distanceView;

                topRight = bottomRight + Vector3.up * height;
                topLeft = bottomLeft + Vector3.up * height;

                // far side
                vertices[vert++] = bottomLeft;
                vertices[vert++] = bottomRight;
                vertices[vert++] = topRight;

                vertices[vert++] = topRight;
                vertices[vert++] = topLeft;
                vertices[vert++] = bottomLeft;
                // top 
                vertices[vert++] = topCenter;
                vertices[vert++] = topLeft;
                vertices[vert++] = topRight;
                // bottom 
                vertices[vert++] = bottomCenter;
                vertices[vert++] = bottomRight;
                vertices[vert++] = bottomLeft;

                currentAngle += deltaAngle;

            }


            for (int i = 0; i < numVertices; ++i)
            {
                triangles[i] = i;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;

        }
        private void OnValidate()
        {
            mesh = CreateWedgeMesh();
        }

        private void OnDrawGizmos()
        {
            if (!IsDrawGizmo) return;

            if (mesh)
            {
                Gizmos.color = meshColor;
                Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
            }

            for (int i = 0; i < count; i++)
            {
                Gizmos.color = Color.green;
                if(ViewPlayer)
                 Gizmos.DrawSphere(ViewPlayer.position, 0.5f);
            }


        }
    }
}