using System.Text;
using System.Collections.Generic;
using UnityEngine;

public struct SpriteInfo {
    public SpriteInfo(Sprite s, int r) {
        sprite = s;
        rotation = r;
    }
    public Sprite sprite { get; }
    public int rotation { get; } // # of rotations 90° counterclockwise
}

[CreateAssetMenu()]
public class TileSpriteFinder : ScriptableObject
{
    [SerializeField] Sprite blank;
    [SerializeField] Sprite blueSingle;
    [SerializeField] Sprite blueLine;
    [SerializeField] Sprite blueCurve;
    [SerializeField] Sprite blueT;
    [SerializeField] Sprite blueCross;
    [SerializeField] Sprite redSingle;
    [SerializeField] Sprite redLine;
    [SerializeField] Sprite redCurve;
    [SerializeField] Sprite redT;
    [SerializeField] Sprite redCross;
    [SerializeField] Sprite multiCurve;
    [SerializeField] Sprite multiCross;
    [SerializeField] Sprite wrongCurve_A;
    [SerializeField] Sprite wrongCurve_B;
    [SerializeField] Sprite wrongStraight;
    [SerializeField] Sprite wrongT_A;
    [SerializeField] Sprite wrongT_B;
    [SerializeField] Sprite wrongT_C;
    [SerializeField] Sprite wrongT_D;
    [SerializeField] Sprite wrongT_E;
    [SerializeField] Sprite wrongT_F;
    [SerializeField] Sprite wrongCrossRed;
    [SerializeField] Sprite wrongCrossBlue;


    public SpriteInfo FindSprite(string openings) {
        // setup hash table
        Dictionary<string, Sprite> spriteLookup = new Dictionary<string, Sprite>() {
            {"XXXX", blank},
            {"XXUX", blueSingle},
            {"UXUX", blueLine},
            {"XXUU", blueCurve},
            {"UXUU", blueT},
            {"UUUU", blueCross},
            {"XXRX", redSingle},
            {"RXRX", redLine},
            {"XXRR", redCurve},
            {"RXRR", redT},
            {"RRRR", redCross},
            {"RRUU", multiCurve},
            {"URUR", multiCross},
            {"XXUR", wrongCurve_A},
            {"XXRU", wrongCurve_B},
            {"UXRX", wrongStraight},
            {"RXUU", wrongT_A},
            {"UXUR", wrongT_B},
            {"UXRU", wrongT_C},
            {"RXUR", wrongT_D},
            {"RXRU", wrongT_E},
            {"UXRR", wrongT_F},
            {"RRUR", wrongCrossRed},
            {"UURU", wrongCrossBlue}
        };

        // variables
        int rotationsNeeded = 0;

        while (rotationsNeeded < 4) {
            if (spriteLookup.TryGetValue(openings, out Sprite sprite)) {
                return new SpriteInfo(sprite, rotationsNeeded);
            }
            else {
                // not found, rotate sprite
                StringBuilder result = new StringBuilder(openings);
                for (int i = 0; i < 4; i++) {
                    result[i] = (openings[(i + 1) % 4]);
                }
                openings = result.ToString();
                rotationsNeeded++;
            }
        }
        // this shouldn't happen
        Debug.Log("something has gone horribly wrong with SpriteFinder!");
        return new SpriteInfo(blank, 0);
    }
}
