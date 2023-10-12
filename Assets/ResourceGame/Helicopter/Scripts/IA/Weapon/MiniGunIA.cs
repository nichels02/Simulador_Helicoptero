using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGunIA : MiniGun
{
    public Transform cannon;
    [Header("Sound Flash")]
    public PlaySound SoundShooting = new PlaySound();
    private void Start()
    {
        base.Init();
        CountBullet = CountBullet_Max;
        SoundShooting.Onwer = this.gameObject;
        SoundShooting.Init();
    }
    public override void PlaySoundShoot()
    {
            SoundShooting.Play();
    }
    public override void StopSoundShoot()
    {
            SoundShooting.Stop();
    }
    public void directionShoot()
    {
        direcctionParticle = cannon.forward;
    }
    private void Update()
    {
        base.UpdateFire();
        //base.DirectionParticle();
    }

    private void LateUpdate()
    {
        base.DirectionParticle();
    }

}
