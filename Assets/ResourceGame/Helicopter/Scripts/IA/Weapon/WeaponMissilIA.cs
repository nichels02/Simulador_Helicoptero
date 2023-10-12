using UnityEngine;
using System.Collections;

public class WeaponMissilIA : WeaponMissil
{
	public Vector2 MinMaxRateLaunchMissil;
	public float[] RateLaunchMissil;
	int indexLaunchMissil=0;

	public Vector2 MinMaxRateReload;
	public float[] RateReload;
	int indexReload = 0;
	public float FrameRateLaunchMissil = 0;
	public float FrameRateReload = 0;

	public bool Active = false;
	public int currentCount;
	public int Count;
	public bool AutomaticReload;
	[Header("Sound Missil")]
	public PlaySound SoundShooting = new PlaySound();
	// Use this for initialization
	void Start ()
	{
		LoadComponent();
		currentCount = Count;
		RateLaunchMissil = new float[10];
		RateReload = new float[10];
		float maxL = Mathf.Max(MinMaxRateLaunchMissil.x, MinMaxRateLaunchMissil.y);
		float minL = Mathf.Max(MinMaxRateLaunchMissil.x, MinMaxRateLaunchMissil.y);

		float maxR = Mathf.Max(MinMaxRateReload.x, MinMaxRateReload.y);
		float minR = Mathf.Max(MinMaxRateReload.x, MinMaxRateReload.y);

		for (int i = 0; i < 10; i++)
        {
			RateLaunchMissil[i] = Random.Range(minL, maxL);
			RateReload[i] = Random.Range(minR, maxR);
		}
		//SoundShooting = GetComponent<AudioSource>();
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
	private void Update()
    {
		if (Active)
        {
			if (FrameRateLaunchMissil > RateLaunchMissil[indexLaunchMissil] && currentCount > 0 && Active)
			{
				FrameRateLaunchMissil = 0;
				currentCount--;

				indexLaunchMissil++;
				indexLaunchMissil = indexLaunchMissil % RateLaunchMissil.Length;

				if (currentCount == 0)
					Active = false;


				FireMissil();
			}
			FrameRateLaunchMissil += Time.deltaTime;

		}
		

        if (AutomaticReload && currentCount == 0 && (target != null))
        {
            if (FrameRateReload > RateReload[indexReload])
            {

                FrameRateReload = 0;
                //if (Active)
                {
                    currentCount = Count;
                }
                Active = true;
				indexReload++;
				indexReload = indexReload % RateReload.Length;
			}
            FrameRateReload += Time.deltaTime;

        }
    }
	public override void LoadComponent()
	{
		base.LoadComponent();
	}
	public override void CanculeDirectionLazer(Transform posPivot)
	{
		directionMissil = posPivot.forward;
	}
	public override void Shoot(Transform posPivot, ParticleSystem particleShoot)
	{
		if (Prefabmissil != null)
		{
			if (particleShoot != null)
				particleShoot.Emit(1);
			CanculeDirectionLazer(posPivot);
			base.InstantiateMissil(posPivot,owner);
			
		}
	}
	public override void LaunchLeftMissil()
	{
		Shoot(LaunchLeft, particleShootLeft);
	}
	public override void LaunchRightMissil()
	{
		Shoot(LaunchRight, particleShootRight);
	}
	
	public void FireMissil()
	{
		if(target!=null)
        {
			LaunchLeftMissil();
			LaunchRightMissil();
		}


	}
}
