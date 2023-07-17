using System;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelManager : MonoBehaviour
{
    public RequiredConfig nominalConfig;
    public RequiredConfig diagnosticConfig;
    public Dictionary<string, int> currentConfig;
    public Transform selectedControl;
    public bool isValid;

    public enum ControlPanelState {
        TRIVIAL, // any config that isn't a significant one
        NOMINALIZED,
        DIAGNOSTIC
    }

    public ControlPanelState state;

    private void Awake() {
        currentConfig = new Dictionary<string, int>() {
            {"Modulation", 1},
            {"Matrix", 0},
            {"Filtration", 0},
            {"Stabilizer", 0},
            {"Catalyst", 22},
        };
    }

    public void UpdateValues() {
        if(MatchesRequirement(nominalConfig)) {
            state = ControlPanelState.NOMINALIZED;
        }
        else if(MatchesRequirement(diagnosticConfig)) {
            state = ControlPanelState.DIAGNOSTIC;
        }
        else {
            state = ControlPanelState.TRIVIAL;
        }
    }

    public bool MatchesRequirement(RequiredConfig required) {
        if (currentConfig["Modulation"] != Convert.ToInt32(required.requiresMonoModulation)) {
            return false;
        }
        if (currentConfig["Matrix"] != Convert.ToInt32(required.requiresNormalMatrix)) {
            return false;
        }
        if ((ModuleConfig.FiltrationSetting)currentConfig["Filtration"] != required.requiredFiltration) {
            return false;
        }
        if (currentConfig["Stabilizer"] < required.stabilizerMin || currentConfig["Stabilizer"] > required.stabilizerMax) {
            return false;
        }
        if (currentConfig["Catalyst"] < required.catalystMin || currentConfig["Catalyst"] > required.catalystMax) {
            return false;
        }
        return true;
    }
}

