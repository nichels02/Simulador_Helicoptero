using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missil : MonoBehaviour
{
    public TypeObjetive ThisTypeObjetive;
    public TypeObjetive EnemyTypeObjetive { get; set; }

    public ParticleSystem ExplotionDefaul;
    public Health targetObjective { get; set; }
    public Health target { get; set; }
    public GameObject onwer { get; set; }
    public LayerMask masckCollider;
    protected Rigidbody rg { get; set; }
    public float Acceleration;
    public LayerMask LayerFlash;
    public Vector3 directionmisil { get; set; }
    public float TimeAutoDestroy;
    Collider[] colliders;
    public int damage;
    public float radius;
    //public float distanceRay;
    // Start is called before the first frame update
    void Awake()
    {
        
       
    }
    public void SetTarget(HealthBase _target)
    {
        target = (Health)_target;
        targetObjective = (Health)_target;
    }
    public virtual void LoadComponent()
    {
        rg = GetComponent<Rigidbody>();

        rg.AddForce(transform.forward * Acceleration,ForceMode.Acceleration);

        rg.velocity = transform.forward;
        
        colliders = GetComponents<Collider>();

        foreach (var item in colliders)
        {
            item.enabled = false;
        }

        StartCoroutine(ActivateMissil());

        Destroy(this.gameObject, TimeAutoDestroy);
       
    }
    IEnumerator ActivateMissil()
    {
        yield return new WaitForSeconds(0.2f);
        foreach (var item in colliders)
        {
            item.enabled = true;
        }

    }
    public void CalculateDirecction(HealthBase candidate)
    {
        if (candidate != null && ThisTypeObjetive == candidate.TypeObjetive)
        {
            directionmisil = (candidate.transform.position - transform.position).normalized;
        }
    }
    // Update is called once per frame
    public HealthBase GetNearEnemy( )
    {
        if (target!=null&& targetObjective != null && target.GetInstanceID() == targetObjective.GetInstanceID())
            return target;
        
        float distanceTarget = 1000000;
        float distanceOther = 1000000;

        if (target != null)
            distanceTarget = (target.transform.position - transform.position).magnitude;
        
        if (targetObjective != null)
            distanceOther = (targetObjective.transform.position - transform.position).magnitude;

        if (distanceOther< distanceTarget)
            return targetObjective;

        if (distanceTarget < distanceOther)
            return target;

        return null;

    }
    public void Desactive()
    {
        var expl = Instantiate(ExplotionDefaul, transform.position, Quaternion.identity);
        DestroyParticle dp = expl.GetComponent<DestroyParticle>();
        if (dp != null)
            dp.Explotion(damage);
        Destroy(this.gameObject);
    }
    
}
