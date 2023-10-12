using UnityEngine;

public class FollowTargetCamera : MonoBehaviour
{
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
    public float Sencitivity = 1;
    private const float _threshold = 0.01f;

    public InputsHelicopter _InputsHelicopter;
    void Start ()
	{
        _InputsHelicopter = GetComponent<InputsHelicopter>();
    }

    
    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_InputsHelicopter.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            _cinemachineTargetYaw += _InputsHelicopter.look.x * Time.deltaTime * Sencitivity;
            _cinemachineTargetPitch -= _InputsHelicopter.look.y * Time.deltaTime * Sencitivity;
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
}

