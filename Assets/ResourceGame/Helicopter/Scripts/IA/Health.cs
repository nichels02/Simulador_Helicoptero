using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public  class Health: HealthBase
{
    

    public ParticleSystem burn;
    public ParticleSystem ExplotionAir;
    public ParticleSystem ExplotionTerrain;

    public int current_health;
    public int _health=100;

    public UnityEvent ExecutEvent;
    public bool isDead { get => current_health == 0; }


    protected HelicopterController _HelicopterController;
    
    protected Rigidbody rg;
    public List<MonoBehaviour> script = new List<MonoBehaviour>();
    protected LayerMask masckColliderTerrain ;
    public LayerMask masckColliderMissil;
    protected ParticleSystem Thisburn =null;
    private void Start()
    {
    }
    public virtual void LoadComponent()
    {
        current_health = _health;
        rg = GetComponent<Rigidbody>();
        _HelicopterController = GetComponent<HelicopterController>();
        MonoBehaviour[] scp = this.gameObject.GetComponentsInChildren<MonoBehaviour>();
        foreach (var item in scp)
        {
            if (this != item && 
                _HelicopterController != item && 
                !(item is HeliRotorController))
                script.Add(item);
        }

       

    }
    public void Dead()
    {
            if(Thisburn==null)
            {
                Thisburn = Instantiate(burn, transform);
                
                rg.isKinematic = false;
                //rg.velocity = Vector3.zero;
                rg.drag = 0;
                rg.angularDrag = 0.05f;
                if (_HelicopterController != null)
                {
                    float velocity = _HelicopterController.Velocity.magnitude;
                    rg.AddForce((transform.forward) * velocity * 50);
                }

                foreach (var item in script)
                {
                    Destroy(item);
                }

                script.Clear();

               
            }
    }
    public virtual void Damage(int damage)
    {
        
        current_health = (int)Mathf.Clamp(current_health - damage,0, _health);
       
    }
     
}
