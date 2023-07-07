using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DraggableManager : MonoBehaviour
{
    // utility stuff
    Camera mainCam;
    [SerializeField] GameObject cursor;

    // tiles
    Draggable selected;
    Draggable hovered;
    List<Draggable> tiles = new List<Draggable>();


    // dropoff points
    List<TileDestination> dropoffs = new List<TileDestination>(); // at some point I bet we can grab these programmatically instead of putting them in by hand in the inspector
    public List<TileDestination> eligibleDropoffs = new List<TileDestination>();
    TileDestination closestEligibleDropoff;

    private void Start() {
        mainCam = Camera.main;
        tiles = Object.FindObjectsOfType<Draggable>().ToList();
        for(int i = 0; i < tiles.Count; i++) {
            tiles[i].id = i;
            tiles[i].startHover.AddListener(HandleHoverTile);
            tiles[i].endHover.AddListener(HandleEndHoverTile);
        }
        dropoffs = Object.FindObjectsOfType<TileDestination>().ToList();
        for(int i = 0; i < dropoffs.Count; i++) {
            dropoffs[i].id = i;
            dropoffs[i].enter.AddListener(AddDropoffToEligible);
            dropoffs[i].exit.AddListener(RemoveDropoffFromEligible);
        }
        cursor = Instantiate(cursor);
        cursor.SetActive(false);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            HandlePickUp();
        }
        if (Input.GetMouseButtonUp(0)) {
            HandleDrop();
        }
        if (selected != null) {
            selected.Drag();
            UpdateExpectedDropoff();
            cursor.transform.position = selected.transform.position;
        }
        else {
            eligibleDropoffs.Clear();
        }
    }

    void HandleHoverTile(int id) {
        hovered = tiles[id];
        cursor.SetActive(true);
        cursor.transform.position = hovered.transform.position;
    }

    void HandleEndHoverTile() {
        hovered = null;
        cursor.SetActive(false);
    }

    void HandlePickUp() {
        selected = GetClickedDraggable();
        if (selected != null) {
            selected.PickUp();
            AddDropoffToEligible(selected.lastKnownLocation.id);
            cursor.SetActive(true);
        }
    }

    void HandleDrop() {
        if (selected != null) {
            cursor.SetActive(false);
            if (closestEligibleDropoff == null) {
                selected.ReturnToHome();
            }
            else {
                selected.lastKnownLocation.vacate.Invoke(selected.lastKnownLocation.id);
                closestEligibleDropoff.ReceiveDrop(selected);
                selected.Drop(closestEligibleDropoff);
            }
            selected = null;
        }
    }

    void UpdateExpectedDropoff() {
        if(eligibleDropoffs.Count == 0) {
            closestEligibleDropoff = null;
        }
        else {
            closestEligibleDropoff = eligibleDropoffs.OrderBy(x => Vector3.Distance(selected.transform.position, x.transform.position)).FirstOrDefault();
        }
        foreach(TileDestination d in eligibleDropoffs) {
            d.DisableGhost();
        }
        if (closestEligibleDropoff != null) {
            closestEligibleDropoff.dropoffGhost.transform.rotation = selected.transform.rotation;
            closestEligibleDropoff.EnableGhost(selected.transform.GetComponent<SpriteRenderer>().sprite);
        }
    }

    void AddDropoffToEligible(int id) {
        if (eligibleDropoffs.Count == 1 && selected != null) {
            if (eligibleDropoffs.Contains(selected.lastKnownLocation)) {
                RemoveDropoffFromEligible(selected.lastKnownLocation.id);
            }
        }
        if (dropoffs[id].isAvailable) {
            eligibleDropoffs.Add(dropoffs[id]);
        }
    }

    void RemoveDropoffFromEligible(int id) {
        eligibleDropoffs.Remove(dropoffs[id]);
        if(eligibleDropoffs.Count == 0 && selected != null) {
            AddDropoffToEligible(selected.lastKnownLocation.id);
        }
        dropoffs[id].DisableGhost();
    }

    Draggable GetClickedDraggable() {
        RaycastHit2D hitInfo = new RaycastHit2D();
        Ray click = mainCam.ScreenPointToRay(Input.mousePosition);
        LayerMask mask = LayerMask.GetMask("Draggable");
        hitInfo = Physics2D.GetRayIntersection(ray: click, distance: Mathf.Infinity, layerMask: mask);
        if (hitInfo.collider != null) {
            // check to see if it's already in range of any dropoffs
            ContactFilter2D filter = new ContactFilter2D();
            List<Collider2D> overlaps = new List<Collider2D>();
            Physics2D.OverlapCollider(hitInfo.collider, filter.NoFilter(), overlaps);
            foreach (Collider2D c in overlaps) {
                TileDestination destination = c.transform.GetComponent<TileDestination>();
                if (destination != null) {
                    AddDropoffToEligible(destination.id);
                }
            }
            return hitInfo.collider.transform.GetComponent<Draggable>();
        }
        else return null;
    }

}
