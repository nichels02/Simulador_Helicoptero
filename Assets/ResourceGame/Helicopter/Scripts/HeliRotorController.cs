using System;
using UnityEngine;

//rename
public class HeliRotorController : MonoBehaviour
{
	public Transform blade;
	public Transform rotor;
	public enum Axis
	{
		X,
		Y,
		Z,
	}

	public bool InverceRotate = false;
	public Axis RotateAxis;
    private float _rotarSpeed;
    public float RotarSpeed
    {
        get { return _rotarSpeed; }
        set { _rotarSpeed = Mathf.Clamp(value,0,3000); }
    }

    private float rotateDegree;
    private Vector3 OriginalRotate;

    void Start ()
	{
		OriginalRotate = rotor.localEulerAngles;
		blade.gameObject.SetActive(false);
		//_Update();
	}

	void Update ()
	{
		_Update();
	}
	void _Update()
	{
		if (InverceRotate)
			rotateDegree -= RotarSpeed * Time.deltaTime;
		else
			rotateDegree += RotarSpeed * Time.deltaTime;

		rotateDegree = rotateDegree % 360;

		switch (RotateAxis)
		{
			case Axis.Y:
				RotateY();
				break;
			case Axis.Z:
				RotateZ();
				break;
			case Axis.X:
				RotateX();
				break;
			default:
				
				break;
		}
	}
	private void RotateY()
	{
		if (RotarSpeed > 1000)
		{
			if (!blade.gameObject.activeSelf)
			{
				blade.gameObject.SetActive(true);
				rotor.gameObject.SetActive(false);
				OriginalRotate = blade.localEulerAngles;
			}

			blade.localRotation = Quaternion.Euler(OriginalRotate.x, rotateDegree, OriginalRotate.z);

		}
		else
		{
			if (!rotor.gameObject.activeSelf)
			{
				rotor.gameObject.SetActive(true);
				blade.gameObject.SetActive(false);
				OriginalRotate = rotor.localEulerAngles;
			}

			rotor.localRotation = Quaternion.Euler(OriginalRotate.x, rotateDegree, OriginalRotate.z);
		}
	}

	private void RotateZ()
	{
		if (RotarSpeed > 1000)
		{
			if (!blade.gameObject.activeSelf)
			{
				blade.gameObject.SetActive(true);
				rotor.gameObject.SetActive(false);
				OriginalRotate = blade.localEulerAngles;
			}

			blade.localRotation = Quaternion.Euler(OriginalRotate.x, OriginalRotate.y, rotateDegree);

		}
		else
		{
			if (!rotor.gameObject.activeSelf)
			{
				rotor.gameObject.SetActive(true);
				blade.gameObject.SetActive(false);
				OriginalRotate = rotor.localEulerAngles;
			}

			rotor.localRotation = Quaternion.Euler(OriginalRotate.x, OriginalRotate.y, rotateDegree);
		}
	}

	private void RotateX()
	{
		if (RotarSpeed > 1000)
		{
			if (!blade.gameObject.activeSelf)
			{
				blade.gameObject.SetActive(true);
				rotor.gameObject.SetActive(false);
				OriginalRotate = blade.localEulerAngles;
			}

			blade.localRotation = Quaternion.Euler(rotateDegree,OriginalRotate.y, OriginalRotate.z );

		}
		else
		{
			if (!rotor.gameObject.activeSelf)
			{
				rotor.gameObject.SetActive(true);
				blade.gameObject.SetActive(false);
				OriginalRotate = rotor.localEulerAngles;
			}

			rotor.localRotation = Quaternion.Euler(rotateDegree, OriginalRotate.y, OriginalRotate.z);
		}
	}
}
