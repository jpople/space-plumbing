using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TileDestination : MonoBehaviour
{
    public int id = 0;
    public SpriteRenderer dropoffGhost;

    public EnterDropoffRadiusEvent enter;
    public ExitDropoffRadiusEvent exit;

    public bool isAvailable = true;
    public bool isReachable = true;

    public InventoryUpdateEvent updateInventory;
    public BoardUpdateEvent updateBoard;
    public DropPointVacateEvent vacate;

    public TextMeshProUGUI debugText;

    private void Start() {
        if (enter == null) {
            enter = new EnterDropoffRadiusEvent();
        }
        if (exit == null) {
            exit = new ExitDropoffRadiusEvent();
        }
        if (vacate == null) {
            vacate = new DropPointVacateEvent();
        }
        dropoffGhost = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (isReachable) {
            enter.Invoke(id);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (isReachable) {
            exit.Invoke(id);
        }
    }

    public void EnableGhost(Sprite selectedSprite) {
        dropoffGhost.sprite = selectedSprite;
        dropoffGhost.enabled = true;
    }

    public void DisableGhost() {
        dropoffGhost.enabled = false;
    }

    public void ReceiveDrop(Draggable droppedDraggable) {
        DisableGhost();
        isAvailable = false;
        if (droppedDraggable.TryGetComponent<TileDraggable>(out TileDraggable newTile)) {
            if(updateInventory != null) {
                updateInventory.Invoke(id, newTile.info);
            }
            if(updateBoard != null) {
                updateBoard.Invoke(id, newTile.info);
            }
        }
    }
}