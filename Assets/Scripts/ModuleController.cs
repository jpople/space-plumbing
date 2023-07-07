using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleController : MonoBehaviour
{
    public enum ModuleState {
        OPERATIONAL,
        INCIDENT,
        NOMINAL,
        SELF_TEST
    }
    ModuleState currentState;
    bool isAccessPanelOpen = false;
    string incidentCode = "162";
    float timeRemaining = 5f;
    // int hypersolventLevel;
    // bool hypersolventDepleted;

    private void Update() {
        switch(currentState) {
            case ModuleState.OPERATIONAL:
                // update
                timeRemaining -= Time.deltaTime;
                // then, check for transition
                if(timeRemaining < 0) {
                    timeRemaining = 5f;
                    currentState = ModuleState.INCIDENT;
                }
                break;
            case ModuleState.INCIDENT:
                // update
                // when player opens panel
                // then, check for transition
                break;
            case ModuleState.NOMINAL:
                // update
                // then, check for transition
                break;
            case ModuleState.SELF_TEST:
                // update
                // then, check for transition
                break;
        }
    }
}
