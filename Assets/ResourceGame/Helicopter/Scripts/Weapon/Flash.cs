using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FlashComponent
{
    public Transform flashPivots;
    public GameObject PrefabFlash { get; set; }
    public ParticleSystem ParticleflashPivots { get; set; }
    FlashComponent() { 
    
    }
    public void Load()
    {
        ParticleflashPivots = flashPivots.GetComponent<ParticleSystem>();
    }
    
    public void InstantiateFlash(float force)
    {
        GameObject flash = GameObject.Instantiate(PrefabFlash, flashPivots.position, flashPivots.rotation);
        GameObject.Destroy(flash, 7);
        Rigidbody rg = flash.GetComponent<Rigidbody>();
        if (ParticleflashPivots != null)
            ParticleflashPivots.Play();
        rg.AddForce(flashPivots.forward * force);
    }
}
public class Flash : MonoBehaviour
{

    public List<FlashComponent> _FlashComponent = new List<FlashComponent>();

    
    //public IteratorFrameRateEvent _IteratorFrameRateEvent = new IteratorFrameRateEvent();
    public SimpleFrameRateEvent _AntimissileSimpleFrameRateEvent = new SimpleFrameRateEvent();

    public GameObject PrefabFlash;
    public bool activeAntimissile = true;
    public float force= 3500;
    [Header("Sound Flash")]
    public PlaySound FlashSound = new PlaySound();
    //public AudioSource SoundShootingFlash;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in _FlashComponent)
        {
            item.Load();
            item.PrefabFlash = PrefabFlash;
        }
        //_IteratorFrameRateEvent.Active = true;
        //SoundShootingFlash = GetComponent<AudioSource>();
        FlashSound.Onwer = this.gameObject;
        FlashSound.Init();
    }
    public void shootAntimissile()
    {
        if (_AntimissileSimpleFrameRateEvent.Active)
        {
            InstantiateAntimissile();
            _AntimissileSimpleFrameRateEvent.Active = false;
        }
         
    }
    public void InstantiateAntimissile()
    {
        FlashSound.Play();
        foreach (var item in _FlashComponent)
        {
            item.InstantiateFlash(force);
        }
        _AntimissileSimpleFrameRateEvent.Active = false;
    }
   
    public void ReloadAntimissile()
    {
        activeAntimissile = true;
        
    }
    // Update is called once per frame
    void Update()
    {
        //_IteratorFrameRateEvent.Update();
        _AntimissileSimpleFrameRateEvent.Update();
    }
}
