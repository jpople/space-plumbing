using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkingTile : MonoBehaviour
{
    // bookkeeping stuff
    int size = 4;
    public SpriteRenderer tileSprite;
    Color pipeBlue = new Color(0.3f, 0.3f, 1.0f, 1.0f);
    Color pipeRed = new Color(1.0f, 0.3f, 0.3f, 1.0f);

    // tile sprite finder
    [SerializeField] TileSpriteFinder spriteFinder;

    // sfx management
    [SerializeField] UIAudioManager source;

    // UI feedback
    [SerializeField] TextMeshProUGUI outputText;

    // tile info
    string openings = "XXXX"; // one character per opening, starting at the top and proceeding clockwise
    public QtyInput quantity;

    private void Awake() {
        tileSprite = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        UpdateSprite();
    }

    private void Update() {
        if (Input.GetKeyDown("q")) {
            Rotate(1);
        }
        if (Input.GetKeyDown("e")) {
            Rotate(3);
        }
        if (Input.GetKeyDown("w")) {
            ToggleColorAtCursor("U");
        }
        if (Input.GetKeyDown("s")) {
            ToggleColorAtCursor("R");
        }
    }

    public void Rotate(int times) { // "times" is # of 90° rotations counterclockwise (-90 on the z-axis)
        source.Rotate();
        StringBuilder result = new StringBuilder(openings);
        for (int i = 0; i < size; i++) {
            result[i] = (openings[(i + times) % size]);
        }
        openings = result.ToString();
        transform.Rotate(0, 0, -90);
        UpdateSprite();
    }

    private void UpdateSprite() {
        SpriteInfo newSpriteInfo = spriteFinder.FindSprite(openings);
        tileSprite.sprite = newSpriteInfo.sprite;
        int orientation = (newSpriteInfo.rotation) % 4;
        transform.localRotation = Quaternion.identity;
        transform.Rotate(0, 0, -90 * orientation);
    }

    public void ToggleColorAtCursor(string color) { // we have to pass this in as a string instead of a char because otherwise Unity doesn't let it show up in the inspector for the buttons
        source.Edit();
        char colorCode = color[0];
        StringBuilder result = new StringBuilder(openings);
        result[0] = result[0] == colorCode ? 'X' : colorCode;
        openings = result.ToString(); 
        UpdateSprite();
    }

    public void Reset() {
        source.Reset();
        openings = "XXXX";
        UpdateOutputText("", false);
        UpdateSprite();
    }

    public void Submit() {
        if (isTileValid()) {
            source.Submit();
            UpdateOutputText($"adding {quantity.currentValue} of tile with code \"{openings}\" to requisition...", false);
            openings = "XXXX";
            UpdateSprite();
        }
        else {
            source.Error();
            UpdateOutputText("invalid tile! each input must have an output", true);
        }
    }

    public void UpdateOutputText(string newText, bool isError) {
        if (isError) {
            outputText.color = new Color(1.0f, 0.3f, 0.3f);
        }
        else {
            outputText.color = Color.white;
        }
        outputText.text = newText;
    }

    bool isTileValid() {
        int blueCount = 0;
        int redCount = 0;
        foreach(char c in openings) {
            if (c == 'U') {
                blueCount++;
            }
            else if (c == 'R') {
                redCount++;
            }
        }
        if (blueCount % 2 != 0) {
            return false;
        }
        else if (redCount % 2 != 0) {
            return false;
        }
        return true;
    }
}