using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
[System.Serializable]
public class SimpleFrameRateEvent
    {
        public float Rate;
        float FrameRate = 0;
        public UnityEvent ExecutEvent;
        public bool Active;
        public SimpleFrameRateEvent()
        { }

        // Update is called once per frame
        public void Update()
        {
            if (FrameRate > Rate && !Active)
            {
                //ExecutEvent.Invoke();
                FrameRate = 0;
                Active = true;
            }
            FrameRate += Time.deltaTime;
        }
    }


