using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AimIK.Properties;
using AimIK.Functions;
public class IKBase : MonoBehaviour
{
    //public Transform target;
    public Vector3 target { get; set; }
    public bool IKActive = false;
    public Part[] IKPart;

    // Start is called before the first frame update
    void Start()
    {
       
    }
    
    [ExecuteInEditMode]
    private void LateUpdate()
    {
        if (!IKActive  || target==null) return;

        foreach (Part item in IKPart)
        {
            item.part.LookAt3D(target - item.positionOffset, item.rotationOffset);
            item.part.CheckClamp3D(item.limitRotation, item.GetRotation());
        }

    }
    

    

}
