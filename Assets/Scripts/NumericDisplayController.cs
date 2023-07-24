using UnityEngine;

public class NumericDisplayController : MonoBehaviour
{
    const int DIGITS = 4;
    [SerializeField] SpriteRenderer[] digits = new SpriteRenderer[DIGITS];
    [SerializeField] DisplayDigitSpriteFinder finder;

    private void Start() {
        ResetDisplay();
    }

    public void ResetDisplay() {
        SetValue("");
    }

    public void SetValue(string newValue) {
        newValue = newValue.PadLeft(4);
        for (int i = 0; i < DIGITS; i++) {
            digits[i].sprite = finder.FindSprite(newValue[i]);
        }
    }

    public void SetValue(int newValue) {
        SetValue(newValue.ToString());
    }
}
