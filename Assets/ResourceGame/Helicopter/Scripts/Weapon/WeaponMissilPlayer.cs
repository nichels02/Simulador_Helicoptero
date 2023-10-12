using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public enum DetectHub { Circle, Box, Static }
public class WeaponMissilPlayer : WeaponMissil
{
    Camera mainCamera;
	//Vector3 poslazerMissil;
	//[Header("Sound Missil")]
	////public AudioSource SoundShooting;
	//public PlaySound SoundShooting = new PlaySound();
	//public RectTransform HubMissil;

	[Header("HUB")]
	#region Hub
	public bool misildirected;
	public DetectHub _DetectHub;
	public RectTransform CanvasRect;// { get; set; }
	public Image HubBaseGun;
	public TargetIndicator indicator;// { get; set; }
	[Header("Count bullet")]
	public TextMeshProUGUI textCountBullet;

	[Header("Distance")]
	public float distance;
	public Transform helicopterTransform;// { get; set; }

	RaycastHit hit;
	#endregion
	[Header("Sound Helicopter")]
	public SoundHelicopter SoundH;
	// Use this for initialization
	void Start ()
	{
		mainCamera = Camera.main;
		CountBullet = CountBullet_Max;
		if (textCountBullet != null)
			textCountBullet.text = Name + ":" + CountBullet.ToString();
		LoadComponent();
	}
	public override void PlaySoundShoot()
	{
		SoundH.PlaySoundShoot();
	}
	public override void StopSoundShoot()
	{
		SoundH.StopSoundShoot();
	}
	public void ResetColor()
	{
			indicator.UpdateColor(100, this.TypeObjetive);
			HubBaseGun.color = Color.Lerp(HubBaseGun.color, Color.green, Time.deltaTime * 15f);
			target = null;
	}
	public void UpdateColorBox()
	{
		if (indicator.target != null && indicator.target.TypeObjetive != TypeObjetive) 
		{
			ResetColor();
			return;
		}

		if (IsPointInRT(indicator.rectTransform.anchoredPosition, HubBaseGun.rectTransform))
		{
			indicator.UpdateColor(1, this.TypeObjetive);
			HubBaseGun.color = Color.Lerp(HubBaseGun.color, Color.red, Time.deltaTime * 0.5f);
		}
		else
		{
			indicator.UpdateColor(100, this.TypeObjetive);
			HubBaseGun.color = Color.Lerp(HubBaseGun.color, Color.green, Time.deltaTime * 15f);
		}
		if (HubBaseGun.color.r >= 0.5f)
			target = indicator.target;
		else
			target = null;
	}
	public void UpdateColorCircle()
	{
		if (indicator.target != null && indicator.target.TypeObjetive != TypeObjetive)
		{
			ResetColor();
			return;
		}

		float d = (HubBaseGun.rectTransform.anchoredPosition - indicator.rectTransform.anchoredPosition).magnitude;
	
		indicator.UpdateColor(d, TypeObjetive);
		
		if (d < 50)
		{
			HubBaseGun.color = Color.Lerp(HubBaseGun.color, Color.red, Time.deltaTime * 0.8f);
		}
		else
		{
			HubBaseGun.color = Color.Lerp(HubBaseGun.color, Color.green, Time.deltaTime * 15f);
		}
		if (HubBaseGun.color.r >= 0.5f)
			target = indicator.target;
		else
			target = null;

	}
	public void UpdatePositionMissil()
	{
		Vector2 ViewportPositionMissil = mainCamera.WorldToViewportPoint(helicopterTransform.position + helicopterTransform.forward * distance);
		Vector2 WorldObject_ScreenPositionMissil = new Vector2(
		((ViewportPositionMissil.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
		((ViewportPositionMissil.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
		HubBaseGun.rectTransform.anchoredPosition = Vector2.Lerp(HubBaseGun.rectTransform.anchoredPosition, WorldObject_ScreenPositionMissil, Time.deltaTime * 10f);
	}
	public void ResetPositionMissil()
	{
		Vector2 ViewportPositionMissil = mainCamera.WorldToViewportPoint(helicopterTransform.position + helicopterTransform.forward * distance);
		Vector2 WorldObject_ScreenPositionMissil = new Vector2(
		((ViewportPositionMissil.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
		((ViewportPositionMissil.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
		HubBaseGun.rectTransform.anchoredPosition = WorldObject_ScreenPositionMissil;
	}
	private bool IsPointInRT(Vector3 point, RectTransform rt)
	{
		//Vector2 point = MainCamera.WorldToScreenPoint(position);
		// Get the rectangular bounding box of your UI element
		Rect rect = rt.rect;

		// Get the left, right, top, and bottom boundaries of the rect
		float leftSide = rt.anchoredPosition.x - rect.width / 2;
		float rightSide = rt.anchoredPosition.x + rect.width / 2;
		float topSide = rt.anchoredPosition.y + rect.height / 2;
		float bottomSide = rt.anchoredPosition.y - rect.height / 2;


		// Check to see if the point is in the calculated bounds
		if (point.x >= leftSide &&
			point.x <= rightSide &&
			point.y >= bottomSide &&
			point.y <= topSide)
		{
			return true;
		}
		return false;
	}
	//private void Update()
 //   {
 //       switch (TypeObjetive)
 //       {
 //           case TypeObjetive.Air:
	//			UpdateColorBox();
	//			break;
 //           case TypeObjetive.Land:
	//			UpdateColorCircle();
	//			break;
 //           default:
 //               break;
 //       }
 //       UpdatePositionMissil(HubBaseGun.rectTransform);
	//}
	public override void ActivateHub()
	{
		if (HubBaseGun != null)
			HubBaseGun.gameObject.SetActive(true);
		ResetColor();
		ResetPositionMissil();
	}
	public override void DesactivateHub()
	{
		if (HubBaseGun != null)
			HubBaseGun.gameObject.SetActive(false);
	}
	public override void LoadComponent()
	{
		mainCamera = Camera.main;
		base.LoadComponent();

        switch (_TypeWeapons)
        {
            
            case TypeWeapons.AirToAir:
				SoundH.SoundShootingAirToAir.Onwer = this.gameObject;
				SoundH.SoundShootingAirToAir.Init();
				break;
            case TypeWeapons.AirToLand:
				SoundH.SoundShootingAirToLand.Onwer = this.gameObject;
				SoundH.SoundShootingAirToLand.Init();
				break;
            default:
                break;
        }
        
	}
	public override void CanculeDirectionLazer(Transform posPivot)
	{
		if (Physics.Raycast(helicopterTransform.position, helicopterTransform.forward, out hit, 1000000, raylazerMask))
		{
			directionMissil = (hit.point - posPivot.position).normalized;
		}
		else
			directionMissil = posPivot.forward;

	}
	public override void Shoot(Transform posPivot, ParticleSystem particleShoot)
	{
		if (Prefabmissil != null && CountBullet > 0)
		{
			if (particleShoot != null)
				particleShoot.Play();

			this.CanculeDirectionLazer(posPivot);

			base.InstantiateMissil(posPivot, owner);

			PlaySoundShoot();

			CountBullet = Mathf.Clamp(CountBullet - 1, 0, CountBullet_Max);

			if (textCountBullet != null)
				textCountBullet.text = Name +":"+ CountBullet.ToString();
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
    private void OnDrawGizmos()
    {
		if (!IsGizmos) return;
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(hit.point, 5f);
    }

}
