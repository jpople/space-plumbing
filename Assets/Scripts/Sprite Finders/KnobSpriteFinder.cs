using System;
using UnityEngine;

[CreateAssetMenu()]
public class KnobSpriteFinder : ScriptableObject
{
    [SerializeField] Texture2D sheet;
    const int SPRITE_SIZE = 32;
    public Sprite FindSprite(int value, bool isHighlighted) {
        if (value >= 9) {
            throw new ArgumentOutOfRangeException("Tried to find a sprite outside the range of the sheet");
        }

        Vector2Int spritePositionOnSheet = new Vector2Int();

        spritePositionOnSheet.y = isHighlighted ? 0 : SPRITE_SIZE;
        spritePositionOnSheet.x = value * SPRITE_SIZE;

        Rect location = new Rect(spritePositionOnSheet, new Vector2(SPRITE_SIZE, SPRITE_SIZE));

        Sprite newSprite = Sprite.Create(sheet, location, new Vector2(0.5f, 0.5f), SPRITE_SIZE, 0, SpriteMeshType.FullRect);
        return newSprite;
    }
}
