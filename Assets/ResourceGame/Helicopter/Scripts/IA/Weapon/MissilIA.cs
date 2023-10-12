using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilIA : Missil
{
   
    // Start is called before the first frame update
    void Awake()
    {
        LoadComponent();
    }
    public override void LoadComponent()
    {
        base.LoadComponent();
        LayerFlash = LayerMask.GetMask("ColliderFlashPlayer");
        masckCollider = LayerMask.GetMask("ColliderPlayer", "ColliderObject");
        
        LayerMask mask = LayerMask.GetMask("ColliderMissileEnemy");
        this.gameObject.layer = (int)Mathf.Log(mask, 2);
       
        targetObjective = target;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        HealthBase candidate = GetNearEnemy();

        if(candidate!=null && !((Health)candidate).isDead)
        {
            float distance = (candidate.transform.position - transform.position).magnitude;
            base.CalculateDirecction(candidate);

            if (distance < radius)
            {
                base.Desactive();
                return;
            }
        }
        rg.velocity = Vector3.Lerp(rg.velocity, directionmisil * Acceleration, Time.deltaTime * 10);
        transform.rotation = Quaternion.LookRotation(rg.velocity);
    }
    //private void OnCollisionExit(Collision collision)
    //{
    //    if (onwer != null && collision.gameObject.GetInstanceID() == onwer.GetInstanceID()) return;
        
    //    if (collision.gameObject.layer == (int)Mathf.Log(masckCollider, 2))
    //        base.Desactive();
    //}
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.layer == (int)Mathf.Log(LayerFlash, 2))
        {
           SetTarget(other.gameObject.GetComponent<Health>());
        }
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
        
        // Gizmos.DrawWireSphere(transform.position + transform.forward * distanceRay, radius);
    }
}
