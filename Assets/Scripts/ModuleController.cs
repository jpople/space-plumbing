
using UnityEngine;
using TMPro;
public class ModuleController : MonoBehaviour {
    public enum ModuleState {
        OPERATIONAL,
        INCIDENT,
        NOMINAL,
        SELF_TEST
    }
    [SerializeField] NumericDisplayController display;

    ModuleState currentState;
    bool accessPanelIsOpen = false;
    // bool internalConfigIsValid = false;
    bool startButtonPressed = false;
    [SerializeField] LEDController statusLED;
    string incidentCode = "181";
    RequiredConfig nominalConfig = new RequiredConfig( // in the future, we'll find these using a lookup by incident code
        true,
        true,
        (ModuleConfig.FiltrationSetting)2, // medium
        40,
        60,
        24,
        34
    );

    RequiredConfig resumeConfig = new RequiredConfig( // see note on nominalConfig
        true,
        true,
        (ModuleConfig.FiltrationSetting)3, // variant
        50,
        70,
        52,
        62
    );
    float timeRemaining = 10f;
    // "inputs"
    ControlPanelManager controlPanel;
    BoardManager board;
    // 
    // int hypersolventLevel;
    // bool hypersolventDepleted;

    private void Awake() {
        controlPanel = GetComponentInChildren<ControlPanelManager>();
        controlPanel.nominalConfig = nominalConfig;
        controlPanel.diagnosticConfig = resumeConfig;
        board = GetComponentInChildren<BoardManager>();
    }

    private void Start() {
        TransitionInto(ModuleState.OPERATIONAL);
    }

    private void Update() {
        switch (currentState) {
            case ModuleState.OPERATIONAL:
                // update
                timeRemaining -= Time.deltaTime;
                // then, check for transition
                if (timeRemaining < 0) {
                    TransitionInto(ModuleState.INCIDENT);
                }
                break;
            case ModuleState.INCIDENT:
                // update
                if (board.isOpen) {
                    Debug.Log("taking damage from open panel...");
                }
                // then, check for transition
                if (controlPanel.state == ControlPanelManager.ControlPanelState.NOMINALIZED) {
                    TransitionInto(ModuleState.NOMINAL);
                }
                break;
            case ModuleState.NOMINAL:
                // update
                // then, check for transition
                if (controlPanel.state == ControlPanelManager.ControlPanelState.DIAGNOSTIC && startButtonPressed) {
                    TransitionInto(ModuleState.SELF_TEST);
                }
                break;
            case ModuleState.SELF_TEST:
                // update
                timeRemaining -= Time.deltaTime;
                // then, check for transition
                if (board.isOpen) {
                    TransitionInto(ModuleState.INCIDENT);
                }
                if (timeRemaining < 0) {
                    if (board.board.isSolved) {
                        Debug.Log("self-test successful!");
                        TransitionInto(ModuleState.OPERATIONAL);
                    }
                    else {
                        Debug.Log("self-test failed!");
                        TransitionInto(ModuleState.INCIDENT);
                    }
                }
                break;
        }
        startButtonPressed = false;
    }

    public void OpenCloseAccessPanel(bool newState) {
        accessPanelIsOpen = newState;
    }

    // public void ToggleCorrectInternalConfig(bool newState) {
    //     internalConfigIsValid = newState;
    // }

    public void HandleStartPressed() {
        startButtonPressed = true;
    }

    void TransitionInto(ModuleState newState) { // fakes "enter()" methods for states
        // boy, do I hate this-- maybe a full class-based FSM *would* be better here
        switch (newState) {
            case ModuleState.OPERATIONAL:
                display.SetValue("");
                statusLED.SetPattern(LEDPatternLookup.on);
                currentState = ModuleState.OPERATIONAL;
                timeRemaining = 5f;
                break;
            case ModuleState.INCIDENT:
                display.SetValue(incidentCode);
                statusLED.SetPattern(LEDPatternLookup.flashSOS);
                currentState = ModuleState.INCIDENT;
                break;
            case ModuleState.NOMINAL:
                statusLED.SetPattern(LEDPatternLookup.intermittentFlash);
                currentState = ModuleState.NOMINAL;
                break;
            case ModuleState.SELF_TEST:
                statusLED.SetPattern(LEDPatternLookup.steadyFlash);
                currentState = ModuleState.SELF_TEST;
                timeRemaining = 5f;
                break;
        }
    }
}
