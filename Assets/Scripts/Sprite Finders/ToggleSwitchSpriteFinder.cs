using UnityEngine;

[CreateAssetMenu()]
public class ToggleSwitchSpriteFinder : ScriptableObject
{
    [SerializeField] Texture2D sheet;
    const int SPRITE_SIZE = 32;

    public Sprite FindSprite(bool isUp, bool isHighlighted) {
        Vector2Int spritePositionOnSheet = new Vector2Int();

        spritePositionOnSheet.x = isUp ? 0 : SPRITE_SIZE;
        spritePositionOnSheet.y = isHighlighted ? 0 : SPRITE_SIZE;

        Rect spriteRect = new Rect(spritePositionOnSheet, new Vector2(SPRITE_SIZE, SPRITE_SIZE));

        Sprite newSprite = Sprite.Create(sheet, spriteRect, new Vector2(0.5f, 0.5f), SPRITE_SIZE, 0, SpriteMeshType.FullRect);
        return newSprite;
    }
}
