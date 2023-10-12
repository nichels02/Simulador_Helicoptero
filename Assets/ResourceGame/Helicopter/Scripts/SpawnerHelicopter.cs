using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using IA;
public class SpawnerHelicopter : MonoBehaviour
{
    public GameObject prefab;
    public Transform pivot;
    public List<Transform> checkpoint = new List<Transform>();
    public UnityEvent _UnityEvent;
    // Start is called before the first frame update
    void Start()
    {
        Spawner();
    }
    public void Spawner()
    {
        GameObject h = Instantiate(prefab, pivot.position, Quaternion.identity);
        PathMap path = h.GetComponent<PathMap>();
        if(h!=null)
        {
            foreach (var item in checkpoint)
            {
                path.checkpoints.Add(item);
            }

        }
        HealthIA he = h.GetComponent<HealthIA>();
        if (he != null)
            he.ExecutEvent = _UnityEvent;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
