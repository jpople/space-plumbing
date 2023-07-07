using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ToggleSwitch : MonoBehaviour, IPointerClickHandler
{
    // size 16/32, 22/32
    // offset 0, 3/32
    public bool isUp = true;
    public bool isHovered;
    [SerializeField] ToggleSwitchSpriteFinder spriteFinder;
    AudioSource source;

    // stuff for interfacing with manager
    ControlPanelManager manager;
    public string controlName;

    private void Awake() {
        source = GetComponent<AudioSource>();
        manager = GetComponentInParent<ControlPanelManager>();
    }

    private void Start() {
        // SetState(manager.currentConfig[controlName] == 1);
        SetState(Random.Range(0f, 1f) < 0.5f);
    }

    private void OnMouseEnter() {
        if (manager.selectedControl == null) {
            manager.selectedControl = transform;
            isHovered = true;
            UpdateSprite();
        }
    }
    private void OnMouseExit() {
        manager.selectedControl = null;
        isHovered = false;
        UpdateSprite();
    }

    public void OnPointerClick(PointerEventData e) {
        Toggle();
    }

    public void SetState(bool newState) {
        isUp = newState;
        manager.currentConfig[controlName] = isUp ? 1 : 0;
        manager.UpdateValues();
        UpdateSprite();
    }

    public void Toggle() {
        // manage state
        SetState(!isUp);
        // interface stuff
        BoxCollider2D c = GetComponent<BoxCollider2D>();
        c.offset = new Vector2(c.offset.x, -c.offset.y);
        source.Play();
    }

    private void UpdateSprite() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = spriteFinder.FindSprite(isUp, isHovered);
    }
}
