using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ninja.Util
{
    public class Func : MonoBehaviour
    {
        public static IEnumerator WaitAndRunAction(float secs, UnityAction action)
        { // wait for secs  
            yield return new WaitForSeconds(secs);
            action.Invoke();
        }
    }

    [System.Serializable]
    public struct FloatRange
    {
        public float min, max;

        public float RandomInRange
        {
            get
            {
                return Random.Range(min, max);
            }
        }
        public float clamp(float num)
        {
            bool neg = num < 0;
            num *= (neg ? -1 : 1);
            return (num < max ? num > min ? num : min : max) * (neg ? -1 : 1);
        }
    }

}