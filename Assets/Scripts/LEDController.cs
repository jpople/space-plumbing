using UnityEngine;

public class LEDController : MonoBehaviour
{
    public bool isOn = false;
    [SerializeField] LEDSpriteFinder spriteFinder;
    LEDPattern currentPattern;
    int currentStep;
    float currentStepDuration;

    private void Start() {
        SetPattern(LEDPatternLookup.flashSOS);
    }

    private void Update() {
        if (currentPattern.stateDurations != null) { // feels like there should be a way to not do this every frame
            currentStepDuration -= Time.deltaTime;
            if (currentStepDuration < 0) {
                Toggle();
                currentStep = (currentStep + 1) % currentPattern.stateDurations.Length;
                currentStepDuration = currentPattern.stateDurations[currentStep];
            }
        }
    }

    private void Toggle() {
        SetState(!isOn);
    }

    public void SetPattern(LEDPattern newPattern) {
        currentPattern = newPattern;
        currentStep = 0;
        if (currentPattern.stateDurations != null) {
            currentStepDuration = newPattern.stateDurations[0];
        }
        SetState(newPattern.startingState);

    }

    public void SetState(bool newState) {
        isOn = newState;
        UpdateSprite();
    }

    private void UpdateSprite() {
        GetComponent<SpriteRenderer>().sprite = spriteFinder.FindSprite(isOn);
    }
}

public struct LEDPattern {
    public bool startingState;
    public float[] stateDurations;

    public LEDPattern(bool start, float[] durations) {
        this.startingState = start;
        this.stateDurations = durations;
    }
}

public struct LEDPatternLookup {
    public static LEDPattern on = new LEDPattern(true, null);
    public static LEDPattern off = new LEDPattern(false, null);
    public static LEDPattern steadyFlash = new LEDPattern(true, new float[] {0.5f, 0.5f});
    public static LEDPattern intermittentFlash = new LEDPattern(false, new float[] {1.5f, 0.25f});
    public static LEDPattern flashSOS = new LEDPattern(false, new float[] {2f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f});
}