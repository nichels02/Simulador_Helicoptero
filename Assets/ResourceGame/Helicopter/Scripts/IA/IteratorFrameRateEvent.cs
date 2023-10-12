using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
[System.Serializable]
public class IteratorFrameRateEvent
{
        public float RateExecutEvent;
        public float RateReloadExecutEvent;
        public float FrameRateExecutEvent = 0;
        public float FrameRateReload = 0;
        public UnityEvent ExecutEvent;
        public bool Active=false;
        public int currentCount;
        public int Count;
        public bool AutomaticReload;
        public void Init()
        {
            currentCount= Count;
        }
        public IteratorFrameRateEvent()
        {
            currentCount = 0;
           
        }
        public void EventTrigger()
        {
            if (Active)
            {
                currentCount = Count;
            }
        }
        // Update is called once per frame
        public void Update()
        {
                if (!Active) return;

                if (FrameRateExecutEvent > RateExecutEvent && currentCount > 0  && Active)
                {
                    ExecutEvent.Invoke();
                    FrameRateExecutEvent = 0;
                    currentCount--;

                    if (currentCount == 0)
                        Active = false;

                   
                }
                FrameRateExecutEvent += Time.deltaTime;

        if (AutomaticReload)
        {
            if (FrameRateReload > RateReloadExecutEvent && currentCount == 0)
            {

                FrameRateReload = 0;
                if (Active)
                {
                    currentCount = Count;
                }
                Active = false;
            }
            FrameRateReload += Time.deltaTime;

        }
    }
}


