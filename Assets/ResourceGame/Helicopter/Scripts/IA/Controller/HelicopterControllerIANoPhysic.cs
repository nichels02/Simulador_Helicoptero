using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HelicopterRayColliderIA : HelicopterRayCollider
{
    public float DistanceForward = 300;
    public float DistanceDown = -20;
    public float DistanceLeftRight = 20;
    public HelicopterRayColliderIA()
    {


    }
}

public class HelicopterControllerIANoPhysic : HelicopterController
{
    public HelicopterRayColliderIA _HelicopterRayColliderIA = new HelicopterRayColliderIA();
    
    public Transform baseHelicopter;
    enum MoveDir { Forward = 1, Back = -1, Right = 1, Left = -1, None = 0 }
    [Range(0, 180)]
    public float Angle; /*{ get; set; }*/
    [Range(0, 150)]
    public float speed = 10f;
    public float speedTurn = 0.1f;
    [Range(0, 1)]
    public float speedTurnMax = 0.1f;
    Vector3 OldPosition;

    public Vector3 Acceleration { get; set; }
    Vector3 OldVelocity = Vector3.zero;
    public void UpdateVelocity()
    {
        float diff = Time.deltaTime;
        diff = (diff == 0) ? 1 : diff;

        Velocity = ((transform.position - OldPosition) / diff);
        OldPosition = transform.position;
        

        Acceleration = ((Velocity - OldVelocity) / diff);
        OldVelocity = Velocity;
       // Debug.Log("Velocity: "+Velocity.magnitude );
    }
   
    // Start is called before the first frame update
    void Start()
    {
        _HelicopterRayColliderIA.Init();
        _HelicopterRayColliderIA.transform = pivot;
        HelicopterModel = GetComponent<Rigidbody>();
        Init();
        OldPosition = transform.position;
        //Angle = 20; 
    }
    public override void Init()
    {
        base.Init();
    }
    public override void TurnOnEngine()
    {
        TurnEngine = true;
    }
    public override void TurnOffEngine()
    {
        TurnEngine = false;
    }
    public override void Up()
    {
        float realEffectiveHeightTop = EffectiveHeightCurrentTop - _HelicopterRayColliderIA.Height;
        float realEffectiveHeight = EffectiveHeight - _HelicopterRayColliderIA.Height;

        if (realEffectiveHeight < realEffectiveHeightTop && (HelicopterModel.position.y < EffectiveHeight))
        {
            
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * 10, Time.deltaTime * 10f);
        }


