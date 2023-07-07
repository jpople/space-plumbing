using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventoryUpdateEvent : UnityEvent<int, TileInfo> {
}

public class BoardUpdateEvent : UnityEvent<int, TileInfo> {
}

[System.Serializable]
public class DropPointVacateEvent : UnityEvent<int> {
}