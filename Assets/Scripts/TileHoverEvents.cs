using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TileStartHoverEvent : UnityEvent<int> {
}

[System.Serializable]
public class TileEndHoverEvent : UnityEvent {
}