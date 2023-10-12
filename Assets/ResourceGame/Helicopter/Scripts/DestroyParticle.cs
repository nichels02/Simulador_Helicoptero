using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    public float radiusDamage;
    public Color GizmoColor;
    public bool IsDrawGizmos=false;
    [Header("Sound Engine")]
    public PlaySound ExplotionSound = new PlaySound();
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem[] particles = GetComponentsInParent<ParticleSystem>();
        float t = -100000f;
        foreach (var item in particles)
        {
            float tt = item.startLifetime;
            if (tt > t)
                t = tt;
        }
        ExplotionSound.Onwer = this.gameObject;
        ExplotionSound.Init();
        Destroy(this.gameObject,t);
    }
    public void Explotion(int damage)
    {
        ExplotionSound.Play();
        #region Damage
        LayerMask masckCollider = LayerMask.GetMask("ColliderEnemy", "ColliderPlayer");
        Collider[] objectt = Physics.OverlapSphere(transform.position, radiusDamage, masckCollider, QueryTriggerInteraction.Ignore);

       
        foreach (var item in objectt)
        {
            Health c = item.gameObject.GetComponent<Health>();
            Debug.Log("item: " + item.name);
            if (c != null)
                c.Damage(damage);
        }
        #endregion

    }
    private void OnDrawGizmos()
    {
        if (!IsDrawGizmos) return;
        Gizmos.color = GizmoColor;

        Gizmos.DrawWireSphere(transform.position, radiusDamage);

    }

}
