using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundHelicopter : MonoBehaviour
{
    HelicopterControllerPlayer _HCP;
    #region Hub
    [Header("Image Power Engine")]
    public Image PowerEngine;
    #endregion
    #region SoundEngine
    [Header("Sound Engine")]
    public PlaySound EngineStartSound = new PlaySound();
    public PlaySound EngineLoopSound = new PlaySound();
    public PlaySound EngineLoopAirSound = new PlaySound();
    [Header("State Engine")]
    public StateEngine _StateEngine;
    #endregion
    #region SoundRadar
    [Header("Sound Detected")]
    public AudioClip AudioClipDetectedNear;
    public AudioClip AudioClipDetectedFar;
    public AudioSource AudioSourceDetectedNear { get; set; }
    public AudioSource AudioSourceDetectedFar { get; set; }
    #endregion

    #region Sound Engine
    public void ActiveEngine()
    {
        _StateEngine = StateEngine.Start;
    }
    public bool InAirEngine()
    {
        return _StateEngine == StateEngine.LoopAir;
    }
    public bool CanPlaySoundEffect { get { return SoundManager.instance.SoundActive==true;} }
    public void DesactiveEngine()
    {
        _StateEngine = StateEngine.None;
    }
    void SetStateEngine()
    {
        float velocity = _HCP.HelicopterModel.velocity.magnitude;

        if (_HCP.TurnEngine && _HCP.EngineForce == 0)
        {
            _StateEngine = StateEngine.Start;
            if(!CanPlaySoundEffect)
                EngineStartSound.Stop();
            else
            if (!EngineStartSound.IsPlaying)
            {
                EngineStartSound.Play();

            }
        }

        switch (_StateEngine)
        {
            case StateEngine.Start:
                if (!CanPlaySoundEffect)
                    EngineStartSound.Stop();
               
                if (_HCP.TurnEngine)
                {

                    EngineStartSound.UpVolumenTime();
                    float time = EngineStartSound.time;
                    float length = EngineStartSound.length;
                    if (_HCP.EngineForce < _HCP.EngineForceTop)
                    {
                        _HCP.EngineForce = _HCP.EngineForceTop * time * Time.deltaTime;
                    }
                    if (PowerEngine != null)
                        PowerEngine.fillAmount = Mathf.Clamp01(time / length);

                    if (time >= length * 0.7)
                    {
                        float volumen = EngineStartSound.time / EngineStartSound.length;

                        if (!CanPlaySoundEffect)
                            EngineLoopSound.Stop();
                        else
                        if (!EngineLoopSound.IsPlaying)
                            EngineLoopSound.Play();

                        EngineLoopSound.UpdateVolumen(volumen);
                        EngineLoopSound.UpdatePitch(volumen + 0.6f);

                        EngineStartSound.UpdateVolumen(1 - volumen);
                    }

                    if (time >= length * 0.9)
                    {
                        _StateEngine = StateEngine.Loop;
                        EngineLoopSound.Pitch = 1;
                        EngineLoopSound.Volumen = 1;

                    }

                    foreach (var item in _HCP.EmitteParticle)
                    {
                        if (!item.isPlaying)
                            item.Play();

                        item.Emit(1);
                    }


                }
                else
                    _StateEngine = StateEngine.None;

                break;
            case StateEngine.Loop:
                if (!CanPlaySoundEffect)
                    EngineLoopSound.Stop();
                else
                if (_HCP.TurnEngine)
                {
                    if (!EngineLoopSound.IsPlaying)
                        EngineLoopSound.Play();
                    if (PowerEngine != null)
                        PowerEngine.fillAmount = 1;
                    if (velocity > 1)
                    {
                        _StateEngine = StateEngine.LoopAir;

                        if (!CanPlaySoundEffect)
                            EngineLoopAirSound.Stop();
                        else
                        if (!EngineLoopAirSound.IsPlaying)
                            EngineLoopAirSound.Play();

                        EngineLoopAirSound.UpdatePitch(0.5f);

                    }

                }
                else
                    _StateEngine = StateEngine.None;
                break;
            case StateEngine.LoopAir:
                if (!CanPlaySoundEffect)
                {
                    if (EngineLoopAirSound.IsPlaying)
                        EngineLoopAirSound.Stop();
                    if (EngineLoopSound.IsPlaying)
                        EngineLoopSound.Stop();
                }
                else
                if (_HCP.TurnEngine)
                {
                    if (!EngineLoopAirSound.IsPlaying)
                        EngineLoopAirSound.Play();

                    if (!EngineLoopSound.IsPlaying)
                        EngineLoopSound.Play();

                    float pitch = ((velocity / _HCP.VelocityMax) * 0.5f);
                    EngineLoopAirSound.Pitch = Mathf.Clamp(pitch, 0.5f, 1f);
                    EngineLoopAirSound.UpVolumenTime();
                    float time = EngineLoopAirSound.time;
                    float length = EngineLoopAirSound.length;
                    if (time >= length * 0.5f && EngineLoopAirSound.IsPlaying)
                    {
                        if (EngineLoopSound.Volumen == 0)
                            EngineLoopSound.Stop();
                        EngineLoopSound.DownVolumenTime();
                    }
                }

                break;
            case StateEngine.Dead:

                
                    
                
                if (EngineStartSound.IsPlaying)
                {
                   
                    EngineStartSound.UpdatePitch(0.3f);
                }
                else
                if (EngineLoopSound.IsPlaying)
                {
                    EngineLoopSound.UpdatePitch(0.3f);
                }
                else
                if (EngineLoopAirSound.IsPlaying)
                {
                    EngineLoopAirSound.UpdatePitch(0.3f);
                }

                break;
            case StateEngine.None:
                if (_HCP.EngineForce == 0) return;
                _HCP.EngineForce = Mathf.Lerp(_HCP.EngineForce, 0, Time.deltaTime);
                float PromEngineForce = _HCP.EngineForce / _HCP.EngineForceTop;
                if (EngineStartSound.IsPlaying)
                {
                    EngineStartSound.DownVolumenTime();
                }

                if (EngineLoopSound.IsPlaying)
                {
                    EngineLoopSound.UpdatePitch(PromEngineForce);
                    EngineLoopSound.DownVolumenTime();
                }

                if (EngineLoopAirSound.IsPlaying)
                {
                    EngineLoopAirSound.UpdatePitch(PromEngineForce);
                    EngineLoopAirSound.DownVolumenTime();
                }

                foreach (var item in _HCP.EmitteParticle)
                {
                    item.Stop();
                }
                break;
            default:
                break;
        }

    }
    public void Dead()
    {
       
        EngineLoopAirSound.UpdatePitch(0.2f);
        _StateEngine = StateEngine.Dead;
        

    }
    #endregion

    #region Sound Weapons
    [Header("Sound Weapons")]
    public PlaySound SoundShootingMiniguns = new PlaySound();
    public PlaySound SoundShootingAirToAir = new PlaySound();
    public PlaySound SoundShootingAirToLand = new PlaySound();

    WeaponManager _WeaponManager;
    #endregion
    #region SoundRadarImplement
    private void CreateAudioSourceHelicopter()
    {
        GameObject RadarSound = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        RadarSound.name = "RadarSound";
        Destroy(RadarSound.GetComponent<Collider>());
        Destroy(RadarSound.GetComponent<Mesh>());
        Destroy(RadarSound.GetComponent<MeshRenderer>());
        Destroy(RadarSound.GetComponent<MeshFilter>());

        RadarSound.transform.parent = this.transform;
        RadarSound.transform.localRotation = Quaternion.identity;
        RadarSound.transform.localPosition = Vector3.zero;
        RadarSound.transform.localScale = Vector3.one;


        AudioSourceDetectedNear = RadarSound.AddComponent<AudioSource>();
        AudioSourceDetectedNear.clip = AudioClipDetectedNear;
        AudioSourceDetectedNear.playOnAwake = false;
        AudioSourceDetectedNear.loop = true;

        AudioSourceDetectedFar = RadarSound.AddComponent<AudioSource>();
        AudioSourceDetectedFar.clip = AudioClipDetectedFar;
        AudioSourceDetectedFar.playOnAwake = false;
        AudioSourceDetectedFar.loop = true;

    }

    public void DetectedNearMissil()
    {

        if (AudioSourceDetectedNear.isPlaying)
            AudioSourceDetectedNear.Stop();
        if (AudioSourceDetectedFar.isPlaying)
            AudioSourceDetectedFar.Stop();
    }
    public void I_detectedNearMissil()
    {

        if (!AudioSourceDetectedNear.isPlaying)
            AudioSourceDetectedNear.Play();
        if (AudioSourceDetectedFar.isPlaying)
            AudioSourceDetectedFar.Stop();
    }
    public void I_detectedFarMissil()
    {

        if (!AudioSourceDetectedFar.isPlaying)
            AudioSourceDetectedFar.Play();
        if (AudioSourceDetectedNear.isPlaying)
            AudioSourceDetectedNear.Stop();
    }
    #endregion
    #region Sound Weapons Implement
    public void PlaySoundShoot()
    {
        switch (_WeaponManager.index_TypeWeapons)
        {
            case TypeWeapons.Miniguns:
                SoundShootingMiniguns.Play();
                break;
            case TypeWeapons.AirToAir:
                SoundShootingAirToAir.Play();
                break;
            case TypeWeapons.AirToLand:
                SoundShootingAirToLand.Play();
                break;
            default:
                break;
        }
        
    }
    public void StopSoundShoot()
    {
        switch (_WeaponManager.index_TypeWeapons)
        {
            case TypeWeapons.Miniguns:
                SoundShootingMiniguns.Stop();
                break;
            case TypeWeapons.AirToAir:
                SoundShootingAirToAir.Stop();
                break;
            case TypeWeapons.AirToLand:
                SoundShootingAirToLand.Stop();
                break;
            default:
                break;
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _HCP = GetComponent<HelicopterControllerPlayer>();
        _WeaponManager = GetComponent<WeaponManager>();
        _StateEngine = StateEngine.None;
        if(PowerEngine!=null)
        PowerEngine.fillAmount = 0;

        EngineStartSound.Onwer = this.gameObject;
        EngineStartSound.Init();
        EngineLoopSound.Onwer = this.gameObject;
        EngineLoopSound.loop = true;
        EngineLoopSound.Init();
        EngineLoopAirSound.Onwer = this.gameObject;
        EngineLoopAirSound.loop = true;
        EngineLoopAirSound.Init();
        CreateAudioSourceHelicopter();
    }

   
    // Update is called once per frame
    void Update()
    {
        SetStateEngine();
        if(!CanPlaySoundEffect&& _WeaponManager!=null)
            _WeaponManager.StopSoundWeapond();

    }
}
