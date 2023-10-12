using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubMissilAirToLand : HubGun
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

    public override void UpdateHub(bool enemy, TypeObjetive type)
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
            UpdateColorCircle(enemy,type);

        }
    }
    public override void UpdateColorCircle(bool enemy, TypeObjetive type)
    {
        if (enemy)
        {
            float d = (HubBaseGun.rectTransform.position - indicator.transform.position).magnitude;
            indicator.UpdateColor(d,type);
            HubBaseGun.color = Color.Lerp(HubBaseGun.color, Color.red, Time.deltaTime * 0.5f);
        }
        else
        {
            indicator.UpdateColor(100,type);
            HubBaseGun.color = Color.Lerp(HubBaseGun.color, Color.green, Time.deltaTime * 15f);
        }
        misildirected = (HubBaseGun.color.r >= 0.9f);
    }

}
