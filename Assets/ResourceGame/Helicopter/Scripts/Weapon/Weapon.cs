using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum TypeWeapons { Miniguns=0,
                          AirToAir=1,
                          AirToLand=2}
public class Weapon : MonoBehaviour
{
    protected bool _fire;
    public int CountBullet;
    public int CountBullet_Max;
    public string Name="";
    public TypeWeapons _TypeWeapons;
    public bool Fire { get => _fire; set => _fire = value; }
    public virtual void LoadComponent()
    {
        CountBullet = CountBullet_Max;
    }
    public virtual void ActivateHub()
    {
        
    }
    public virtual void DesactivateHub()
    {
        
    }
    public virtual void PlaySoundShoot()
    {

    }
    public virtual void StopSoundShoot()
    {

    }
}
