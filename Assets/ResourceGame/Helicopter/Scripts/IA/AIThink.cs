using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IA
{
    public enum State { MOVE, MOVE_TO_POSITION, FOLLOW_MOVE, FOLLOW_ATTACK, FINDER, EVADE, KOMBAT, ATTACK, TAKEOFF, None }
    public class AIThink : MonoBehaviour
    {
        #region Attribute
        public State IA_State;
        RadarControllerIA m_Radar;
        public bool Takeoff = true;
        bool FollowMode = false;
        PathMap PathMap;
        HelicopterController m_HelicopterController;
        #endregion
        [Header("Interval Action Move")]
        #region AttackModeMove
        public ArrayFrameRateEvent TimeIntervalEventModeMove = new ArrayFrameRateEvent();
        public bool activeMode { get; set; }
        #endregion
        Health enemy = null;

        [Header("Box Action Move")]
        #region Wander

        public float BoxWidt;
        public float BoxDepth;
        public float BoxHigh;
        public float TimeIntervalWander = 0.55f;
        float framerateWander = 0;
        public Vector3 m_vWanderTarget { get; set; }
        public Vector3 center { get; set; }

        [Header("Draw Gizmo")]
        public bool isDrawGizmo = true;
        public Color Color_DrawGizmo;
        
        #endregion
        WeaponManagerIA _WeaponManagerIA;

        [Header("Sound")]
        public PlaySound AudioSourceHelicopter = new PlaySound();
        public float RadiusAttack;
        // Start is called before the first frame update
        void Start()
        {
            TimeIntervalEventModeMove.Init();
            IA_State = State.TAKEOFF;
            center = this.transform.position;
            m_Radar = GetComponent<RadarControllerIA>();
            PathMap = GetComponent<PathMap>();
            _WeaponManagerIA = GetComponent<WeaponManagerIA>();
            m_HelicopterController = GetComponent<HelicopterControllerIANoPhysic>();
            if (Takeoff)
                ((HelicopterControllerIANoPhysic)m_HelicopterController).TurnOnEngine();

            //AudioSourceHelicopter = GetComponent<AudioSource>();
            AudioSourceHelicopter.Onwer = this.gameObject;
            AudioSourceHelicopter.Init();
            AudioSourceHelicopter.Play();
        }
        // Update is called once per frame
        void Update()
        {
            switch (IA_State)
            {
                case State.FOLLOW_ATTACK:
                    FollowAttack();
                    break;

                case State.MOVE:
                    MovePath();
                    break;

                case State.FOLLOW_MOVE:
                    FollowMove();
                    break;
                case State.TAKEOFF:
                    TakeOff();
                    break;
                case State.None:
                    break;
                default:
                    break;
            }

            if(enemy!=null)
                TimeIntervalEventModeMove.active = true;
            else
                TimeIntervalEventModeMove.active = false;

            TimeIntervalEventModeMove.Update();
        }
        public void Change_FollowMode()
        {
            //FollowMode = !FollowMode;
            //if (FollowMode)
            //    IA_State = State.FOLLOW_MOVE;
            //else
            //    IA_State = State.FOLLOW_ATTACK;
            _WeaponManagerIA.SelectWapons(); 
            //Debug.Log("Change_FollowMode: " + IA_State);
        }
        void TakeOff()
        {
            if (((HelicopterControllerIANoPhysic)m_HelicopterController).CantTakeOff)
            {
                Transform t = PathMap.currentCheckPoint();
                if (t!=null&&transform.position.y >= t.position.y)
                    IA_State= State.MOVE;
                else
                    
                    ((HelicopterControllerIANoPhysic)m_HelicopterController).Up();

                CalculateCenter(transform.position);

                enemy = m_Radar.GetNearEnemy();
                if(enemy)
                    IA_State = State.FOLLOW_ATTACK;
            }
        }
        void MovePath()
        {
             enemy = m_Radar.GetNearEnemy();
            if (enemy != null)
            {
                IA_State = State.FOLLOW_MOVE;
               
                return;
            }
          
            if (PathMap.pathFollow)
            {
                PathMap.GoToGoal(this.transform, PathMap.radioNode);
                ((HelicopterControllerIANoPhysic)m_HelicopterController).MoveToFront(PathMap.currentCheckPoint().position);
            }

            CalculateCenter(transform.position);
        }
        void FollowMove()
        {
           
            #region Not View Enemy
            enemy = m_Radar.GetNearEnemy();

            if (enemy == null)
            {
                IA_State = State.MOVE;
                _WeaponManagerIA.StopFire();
                return;
            }
            #endregion

            #region Shoot Enemy View

            if (m_Radar.IsInSightShoot(enemy))
            {
                ShootEnemy(enemy);
                return;
            }
            #endregion

            #region in Radius Attack move
            float distance = (transform.position - enemy.transform.position).magnitude;
            if (distance < RadiusAttack)
            {
                IA_State = State.FOLLOW_ATTACK;
                CalculateCenter(enemy.transform.position);
                return;
            }
            #endregion



            _WeaponManagerIA.StopFire();

            ((HelicopterControllerIANoPhysic)m_HelicopterController).MoveToFront(enemy.transform.position);
            
            
        }
        void FollowAttack()
        {
                Wander();
                enemy = m_Radar.GetNearEnemy();
                if (enemy == null)
                {
                    IA_State = State.FOLLOW_MOVE;
                return;
                }
                if (enemy != null)
                {
                    ((HelicopterControllerIANoPhysic)m_HelicopterController).MoveToLookEnemy(m_vWanderTarget, enemy.transform.position);
                }

                if (m_Radar.IsInSightShoot(enemy))
                {
                    ShootEnemy(enemy);
                }

                #region in Radius Attack move
                float distance = (transform.position - enemy.transform.position).magnitude;
                if (distance > RadiusAttack)
                {
                    IA_State = State.FOLLOW_MOVE;
                    CalculateCenter(enemy.transform.position);
                    return;
                }
                #endregion

        }

        void ShootEnemy(Health enemy)
        {
            _WeaponManagerIA.target = enemy;

            if (_WeaponManagerIA.target != null)
            {

                _WeaponManagerIA.Fire();

                CalculateCenter(_WeaponManagerIA.target.transform.position);

                return;
            }

            //IA_State = State.FOLLOW_MOVE;

        }
        #region Wander
        void Wander()
        {
            #region Wander
            if (framerateWander > TimeIntervalWander)
            {
                CalculateRandomRadioWander();
                framerateWander = 0;
            }
            framerateWander += Time.deltaTime;

            float dist = (transform.position - m_vWanderTarget).magnitude;

            if (dist < 20)
                CalculateRandomRadioWander();


            #endregion
        }
        void CalculateRandomRadioWander()
        {
            m_vWanderTarget = RandomRadioWander();
        }
        Vector3 RandomRadioWander()
        {
             float X = Random.Range(-(BoxWidt), (BoxWidt)) + center.x;
             float Y = Mathf.Clamp( Random.Range(0, (BoxHigh)) + center.y, center.y, ((HelicopterControllerIANoPhysic)m_HelicopterController).EffectiveHeightTop);
             float Z = Random.Range(-(BoxDepth), (BoxDepth)) + center.z;
             return new Vector3(X, Y, Z); 
        }
        #endregion
        void CalculateCenter(Vector3 pos)
        {
            
            RaycastHit hit;

            if (Physics.Raycast(center + Vector3.up*50,  Vector3.down, out hit, 100, ((HelicopterControllerIANoPhysic)m_HelicopterController)._HelicopterRayColliderIA.maskColliderHeith))
            {
                pos.y = hit.point.y + 150f;
            }
            center = new Vector3(pos.x, pos.y, pos.z);
        }

        private void OnDrawGizmos()
        {
            if (!isDrawGizmo) return;


            if(((HelicopterControllerIANoPhysic)m_HelicopterController)!=null)
            {
                RaycastHit hit;

                if (Physics.Raycast(center + Vector3.up * 50, Vector3.down, out hit, 100, ((HelicopterControllerIANoPhysic)m_HelicopterController)._HelicopterRayColliderIA.maskColliderHeith))
                {
                    Vector3 pos = center;
                    pos.y = hit.point.y + 150f;
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(pos, 5.6f);

                }
                else
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(center, 5.6f);

                }
                //------------------------------------------------------------------------------------------------------------------------------------------------


            }

            Gizmos.color = Color_DrawGizmo;
            Gizmos.DrawSphere(m_vWanderTarget, 5.6f);

            Gizmos.DrawWireSphere(transform.position,RadiusAttack);

            //RaycastHit hit;
            //float h;
            //if (Physics.Raycast(center.position + Vector3.up * 50, Vector3.down, out hit, 100000))
            //{
            //    h = hit.point.y + 50f;
            //    Gizmos.color = Color.blue;
            //    Gizmos.DrawSphere(new Vector3(center.x,h, center.z), 5.6f);
            //}


            // Quad Up

            Gizmos.DrawLine(new Vector3(center.x + BoxWidt, center.y + (BoxHigh), center.z + BoxDepth),
                             new Vector3(center.x + BoxWidt, center.y + (BoxHigh), center.z - BoxDepth));

            Gizmos.DrawLine(new Vector3(center.x + BoxWidt, center.y + (BoxHigh), center.z - BoxDepth),
                             new Vector3(center.x - BoxWidt, center.y + (BoxHigh), center.z - BoxDepth));

            Gizmos.DrawLine(new Vector3(center.x - BoxWidt, center.y + (BoxHigh), center.z - BoxDepth),
                             new Vector3(center.x - BoxWidt, center.y + (BoxHigh), center.z + BoxDepth));

            Gizmos.DrawLine(new Vector3(center.x - BoxWidt, center.y + (BoxHigh), center.z + BoxDepth),
                             new Vector3(center.x + BoxWidt, center.y + (BoxHigh), center.z + BoxDepth));

            // Quad Down

            Gizmos.DrawLine(new Vector3(center.x + BoxWidt, center.y, center.z + BoxDepth),
                             new Vector3(center.x + BoxWidt, center.y, center.z - BoxDepth));

            Gizmos.DrawLine(new Vector3(center.x + BoxWidt, center.y, center.z - BoxDepth),
                             new Vector3(center.x - BoxWidt, center.y, center.z - BoxDepth));

            Gizmos.DrawLine(new Vector3(center.x - BoxWidt, center.y, center.z - BoxDepth),
                             new Vector3(center.x - BoxWidt, center.y, center.z + BoxDepth));

            Gizmos.DrawLine(new Vector3(center.x - BoxWidt, center.y, center.z + BoxDepth),
                             new Vector3(center.x + BoxWidt, center.y, center.z + BoxDepth));


            //Line down 

            Gizmos.DrawLine(new Vector3(center.x + BoxWidt, center.y, center.z + BoxDepth),
                             new Vector3(center.x + BoxWidt, center.y + BoxHigh, center.z + BoxDepth));

            Gizmos.DrawLine(new Vector3(center.x - BoxWidt, center.y, center.z + BoxDepth),
                             new Vector3(center.x - BoxWidt, center.y + BoxHigh, center.z + BoxDepth));

            Gizmos.DrawLine(new Vector3(center.x + BoxWidt, center.y, center.z - BoxDepth),
                             new Vector3(center.x + BoxWidt, center.y + BoxHigh, center.z - BoxDepth));

            Gizmos.DrawLine(new Vector3(center.x - BoxWidt, center.y, center.z - BoxDepth),
                             new Vector3(center.x - BoxWidt, center.y + BoxHigh, center.z - BoxDepth));



        }
    }
}

