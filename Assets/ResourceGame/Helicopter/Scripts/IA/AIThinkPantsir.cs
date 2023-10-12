using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IA
{
    public class AIThinkPantsir : MonoBehaviour
    {
        #region Attribute
        public State IA_State;
        RadarControllerIAAntiAerea m_Radar;
        #endregion
        [Header("Interval Action Move")]
        #region AttackModeMove
        public ArrayFrameRateEvent TimeIntervalEventModeMove = new ArrayFrameRateEvent();
        public bool activeMode { get; set; }
        #endregion
        [Header("Box Action Move")]
        #region Wander

        #endregion
        WeaponManagerIA _WeaponManagerIA;
        // Start is called before the first frame update
        void Start()
        {
            IA_State = State.MOVE;
            m_Radar = GetComponent<RadarControllerIAAntiAerea>();
            _WeaponManagerIA = GetComponent<WeaponManagerIA>();
        }

        // Update is called once per frame
        void Update()
        {
            #region IKEnemy
            Health enemy = m_Radar.GetNearEnemy();
            if (enemy != null)
            {
                ((RadarControllerIAAntiAerea)m_Radar).ActiveIK(enemy.transform);
            }
            else
                ((RadarControllerIAAntiAerea)m_Radar).DesactiveIK();
            #endregion


            switch (IA_State)
            {
                case State.MOVE:
                   
                    if (enemy != null)
                    {
                        IA_State = State.FOLLOW_ATTACK;
                        return;
                    }
                    _WeaponManagerIA.StopFire();

                    break;
                case State.FOLLOW_MOVE:
                    break;
                case State.FOLLOW_ATTACK:
                    if (enemy != null)
                    {
                        _WeaponManagerIA.target = m_Radar.IsInSightShoot(enemy);
                        if (_WeaponManagerIA.target != null)
                        {
                            _WeaponManagerIA.SelectWapons();
                            _WeaponManagerIA.Fire();
                            return;
                        }
                        _WeaponManagerIA.StopFire();
                        IA_State = State.MOVE;
                    }
                    break;
                case State.None:
                    break;
                default:
                    break;
            }
        }
    }

}

