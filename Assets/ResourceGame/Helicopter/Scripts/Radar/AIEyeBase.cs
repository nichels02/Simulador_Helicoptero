using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEyeBase : MonoBehaviour
{
        protected int count = 0;
        protected Collider[] colliders = new Collider[50];
        public float distance = 10f;
        #region RangeView
        [Range(0, 180)]
        public float angle = 30f;
        public float height = 1.0f;
        public Color meshColor = Color.red;
        protected Mesh mesh;
        #endregion

        #region Rate
        protected int index = 0;
        protected float[] arrayRate;
        protected int bufferSize = 10;
        public float randomWaitScandMin = 1;
        public float randomWaitScandMax = 1;


        protected float Framerate = 0;
        #endregion
        public LayerMask Enemylayers;
        public LayerMask occlusionlayers;
        public Health health { get; set; }

        public bool IsDrawGizmo = false;
        public Transform AimOffset;
        public Health ViewEnemy;
        public Health ViewAllie;// { get; set; }

        public Vector3 Target { get; set; }

        public GameObject Memory { get; set; }

        public bool CantMemory { get { return Memory.transform.position == Vector3.zero; } }

        #region Direction and Distance
        public float DistanceEnemy
        {
            get
            {
                return (this.ViewEnemy != null) ? (transform.position - this.ViewEnemy.transform.position).magnitude : -1;
            }
        }
        public Vector3 DirectionEnemy
        {
            get
            {
                if (this.ViewEnemy != null)
                {
                    return (this.ViewEnemy.transform.position - transform.position).normalized;
                }
                return Vector3.zero;
            }
        }
        public float DistanceAllied
        {
            get
            {
                return (this.ViewAllie != null) ? (transform.position - this.ViewAllie.transform.position).magnitude : -1;
            }
        }
        public Vector3 DirectionAllied
        {
            get
            {
                if (this.ViewAllie != null)
                {
                    return (this.ViewAllie.transform.position - transform.position).normalized;
                }
                return Vector3.zero;
            }
        }
        //public Vector3 VelocityEnemy
        //{
        //    get
        //    {
        //        if (this.ViewAllie != null && this.ViewAllie.health.Rigidbody != null)
        //        {
        //            return this.ViewAllie.DeadUnit.Rigidbody.velocity;
        //        }
        //        return Vector3.zero;
        //    }
        //}
        public float DistanceTarget
        {
            get
            {
                return (transform.position - this.Target).magnitude;
            }
        }
        public Vector3 DirectionTarget
        {
            get
            {
                return (Target - transform.position).normalized;
            }
        }
        #endregion


    // Start is called before the first frame update
        void Start()
        {
            
        }
        public virtual void LoadComponent()
        {
            Memory = new GameObject("TagPlayer");
            Destroy(Memory.GetComponent<BoxCollider>());
            health = GetComponent<Health>();
            Framerate = 0;
            arrayRate = new float[bufferSize];
            for (int i = 0; i < arrayRate.Length; i++)
            {
                arrayRate[i] = (float)UnityEngine.Random.Range(randomWaitScandMin, randomWaitScandMax);
            }
        }
        private void OnDestroy()
        {
            Destroy(Memory);
        }
        public virtual void UpdateScan()
        {

            if (Framerate > arrayRate[index])
            {

                index++;
                index = index % arrayRate.Length;
                Scan();
                Framerate = 0;
            }
            Framerate += Time.deltaTime;
        }


        public virtual void Scan()
        {
            
        }
        //public virtual bool IsAllies(Health heatlhScan)
        //{
        //    for (int j = 0; (health != null && j <  health.typeAgentAllies.Count); j++)
        //    {
        //        if (health.typeAgentAllies[j] == heatlhScan.typeAgent)
        //        {
        //        return true;
        //        }
        //    }
           
        //return false;
        //}
        public virtual bool IsNotIsThis(GameObject obj)
        {
            return (this.gameObject.GetInstanceID() != obj.GetInstanceID());
        }
        public virtual bool IsInSight(Health obj)
        {
            Vector3 origin = transform.position;
            Vector3 dest = obj.transform.position;
            Vector3 direcction = dest - origin;

            if (direcction.magnitude > distance)
                return false;
            
            if (dest.y < -(height+transform.position.y) || dest.y > (height + transform.position.y))
            {
                return false;
            }
           
            direcction.y = 0;
       
        float deltaAngle = Vector3.Angle(direcction.normalized, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }


        if (Physics.Linecast(origin, dest, occlusionlayers))
            {
                return false;
            }

        return true;
        }
        Mesh CreateWedgeMesh()
        {
            Mesh mesh = new Mesh();
            int segments = 10;
            int numTriangles = (segments*4) +4 ;
            int numVertices = numTriangles * 3;
            Vector3[] vertices = new Vector3[numVertices];
            int[] triangles = new int[numVertices];

            Vector3 bottomCenter = Vector3.zero;
            Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
            Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

            Vector3 topCenter = bottomCenter + Vector3.up *height;
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
                bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
                bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

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

            
        }

    }
