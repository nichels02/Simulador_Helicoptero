using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public  class HealthPlayer : Health
{
    bool band = false;
    CameraManager _CameraManager;

    [Header("Sound Engine")]
    public RandomPlaySound hitMetal = new RandomPlaySound();
    
    private void Start()
    {
        masckColliderTerrain = LayerMask.GetMask("ColliderObject");
        masckColliderMissil = LayerMask.GetMask("ColliderMissileEnemy");
        this.LoadComponent();
    }
    public override void LoadComponent()
    {
        _CameraManager = GetComponent<CameraManager>();
        hitMetal.Onwer = this.gameObject;
        hitMetal.Init();
        base.LoadComponent();
    }

    private void OnParticleCollision(GameObject other)
    {
        Damage(5);
        _CameraManager.ImpulseCamera(0.2f);
        hitMetal.Play();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == (int)Mathf.Log(masckColliderTerrain, 2))
        {

            if (isDead && !band)
            {
                GameObject obj = Instantiate(ExplotionTerrain, transform.position, Quaternion.identity).gameObject;
                Destroy(obj, 25);
                Destroy(this.gameObject);
                band = true;

                //GameManager.instance.StartResetScene(3);
                if (ExecutEvent != null)
                    ExecutEvent.Invoke();
            }
        }
        else
        if (collision.gameObject.layer == (int)Mathf.Log(masckColliderMissil, 2))
        {
            //Damage(50);
            _CameraManager.ImpulseCamera(2);
        }


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

            Dead();
        }

    }

    private void Update()
    {
        if(Thisburn!=null)
            Thisburn.Emit(1);
    }
}
