using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class InventoryManager : MonoBehaviour
{
    public Inventory inventory;
    public TileDestination[] dropPoints;
    Transform inventoryTileHolder;
    [SerializeField] GameObject dropPointWithTilePrefab;
    [SerializeField] GameObject emptyDropPointPrefab;
    [SerializeField] TileSpriteFinder spriteFinder;

    private Dictionary<int, int> inventoryIndexLookup;
    const float TILE_OFFSET = 36/32f;

    private void Awake() {
        TileInfo[] sampleInv = new TileInfo[] {
            new TileInfo("RXXR"),
            new TileInfo("XUXU"),
            new TileInfo("URUR"),
            null,
            null,
        };

        inventoryIndexLookup = new Dictionary<int, int>();
        dropPoints = new TileDestination[5];

        inventory = new Inventory(sampleInv);
        inventoryTileHolder = transform.GetChild(0);

        for (int i = 0; i < inventory.contents.Length; i++) {
            Vector3 tilePos = new Vector3(inventoryTileHolder.position.x + (TILE_OFFSET * (i - 2f)), inventoryTileHolder.position.y, 0);
            GameObject newTileDropPoint;
            if (inventory.contents[i] == null) {
                newTileDropPoint = Instantiate(emptyDropPointPrefab, tilePos, Quaternion.identity, inventoryTileHolder);
            }
            else {
                newTileDropPoint = Instantiate(dropPointWithTilePrefab, tilePos, Quaternion.identity, inventoryTileHolder);
                Transform newTile = newTileDropPoint.transform.GetChild(1);
                SpriteInfo newTileSpriteInfo = spriteFinder.FindSprite(inventory.contents[i].tileCode);
                newTile.GetComponent<SpriteRenderer>().sprite = newTileSpriteInfo.sprite;
                newTile.GetComponent<TileDraggable>().info = inventory.contents[i];
                newTile.Rotate(0, 0, -90 * newTileSpriteInfo.rotation);
            }
            TileDestination dropPointInfo = newTileDropPoint.GetComponent<TileDestination>();
            if (dropPointInfo.updateInventory == null) {
                dropPointInfo.updateInventory = new InventoryUpdateEvent();
            }
            dropPointInfo.updateInventory.AddListener(UpdateInventory);
            dropPointInfo.vacate.AddListener(ClearInventoryDropPoint);
            dropPoints[i] = dropPointInfo;
        }
    }

    private void Start() {
        for (int i = 0; i < inventory.contents.Length; i++) {
            inventoryIndexLookup[dropPoints[i].id] = i;
        }
    }

    private void UpdateInventory(int id, TileInfo newInfo) {
        if (inventoryIndexLookup.TryGetValue(id, out int value)) {
            inventory.contents[value] = newInfo;
        }
    }

    private void ClearInventoryDropPoint(int id) {
        if (inventoryIndexLookup.TryGetValue(id, out int value)) {
            inventory.contents[value] = null;
        }
    }
}
