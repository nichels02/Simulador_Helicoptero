using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubMiniGun : HubGun
{

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
           
            UpdateColorBox(enemy, type);
           
        }

    }

}
