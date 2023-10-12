using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public List<Health> enemys = new List<Health>();
    public LayerMask ColliderMissile;
    [Range(100,500)]
    public float DistanceDetectMissil=250f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public virtual void LoadComponent()
    {
        SphereCollider sphere = this.gameObject.AddComponent<SphereCollider>();
        sphere.isTrigger = true;
        sphere.radius = DistanceDetectMissil;

    }
    public Health GetNearEnemy()
    {
        if (enemys.Count == 0) return null;

        Health near = enemys[0];

        if (near == null) return null;

        float distance = (transform.position - near.transform.position).magnitude;

        for (int i = 1; i < enemys.Count; i++)
        {
            float d = (transform.position - near.transform.position).magnitude;
            if (d < distance)
            {
                near = enemys[i];
            }
        }
        return near;

    }
    public bool FindEnemy(GameObject enemy)
    {
        foreach (var item in enemys)
        {
            if (item.gameObject.GetInstanceID() == enemy.GetInstanceID())
                return true;
        }
        return false;
    }
    public virtual bool Delete_Enemy(GameObject enemy)
    {
        if (FindEnemy(enemy))
        {
            Health h = enemy.gameObject.GetComponent<Health>();
            enemys.Remove(h);
            return true;
        }
        return false;
    }
}
