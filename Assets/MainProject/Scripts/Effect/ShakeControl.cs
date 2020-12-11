using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sinabro
{
    public class ShakeControl : MonoBehaviour
    {
        //
        public IEnumerator Shake(Vector3 originPos, float duration, float magitude)
        {
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                transform.position = originPos;

                float x = Random.Range(-1f, 1f) * magitude;
                float y = Random.Range(-1f, 1f) * magitude;

                transform.position += new Vector3(x, y, 0);

                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = originPos;
        }
    }
}


