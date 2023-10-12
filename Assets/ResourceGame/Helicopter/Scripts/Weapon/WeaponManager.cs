using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WeaponManager : MonoBehaviour
{
    public List<Weapon> listweapons = new List<Weapon>();
    public List<RectTransform> listweaponsSelect = new List<RectTransform>();
    public Weapon currentWeapon;
    public RectTransform selectWeapons;
    public TypeWeapons index_TypeWeapons;
    //public int index = 0;
    bool press = false;
    bool fire = true;
   
   
    RadarController _Radar;
    public Flash _Flash;
    [Header("Count bullet")]
    public TextMeshProUGUI textCountBullet;
    HelicopterControllerPlayer _HelicopterControllerPlayer;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in listweapons)
        {
            item.enabled =(false);
        }
        index_TypeWeapons = TypeWeapons.Miniguns;
        //index = 0;
        ActiveWeapon(index_TypeWeapons);
        
        _Radar = GetComponent<RadarController>();
        foreach (var item in listweapons)
        {
            if ((item is WeaponMissilPlayer))
            {
                ((WeaponMissilPlayer)item).owner = this.gameObject;
                ((WeaponMissilPlayer)item).CanvasRect = _Radar._HelicopterHub.CanvasRect;
                ((WeaponMissilPlayer)item).helicopterTransform = this.transform;
                ((WeaponMissilPlayer)item).indicator = _Radar._HelicopterHub.indicator;
            }
        }
        _HelicopterControllerPlayer = GetComponent<HelicopterControllerPlayer>();
    }
    public void ActiveWeapon(TypeWeapons ind)
    {
        for (int i = 0; i < listweapons.Count; i++)
        {
            if (i == (int)ind)
            {

                currentWeapon = listweapons[i];
                currentWeapon.gameObject.SetActive(true);
                currentWeapon.enabled = true;
                currentWeapon.ActivateHub();
                selectWeapons.position = listweaponsSelect[i].position;
            }
            else
            {
                listweapons[i].gameObject.SetActive(false);
                listweapons[i].enabled = false;
                listweapons[i].DesactivateHub();
            }
               
        }

    }
    // Update is called once per frame
    void Update()
    {
       
        #region ChangeWeapons
        if (InputsHelicopter.instance.changeweapon && !press)
        {
            index_TypeWeapons++;
            int index = (int)(index_TypeWeapons) % (listweapons.Count);
            index_TypeWeapons = ((TypeWeapons)(index));
            ActiveWeapon(index_TypeWeapons);
            press = true;
        }
        else
        if (!InputsHelicopter.instance.changeweapon && press)
            press = false;
        #endregion

        #region FireWeapons
        if ((currentWeapon is MiniGunPlayer))
        {
            if (InputsHelicopter.instance.fire1 && _HelicopterControllerPlayer.InAirEngine())
            {
                fire = ((MiniGunPlayer)currentWeapon).CantShoot;
                
                if (fire)
                {
                    ((MiniGunPlayer)currentWeapon).Fire = fire;
                    ((MiniGunPlayer)currentWeapon).Play();
                    ((MiniGunPlayer)currentWeapon).PlaySoundShoot();
                }
            }
            else
            {
                ((MiniGunPlayer)currentWeapon).Stop();
                ((MiniGunPlayer)currentWeapon).StopSoundShoot();

            }
                
        }
        else
        if ((currentWeapon is WeaponMissilPlayer))
        {

            switch (((WeaponMissilPlayer)currentWeapon).TypeObjetive)
            {
                case TypeObjetive.Air:
                    ((WeaponMissilPlayer)currentWeapon).UpdateColorBox();
                    break;
                case TypeObjetive.Land:
                    ((WeaponMissilPlayer)currentWeapon).UpdateColorCircle();
                    break;
                default:
                    ((WeaponMissilPlayer)currentWeapon).ResetColor();
                    break;
            }
             ((WeaponMissilPlayer)currentWeapon).UpdatePositionMissil();
           

            if (InputsHelicopter.instance.fire1Trigger && _HelicopterControllerPlayer.InAirEngine())
                ((WeaponMissilPlayer)currentWeapon).LaunchRightMissil();
            if (InputsHelicopter.instance.fire3Trigger && _HelicopterControllerPlayer.InAirEngine())
                ((WeaponMissilPlayer)currentWeapon).LaunchLeftMissil();
        }
        #endregion

        #region Flash
        if (_Flash != null && _HelicopterControllerPlayer.InAirEngine())
        {
            if (InputsHelicopter.instance.flash)
            {
                LayerMask mask = LayerMask.GetMask("ColliderFlashPlayer");
                _Flash.gameObject.layer = (int)Mathf.Log(mask, 2);
                _Flash.shootAntimissile();

            }    
               
        }
        #endregion

        if (textCountBullet != null)
            textCountBullet.text = currentWeapon.Name + ": " + currentWeapon.CountBullet.ToString();
    }

    public void StopSoundWeapond()
    {
        #region FireWeapons
        if ((currentWeapon is MiniGunPlayer))
        {
            fire = ((MiniGunPlayer)currentWeapon).CantShoot;

            if (fire)
            {
                ((MiniGunPlayer)currentWeapon).Stop();
                ((MiniGunPlayer)currentWeapon).StopSoundShoot();
            }
           
        }
        else
        if ((currentWeapon is WeaponMissilPlayer))
        {
                ((WeaponMissilPlayer)currentWeapon).StopSoundShoot();
        }
        #endregion

    }
}
