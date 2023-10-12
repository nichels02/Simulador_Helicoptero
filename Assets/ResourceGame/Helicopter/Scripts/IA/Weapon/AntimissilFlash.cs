using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntimissilFlash : MonoBehaviour
{
    Collider collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        //StartCoroutine(ActivateAntiMissil());
        collider.enabled = true;
    }
    IEnumerator ActivateAntiMissil()
    {
        yield return new WaitForSeconds(0.001f);
        collider.enabled = true;
       
    }
   
}
