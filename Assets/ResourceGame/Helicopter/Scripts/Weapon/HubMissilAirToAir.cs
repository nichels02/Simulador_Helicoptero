using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubMissilAirToAir : HubGun
{
    public bool misildirected;
    private void Start()
    {
        this.LoadComponent();
    }
    public override void LoadComponent()
    {
        base.LoadComponent();
    }

    public override void UpdateHub(bool enemy,TypeObjetive type)
    {
        if(!this.WeaponActive)
        {
            HubBaseGun.gameObject.SetActive(false);
            return;

        }
        if (this.WeaponActive)
        {
            if (!HubBaseGun.gameObject.activeSelf)
                HubBaseGun.gameObject.SetActive(true);

            UpdatePositionMissil(HubBaseGun.rectTransform);
            UpdateColorBox(enemy,type);

        }
    }

    public override void UpdateColorBox(bool enemy, TypeObjetive type)
    {
        if (IsPointInRT(indicator.rectTransform.anchoredPosition, HubBaseGun.rectTransform) && enemy)
        {
            indicator.UpdateColor(1, type);
            HubBaseGun.color = Color.Lerp(HubBaseGun.color, Color.red, Time.deltaTime * 0.5f);
        }
        else
        {
            indicator.UpdateColor(100, type);
            HubBaseGun.color = Color.Lerp(HubBaseGun.color, Color.green, Time.deltaTime * 15f);
        }
        misildirected = (HubBaseGun.color.r >= 0.3f);
    }

}
