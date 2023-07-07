using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public TileDestination lastKnownLocation;
    AudioSource source;

    const int DRAG_SPEED = 100;
    const float PICKUP_DURATION = 0.1f;
    const float RETURN_DURATION = 0.2f;
    const float SNAP_DURATION = 0.1f;

    // stuff for selection cursor
    public int id;
    public TileStartHoverEvent startHover;
    public TileEndHoverEvent endHover;

    // sounds
    [SerializeField] AudioClip pickupSound;
    [SerializeField] AudioClip dropSound;
    [SerializeField] AudioClip lockInSound;

    private void Start() {
        if (startHover == null) {
            startHover = new TileStartHoverEvent();
        }
        if (endHover == null) {
            endHover = new TileEndHoverEvent();
        }
        source = GetComponent<AudioSource>();
        lastKnownLocation = transform.parent.GetComponent<TileDestination>();
    }

    private void OnMouseEnter() {
        startHover.Invoke(id);
    }

    private void OnMouseExit() {
        endHover.Invoke();
    }

    public void Drag() {
        transform.position = Vector3.MoveTowards(transform.position, GetMousePos(), DRAG_SPEED * Time.deltaTime);
    }

    public void PickUp() {
        lastKnownLocation.isAvailable = true;
        source.PlayOneShot(pickupSound);
        GetComponent<SpriteRenderer>().sortingLayerName = "Held Tile";
        StartCoroutine(MoveToMouse());
    }

    public void Drop(TileDestination destination) {
        lastKnownLocation.isAvailable = true;
        StartCoroutine(SnapToLocation(destination, SNAP_DURATION));
    }

    public void ReturnToHome() {
        source.PlayOneShot(dropSound);
        StartCoroutine(SnapToLocation(lastKnownLocation, RETURN_DURATION, false));
    }

    Vector3 GetMousePos() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    IEnumerator MoveToMouse() {
        float timeElapsed = 0;
        float t = 0;
        Vector3 startPos = transform.position;
        while (timeElapsed < PICKUP_DURATION) {
            if (!Input.GetMouseButton(0)) {
                yield break;
            }
            t = Mathf.Sin(timeElapsed / PICKUP_DURATION * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp(startPos, GetMousePos(), t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = GetMousePos();
    }

    IEnumerator SnapToLocation(TileDestination destination, float duration, bool playingSound = true) {
        float timeElapsed = 0;
        float t = 0;
        Vector3 startPos = transform.position;
        while (timeElapsed < duration) {
            t = Mathf.Sin(timeElapsed / duration * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp(startPos, destination.transform.position, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        if (playingSound) {
            source.PlayOneShot(lockInSound);
        }
        transform.position = destination.transform.position;
        lastKnownLocation.isAvailable = true;
        destination.isAvailable = false;
        lastKnownLocation = destination;
        GetComponent<SpriteRenderer>().sortingLayerName = "Tiles";
    }
}
