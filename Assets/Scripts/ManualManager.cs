using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ManualManager : MonoBehaviour {
    // ManualController manual;
    [SerializeField] RectTransform manual;
    [SerializeField] Transform overlay;
    [SerializeField] Image backdrop;
    const float ANIMATION_TIME = 0.2f;
    Vector2 INACTIVE_POSITION = new Vector2(-3, -10);
    Color TRANSPARENT = new Color(0f, 0f, 0f, 0f);
    Color BACKDROP_COLOR = new Color(0f, 0f, 0f, 0.8f);

    private void Update() {
        if (Input.GetKeyDown("w")) {
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }
        else if (Input.GetKeyUp("w")) {
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn() {
        overlay.gameObject.SetActive(true);
        float timeElapsed = 0f;
        float t = 0f;
        Vector2 startPos = manual.position;
        Quaternion startRotation = manual.rotation;
        while (timeElapsed < ANIMATION_TIME) {
            t = Mathf.Sin(timeElapsed / ANIMATION_TIME * Mathf.PI * 0.5f);
            manual.position = Vector2.Lerp(startPos, Vector2.zero, t);
            manual.rotation = Quaternion.Lerp(startRotation, Quaternion.identity, t);
            backdrop.color = Color.Lerp(TRANSPARENT, BACKDROP_COLOR, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        manual.position = Vector2.zero;
        manual.rotation = Quaternion.identity;
        backdrop.color = BACKDROP_COLOR;
    }

    private IEnumerator FadeOut() {
        float timeElapsed = 0f;
        float t = 0f;
        Vector2 startPos = manual.position;
        Vector2 endPos = new Vector2(-3, -10);
        Quaternion startRotation = manual.rotation;
        Quaternion endRotation = Quaternion.identity;
        endRotation.eulerAngles = new Vector3(0, 0, 20);
        while (timeElapsed < ANIMATION_TIME) {
            t = Mathf.Sin(timeElapsed / ANIMATION_TIME * Mathf.PI * 0.5f);
            manual.position = Vector2.Lerp(startPos, endPos, t);
            manual.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            backdrop.color = Color.Lerp(BACKDROP_COLOR, TRANSPARENT, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        manual.position = endPos;
        manual.rotation = endRotation;
        backdrop.color = TRANSPARENT;
        overlay.gameObject.SetActive(false);
    }
}