        if (EffectiveHeight < EffectiveHeightCurrentTop)
            EffectiveHeight = Mathf.Clamp(EffectiveHeight + 1f, 0, EffectiveHeightCurrentTop);



    }
    private void LateUpdate()
    {
        UpdateVelocity();
        _HelicopterRayColliderIA.ShootRay(EngineForce, TurnEngine);
        if(EffectiveHeightCurrentTop<(_HelicopterRayColliderIA.Height + EffectiveHeightTop))
        EffectiveHeightCurrentTop = Mathf.Abs(_HelicopterRayColliderIA.Height - (EffectiveHeightTop + EffectiveHeightCurrentTop));
    }
    void FixedUpdate()
    {
        
        Engine();
        TiltProcess();
    }
    //-------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------
    Vector3 ColliderRayPlaneXZ()
    {
        Vector3 correctPath = Vector3.zero;
        RaycastHit[] Hit = new RaycastHit[5];

        #region Forward
        Vector3 forward = transform.forward ;

        if (Physics.Raycast(transform.position,  forward.normalized, out Hit[(int)enumCollider.Forward], _HelicopterRayColliderIA.DistanceForward * 2, _HelicopterRayColliderIA.maskColliderHeith))
        {
            correctPath += Hit[(int)enumCollider.Forward].normal;
            
        }

        #endregion

        #region ForwardRight
        float midDistance = _HelicopterRayColliderIA.DistanceForward * 0.5f;
        Vector3 ForwardRight = (transform.forward + transform.right);

        if (Physics.Raycast(transform.position, ForwardRight, out Hit[(int)enumCollider.ForwardRight], midDistance, _HelicopterRayColliderIA.maskColliderHeith))
        {
            correctPath += Hit[(int)enumCollider.ForwardRight].normal;
        }

        #endregion

        #region Right
        float quarterDistance = _HelicopterRayColliderIA.DistanceForward * 0.25f;
        Vector3 Right = transform.position + transform.right ;

        if (Physics.Raycast(transform.position, Right, out Hit[(int)enumCollider.Right], quarterDistance, _HelicopterRayColliderIA.maskColliderHeith))
        {
            correctPath += Hit[(int)enumCollider.Right].normal;
        }
        #endregion

        #region ForwardLeft
        Vector3 ForwardLeft = (transform.forward - transform.right);

        if (Physics.Raycast(transform.position, ForwardLeft, out Hit[(int)enumCollider.ForwardLeft], midDistance, _HelicopterRayColliderIA.maskColliderHeith))
        {
            correctPath += Hit[(int)enumCollider.ForwardLeft].normal;
        }

        #endregion

        #region Left
        Vector3 Left = - transform.right ;

        if (Physics.Raycast(transform.position, Left, out Hit[(int)enumCollider.Left], quarterDistance, _HelicopterRayColliderIA.maskColliderHeith))
        {
            correctPath += Hit[(int)enumCollider.Left].normal;
        }
        #endregion

        return correctPath;
    }
    bool ColliderRayDown()
    {
        RaycastHit hit;
        return (Physics.Raycast(transform.position, Vector3.up * _HelicopterRayColliderIA.DistanceDown, out hit, _HelicopterRayColliderIA.DistanceDown, _HelicopterRayColliderIA.maskColliderHeith));
    }
    bool ColliderRayUp()
    {
        RaycastHit hit;
        return (Physics.Raycast(transform.position, Vector3.up * -_HelicopterRayColliderIA.DistanceDown, out hit, _HelicopterRayColliderIA.DistanceDown, _HelicopterRayColliderIA.maskColliderHeith));
    }
    //-------------------------------------------------------------------------------------------
    public void MoveToFront(Vector3 pos)
    {
        Vector3 collider = ColliderRayPlaneXZ();
        collider.y = 0;
        if (collider!=Vector3.zero)
        {
            speed = Mathf.Lerp(VelocityMax / 2, speed, Time.deltaTime * 5f);
            speedTurn = Mathf.Lerp(speedTurnMax / 2, speedTurn, Time.deltaTime * 5f);
        }
        else
        {
            speed = Mathf.Lerp(speed, VelocityMax, Time.deltaTime * 5f);
            speedTurn = Mathf.Lerp(speedTurn, speedTurnMax, Time.deltaTime * 5f);
        }
          

        Vector3 dir = (new Vector3(pos.x,transform.position.y,pos.z) - transform.position).normalized;

        Vector3 direction = (pos - transform.position).normalized ;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction + collider),Time.deltaTime * speedTurn );

        transform.position +=  (transform.forward) * (speed ) * Time.deltaTime;

        RotateForward(direction + collider);
        //RotateForward(transform.forward);

        RotateRight();

       // RotateUp(dir + collider);

    }
    public void MoveToPosition(Vector3 pos)
    {
        Vector3 collider = ColliderRayPlaneXZ();
        collider.y = 0;
        if (collider != Vector3.zero)
        {
            speed = Mathf.Lerp(VelocityMax / 2, speed, Time.deltaTime * 2f);
            speedTurn = Mathf.Lerp(speedTurnMax / 2, speedTurn, Time.deltaTime * 2f);
        }
        else
        {
            speed = Mathf.Lerp(speed, VelocityMax, Time.deltaTime * 2f);
            speedTurn = Mathf.Lerp(speedTurn, speedTurnMax, Time.deltaTime * 2f);
        }

        Vector3 dir = (new Vector3(pos.x, transform.position.y, pos.z) - transform.position).normalized;

        Vector3 direction = (pos - transform.position).normalized;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction + collider), Time.deltaTime * speedTurn*0.5f);

        transform.position += (transform.forward) * speed/2 * Time.deltaTime;

        RotateForward(direction + collider);

        RotateRight();

        RotateUp(dir + collider);

    }
    public void MoveToLookEnemy(Vector3 pos,Vector3 enemy)
    {
        Vector3 collider = ColliderRayPlaneXZ();
        collider.y = 0;
        if (collider != Vector3.zero)
        {
            speed = Mathf.Lerp(VelocityMax / 2, speed, Time.deltaTime * 2f);
            speedTurn = Mathf.Lerp(speedTurnMax / 2, speedTurn, Time.deltaTime * 2f);
        }
        else
        {
            speed = Mathf.Lerp(speed, VelocityMax, Time.deltaTime * 2f);
            speedTurn = Mathf.Lerp(speedTurn, speedTurnMax, Time.deltaTime * 2f);
        }


        Vector3 dir = (new Vector3(enemy.x, transform.position.y, enemy.z) - transform.position).normalized;

        Vector3 direction = (pos - transform.position).normalized + collider;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * speedTurn);

        //transform.position = Vector3.Lerp(transform.position, transform.position + direction * speed, Time.deltaTime * 20f);
        transform.position += direction * speed * Time.deltaTime;

        RotateForward(direction);

        RotateRight();

        RotateUp(dir + collider);
    }
    private float CalculateAngle(Vector3 direction)
    {
        return Vector3.SignedAngle(direction.normalized, transform.forward, Vector3.up);
    }
    void RotateForward( Vector3 direction)
    {
        float c = CalculateAngle(direction);
       // Debug.Log("CalculateAngle: " + Mathf.Abs(c) + " Angle: "+Angle);
        if (Mathf.Abs(c) > Angle)
        {

            Vector3 cross = Vector3.Cross(transform.forward, direction.normalized);
           
            if (cross.y > 0)
            {
                hMove.x = 1;
            }
            else
            {
                hMove.x = -1;
            }
        }
        else
        {
            hMove.x = Mathf.Lerp(hMove.x, 0, Time.deltaTime * 2.5f);
        }
    }
    void RotateRight()
    {
        if(ColliderRayUp())
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up* 2f, Time.deltaTime * 2f);

        if(ColliderRayDown())
            transform.position = Vector3.Lerp(transform.position, transform.position - Vector3.up * 2f, Time.deltaTime * 2f);

        if (Velocity.magnitude > 0)
            hMove.y = 1;
        else
        if (Velocity.magnitude < 0)
            hMove.y = -1;
        else
            hMove.y = Mathf.Lerp(hMove.y, 0.0f, Time.deltaTime * 3.5f);
    }
    void RotateUp(Vector3 dir)
    {
        Quaternion rot = Quaternion.LookRotation(dir);

        //HelicopterModel.transform.rotation = Quaternion.Lerp(HelicopterModel.transform.rotation, rot, Time.deltaTime *2* speedTurn);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 2 * speedTurn);
    }
    public override void TiltProcess()
    {

       
        if (TurnEngine && _HelicopterRayColliderIA.Height > 5)
        {
            float velocity = Velocity.magnitude;
            velocity = (velocity == 0) ? 1 : velocity;

            float velocityF = 1000 / velocity;
            ForwardTiltForce = Mathf.Clamp(10, 10, 100);

           

            hTilt.x = Mathf.Lerp(hTilt.x, hMove.x * TurnTiltForce, Time.deltaTime * speedTurn);
            hTilt.y = Mathf.Lerp(hTilt.y, hMove.y * ForwardTiltForce, Time.deltaTime * speedTurn);
            Quaternion rot = Quaternion.Euler(hTilt.y, baseHelicopter.localEulerAngles.y, -hTilt.x);
            baseHelicopter.localRotation = Quaternion.Lerp(baseHelicopter.localRotation, rot, Time.deltaTime * speedTurn);

        }
    }
    public override void Engine()
    {
        if (TurnEngine && EngineForce < EngineForceTop)
        {
            EngineForce = Mathf.Clamp(EngineForce + 1.1f, 0, EngineForceTop);
        }
        else
        if (!TurnEngine && EngineForce > 0f)
        {
            EngineForce = Mathf.Clamp(EngineForce - 1.1f, 0, EngineForceTop);
        }

    }
    public bool CantTakeOff { get { return (EngineForce == EngineForceTop); } }
    void GizmosRayCollider()
    {
        RaycastHit[] Hit = new RaycastHit[5];

        #region Forward
        Vector3 forward = transform.forward ;

        if (Physics.Raycast(transform.position,forward, out Hit[(int)enumCollider.Forward], _HelicopterRayColliderIA .DistanceForward* 2, _HelicopterRayColliderIA.maskColliderHeith))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Hit[(int)enumCollider.Forward].point, 5f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Hit[(int)enumCollider.Forward].point, Hit[(int)enumCollider.Forward].point + Hit[(int)enumCollider.Forward].normal * 50);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position+ forward* _HelicopterRayColliderIA .DistanceForward* 2);
            Gizmos.DrawSphere(transform.position+forward * _HelicopterRayColliderIA.DistanceForward * 2, 2f);
        }

        #endregion

        #region ForwardRight
        float midDistance = _HelicopterRayColliderIA.DistanceForward * 0.5f;
        Vector3 ForwardRight = (transform.forward + transform.right) ;

        if (Physics.Raycast(transform.position,  ForwardRight, out Hit[(int)enumCollider.ForwardRight], midDistance, _HelicopterRayColliderIA.maskColliderHeith))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Hit[(int)enumCollider.ForwardRight].point, 5f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Hit[(int)enumCollider.ForwardRight].point, Hit[(int)enumCollider.ForwardRight].point + Hit[(int)enumCollider.ForwardRight].normal * 50);

        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position+ ForwardRight * midDistance);
            Gizmos.DrawSphere(transform.position+ForwardRight * midDistance, 2f);
        }

        #endregion

        #region Right
        float quarterDistance = _HelicopterRayColliderIA.DistanceForward * 0.25f;
        Vector3 Right = transform.right;

        if (Physics.Raycast(transform.position, Right, out Hit[(int)enumCollider.Right], quarterDistance, _HelicopterRayColliderIA.maskColliderHeith))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Hit[(int)enumCollider.Right].point, 5f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Hit[(int)enumCollider.Right].point, Hit[(int)enumCollider.Right].point + Hit[(int)enumCollider.Right].normal * 50);
            //barricentro += Hit[(int)enumCollider.Right].point;
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position+ Right * quarterDistance);
            Gizmos.DrawSphere(transform.position + Right * quarterDistance, 2f);
        }
        #endregion

        #region ForwardLeft
        Vector3 ForwardLeft = (transform.forward - transform.right);
        if (Physics.Raycast(transform.position,ForwardLeft, out Hit[(int)enumCollider.ForwardLeft], midDistance, _HelicopterRayColliderIA.maskColliderHeith))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Hit[(int)enumCollider.ForwardLeft].point, 5f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Hit[(int)enumCollider.ForwardLeft].point, Hit[(int)enumCollider.ForwardLeft].point + Hit[(int)enumCollider.ForwardRight].normal * 50);

        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + ForwardLeft * midDistance);
            Gizmos.DrawSphere(transform.position + ForwardLeft * midDistance, 2f);
        }

        #endregion

        #region Left
        Vector3 Left = - transform.right ;

        if (Physics.Raycast(transform.position,Left, out Hit[(int)enumCollider.Left], quarterDistance, _HelicopterRayColliderIA.maskColliderHeith))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Hit[(int)enumCollider.Left].point, 5f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Hit[(int)enumCollider.Left].point, Hit[(int)enumCollider.Left].point + Hit[(int)enumCollider.Right].normal * 50);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position+Left * quarterDistance);
            Gizmos.DrawSphere(transform.position+Left * quarterDistance, 2f);
        }
        #endregion

        RaycastHit hit;
        #region YouhaveDownRay
        if (Physics.Raycast(transform.position, Vector3.up * _HelicopterRayColliderIA.DistanceDown, out hit, _HelicopterRayColliderIA.DistanceDown, _HelicopterRayColliderIA.maskColliderHeith))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 2f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(hit.point, hit.point + hit.normal * 50);
            Gizmos.DrawLine(transform.position, hit.point);

        }
        else
        {

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * _HelicopterRayColliderIA.DistanceDown);
            Gizmos.DrawSphere(transform.position + Vector3.up * _HelicopterRayColliderIA.DistanceDown, 2f);
        }

        //perpendicular
        //Vector3 forward1 =(transform.forward + Vector3.up) * distanceDown;

        //if (Physics.Raycast(transform.position, forward1 , out hit, distanceDown, HelicopterRayCollider.maskColliderHeith))
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawSphere(hit.point, 2f);
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawLine(hit.point, hit.point + hit.normal * 50);
        //    Gizmos.DrawLine(transform.position, hit.point);

        //}
        //else
        //{

        //    Gizmos.color = Color.green;
            
        //    Gizmos.DrawLine(transform.position, transform.position + forward1 * distanceDown);
        //    Gizmos.DrawSphere(transform.position + forward1* distanceDown, 2f);
        //}
        #endregion

        #region YouhaveUpRay

        if (Physics.Raycast(transform.position, Vector3.up * -_HelicopterRayColliderIA.DistanceDown, out hit, _HelicopterRayColliderIA.DistanceDown, _HelicopterRayColliderIA.maskColliderHeith))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 2f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(hit.point, hit.point + hit.normal * 50);
            Gizmos.DrawLine(transform.position, hit.point);

        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * -_HelicopterRayColliderIA.DistanceDown);
            Gizmos.DrawSphere(transform.position + Vector3.up * -_HelicopterRayColliderIA.DistanceDown, 2f);
        }

        
        #endregion
    }
    private void OnDrawGizmos()
    {
        if (IsGizmo == false) return;
        GizmosRayCollider();
    }
}
