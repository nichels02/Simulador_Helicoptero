using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarControllerIAAntiAerea : Radar
{
    public float rate;
    float FrameRate = 0;
    public float distanceView = 10f;
    public Transform Antena;
    public Transform BaseTurrent;
    //public MiniGunIA _MiniGunIA;
    public float Distance
    {
        get
        {
            if (GetNearEnemy() != null)
                return Direction.magnitude;
            return -1;
        }
    }
    public Vector3 Direction
    {
        get
        {
            if (GetNearEnemy() != null)
            {
                 Health enemy = GetNearEnemy();
                return (enemy.transform.position - transform.position);
            }
            
            return Vector3.zero;
        }
    }
    [Range(0, 180)]
    public float angle = 30f;
    [Range(0, 180)]
    public float angleShoot = 30f;
    Mesh mesh;
    Mesh meshShoot;
    public float height = 1.0f;
    public float heightShoot = 1.0f;
    public Color meshColor = Color.red;
    public Color meshShootColor = Color.yellow;
    public bool IsDrawGizmoMesh = false;
    public bool IsDrawGizmoMeshColor = false;
    public LayerMask maskColliderEnemy;
    public LayerMask maskColliderObstacle;
    public IKBase IKBase;
    #region Antimissile


    public Flash _Flash;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        ColliderMissile = LayerMask.GetMask("ColliderMissilePlayer");
        this.LoadComponent();
    }
    public override void LoadComponent()
    {
        IKBase = GetComponent<IKBase>();
        base.LoadComponent();
    }
    void Update()
    {

        if (FrameRate > rate)
        {
            Scan();
            FrameRate = 0;
        }
        FrameRate += Time.deltaTime;

        Antena.Rotate(Vector3.up* Time.deltaTime * 100f, Space.Self); 
    }
    public void ActiveIK(Transform enemy)
    {
        IKBase.IKActive = true;
        IKBase.target = enemy.position;
        Debug.LogWarning("ActiveIK");
    }
    public void DesactiveIK()
    {
        IKBase.IKActive = false;
        IKBase.target = Vector3.forward;
    }
    void Scan()
    {
        Collider[] coll = Physics.OverlapSphere(transform.position, distanceView, maskColliderEnemy, QueryTriggerInteraction.Ignore);

        enemys.Clear();
        foreach (var item in coll)
        {
            if (IsInSight(item.transform))
            {
                Health h = item.gameObject.GetComponent<Health>();
                if(!h.isDead)
                    enemys.Add(h);
            }
        }

    }
    bool IsInSight(Transform obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;

        if (Physics.Linecast(origin, dest, maskColliderObstacle))
            return false;

        Vector3 direcction = dest - origin;
        if (direcction.magnitude > distanceView)
            return false;

        float deltaAngle = Vector3.Angle(direcction.normalized, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }
        return true;
    }
    public Health IsInSightShoot(Health enemy)
    {
        
        if (enemy == null) return enemy;
        Vector3 origin = BaseTurrent.position;
        Vector3 dest = enemy.transform.position;
        Vector3 direcction = dest - origin;

        meshShootColor = Color.yellow;

        if (Physics.Linecast(origin, dest, maskColliderObstacle))
            return null;
        if (direcction.magnitude > distanceView)
            return null;

        float deltaAngle = Vector3.Angle(direcction.normalized, BaseTurrent.forward);
        
        if (deltaAngle > angleShoot)
        {
            return null;
        }
        if (deltaAngle > heightShoot)
        {
            return null;
        }

        meshShootColor = Color.red;
        
        return enemy;
    }
    public bool InFire()
    {
        Health enemy = GetNearEnemy();
        if (enemy != null)
        {
            Vector3 origin = transform.position;
            Vector3 dest = enemy.transform.position;
            Vector3 direcction = (dest - origin).normalized;

            float deltaAngle = Vector3.Angle(direcction, transform.forward);

            if (deltaAngle > angleShoot)
                return false;
            return true;
        }
        return false;
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
    Mesh CreateWedgeMeshShoot()
    {
        Mesh mesh = new Mesh();
        int segments = 10;
        int numTriangles = (segments * 4) + 4;
        int numVertices = numTriangles * 3;
        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angleShoot, 0) * BaseTurrent.forward * distanceView;
        Vector3 bottomRight = Quaternion.Euler(0, angleShoot, 0) * BaseTurrent.forward * distanceView;

        Vector3 topCenter = bottomCenter + BaseTurrent.up * heightShoot;
        Vector3 topLeft = bottomLeft + BaseTurrent.up * heightShoot;
        Vector3 topRight = bottomRight + BaseTurrent.up * heightShoot;

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

        float currentAngle = -angleShoot;
        float deltaAngle = (angleShoot * 2) / segments;
        for (int i = 0; i < segments; ++i)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * BaseTurrent.forward * distanceView;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * BaseTurrent.forward * distanceView;

            topRight = bottomRight + BaseTurrent.up * heightShoot;
            topLeft = bottomLeft + BaseTurrent.up * heightShoot;

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
        meshShoot = CreateWedgeMeshShoot();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == (int)Mathf.Log(ColliderMissile, 2)&& _Flash!=null)
        {
            LayerMask mask = LayerMask.GetMask("ColliderFlashEnemy");
            _Flash.gameObject.layer = (int)Mathf.Log(mask, 2);
            _Flash.shootAntimissile();
        }
    }
    private void OnDrawGizmos()
    {
       
        if (mesh && IsDrawGizmoMesh)
        {
           // meshColor.a = 0.2f;
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }
        if (meshShoot && IsDrawGizmoMeshColor)
        {
          //  meshShootColor.a = 0.2f;
            Gizmos.color = meshShootColor;
            Gizmos.DrawMesh(meshShoot, BaseTurrent.position, BaseTurrent.rotation);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Antena.position, Antena.position + Antena.forward * distanceView);


            ////Scan();
            //Transform enemy = enemys[0];
            //if (enemy != null)
            //{
            //    Gizmos.color = Color.yellow;
            //    Gizmos.DrawSphere(enemy.position, 5f);
            //    Gizmos.color = Color.yellow;
            //    Gizmos.DrawLine(transform.position, enemy.position);

            //    _MiniGunIA.UpdateFire();
            //    _MiniGunIA.directionShoot(enemy);
            //}
        }
       

    }


}
