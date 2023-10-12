using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGun : Weapon
{

    public List< ParticleSystem> MuzzleFlash = new List<ParticleSystem>();
    public List<ParticleSystem> Bullet = new List<ParticleSystem>();
    public Radar radar;
    protected Vector3 direcctionParticle;
    public float RateReload = 1f;
    float FrameRateReload = 0;

    public float RateFire = 1f;
    float FrameRateFire = 0;
    public int CadenceShoot = 25;
    public int CadenceShootMax = 50;
    public float speedParticle;
    public IKBase IKBase;
    public void Init()
    {
        CadenceShoot = CadenceShootMax;
        IKBase = GetComponent<IKBase>();
       // CountBullet = CountBullet_Max;
    }
    public void ActiveIK(Transform enemy)
    {
        IKBase.IKActive = true;
        IKBase.target = enemy.position;
        
    }
    public void DesactiveIK()
    {
        IKBase.IKActive = false;
        IKBase.target = Vector3.forward;
    }
    // Update is called once per frame
    public void UpdateFire()
    {
        #region Reload
        if (CadenceShoot <= 0 && CountBullet > 0)
        {
            if (FrameRateReload > RateReload)
            {
                FrameRateReload = 0;
                CadenceShoot = CadenceShootMax;
            }
            else
                Stop();
            FrameRateReload += Time.deltaTime;

        }
        else
            Stop();
        
        #endregion
    }
    public bool CantShoot { get => (CadenceShoot > 0); }
    public virtual bool Play()
    {
            if (CadenceShoot > 0 && Fire && CountBullet > 0)
            {
                if (FrameRateFire > RateFire)
                {
                    foreach (var item in MuzzleFlash)
                    {
                        item.Emit(50);
                    }
                    foreach (var item in Bullet)
                    {
                        item.Emit(50);
                    }
                    CadenceShoot--;
                    CountBullet = Mathf.Clamp(CountBullet - 1, 0, CountBullet_Max);

                    FrameRateFire = 0;
                }
                FrameRateFire += Time.deltaTime;
                return true;
            }
        return false;
    }
    public void Stop()
    {
        Fire = false;
        foreach (var item in MuzzleFlash)
        {
            item.Stop();
        }
        foreach (var item in Bullet)
        {
            item.Stop();
        }
    }
    public void DirectionParticle()
    {
        foreach (var item in Bullet)
        {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[item.particleCount];
            int count = item.GetParticles(particles);
            for (int i = 0; i < count; i++)
            {
                float speed = particles[i].remainingLifetime / particles[i].startLifetime * speedParticle;
                particles[i].velocity = Vector3.Lerp(particles[i].velocity, direcctionParticle * speed,Time.deltaTime*50f) ;
            }
            item.SetParticles(particles, count);
        }
       
    }
    
}
