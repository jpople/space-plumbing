using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QtyInput : MonoBehaviour
{
    public int currentValue = 1;
    int maxValue = 99;
    int minValue = 1;
    TMP_InputField inputField;

    public Button plusButton;
    public Button minusButton;

    private void OnEnable() {
        inputField = GetComponent<TMP_InputField>();
        setValue(currentValue);
    }

    public void ChangeValue(int delta) {
        int value = int.Parse(inputField.text);
        value += delta;
        setValue(value);
    }

    void setValue(int value) {
        value = Mathf.Clamp(value, minValue, maxValue);

        plusButton.interactable = value != maxValue;
        minusButton.interactable = value != minValue;

        currentValue = value;
        inputField.text = value.ToString();
    }
}
