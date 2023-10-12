using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomPlaySound
{
	int[] arrayPlay;
	int index = 0;
	float[] arrayPlayAP;
	int indexAP = 0;
	public GameObject Onwer;
	public List<AudioClip> _AudioClip = new List<AudioClip>();
	List<AudioSource> _AudioSource = new List<AudioSource>();
	
	public Vector2 MinMaxAP = new Vector2(1.5f, 3f);
	[Range(0, 1)]
	public float VolumenSoundEffect = 0;
	float FrameRate = 0;
	float Rate = 3;
	float FrameRateAutiomaticPlay = 0;
	public bool Active = true;

	public bool Is3D = false;
	public float spatialBlend;
	public float MinDistance;
	public float MaxDistance;
	public bool playOnAwake;
	public bool loop;
	public RandomPlaySound()
	{

	}

	public bool IsNotLoaded { get => (_AudioSource.Count == 0); }
	public void UpdateVolumen(float volumen)
	{

		if (_AudioSource != null)
		{
			VolumenSoundEffect = Mathf.Clamp01(volumen);
            foreach (var item in _AudioSource)
            {
				item.volume = VolumenSoundEffect;
			}
			
		}

	}
	public void UpdateRandom()
	{
		if (_AudioSource.Count == 0) return;
		if (FrameRate > Rate)
		{
			RandonArray();
			RandonArrayAutomaticPlay();
			FrameRate = 0;
			Active = true;

		}
		FrameRate += Time.deltaTime;
	}
	public void UpdateRandomAutomaticPlay()
	{
		if (_AudioSource.Count == 0) return;
		if (FrameRateAutiomaticPlay > arrayPlayAP[indexAP])
		{
			Play();
			FrameRateAutiomaticPlay = 0;
			indexAP++;
			indexAP = indexAP % arrayPlayAP.Length;
		}
		FrameRateAutiomaticPlay += Time.deltaTime;
	}
	public void Init()
	{


		foreach (var item in _AudioClip)
		{
			AudioSource _ASource = Onwer.AddComponent<AudioSource>();
			_ASource.clip = item;
			_ASource.playOnAwake = playOnAwake;
			_ASource.loop = loop;
			if (Is3D)
			{
				_ASource.spatialBlend = spatialBlend;
				_ASource.minDistance = MinDistance;
				_ASource.maxDistance = MaxDistance;
			}
			_AudioSource.Add(_ASource);
		}

		


		arrayPlay = new int[_AudioSource.Count];
		for (int i = 0; i < _AudioSource.Count; i++)
		{
			arrayPlay[i] = (int)Random.Range(0, _AudioSource.Count - 1);
		}
		arrayPlayAP = new float[10];
		for (int i = 0; i < arrayPlayAP.Length; i++)
		{
			arrayPlayAP[i] = (float)Random.Range(MinMaxAP.x, MinMaxAP.y);
		}
	}
	void RandonArray()
	{
		if (_AudioSource.Count == 0) return;
		for (int i = 0; i < arrayPlay.Length; i++)
		{
			arrayPlay[i] = (int)Random.Range(0, _AudioSource.Count - 1);

		}
	}
	void RandonArrayAutomaticPlay()
	{
		if (_AudioSource.Count == 0) return;
		for (int i = 0; i < arrayPlayAP.Length; i++)
		{
			arrayPlayAP[i] = (float)Random.Range(MinMaxAP.x, MinMaxAP.y);
		}
	}

	
	public void Play()
	{
		
		if (_AudioSource.Count == 0 ) return;

		if (!_AudioSource[arrayPlay[index]].isPlaying)
		{
			_AudioSource[arrayPlay[index]].volume = (VolumenSoundEffect);
			_AudioSource[arrayPlay[index]].Play();
			index++;
			index = index % arrayPlay.Length;
		}

	}
	public void PlayInTime()
	{
		
		if (_AudioSource.Count == 0 || IsPlaying() || !Active) return;
		
		if (!_AudioSource[arrayPlay[index]].isPlaying)
		{
			_AudioSource[arrayPlay[index]].volume = (VolumenSoundEffect);
			_AudioSource[arrayPlay[index]].Play();
			index++;
			index = index % arrayPlay.Length;
			Active = false;
		}

	}
	public bool IsPlaying()
	{
		return (_AudioSource[arrayPlay[index]].isPlaying);
	}
	public bool IsStopped()
	{
		return (!IsPlaying());
	}
	
	public void Stop()
	{

		if (_AudioSource.Count == 0) return;
		foreach (var item in _AudioSource)
		{
			if (item.isPlaying)
				item.Stop();
		}
		Active = true;
	}
	
}

[System.Serializable]
public class PlaySound
{

	public AudioClip _Audio;
	public GameObject Onwer;
	public AudioSource _AudioSource;
	[Range(0, 1)]
	public float VolumenSoundEffect = 0;
	public float Volumen { get => _AudioSource.volume; set => _AudioSource.volume = value; }
	public float time { get => _AudioSource.time; }
	public float length { get => _AudioSource.clip.length; }

