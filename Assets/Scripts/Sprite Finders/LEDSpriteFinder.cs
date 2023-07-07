using UnityEngine;

[CreateAssetMenu()]
public class LEDSpriteFinder : ScriptableObject
{
    [SerializeField] Texture2D sheet;

    const int SPRITE_SIZE = 16;

    public Sprite FindSprite(bool isOn) {
        Vector2Int spritePositionOnSheet = new Vector2Int();

        spritePositionOnSheet.y = 0;
        spritePositionOnSheet.x = isOn ? SPRITE_SIZE : 0;
        Rect spriteLocation = new Rect(spritePositionOnSheet, new Vector2(SPRITE_SIZE, SPRITE_SIZE));

        Sprite newSprite = Sprite.Create(sheet, spriteLocation, new Vector2(0.5f, 0.5f), 32, 0, SpriteMeshType.FullRect);
        return newSprite;
    }
}
