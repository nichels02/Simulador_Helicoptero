using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public enum enumCollider { Forward = 0, ForwardRight = 1, Right = 2, ForwardLeft = 3, Left = 4 }

[System.Serializable]
public class HelicopterRayCollider
{
    public Transform transform { get; set; }
    public float Height { get; set; }
    public float CurrentHeight;/*{ get; set; }*/
    public Vector3 pointHit { get; set; }
    RaycastHit hit = new RaycastHit();
    public ParticleSystem DustStorm;
    public LayerMask maskColliderHeith;

    //ParticleSystem.EmissionModule emissionModule;
    public HelicopterRayCollider()
    {
       
    }
    public virtual void Init()
    {
       
    }
    public void ShootRay(float EngineForce, bool TurnEngine)
    {

        if (Physics.Raycast(transform.position, Vector3.up * -1000000, out hit, 1000000, maskColliderHeith))
        {
            Height = hit.distance;
            pointHit = hit.point;
            if (Height < 2 && EngineForce > 30 && TurnEngine)
            {
                if (DustStorm != null)
                {
                    DustStorm.Emit(5);
                }
            }
            else
            {
                if (DustStorm != null)
                    DustStorm.Stop();
            }

        }
        else
        {
            Height = 1f;
            pointHit = Vector3.zero;
            if (DustStorm != null)
                DustStorm.Stop();
            
        }

    }
}

public class HelicopterController : MonoBehaviour
{
    public Rigidbody HelicopterModel { get; set; }

    public List<HeliRotorController> Rotores = new List<HeliRotorController>();

    public List<ParticleSystem> EmitteParticle = new List<ParticleSystem>();

    public Transform pivot;
     
    public HelicopterScriptableObject m_HelicopterScriptableObject;

    public Vector3 Velocity { get; set; }

    [Range(0,500)]
    public float VelocityMax;
    public float VelocityNormalice {get => Mathf.Clamp(HelicopterModel.velocity.magnitude/VelocityMax,0,1);}
    public float TurnForce { get; set; }

    public float ForwardForce { get; set; }

    public float ForwardTiltForce { get; set; }

    public float TurnTiltForce { get; set; }

    public float turnTiltForcePercent { get; set; }

    public float turnForcePercent { get; set; }


    public float EffectiveHeight;// { get; set; }

    public float EffectiveHeightTop; //{ get; set; }
    public float EffectiveHeightCurrentTop; //{ get; set; }
    public float EngineForceTop { get; set; }
    protected float _engineForce;
    public float EngineForce
    {
        get { return _engineForce; }
        set
        {
            foreach (var item in Rotores)
            {
                item.RotarSpeed = value *180;
            }
            _engineForce = value;
        }
    }
    public float EngineForceNormalize
    {
        get { return _engineForce/ EngineForceTop; }
        
    }
    protected Vector2 hMove = Vector2.zero;
    protected Vector2 hTilt = Vector2.zero;

    protected Vector3 ForceLiftProcess = Vector3.zero; 
    protected Vector3 MoveForceRelative = Vector3.zero;

    protected float hTurn = 0f;
    public bool IsOnGround { get; set; }
    public bool BandDead { get ; set; }
    public bool TurnEngine { get; set; }

    public bool IsGizmo = false;
    protected float upForce;
    public Health _Health { get; set; }
    // Use this for initialization
    void Start ()
	{
        
    }
    public virtual void Init()
    {
        
        TurnForce = m_HelicopterScriptableObject.TurnForce;
        ForwardForce = m_HelicopterScriptableObject.ForwardForce;
        ForwardTiltForce = m_HelicopterScriptableObject.ForwardTiltForce;
        TurnTiltForce = m_HelicopterScriptableObject.TurnTiltForce;
        turnTiltForcePercent = m_HelicopterScriptableObject.turnTiltForcePercent;
        turnForcePercent = m_HelicopterScriptableObject.turnForcePercent;
        EffectiveHeight = m_HelicopterScriptableObject.EffectiveHeight;
        EffectiveHeightTop = m_HelicopterScriptableObject.EffectiveHeightTop;
        EngineForceTop = m_HelicopterScriptableObject.EngineForceTop;

        _Health = GetComponent<Health>();
    }
    public virtual void Up()
    {
    }
    public virtual void Down()
    {
    }
    // to helicopter system
    public virtual void TurnOnEngine()
    { 
    
    }
    public virtual void TurnOffEngine()
    {

    }
    public virtual void Engine()
    {

    }
    public virtual void MoveProcess()
    {
        
    }

    public virtual void LiftProcess()
    {
        
    }

    public virtual void TiltProcess()
    {
        
    }
    public virtual void Dead()
    {

    }
    // params to input compoment
    // todo to input system;


    private void OnCollisionEnter()
    {
        IsOnGround = true;
    }

    private void OnCollisionExit()
    {
        IsOnGround = false;
    }

    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(0, 0, 200, 50), "Engine: " + EngineForce.ToString().Substring(0, 5));
    //    GUI.Label(new Rect(0, 5, 200, 50), "ForwardForce: " + ForwardForce.ToString().Substring(0, 5));
    //}
}