	public bool Is3D = false;
	public float spatialBlend;
	public float MinDistance;
	public float MaxDistance;
	public bool playOnAwake;
	public bool loop;
	public float Pitch { get => _AudioSource.pitch; set => _AudioSource.pitch = value; }
	
	public PlaySound()
	{

	}
	public void Init()
	{
		if (Onwer!=null)
        {

			GameObject Sound = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			Sound.name = Onwer.name +"Sound";
			GameObject.Destroy(Sound.GetComponent<Collider>());
			GameObject.Destroy(Sound.GetComponent<Mesh>());
			GameObject.Destroy(Sound.GetComponent<MeshRenderer>());
			GameObject.Destroy(Sound.GetComponent<MeshFilter>());

			Sound.transform.parent = Onwer.transform;
			Sound.transform.localRotation = Quaternion.identity;
			Sound.transform.localPosition = Vector3.zero;
			Sound.transform.localScale = Vector3.one;
			_AudioSource = Sound.AddComponent<AudioSource>();
			_AudioSource.clip = _Audio;
			_AudioSource.playOnAwake = playOnAwake;
			_AudioSource.loop = loop;
			if (Is3D)
            {
				_AudioSource.spatialBlend = spatialBlend;
				_AudioSource.minDistance = MinDistance;
				_AudioSource.maxDistance = MaxDistance;
            }

		}
			
	}
	
	public void Play()
	{
		
		if (_AudioSource!=null && !_AudioSource.isPlaying)
		{
			_AudioSource.volume = (VolumenSoundEffect);
			_AudioSource.Play();
		}
	}
	public bool IsPlaying { get => _AudioSource.isPlaying; }
	public bool IsStopped { get => !_AudioSource.isPlaying; }

	public void Stop()
	{
		if (_AudioSource!=null&&_AudioSource.isPlaying)
			_AudioSource.Stop();
	}
	public void UpdateVolumen()
	{
		
		if (_AudioSource != null)
			_AudioSource.volume =VolumenSoundEffect;
	}
	public void UpdateVolumen(float volumen)
	{

		if (_AudioSource != null)
        {
			VolumenSoundEffect = Mathf.Clamp01(volumen);
			_AudioSource.volume = VolumenSoundEffect;
		}
			
	}
	public void UpVolumenTime()
	{
		
		if (_AudioSource != null)
			_AudioSource.volume = Mathf.Lerp(_AudioSource.volume,1,Time.deltaTime);
	}
	public void DownVolumenTime()
	{
		
		if (_AudioSource != null)
        {
			_AudioSource.volume = Mathf.Lerp(_AudioSource.volume, 0, Time.deltaTime);
			if (_AudioSource.volume == 0)
				_AudioSource.Stop();
		}
			
	}
	public void UpdateVolumenToTop(float volumenTop)
	{
		
		if (_AudioSource != null)
		{
			VolumenSoundEffect = Mathf.Lerp(_AudioSource.volume, volumenTop, Time.deltaTime);
			_AudioSource.volume = Mathf.Lerp(_AudioSource.volume, volumenTop, Time.deltaTime);
		}
			
	}


	public void UpPitchTime()
	{
		
		if (_AudioSource != null)
			_AudioSource.pitch = Mathf.Lerp(_AudioSource.pitch, 1, Time.deltaTime);
	}
	public void DownPitchTime()
	{
		
		if (_AudioSource != null)
			_AudioSource.pitch = Mathf.Lerp(_AudioSource.pitch, 0, Time.deltaTime);
	}
	public void UpdatePitch(float pitchTop)
	{
		
		if (_AudioSource != null)
        {
			_AudioSource.pitch = Mathf.Lerp(_AudioSource.pitch, pitchTop, Time.deltaTime);
			_AudioSource.pitch = Mathf.Clamp01(_AudioSource.pitch);
		}
			
	}
}
public class SoundManager : MonoBehaviour
{
	#region Singleton
	static SoundManager _instance;
	static public bool isActive
	{
		get
		{
			return _instance != null;
		}
	}
	static public SoundManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Object.FindObjectOfType(typeof(SoundManager)) as SoundManager;

				if (_instance == null)
				{
					GameObject go = new GameObject("SoundManager");
					// DontDestroyOnLoad(go);
					_instance = go.AddComponent<SoundManager>();
				}
			}
			return _instance;
		}
	}
	#endregion

	public bool SoundActive = false;
	[Range(0,1)]
	public float VolumenGlobal;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (!InputsHelicopter.instance.active)
		{
			SoundActive = false;
			if (VolumenGlobal>0)
				VolumenGlobal=0;
		}
		else
        {
			SoundActive = true;
			if (VolumenGlobal < 1)
				VolumenGlobal = 1;
		}
			
	}
}
