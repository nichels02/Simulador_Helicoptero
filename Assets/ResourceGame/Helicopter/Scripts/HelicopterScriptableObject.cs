using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Helicopter/HelicopterObject")]
public class HelicopterScriptableObject : ScriptableObject
{
    [Range(0, 100)]
    public float TurnForce = 3f;

    [Range(0, 500)]
    public float ForwardForce = 40f;

    [Range(0, 100)]
    public float ForwardTiltForce = 15f;

    [Range(0, 100)]
    public float TurnTiltForce = 50f;

    [Range(0, 100)]
    public float turnTiltForcePercent = 20.5f;

    [Range(0, 300)]
    public float turnForcePercent = 270.3f;


    public float EffectiveHeight = 0f;
    [Range(0, 3000)]
    public float EffectiveHeightTop = 1000f;
    public float EngineForceTop = 100;
    //[Range(0, 100)]
    //public float TurnForce = 3f;

    //[Range(0, 100)]
    //public float ForwardForce = 40f;

    //[Range(0, 100)]
    //public float ForwardTiltForce = 15f;

    //[Range(0, 100)]
    //public float TurnTiltForce = 50f;

    //[Range(0, 100)]
    //public float turnTiltForcePercent = 20.5f;

    //[Range(0, 300)]
    //public float turnForcePercent = 270.3f;


    //public float EffectiveHeight = 0f;
    //[Range(0, 3000)]
    //public float EffectiveHeightTop = 1000f;
    //public float EngineForceTop = 100;
}
