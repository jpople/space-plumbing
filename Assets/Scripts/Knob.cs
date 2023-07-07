using UnityEngine;
using UnityEngine.EventSystems;

public class Knob : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // value stuff
    [SerializeField] float MIN_VALUE_ACTUAL;
    [SerializeField] float MAX_VALUE_ACTUAL;
    [SerializeField] float valueProportional = 0f;
    [SerializeField] float valueActual = 0;
    // value display stuff
    [Tooltip("Smallest possible increment of actual value.")]
    [SerializeField] float SCALE_FACTOR = 1;
    [SerializeField] string formatString;
    [Tooltip("Text names for knob positions.  \n\nNOTE: If this list has any items in it, the values of MIN_VALUE_ACTUAL, MAX_VALUE_ACTUAL, and SCALE_FACTOR will be overwritten and ignored.")]
    [SerializeField] string[] valueLabels;
    // management
    ControlPanelManager manager;
    public string controlName;
    // sprite related
    [SerializeField] KnobSpriteFinder spriteFinder;
    [SerializeField] bool isHovered = false;
    [SerializeField] bool isBeingOperated = false;
    int lastKnownSpritePosition = -1;
    // dragging related
    Vector3 mouseOffset = Vector3.zero;
    float lastUnadjustedValue = 0;
    [SerializeField] Texture2D arrowCursor;
    [SerializeField] Texture2D defaultCursor;
    const float DRAG_SCALE_FACTOR = 0.7f; // subject to change
    // other required stuff references
    AudioSource source;
    [SerializeField] PopoverController popover;
    Camera cam;

    private void Awake() {
        source = GetComponent<AudioSource>();
        cam = Camera.main;
        if (valueLabels.Length > 0) {
            MIN_VALUE_ACTUAL = 0;
            MAX_VALUE_ACTUAL = valueLabels.Length - 1;
            SCALE_FACTOR = 1;
        }
        manager = GetComponentInParent<ControlPanelManager>();

        // SetValue(
        //     (((float)manager.currentConfig[controlName] * SCALE_FACTOR) - MIN_VALUE_ACTUAL) / (MAX_VALUE_ACTUAL - MIN_VALUE_ACTUAL)
        // );

        SetValue(Random.Range(0f, 1f));
    }

    private void Start() {
        popover.gameObject.SetActive(false);
    }

    private void Update() {
        if (isBeingOperated) {
            Vector3 newPosition = cam.ScreenToWorldPoint(Input.mousePosition) - mouseOffset;
            float change = newPosition.x;
            SetValue(lastUnadjustedValue + (change * DRAG_SCALE_FACTOR));
        }
    }

    private void OnMouseEnter() {
        if (manager.selectedControl == null) {
            isHovered = true;
            manager.selectedControl = transform;
            // if (!isBeingOperated) {
            UpdateSprite();
            // }
        }
    }

    private void OnMouseExit() {
        isHovered = false;
        if (!isBeingOperated) {
            manager.selectedControl = null;
        }
        UpdateSprite();
    }

    public void OnPointerDown(PointerEventData e) {
        if (isHovered) {
            mouseOffset = cam.ScreenToWorldPoint(e.position);
            lastUnadjustedValue = valueProportional;
            isBeingOperated = true;
            Cursor.SetCursor(arrowCursor, new Vector2(0.5f, 0.5f), CursorMode.Auto);
            UpdateSprite();
        }
    }

    public void OnPointerUp(PointerEventData e) {
        mouseOffset = Vector3.zero;
        isBeingOperated = false;
        manager.selectedControl = null;
        Cursor.SetCursor(defaultCursor, new Vector2(0.25f, 0.25f), CursorMode.Auto);
        UpdateSprite();
    }

    private void SetValue(float newValue) {
        float lastKnownActualValue = valueActual;
        valueProportional = Mathf.Clamp(newValue, 0f, 1f);
        float calculatedValue = MIN_VALUE_ACTUAL + ((MAX_VALUE_ACTUAL - MIN_VALUE_ACTUAL) * valueProportional);
        int intValueActual = Mathf.FloorToInt(calculatedValue / SCALE_FACTOR);
        valueActual = intValueActual * SCALE_FACTOR;
        manager.currentConfig[controlName] = intValueActual;
        manager.UpdateValues();
        if (valueActual != lastKnownActualValue) {
            if (valueLabels.Length > 0) {
                popover.SetText(valueLabels[(int)valueActual]);
            }
            else {
                popover.SetText(valueActual.ToString(formatString));
            }
            UpdateSprite();
        }
    }

    private void UpdateSprite() {
        float calculatedValueProportional = (float)(valueActual - MIN_VALUE_ACTUAL) / (float)(MAX_VALUE_ACTUAL - MIN_VALUE_ACTUAL);
        int spritePosition = Mathf.FloorToInt(calculatedValueProportional * 9);
        if (spritePosition == 9) {
            spritePosition = 8;
        }
        if (lastKnownSpritePosition != -1 && spritePosition != lastKnownSpritePosition) {
            source.Play();
        }
        GetComponent<SpriteRenderer>().sprite = spriteFinder.FindSprite((int)spritePosition, isHovered || isBeingOperated);
        lastKnownSpritePosition = spritePosition;
    }
}
