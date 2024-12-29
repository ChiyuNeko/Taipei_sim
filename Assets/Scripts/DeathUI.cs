using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI_Control
{
    public class DeathUI: MonoBehaviour
    {
        public int CountDownText(float startTime, float durationTime)
        {
            float delta = durationTime - (Time.time - startTime);
            if((Time.time - startTime) < durationTime)
                Debug.Log((int)delta);
            
            if(delta > 0)
                return (int)delta;
            else
                return 0;
            
        }
    }
}
