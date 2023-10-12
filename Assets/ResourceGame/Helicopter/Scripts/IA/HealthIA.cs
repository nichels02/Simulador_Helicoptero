using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public  class HealthIA: Health
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

    private void Update()
    {
        if (rg.isKinematic && isDead)
            Dead();
    }
    private void OnParticleCollision(GameObject other)
    {
        Damage(2);
    }
    public override void Damage(int damage)
    {
        base.Damage(damage);
        if (isDead )
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

            if(obj != null)
                Destroy(obj, 25);

            rg.velocity = Vector3.zero;

            if (ExecutEvent != null)
                ExecutEvent.Invoke();
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == (int)Mathf.Log(masckColliderTerrain, 2) && (isDead))
        {
            GameObject obj = Instantiate(ExplotionTerrain, transform.position, Quaternion.identity).gameObject;
            if (obj != null)
                Destroy(obj, 25);

            Destroy(this.gameObject);
        }
        //else
        //if (collision.gameObject.layer == (int)Mathf.Log(masckColliderMissil, 2))
        //{
        //    Debug.Log("masckColliderMissil: " + collision.gameObject.name);
        //    this.Damage(50);
        //}


    }
}
