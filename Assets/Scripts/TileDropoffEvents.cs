using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EnterDropoffRadiusEvent : UnityEvent<int> {
}

[System.Serializable]
public class ExitDropoffRadiusEvent : UnityEvent<int> {
}