using UnityEngine;

[CreateAssetMenu()]
public class DisplayDigitSpriteFinder : ScriptableObject
{
    [SerializeField] Texture2D sheet;
    const int SPRITE_WIDTH = 16;
    const int SPRITE_HEIGHT = 22;
    const int SPRITE_PPU = 32;

    public Sprite FindSprite(char c) {

        Vector2 spritePositionOnSheet = new Vector2();
        
        int spriteNumber;
        if (int.TryParse(c.ToString(), out int pos)) {
            spriteNumber = pos;
        }
        else if (c == ' ') {
            spriteNumber = 11;
        }
        else {
            spriteNumber = 10;
        }

        spritePositionOnSheet.x = spriteNumber * SPRITE_WIDTH;
        spritePositionOnSheet.y = 0;

        Rect spriteRect = new Rect(spritePositionOnSheet, new Vector2(SPRITE_WIDTH, SPRITE_HEIGHT));

        Sprite newSprite = Sprite.Create(sheet, spriteRect, new Vector2(0.5f, 0.5f), 32, 0);

        return newSprite;
    }
}
