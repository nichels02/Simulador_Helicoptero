using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGunPlayer : MiniGun
{
    //public Transform air;
    Camera mainCamera;
    public bool IsGizmos = false;
   // public RectTransform HubMiniGun;

    #region Hub
    [Header("Hub")]
    public RawImage HubBaseGun;


    #endregion
    [Header("Sound Helicopter")]
    public SoundHelicopter SoundH;
    // public AudioSource SoundShooting;
    // Start is called before the first frame update
    void Start()
    {
        this.LoadComponent();
    }
    public override void LoadComponent()
    {
        base.Init();
        mainCamera = Camera.main;
        SoundH.SoundShootingMiniguns.Onwer = this.gameObject;
        SoundH.SoundShootingMiniguns.Init();
        base.LoadComponent();
    }
    public override void PlaySoundShoot()
    {
        SoundH.PlaySoundShoot();
    }
    public override void StopSoundShoot()
    {
        SoundH.StopSoundShoot();
    }
    public override void ActivateHub()
    {
        if(HubBaseGun!= null)
            HubBaseGun.gameObject.SetActive(true);
    }
    public override void DesactivateHub()
    {
        if (HubBaseGun != null)
            HubBaseGun.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        base.UpdateFire();
        #region Aim To Enemy
        IKBase.target = mainCamera.transform.position + mainCamera.transform.forward * 150;
        direcctionParticle = mainCamera.transform.forward;
        #endregion
    }
    private void LateUpdate()
    {
        base.DirectionParticle();
    }
    
    private void OnDrawGizmos()
    {
        if (!IsGizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(IKBase.target, 2);
    }
}
