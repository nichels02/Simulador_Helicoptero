using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivoteCamera : MonoBehaviour
{
    public List<GameObject> listCamera = new List<GameObject>();
    public List<Transform> listCameraPivot = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < listCamera.Count; i++)
        {
            listCamera[i].transform.parent = listCameraPivot[i];
            listCamera[i].transform.localPosition = Vector3.zero;
            listCamera[i].transform.rotation = listCameraPivot[i].rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
