﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterCameraBase : MonoBehaviour
{
    public Transform baseHelicopter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position ,baseHelicopter.position,Time.deltaTime*20f);
    }
}
