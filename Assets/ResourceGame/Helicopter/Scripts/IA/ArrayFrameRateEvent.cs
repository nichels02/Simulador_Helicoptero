using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
[System.Serializable]
public class ArrayFrameRateEvent 
    {
        float FrameRate = 0;
        public UnityEvent ExecutEvent;

        float[] array = new float[50];
        int indexRate = 0;
        public float minRate = 0;
        public float maxRate = 1;
        public bool active;// { get; set; }
        public ArrayFrameRateEvent()
        { }
        // Start is called before the first frame update
        public void Init()
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Random.Range(minRate, maxRate);
            }
            indexRate = 0;
            FrameRate = 0;
        }

        public void ReCalculate()
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Random.Range(minRate, maxRate);
            }
        }
        public void Update()
        {
           
            if (FrameRate > array[indexRate] && active)
            {
                indexRate = (indexRate++) % array.Length;
                ExecutEvent.Invoke();
                active = false;
                FrameRate = 0;
            }
            FrameRate += Time.deltaTime;
        
        }
}

