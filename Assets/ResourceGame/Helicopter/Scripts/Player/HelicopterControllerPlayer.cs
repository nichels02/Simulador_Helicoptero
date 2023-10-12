using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
[System.Serializable]
public class HelicopterRayColliderPlayer : HelicopterRayCollider
{
    
    public HelicopterRayColliderPlayer()
    {


    }
    public override void Init()
    {
        maskColliderHeith = LayerMask.GetMask("ColliderObject");
    }
}
public enum StateEngine { Start, Loop, LoopAir, Dead, None }
public class HelicopterControllerPlayer : HelicopterController
{

    public HelicopterRayColliderPlayer _HelicopterRayColliderPlayer = new HelicopterRayColliderPlayer();
    SoundHelicopter SoundH;
    private Transform m_Cam;
    float _Fuel;
    public float Fuel_Max;
    public float Fuel { get => _Fuel/ Fuel_Max; }
    //public StateEngine _StateEngine;
    #region FollowCamera
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    [Tooltip("How far in degrees can you move the camera down")]
    [Range(0,10)]
    public float Sencitivity = 1;
    private const float _threshold = 0.01f;
    #endregion

    #region Start,Update,FixeUpdate
    void Start()
    {
        _Fuel = Fuel_Max;
        _HelicopterRayColliderPlayer.Init();
        _HelicopterRayColliderPlayer.transform = this.transform;
        Init();
        m_Cam = Camera.main.transform;

        HelicopterModel = GetComponent<Rigidbody>();
        SoundH = GetComponent<SoundHelicopter>();
    }

    public override void Init()
    {
        base.Init();
        _HelicopterRayColliderPlayer.Init();
        //EngineStartSound.Onwer = this.gameObject;
        //EngineStartSound.Init();
        //EngineLoopSound.Onwer = this.gameObject;
        //EngineLoopSound.loop = true;
        //EngineLoopSound.Init();
        //EngineLoopAirSound.Onwer = this.gameObject;
        //EngineLoopAirSound.loop = true;
        //EngineLoopAirSound.Init();
    }
    // Update is called once per frame
    public void FixedUpdate()
    {
        if (!_Health.isDead)
        {
            _HelicopterRayColliderPlayer.ShootRay(EngineForce, TurnEngine);

            KeyPressed();
            Engine();
            LiftProcess();
            MoveProcess();
            TiltProcess();
        }
        else
            Dead();

    }

    
    public override void Dead()
    {
        hMove.x = 1;

        hMove.y = 0;

        hTilt.x = Mathf.Lerp(hTilt.x, hMove.x * TurnTiltForce, Time.deltaTime);

        hTilt.y = Mathf.Lerp(hTilt.y, hMove.y * ForwardTiltForce, Time.deltaTime);

        Quaternion rot = Quaternion.Euler(hTilt.y, HelicopterModel.transform.localEulerAngles.y, -hTilt.x);

        HelicopterModel.transform.rotation = Quaternion.Lerp(HelicopterModel.transform.rotation, rot, Time.deltaTime * 2f);

        HelicopterModel.AddRelativeForce(transform.up * -HelicopterModel.mass);

        //EngineLoopAirSound.UpdatePitch(0.2f);

        if (!BandDead)
        {
            foreach (var item in Rotores)
            {
                item.RotarSpeed = 900f;
            }
            //_StateEngine = StateEngine.Dead;
            BandDead = true;
        }

    }
    #endregion
    // Start is called before the first frame update


    #region HelicopterController
    public override void LiftProcess()
    {
        float HeightCurrentTop = HelicopterModel.position.y - _HelicopterRayColliderPlayer.pointHit.y;

        EffectiveHeightCurrentTop = Mathf.Abs(_HelicopterRayColliderPlayer.Height - (EffectiveHeightTop + EffectiveHeightCurrentTop));
        if (TurnEngine && (!InputsHelicopter.instance.crouch) && (SoundH._StateEngine == StateEngine.Loop || SoundH._StateEngine == StateEngine.LoopAir))
        {
            //if (HelicopterModel.position.y < EffectiveHeight)
            if (HeightCurrentTop < EffectiveHeightTop)
            {
                CalculateUpForce();

                HelicopterModel.AddRelativeForce(Vector3.up * upForce);
            }

        }

    }

    private void CalculateUpForce()
    {
        float realEffectiveHeightTop = EffectiveHeightCurrentTop - _HelicopterRayColliderPlayer.Height;
        float realEffectiveHeight = EffectiveHeight - _HelicopterRayColliderPlayer.Height;

        if (realEffectiveHeight < realEffectiveHeightTop)
        {
            upForce = 1 - Mathf.Clamp(HelicopterModel.position.y / EffectiveHeight, 0f, 1f);

            upForce = Mathf.Lerp(0f, EffectiveHeight, upForce) * (HelicopterModel.mass);
        }

    }

