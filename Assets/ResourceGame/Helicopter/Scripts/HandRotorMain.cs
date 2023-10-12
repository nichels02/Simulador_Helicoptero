using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RotateAxis
{
	public bool Active = false;
	#region Axis
	public HeliRotorController.Axis _Axis;
	
	public float LimitMin = 0;
	public float LimitMax = 0;
	#endregion
	public RotateAxis()
	{ 
	
	}
}
public class HandRotorMain : MonoBehaviour
{
	public Transform hand;

	public RotateAxis _RotateAxis_X = new RotateAxis();
	public RotateAxis _RotateAxis_Z = new RotateAxis();

	private float rotateZDegree;
	private float rotateXDegree;
	private Vector3 OriginalRotate;

	[Range(0, 3000)]
	public float _rotarSpeed;
	[Range(-200, 200)]
	public float _forceSpeed;

	public float RotarSpeedMax=3000;
	public float RotarSpeed
    {
        get { return _rotarSpeed; }
        set { _rotarSpeed = Mathf.Clamp(value, 0, RotarSpeedMax); }
    }
	public float ForceSpeedMax=200;
	public float ForceSpeed
	{
		get { return _forceSpeed; }
		set { _forceSpeed = Mathf.Clamp(value, 0, ForceSpeedMax); }
	}
	// Start is called before the first frame update
	void Start()
    {
		rotateZDegree = _RotateAxis_Z.LimitMin;
		OriginalRotate = hand.localEulerAngles;
		hand.localRotation = Quaternion.Euler(OriginalRotate.x, OriginalRotate.y, rotateZDegree);
		rotateXDegree = 0;
		OriginalRotate = hand.localEulerAngles;
		hand.localRotation = Quaternion.Euler(rotateZDegree, OriginalRotate.y, OriginalRotate.z);
	}

	// Update is called once per frame
	//   void Update()
	//   {
	//	RotateX();
	//	RotateZ();
	//}

	public void RotateX()
	{
		if (!_RotateAxis_X.Active) return;

		if (ForceSpeed > 0)
		{
			//rotateXDegree = Mathf.Clamp(rotateXDegree + 1, _RotateAxis_X.LimitMin, _RotateAxis_X.LimitMax);
			rotateXDegree = Mathf.Lerp(_RotateAxis_X.LimitMin, _RotateAxis_X.LimitMax, Time.deltaTime * 200f);
			OriginalRotate = hand.localEulerAngles;
		}
		else
		if (ForceSpeed ==0)
		{
			rotateXDegree = 0;
			OriginalRotate = hand.localEulerAngles;
		}
		else
		{
			rotateXDegree = Mathf.Lerp(_RotateAxis_X.LimitMax, _RotateAxis_X.LimitMin, Time.deltaTime * 2f);
			OriginalRotate = hand.localEulerAngles;
		}

		hand.localRotation = Quaternion.Lerp(hand.localRotation, Quaternion.Euler(rotateXDegree, OriginalRotate.y,OriginalRotate.z), Time.deltaTime * 2f);
	}
	public void ForceSpeedIncrement()
	{
		ForceSpeed = Mathf.Lerp(ForceSpeed, 1, Time.deltaTime * 20f);
		ForceSpeed = Mathf.Clamp(ForceSpeed, -1, 1);
	}
	public void ForceSpeedDecrement()
	{
		ForceSpeed = Mathf.Lerp(ForceSpeed, -1, Time.deltaTime * 20f);
		ForceSpeed = Mathf.Clamp(ForceSpeed, -1, 1);
	}
	public void ForceSpeedRelease()
	{
		ForceSpeed = Mathf.Lerp(ForceSpeed, 0, Time.deltaTime * 20f);
		ForceSpeed = Mathf.Clamp(ForceSpeed, -1, 1);
	}
	public void RotateZ()
	{
		if (!_RotateAxis_Z.Active) return;

		if (RotarSpeed > 30)
		{
			rotateZDegree = Mathf.Lerp(_RotateAxis_Z.LimitMin, _RotateAxis_Z.LimitMax,Time.deltaTime*200f);
			OriginalRotate = hand.localEulerAngles;
		}
		else
		{
			rotateZDegree = Mathf.Lerp(_RotateAxis_Z.LimitMax, _RotateAxis_Z.LimitMin, Time.deltaTime * 200f);
			OriginalRotate = hand.localEulerAngles;
		}
		hand.localRotation = Quaternion.Lerp(hand.localRotation, Quaternion.Euler(OriginalRotate.x, OriginalRotate.y, rotateZDegree), Time.deltaTime * 2f);
	}

 //   private void OnDrawGizmos()
 //   {
	//	RotateX();
	//	RotateZ();
	//}
}
