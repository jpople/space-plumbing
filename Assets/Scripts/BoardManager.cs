using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardManager : MonoBehaviour
{
    [SerializeField] GameObject dropPointWithTilePrefab;
    [SerializeField] GameObject emptyDropPointPrefab;
    [SerializeField] TileSpriteFinder spriteFinder;
    public Board board;
    [SerializeField] Transform boardTileHolder;
    public TileDestination[,] dropPoints;
    private Dictionary<int, Vector2Int> boardPositionLookup;
    const float TILE_OFFSET = 36/32f;
    // door
    [SerializeField] Transform door;
    public bool isOpen;
    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        // hardcoded tiles
        string[,] tileCodes = {
            { "XXXX", "XXXX", "XXUX", "XXXX", "XXXX"},
            { "XXXX", "XRRX", "URUR", "XRXR", "XXXR"},
            { "XRXX", "RXXR", "UUXX", "XXUU", "XXXX"},
            { "XUXX", "XUXU", "XUXU", "UXXU", "XXXX"},
            { "XXXX", "XXXX", "XXXX", "XXXX", "XXXX"}
        };
        bool[,] brokens = {
            { false, false, false, false, false },
            { false, false, true, false, false },
            { false, true, false, false, false },
            { false, false, true, false, false },
            { false, false, false, false, false }
        };

        boardPositionLookup = new Dictionary<int, Vector2Int>();
        dropPoints = new TileDestination[5,5];
        
        // generate code board
        TileInfo[,] tiles = new TileInfo[tileCodes.GetLength(0), tileCodes.GetLength(1)];
        for(int i = 0; i < tiles.GetLength(0); i++) {
            for (int j = 0; j < tiles.GetLength(1); j++) {
                tiles[i, j] = new TileInfo(tileCodes[i, j], brokens[i, j]);
            }
        }
        board = new Board(
            tiles, 
            new BoardPath[] {
                new BoardPath(new Vector2Int(2, 0), new Vector2Int(1, 4)), // red path
                new BoardPath(new Vector2Int(3, 0), new Vector2Int(0, 2)), // blue path
            }
        );
        
        // make tile game objects
        for(int i = 0; i < tileCodes.GetLength(0); i++) {
            for (int j = 0; j < tileCodes.GetLength(1); j++) {
                Vector3 tilePos = new Vector3(boardTileHolder.position.x + (TILE_OFFSET * (j - 2f)), boardTileHolder.position.y + (TILE_OFFSET * -(i - 2)), 0);
                Vector3 outsidePos = new Vector3(4 , (TILE_OFFSET * i * 1.5f) - 2, 0);
                GameObject newTileDropoff, newTileHolder;
                TileDestination dropPointInfo;
                if (board.tiles[i, j].isBroken) {
                    board.SetTile(new Vector2Int(i, j), null);
                    newTileDropoff = Instantiate(emptyDropPointPrefab, tilePos, Quaternion.identity, boardTileHolder);
                    dropPointInfo = newTileDropoff.GetComponent<TileDestination>();
                }
                else {
                    newTileHolder = Instantiate(dropPointWithTilePrefab, tilePos, Quaternion.identity, boardTileHolder);
                    dropPointInfo = newTileHolder.GetComponent<TileDestination>();
                    // put tile on holder if applicable
                    Transform newTile = newTileHolder.transform.GetChild(1);
                    if(board.tiles[i, j].isEdge) {
                        Destroy(newTile.GetComponent<Draggable>());
                    }
                    SpriteInfo newTileSpriteInfo = spriteFinder.FindSprite(board.GetTile(i, j).tileCode);
                    newTile.GetComponent<SpriteRenderer>().sprite = newTileSpriteInfo.sprite;
                    newTile.GetComponent<TileDraggable>().info = board.tiles[i, j];
                    newTile.Rotate(0, 0, -90 * newTileSpriteInfo.rotation);
                }
                // setup events for updating
                if(dropPointInfo.updateBoard == null) {
                    dropPointInfo.updateBoard = new BoardUpdateEvent();
                }
                dropPointInfo.updateBoard.AddListener(UpdateBoard);
                dropPointInfo.vacate.AddListener(ClearBoardDropPoint);
                dropPoints[i, j] = dropPointInfo;
            }
        }
    }

    private void Start() {
        for(int i = 0; i < board.tiles.GetLength(0); i++) {
            for (int j = 0; j < board.tiles.GetLength(1); j++) {
                boardPositionLookup[dropPoints[i, j].id] = new Vector2Int(i, j);
            }
        }
        SetAccessPanelState(false);
    }

    void SetAccessPanelState(bool newState) {
        door.gameObject.SetActive(!newState);
        isOpen = newState;
        for(int i = 0; i < board.tiles.GetLength(0); i++) {
            for (int j = 0; j < board.tiles.GetLength(1); j++) {
                dropPoints[i, j].isAvailable = isOpen && board.GetTile(i, j) == null; // this causes problems
                dropPoints[i, j].isReachable = isOpen;
            }
        }
    }

    public void HandleOpenButtonPress() {
        if(!isOpen) {
            source.Play();
        }
        SetAccessPanelState(true);
    }

    public void HandleCloseButtonPress() {
        if(isOpen) {
            source.Play();
        }
        SetAccessPanelState(false);
    }

    private void UpdateBoard(int id, TileInfo newInfo) {
        if (boardPositionLookup.TryGetValue(id, out Vector2Int position)) {
            board.SetTile(position, newInfo); 
        }
        board.CheckState();
    }

    private void ClearBoardDropPoint(int id) {
        if(boardPositionLookup.TryGetValue(id, out Vector2Int position)) {
            board.SetTile(position, null);
        }
    }
}
