using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class PopoverController : MonoBehaviour
{
    public List<SpriteRenderer> renderers;
    public float width; // in units (32 pixels)
    const float CHARACTER_WIDTH = 0.25f;
    TextMeshProUGUI text;

    // fading
    const float FADE_DURATION = 0.5f;
    const float FADE_DELAY = 1f;


    private void Awake() {
        renderers = GetComponentsInChildren<SpriteRenderer>().ToList();
        text = GetComponentInChildren<TextMeshProUGUI>();
        SetText("0");
        gameObject.SetActive(false);
    }

    private void Start() {
    }

    public void SetText(string newText) {
        StopAllCoroutines();
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
        StartCoroutine(FadeOut());
        text.text = $"{newText}";
        SetWidth(newText.Length * CHARACTER_WIDTH);
    }

    public void SetWidth(float newWidth) {
        float clampedWidth = Mathf.Clamp(newWidth, 0.5f, 10f);
        foreach(SpriteRenderer sr in renderers) {
            sr.size = new Vector2(clampedWidth, sr.size.y);
        }
    }

    public void SetTransparency(float newValue) {
        foreach(SpriteRenderer sr in renderers) {
            sr.color = new Color(1.0f, 1.0f, 1.0f, newValue);
        }
        text.color = new Color(0.0f, 0.0f, 0.0f, newValue);
    }

    IEnumerator FadeOut() {
        SetTransparency(1f);
        float timeElapsed = 0f;
        while (timeElapsed < FADE_DELAY) {
            yield return null;
            timeElapsed += Time.deltaTime;
        }
        timeElapsed = 0f;
        float t = 0f;
        while (timeElapsed < FADE_DURATION) {
            t = Mathf.Sin(timeElapsed / FADE_DURATION * Mathf.PI * 0.5f);
            SetTransparency(Mathf.Lerp(1f, 0f, t));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
