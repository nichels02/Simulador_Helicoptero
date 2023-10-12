using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilPlayer : Missil
{
    // Start is called before the first frame update
    void Awake()
    {
        LoadComponent();
    }
    public override void LoadComponent()
    {
        
        LayerFlash = LayerMask.GetMask("ColliderFlashEnemy");
        masckCollider = LayerMask.GetMask("ColliderEnemy", "ColliderObject");
        LayerMask mask = LayerMask.GetMask("ColliderMissilePlayer");
        this.gameObject.layer = (int)Mathf.Log(mask,2);
        base.LoadComponent();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        HealthBase candidate = GetNearEnemy();

        if (candidate != null && !((Health)candidate).isDead)
        { 
            base.CalculateDirecction(candidate);

            float distance = (candidate.transform.position - transform.position).magnitude;

            if (distance < radius)
            {
                this.Desactive();
                return;
            }
        }
        rg.velocity = Vector3.Lerp(rg.velocity, directionmisil * Acceleration, Time.deltaTime * 10);
        transform.rotation = Quaternion.LookRotation(rg.velocity);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (onwer != null && collision.gameObject.GetInstanceID() == onwer.GetInstanceID()) return;
        base.Desactive();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == (int)Mathf.Log(LayerFlash, 2))
        {
            SetTarget(other.gameObject.GetComponent<Health>());
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        //Gizmos.DrawWireSphere(transform.position+ transform.forward * distanceRay, radius);
    }
}
