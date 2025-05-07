using UnityEngine;
using System.Collections;

public class PathIndicatorFade : MonoBehaviour{
    public float fadeDuration = 1.0f;
    public float stayDuration = 3.0f;
    private CanvasGroup canvasGroup;

// This method is called when the script instance is being loaded
    void Awake(){
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null){
            Debug.LogError("Missing CanvasGroup on indicator.");
            return;
        }
        canvasGroup.alpha = 0f;
    }

// This method is called when the script instance is being loaded
    void Start() => StartCoroutine(FadeInOut());

// This method causes the indicator to fade in and out
    public IEnumerator FadeInOut(){
        yield return StartCoroutine(Fade(0f, 1f)); // Fade in
        yield return new WaitForSeconds(stayDuration);
        yield return StartCoroutine(Fade(1f, 0f)); // Fade out
        Destroy(gameObject); // Optional: remove after fade out
    }

// This method causes the indicator to fade out early
    public void StartFadeOutEarly(){
        StopAllCoroutines();
        StartCoroutine(Fade(1f, 0f));
        Destroy(gameObject, fadeDuration);
    }

// This method causes the indicator to fade in and out
    private IEnumerator Fade(float startAlpha, float endAlpha){
        float time = 0f;
        while (time < fadeDuration){
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }
}//End of PathIndicatorFade.cs