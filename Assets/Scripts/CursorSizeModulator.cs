using UnityEngine;

public class CursorSizeModulator : MonoBehaviour
{
    [SerializeField] float rate = 3f; // period of sine
    [SerializeField] float depth = 0.01f;

    SpriteRenderer sr;
    float startingSize;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        startingSize = sr.size.x;
    }

    private void Update() {
        float value = startingSize + (depth * Mathf.Sin(Time.time * rate));
        sr.size = new Vector2(value, value);
    }
}
