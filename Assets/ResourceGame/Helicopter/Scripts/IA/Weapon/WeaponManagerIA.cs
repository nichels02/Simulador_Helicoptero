using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IA
{
    public enum SelectWeapon
    {
        MissilAirToAir, MissilAirToTerrain, Minigun, None
    }
    [System.Serializable]
    public class TimeInterval
    {
        #region Attack
        public ArrayFrameRateEvent TimeIntervalEventAttack = new ArrayFrameRateEvent();
        public bool activeAttack { get; set; }
        #endregion
        
        #region AttackSelectWeapon
        //public ArrayFrameRateEvent TimeIntervalEventModeMove = new ArrayFrameRateEvent();

        #endregion

        public TimeInterval()
        {


        }
        public void Update()
        {
            TimeIntervalEventAttack.Update();
        }
        public void Init()
        {
           // TimeIntervalEventAttack.Init();
            TimeIntervalEventAttack.Init();
            //_ArrayFrameRateEventSelectWeapon.Init();

        }

    }
    public class WeaponManagerIA : MonoBehaviour
    {
        public List<Weapon> listweapons = new List<Weapon>();
        public int index = 0;
        public Weapon currentWeapon;
        public Health target;
        public SelectWeapon _SelectWeapon;
        [Range(0,100)]
        public float DistanceMinigunMin;

        [Range(100, 1000)]
        public float DistanceMinigunMax;

        [Range(500, 2100)]
        public float DistanceMissilAirToAirMin;

        [Range(350, 5500)]
        public float DistanceMissilAirToAirMax;

        [Header("Time Interval Action")]
        public TimeInterval _TimeInterval = new TimeInterval();

        public Radar m_Radar;
        // Start is called before the first frame update
        void Start()
        {
            foreach (var item in listweapons)
            {
                item.enabled = (false);
            }

            index = 0;

            _SelectWeapon = SelectWeapon.MissilAirToAir;

            ActiveWeapon(_SelectWeapon);

            foreach (var item in listweapons)
            {
                if ((item is WeaponMissil))
                {
                    ((WeaponMissil)item).owner = this.gameObject;
                    break;
                }
            }
            m_Radar = GetComponent<Radar>();
        }
        public void ActiveWeapon(SelectWeapon ind)
        {
            switch (ind)
            {
                case SelectWeapon.MissilAirToAir:
                    foreach (var item in listweapons)
                    {
                        if (item is WeaponMissilIA)
                        {
                            item.enabled = (true);
                            currentWeapon = item;
                            _SelectWeapon = SelectWeapon.MissilAirToAir;
                        }
                        else
                            item.enabled = (false);
                    }
                    break;
                case SelectWeapon.Minigun:
                    foreach (var item in listweapons)
                    {
                        if (item is MiniGunIA)
                        {
                            item.enabled = (true);
                            currentWeapon = item;
                            _SelectWeapon = SelectWeapon.Minigun;
                        }
                        else
                            item.enabled = (false);
                    }
                    break;
                default:
                    break;
            }
        }
        // Update is called once per frame
        T GetRandomEnum<T>()
        {
            System.Array A = System.Enum.GetValues(typeof(T));
            T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length-1));
            return V;
        }
        public void SelectWeaponEvent()
        {
            ActiveWeapon(GetRandomEnum<SelectWeapon>());
        }
        public void Fire()
        {
            if (currentWeapon is WeaponMissilIA)
            {
                ((WeaponMissilIA)currentWeapon).target = target;
                if(target!=null)
                 ((WeaponMissilIA)currentWeapon).Active=true;
                else
                    ((WeaponMissilIA)currentWeapon).Active = false;
            }
            else
            if (currentWeapon is MiniGunIA)
            {
                ((MiniGunIA)currentWeapon).ActiveIK(target.transform);
                ((MiniGunIA)currentWeapon).directionShoot();
                ((MiniGunIA)currentWeapon).Fire = true;
                ((MiniGunIA)currentWeapon).Play();
                ((MiniGunIA)currentWeapon).PlaySoundShoot();
            }
        }
        public void StopFire()
        {
            if (currentWeapon is MiniGunIA)
            {
                ((MiniGunIA)currentWeapon).DesactiveIK();
                ((MiniGunIA)currentWeapon).Stop();
                ((MiniGunIA)currentWeapon).StopSoundShoot();
            }
        }
        public void SelectWapons()
        {
           

            if (target != null )
            {
                float distance = (target.transform.position - transform.position).magnitude;
                if (distance < DistanceMinigunMax)
                    ActiveWeapon(SelectWeapon.Minigun);
                else
                if (distance > DistanceMissilAirToAirMin && distance < DistanceMinigunMax)
                    ActiveWeapon(GetRandomEnum<SelectWeapon>());
                else
                if (distance > DistanceMinigunMax && distance < DistanceMissilAirToAirMax)
                    ActiveWeapon(SelectWeapon.MissilAirToAir);

             

            }
        }
        private void Update()
        {
            target = m_Radar.GetNearEnemy();
            if (currentWeapon is WeaponMissilIA)
             ((WeaponMissilIA)currentWeapon).target = target;
            _TimeInterval.Update();
        }

    }
}

