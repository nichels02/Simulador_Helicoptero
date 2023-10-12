using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
public class CameraManager : MonoBehaviour
{
    //public List<GameObject> CameraMachine = new List<GameObject>();
    public Cinemachine.CinemachineVirtualCamera VCamera;
    CinemachineImpulseSource ImpulseSource;
    float scrollY = 0;
    private void Start()
    {
        ImpulseSource = VCamera.GetComponent<CinemachineImpulseSource>();
    }
    public void ImpulseCamera(float impulse)
    {
        ImpulseSource.GenerateImpulse(impulse);
    }
    private void Update()
    {
        if (InputsHelicopter.instance.mouseScrollY > 0)
            VCamera.m_Lens.FieldOfView = Mathf.Clamp(VCamera.m_Lens.FieldOfView - 5f, 15, 60);
        else
           if (InputsHelicopter.instance.mouseScrollY < 0)
            VCamera.m_Lens.FieldOfView = Mathf.Clamp(VCamera.m_Lens.FieldOfView + 5f, 15, 60);
    }
    
}
    
     
