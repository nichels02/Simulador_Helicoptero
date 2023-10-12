using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class HelicopterHub
{
    [Header("Canvas")]
    public RectTransform CanvasRect;

    [Header("Hub Stabilizer")]
    public RectTransform HubStabilizer;
    [Header("Hub Vertical Rotation")]
    public RawImage HubVerticalRotation;
    [Header("Hub Angle Vertical Rotation")]
    public TextMeshProUGUI textVerticalRotation;
    [Header("Hub Horizontal Rotation")]
    public RawImage HubHorizontalRotation;
    [Header("Hub Angle Horizontal Rotation")]
    public TextMeshProUGUI textHorizontalRotation;

    [Header("Hub Fuel")]
    public Image ImageFuel;
    [Header("Hub distance")]
    public float distance = 150f;

    public Health enemy;
    public Transform transform;
    //public Transform Aim;
    public Camera MainCamera;
    public TargetIndicator indicator;
    public GameObject TargetIndicatorPrefab;
    public Rigidbody HelicopterRigibody;
    public HelicopterControllerPlayer _HelicopterControllerPlayer { get; set; }
    public HelicopterHub() { }
    public Vector2 look { get; set; }
    public void Init()
    {
        indicator = GameObject.Instantiate(TargetIndicatorPrefab, CanvasRect.transform).GetComponent<TargetIndicator>();
        indicator.gameObject.SetActive(false);

       
    }
    public void Update()
    {

       

        UpdateHubVecticalRotation();

        UpdateIndicator();

        ImageFuel.fillAmount = _HelicopterControllerPlayer.Fuel;
    }

    public void AddTargetIndicator(Health target)
    {

        indicator.InitialiseTargetIndicator(target, MainCamera, CanvasRect);
        indicator.gameObject.SetActive(true);
    }
    private void UpdateHubVecticalRotation()
    {
        Rect uvRect1 = HubVerticalRotation.uvRect;
        Rect uvRect2 = HubHorizontalRotation.uvRect;

        uvRect1.y = transform.rotation.eulerAngles.x;
        textVerticalRotation.text = uvRect1.y.ToString("0");
        uvRect2.y = transform.rotation.eulerAngles.y;
        textHorizontalRotation.text = uvRect2.y.ToString("0");

        HubVerticalRotation.uvRect = uvRect1;
        HubHorizontalRotation.uvRect = uvRect2;

        HubStabilizer.rotation = Quaternion.Euler(0,0, transform.rotation.eulerAngles.z);

    }
    private void UpdateIndicator()
    {
        if (indicator.gameObject.activeSelf)
        {
            indicator.UpdateTargetIndicator();
        }
    }

}
public class RadarController : Radar
{
    #region radaEye
    public float rate;
    float FrameRate = 0;
    public float distanceView = 10f;
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
    Mesh mesh;
    public float height = 1.0f;
    public Color meshColor = Color.red;
    public bool IsDrawGizmoMesh = false;
    public LayerMask maskColliderEnemy;
    public LayerMask maskColliderObstacle;
    // Start is called before the first frame update
    #endregion
    public TextMeshProUGUI textMissil;
    public HelicopterHub _HelicopterHub = new HelicopterHub();
    WeaponManager _WeaponManager;
    #region SoundHelicopter
    SoundHelicopter SoundH;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _HelicopterHub._HelicopterControllerPlayer = GetComponent<HelicopterControllerPlayer>();
        _HelicopterHub.MainCamera = Camera.main;
        _HelicopterHub.transform = transform;
        _HelicopterHub.HelicopterRigibody = GetComponent<Rigidbody>();
        _HelicopterHub.Init();
        ColliderMissile = LayerMask.GetMask("ColliderMissileEnemy");
        LoadComponent();
        _WeaponManager = GetComponent<WeaponManager>();
        SoundH = GetComponent<SoundHelicopter>();
    }
    
    public override void LoadComponent()
    {
        base.LoadComponent();
    }
    private void Update()
    {
        _HelicopterHub.enemy = GetNearEnemy();
        _HelicopterHub.look = InputsHelicopter.instance.look;
        _HelicopterHub.Update();

        if (FrameRate > rate)
        {
            Scan();
            FrameRate = 0;
        }
        FrameRate += Time.deltaTime;

    }
    void Scan()
    {
        Collider[] coll = Physics.OverlapSphere(transform.position, distanceView, maskColliderEnemy,QueryTriggerInteraction.Ignore);
    
        enemys.Clear();
        
        _HelicopterHub.indicator.gameObject.SetActive(false);
        
        foreach (var item in coll)
        {

            if (IsInSight(item.transform))
            {
                Health h = item.gameObject.GetComponent<Health>();
                if(!h.isDead)
                {
                    if (_WeaponManager.currentWeapon is WeaponMissilPlayer)
                    {
                        if (((WeaponMissilPlayer)_WeaponManager.currentWeapon).TypeObjetive == h.TypeObjetive)
                        {
                            _HelicopterHub.AddTargetIndicator(h);
                        }
                    }

                    enemys.Add(h);

                }
                
            }
        }
    }
    bool IsInSight(Transform obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direcction = dest - origin;

        if (Physics.Linecast(origin, dest, maskColliderObstacle))
            return false;

        if (direcction.magnitude > distanceView)
            return false;


        float deltaAngle = Vector3.Angle(direcction.normalized, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }

        return true;
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
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == (int)Mathf.Log(ColliderMissile, 2))
        {
            float dist = (other.gameObject.transform.position - transform.position).magnitude;
            textMissil.color = Color.red;
            textMissil.text="Missil Enemy: "+(int)dist;
            if(dist>250)
            {
                SoundH.I_detectedFarMissil();
            }
            else
            {
                SoundH.I_detectedNearMissil();
                
            }
                
        }
        else
        {
            textMissil.color = Color.green;
            textMissil.text = "Missil Enemy: 0";
            SoundH.DetectedNearMissil();
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == (int)Mathf.Log(ColliderMissile, 2))
        {
            textMissil.color = Color.green;
            textMissil.text = "Missil Enemy: 0";
            SoundH.DetectedNearMissil();
        }
    }
    private void OnDrawGizmos()
    {
        if (!IsDrawGizmoMesh) return;
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }
    }
    



}
