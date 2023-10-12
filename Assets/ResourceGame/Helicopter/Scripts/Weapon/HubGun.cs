using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HubGun : MonoBehaviour
{

    public enum DetectHub { Circle, Box, Static }
    public DetectHub _DetectHub;
   // [Header("Canvas")]
    public RectTransform CanvasRect { get; set; }
   // [Header("Hub MiniGun")]
    public RawImage HubBaseGun;
    //[Header("Weapon")]
    public Weapon Weapon { get; set; }
    public bool WeaponActive { get => Weapon.enabled; }
    //[Header("Target Indicator")]
    public TargetIndicator indicator { get; set; }
    //[Header("MainCamera")]
    public Camera MainCamera { get; set; }
    [Header("Distance")]
    public float distance;
    public Transform helicopterTransform { get; set; }
    private void Start()
    {
        
    }
    public virtual void LoadComponent()
    {
        Weapon = GetComponent<Weapon>();
    }
    public virtual void UpdateHub(bool enemy, TypeObjetive type)
    {
        //if(!this.WeaponActive)
        //{
        //    HubBaseGun.gameObject.SetActive(false);
        //    return;

        //}

        //if (this.WeaponActive)
        //{
        //    if (!HubBaseGun.gameObject.activeSelf)
        //        HubBaseGun.gameObject.SetActive(true);

        //    switch (_DetectHub)
        //    {
        //        case DetectHub.Circle:
        //            UpdateColorCircle(enemy);
        //            UpdatePositionMissil(HubBaseGun);
        //            break;
        //        case DetectHub.Box:
        //            UpdatePositionMissil(HubBaseGun);
        //            UpdateColorBox(enemy);
        //            break;
        //        case DetectHub.Static:
                   
        //            UpdateColorBox(enemy);
        //            break;
        //        default:
        //            break;
        //    }

        //}

    }
    public virtual void UpdateColorCircle(bool enemy, TypeObjetive type)
    {
        //if (enemy)
        //{
        //    float d = (HubBaseGun.position - indicator.transform.position).magnitude;
        //    indicator.UpdateColor(d);
        //}
        //else
        //    indicator.UpdateColor(100);

    }
    public virtual void UpdateColorBox(bool enemy, TypeObjetive type)
    {
       
        //if (IsPointInRT(indicator.rectTransform.anchoredPosition, HubBaseGun) && enemy)
        //    {
            
        //    indicator.UpdateColor(1);
        //    }
        //    else
        //        indicator.UpdateColor(100);
    }
    public void UpdatePositionMissil(RectTransform HubMissil)
    {
        Vector2 ViewportPositionMissil = MainCamera.WorldToViewportPoint(helicopterTransform.position + helicopterTransform.forward * distance);

        Vector2 WorldObject_ScreenPositionMissil = new Vector2(
        ((ViewportPositionMissil.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPositionMissil.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
        HubMissil.anchoredPosition = Vector2.Lerp(HubMissil.anchoredPosition, WorldObject_ScreenPositionMissil, Time.deltaTime * 10f);
    }
    public bool IsPointInRT(Vector3 point, RectTransform rt)
    {
        //Vector2 point = MainCamera.WorldToScreenPoint(position);
        // Get the rectangular bounding box of your UI element
        Rect rect = rt.rect;

        // Get the left, right, top, and bottom boundaries of the rect
        float leftSide = rt.anchoredPosition.x - rect.width / 2;
        float rightSide = rt.anchoredPosition.x + rect.width / 2;
        float topSide = rt.anchoredPosition.y + rect.height / 2;
        float bottomSide = rt.anchoredPosition.y - rect.height / 2;


        // Check to see if the point is in the calculated bounds
        if (point.x >= leftSide &&
            point.x <= rightSide &&
            point.y >= bottomSide &&
            point.y <= topSide)
        {
            return true;
        }
        return false;
    }
}
