using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthObject : Health
{
    private void Start()
    {
        masckColliderTerrain = LayerMask.GetMask("ColliderObject");
        masckColliderMissil = LayerMask.GetMask("ColliderMissilePlayer");
        this.LoadComponent();
    }
    public override void LoadComponent()
    {
        base.LoadComponent();
    }

    
    private void OnParticleCollision(GameObject other)
    {
        Damage(2);
    }
    public override void Damage(int damage)
    {
        base.Damage(damage);
        if (isDead)
        {
            GameObject obj = null;
            switch (TypeObjetive)
            {
                case TypeObjetive.Air:
                    obj = Instantiate(ExplotionAir, transform.position, Quaternion.identity).gameObject;
                    break;
                case TypeObjetive.Land:
                    obj = Instantiate(ExplotionTerrain, transform.position, Quaternion.identity).gameObject;
                    break;
                default:
                    break;
            }

            if (obj != null)
                Destroy(obj, 25);

            rg.velocity = Vector3.zero;

            if (ExecutEvent != null)
                ExecutEvent.Invoke();
            Destroy(this.gameObject);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.layer == (int)Mathf.Log(masckColliderMissil, 2))
        {
           
            this.Damage(50);
        }


    }
}
