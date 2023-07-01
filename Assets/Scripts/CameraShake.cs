using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake( float duration, float magnitude)
    {
        Vector3 originalPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f) + magnitude;
            float y = Random.Range(-1f, 1f) + magnitude;

            transform.position = new Vector3(x, y, -10f);
            elapsedTime += Time.deltaTime;
            yield return 0;
        }
        transform.position = originalPosition;
    }
}
