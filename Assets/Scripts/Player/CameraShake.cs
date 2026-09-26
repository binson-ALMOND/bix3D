using UnityEngine;
using System.Collections;
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    
    private Vector3 initialPosition;
    private Coroutine currentShake;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        initialPosition = transform.localPosition;
    }


    public void Shake(float duration = 0.1f, float strength = 0.05f)
    {
        if(currentShake != null) StopCoroutine(currentShake);
        currentShake = StartCoroutine(ShakeRoutine(duration, strength));
    }

    private IEnumerator ShakeRoutine(float duration, float strength)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float currentStrength = Mathf.Lerp(strength, 0f, elapsed / duration);
            Vector2 randomPoint = Random.insideUnitCircle * currentStrength;
            transform.localPosition = initialPosition + new Vector3(randomPoint.x, randomPoint.y, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.localPosition = initialPosition;
        currentShake = null;
    }
}
