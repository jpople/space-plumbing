// using UnityEngine;

// public abstract class ModuleState
// {
//     public abstract void InternalUpdate();
//     public abstract ModuleState NextState();
//     public abstract void Enter();
//     public abstract void Exit(); 
// }

// public class NormalState : ModuleState
// {
//     public LEDPattern led = LEDPatternLookup.off;
//     string nextIncidentCode = "162";
//     float timeToNextIncident = 5f;
//     public override void Enter()
//     {
//         // pass
//     }

//     public override void Exit()
//     {
//         // pass
//     }

//     public override void InternalUpdate()
//     {
//         timeToNextIncident -= Time.deltaTime;
//     }

//     public override ModuleState NextState() {
//         if (timeToNextIncident < 0) {
//             return new IncidentState(nextIncidentCode);
//         }
//         return null;
//     }
// }

// public class IncidentState : ModuleState {
//     public LEDPattern led = LEDPatternLookup.flashSOS;
//     string incidentCode; // should this actually just be stored in the main Module object?  probably, right?
//     RequiredConfig nominalizationConfig;
//     ControlPanelManager controlPanel; // it's possible 

//     public IncidentState(string incidentCode) {
//         this.incidentCode = incidentCode;
//     }
//     public override void Enter() {
//         // pass
//     }
//     public override void Exit()
//     {
//         // pass
//     }

//     public override void InternalUpdate()
//     {
//         // pass
//     }

//     public override ModuleState NextState()
//     {
//         if (controlPanel.isValid) {
//             // maybe, since the required config comes from this object, we should be checking validity in here instead of inside ControlPanelManager
//             return new NominalState();
//         }
//         return null;
//     }
// }

// public class NominalState : ModuleState {
//     public LEDPattern led = LEDPatternLookup.intermittentFlash;
//     string incidentCode;
//     RequiredConfig diagnosticConfig;
//     bool diagnosticStarted = false;


//     public override void Enter()
//     {
//         // pass
//     }

//     public override void Exit()
//     {
//         // pass
//     }

//     public override void InternalUpdate()
//     {
//         // pass
//     }

//     public override ModuleState NextState()
//     {
//         if (diagnosticStarted) {
//             return new DiagnosticState();
//         }
//         return null;
//     }
// }

// public class DiagnosticState : ModuleState {
//     public LEDPattern led = LEDPatternLookup.steadyFlash;
//     float diagnosticTimeRemaining = 5f;
//     public override void Enter()
//     {
//         // pass
//     }
//     public override void Exit()
//     {
//         // pass
//     }
//     public override void InternalUpdate()
//     {
//         diagnosticTimeRemaining -= Time.deltaTime;
//     }
//     public override ModuleState NextState()
//     {
//         if (diagnosticTimeRemaining < 0) {
//             return new NormalState();
//         }
//         return null;
//     }
// }