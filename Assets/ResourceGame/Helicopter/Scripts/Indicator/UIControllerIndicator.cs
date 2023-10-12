using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerIndicator : MonoBehaviour
{
    
    public List<TargetIndicator> targetIndicators = new List<TargetIndicator>();

    public Camera MainCamera;
    public Canvas canvas;
    public GameObject TargetIndicatorPrefab;
    public RadarController _RadarController;
    public InputsHelicopter _InputsHelicopter;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        _InputsHelicopter = GetComponent<InputsHelicopter>();
        _RadarController = GetComponent<RadarController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateColorTargetIndicator();
        RemoveUnnecessaryIndicatorAll();
    }
    void UpdateColorTargetIndicator()
    {
        //if (targetIndicators.Count > 0)
        //{
        //    Vector2 cursor = _InputsHelicopter.look;

        //    for (int i = 0; i < targetIndicators.Count; i++)
        //    {

        //        targetIndicators[i].UpdateTargetIndicator();

        //        targetIndicators[i].UpdateColor(cursor);

        //    }
        //}
    }
    void RemoveUnnecessaryIndicatorAll()
    {
        //if (targetIndicators.Count > 0)
        //{
        //    for (int i = 0; i < targetIndicators.Count; i++)
        //    {
        //        TargetIndicator obj = targetIndicators[i];
        //        if (obj.health.IsDead || !_RadarController.IsInSight(obj.target) )
        //        {
        //            RemoveTargetIndicator(obj.target);
        //            Destroy(obj.gameObject);
        //        }
        //    }
        //}
    }
    void RemoveTargetIndicator(GameObject obj)
    {
        TargetIndicator ind = isExitIndicator(obj);
        targetIndicators.Remove(ind);
    }
    TargetIndicator isExitIndicator(GameObject target)
    {
        return (targetIndicators.Find(x => x.target.GetInstanceID() == target.GetInstanceID()));
    }
    public void AddTargetIndicator(GameObject obj)
    {
        //Health _health = obj.GetComponent<Health>();

        //if (_health != null && _health.health >= 0 )
        //{
        //    if (isExitIndicator(obj) == null)
        //    {
        //        TargetIndicator indicator = GameObject.Instantiate(TargetIndicatorPrefab, canvas.transform).GetComponent<TargetIndicator>();
        //        indicator.InitialiseTargetIndicator(_health, MainCamera, canvas);
        //        targetIndicators.Add(indicator);
        //    }
        //}
    }
    public TargetIndicator GetIndicatorSelect()
    {
        if (targetIndicators.Count > 0)
        {
            //Vector2 cursor = _InputsHelicopter.look;

            //for (int i = 0; i < targetIndicators.Count; i++)
            //{

            //    if (targetIndicators[i].IntroRect(cursor))
            //        return targetIndicators[i];

            //}
        }
        return null;
    }
}
