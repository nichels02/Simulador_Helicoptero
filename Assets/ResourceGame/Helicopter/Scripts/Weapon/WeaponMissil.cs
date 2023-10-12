using UnityEngine;
using System.Collections;
public enum TypeObjetive { Air,Land}
public class WeaponMissil : Weapon {

    public TypeObjetive TypeObjetive;
    public GameObject Prefabmissil;
	public Health target;
    public GameObject owner;
    public Transform LaunchLeft;
    public Transform LaunchRight;
    protected ParticleSystem particleShootLeft;
    protected ParticleSystem particleShootRight;
    protected Vector3 directionMissil;
	
	public bool IsGizmos = false;
    public LayerMask raylazerMask;
	// Use this for initialization
	void Start ()
	{
		
	}
    public override void  LoadComponent()
    {
        particleShootLeft = LaunchLeft.GetComponent<ParticleSystem>();
        particleShootRight = LaunchRight.GetComponent<ParticleSystem>();
        base.LoadComponent();
    }
	public virtual void CanculeDirectionLazer(Transform posPivot)
	{

	}
    public virtual void LaunchLeftMissil()
    {
        Shoot(LaunchLeft, particleShootLeft);
    }
    public virtual void LaunchRightMissil()
    {
        Shoot(LaunchRight, particleShootRight);
    }
    public virtual void Shoot(Transform posPivot, ParticleSystem particleShoot)
    {
    }

    public void InstantiateMissil(Transform posPivot,GameObject _owner)
    {
        if (Prefabmissil == null)
        {
            Debug.Log("Not add Prefabmissil");
            return;

        }
        GameObject Rockmissil = Instantiate(Prefabmissil, posPivot.position,  Quaternion.LookRotation(directionMissil));
        
        Rigidbody rg = Rockmissil.GetComponent<Rigidbody>();

        if (rg != null)
            rg.velocity = directionMissil;

        Missil m = Rockmissil.GetComponent<Missil>();
       
        if (m != null)
        {
            m.ThisTypeObjetive = TypeObjetive;
            m.directionmisil = directionMissil;

            if (target != null)
                {
                    m.SetTarget(target);
                    m.EnemyTypeObjetive = target.TypeObjetive;
                }
                
            m.onwer = _owner;
        }
    }

    

}
