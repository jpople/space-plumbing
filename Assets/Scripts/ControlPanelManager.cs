using System;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelManager : MonoBehaviour
{
    ModuleConfig config;
    RequiredConfig requiredConfig;
    public Dictionary<string, int> currentConfig;
    public Transform selectedControl;
    LEDController led;
    public bool isValid;

    private void Awake() {
        led = GetComponentInChildren<LEDController>();
        currentConfig = new Dictionary<string, int>() {
            {"Modulation", 1},
            {"Matrix", 0},
            {"Filtration", 0},
            {"Stabilizer", 0},
            {"Catalyst", 22},
        };
        requiredConfig = new RequiredConfig(
            true, // mono 
            false, // disjoint
            (ModuleConfig.FiltrationSetting) 1, // medium
            20, 
            70, 
            54, 
            64
        );
    }

    private void Update() {
    }

    void LogValues() {
        // Debug.Log(
        //     @$"Modulation: {currentConfig["Modulation"]}, Matrix: {currentConfig["Matrix"]}, Filtration: {currentConfig["Filtration"]}, Stabilizer: {currentConfig["Stabilizer"]}, Catalyst: {currentConfig["Catalyst"]}"
        // );
    }

    public void UpdateValues() {
        LogValues();
        CheckIfValid();
        // led.SetState(isValid);
    }

    public void CheckIfValid() {
        if (currentConfig["Modulation"] != Convert.ToInt32(requiredConfig.requiresMonoModulation)) {
            isValid = false;
            return;
        }
        if (currentConfig["Matrix"] != Convert.ToInt32(requiredConfig.requiresNormalMatrix)) {
            isValid = false;
            return;
        }
        if ((ModuleConfig.FiltrationSetting)currentConfig["Filtration"] != requiredConfig.requiredFiltration) {
            isValid = false;
            return;
        }
        if (currentConfig["Stabilizer"] < requiredConfig.stabilizerMin || currentConfig["Stabilizer"] > requiredConfig.stabilizerMax) {
            isValid = false;
            return;
        }
        if (currentConfig["Catalyst"] < requiredConfig.catalystMin || currentConfig["Catalyst"] > requiredConfig.catalystMax) {
            isValid = false;
            return;
        }
        isValid = true;
    }
}