    public override void Up()
    {

        float realEffectiveHeightTop = EffectiveHeightCurrentTop - _HelicopterRayColliderPlayer.Height;
        float realEffectiveHeight = EffectiveHeight - _HelicopterRayColliderPlayer.Height;

        if (realEffectiveHeight < realEffectiveHeightTop)
            EffectiveHeight = Mathf.Clamp(EffectiveHeight + 1f, 0, EffectiveHeightCurrentTop);

    }
    public override void Down()
    {
        if (_HelicopterRayColliderPlayer.Height > 5)
        {
            EffectiveHeight = Mathf.Clamp(EffectiveHeight - 1f, 0, EffectiveHeightCurrentTop);
            HelicopterModel.AddRelativeForce(Vector3.up * -(HelicopterModel.mass));
        }

    }
    public override void MoveProcess()
    {
       
        if (TurnEngine && SoundH._StateEngine == StateEngine.LoopAir && (_HelicopterRayColliderPlayer.Height > _HelicopterRayColliderPlayer.CurrentHeight))
        {
            float speed = 1;
            if (InputsHelicopter.instance.sprint)
            {
                speed = 5;
            }
            Vector3 ForceForward = hMove.y * Vector3.forward * Mathf.Max(0f, Mathf.Abs(hMove.y) * ForwardForce * HelicopterModel.mass * speed);
            Vector3 ForceRight = hMove.x * Vector3.right * Mathf.Max(0f, Mathf.Abs(hMove.x) * ForwardForce * HelicopterModel.mass * speed);

            HelicopterModel.AddRelativeForce(ForceForward + ForceRight);
            HelicopterModel.velocity = Vector3.ClampMagnitude(HelicopterModel.velocity, VelocityMax);
            MoveForceRelative = ForceForward;
            
        }
    }
    public override void TiltProcess()
    {
        if (TurnEngine && SoundH._StateEngine == StateEngine.LoopAir && (_HelicopterRayColliderPlayer.Height > _HelicopterRayColliderPlayer.CurrentHeight))
        {
            LookShipCamera();

            hTilt.x = Mathf.Lerp(hTilt.x, hMove.x * TurnTiltForce, Time.deltaTime);

            hTilt.y = Mathf.Lerp(hTilt.y, hMove.y * ForwardTiltForce, Time.deltaTime);

            Quaternion rot = Quaternion.Euler(hTilt.y, HelicopterModel.transform.localEulerAngles.y, -hTilt.x);

            HelicopterModel.transform.rotation = Quaternion.Lerp(HelicopterModel.transform.rotation, rot, Time.deltaTime * 2f);
        }

    }
    public override void TurnOnEngine()
    {
        if (InputsHelicopter.instance.on && !TurnEngine && _Fuel > (10 * (_Fuel / 100)))
        {
            TurnEngine = true;
            ActiveEngine();
        }
    }
    public override void TurnOffEngine()
    {
        if (InputsHelicopter.instance.off && TurnEngine)
        {
            TurnEngine = false;
            DesactiveEngine();
        }
    }
    public override void Engine()
    {
        TurnOnEngine();

        TurnOffEngine();
        
        if (EngineForce > 0)
        {
            float f = EngineForce / EngineForceTop;
            _Fuel = Mathf.Lerp(_Fuel, Mathf.Clamp(_Fuel - f, 0, Fuel_Max), Time.deltaTime * 0.5f);
        }

        if (_Fuel < (25 * (_Fuel / 100)))
            TurnOffEngine();

        
    }
    private void KeyPressed()
    {
        if (SoundH._StateEngine == StateEngine.Loop ||
            SoundH._StateEngine == StateEngine.LoopAir)
        {
            #region Up Down
            if (InputsHelicopter.instance.jump)
            {
                Up();
            }
            if (InputsHelicopter.instance.crouch)
            {
                Down();
            }
            #endregion

            #region Horizontal  Vertical
            if (InputsHelicopter.instance.move.x > 0.6f || InputsHelicopter.instance.move.x < -0.6f)
            {
                hMove.x += InputsHelicopter.instance.move.x;

                hMove.x = Mathf.Clamp(hMove.x, -1, 1);

            }
            else
                hMove.x = 0f;

            if (InputsHelicopter.instance.move.y > 0.6f || InputsHelicopter.instance.move.y < -0.6f)
            {
                hMove.y += InputsHelicopter.instance.move.y;
                hMove.y = Mathf.Clamp(hMove.y, -1, 1);
            }
            else
                hMove.y = 0f;
            #endregion
        }
    }
    #endregion

    #region Sound Engine
    public void ActiveEngine()
    {
        SoundH._StateEngine = StateEngine.Start;
    }
    public bool InAirEngine()
    {
        return SoundH._StateEngine == StateEngine.LoopAir;
    }
    public void DesactiveEngine()
    {
        SoundH._StateEngine = StateEngine.None;
    }
    
    #endregion

    #region FollowCamera
    private void LookShipCamera()
    {
        if (TurnEngine && (_HelicopterRayColliderPlayer.Height > 2))
        {
            Quaternion rot = Quaternion.LookRotation(m_Cam.forward);
            HelicopterModel.transform.rotation = Quaternion.Lerp(HelicopterModel.transform.rotation, rot, Time.deltaTime * 2f);
        }

    }
    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (InputsHelicopter.instance.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            _cinemachineTargetYaw += InputsHelicopter.instance.look.x * Time.deltaTime * Sencitivity;
            _cinemachineTargetPitch -= InputsHelicopter.instance.look.y * Time.deltaTime * Sencitivity;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }
    private void LateUpdate()
    {
        CameraRotation();
        
    }
    float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (IsGizmo == false) return;
       
        RaycastHit hit;

        if (Physics.Raycast(pivot.position, Vector3.up * -1000, out hit, 10000, _HelicopterRayColliderPlayer.maskColliderHeith))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pivot.position, pivot.position + Vector3.up * -hit.distance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hit.point,0.5f);
            //_HelicopterRayColliderPlayer.CurrentHeight = hit.distance;
        }
        

    }
}
