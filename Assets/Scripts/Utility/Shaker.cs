using UnityEngine;
using System.Collections;

namespace Chasa
{
    [ExecuteInEditMode]
    [System.Serializable]
    public class Shaker : MonoBehaviour
    {
        private static Shaker instance;
        public static Shaker GetInstance
        {
            get
            {
                return instance;
            }

        }
        // =================================	
        // Nested classes and structures.
        // =================================

        // ...

        [System.Serializable]
        public class Parameters
        {
            public float force = 1.0f;
            public float duration = 0.5f;
            public float damper = 0.98f;
        }

        // =================================	
        // Variables.
        // =================================

        // ...

        Vector3 startPosition;

        // Circular lerp to start position.

        public float returnToNormalSpeed = 8.0f;

        // =================================	
        // Functions.
        // =================================

        // ...

        void Awake()
        {
            instance = this;
        }

        // ...

        void Start()
        {
            startPosition = transform.localPosition;
        }

        IEnumerator _shake(float force, float duration, float damper)
        {
            float time = 0.0f;
            float force2 = force;

            while (time <= duration)
            {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-force2, force2),
                    Random.Range(-force2, force2),
                    Random.Range(-force2, force2));

                transform.localPosition += randomOffset;

                force2 *= damper;
                time += Time.deltaTime;

                yield return this;
            }
        }

        // ...

        public void shake(float force, float duration, float damper)
        {
            StartCoroutine(_shake(force, duration, damper));
        }
        public void shake(Parameters parameters)
        {
            shake(parameters.force, parameters.duration, parameters.damper);
        }

        // ...

        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     shake(0.1f, 1.0f, 0.92f);
            // }

            // Ease back to resting position (starting position).

            transform.localPosition = Vector3.Lerp(
                transform.localPosition, startPosition, Time.deltaTime * returnToNormalSpeed);
        }

        // =================================	
        // End functions.
        // =================================

    }

    // =================================	
    // End namespace.
    // =================================

}