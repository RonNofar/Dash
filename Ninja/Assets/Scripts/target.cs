using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ninja
{
    public class target : MonoBehaviour
    {

        [SerializeField]
        private Util.FloatRange scaleRange; // min and max
        [SerializeField]
        private float totalTime; // total time it takes to scale and transform from min to max (or min to max back to min)
        [SerializeField]
        private Util.FloatRange heightRange; // min and max (relative)
        [SerializeField]
        private float rotationRate; // rate of rotation, negatives will be reversed

        private new Transform transform;

        private Coroutine currCoroutine;

        void Awake()
        {
            transform = GetComponent<Transform>();
        }

        private void FixedUpdate()
        {
            transform.Rotate(rotationRate * Vector3.back);
        }

        IEnumerator animate()
        {
            float startTime = Time.time;
            float timeRatio = 0;
            Vector3 scaleMax = new Vector3(scaleRange.max, scaleRange.max);
            Vector3 scaleMin = new Vector3(scaleRange.min, scaleRange.min);

            while (timeRatio < 1)
            {
                timeRatio = (Time.time - startTime) / totalTime;
                if (timeRatio > 1) timeRatio = 1;

                // Do shit here
                float t = -(0.5f) * Mathf.Cos(Mathf.PI * 2 * timeRatio) + (0.5f);
                transform.localScale = Vector2.Lerp(scaleMin, scaleMax, t);

                if (timeRatio == 1)
                { // Clean up here
                    startTime = Time.time;
                    timeRatio = 0;
                    break;
                }
                yield return null;
            }
        }

        private void OnEnable()
        {
            currCoroutine = StartCoroutine(animate());
        }

        private void OnDisable()
        {
            StopCoroutine(currCoroutine);
        }
    }
}
