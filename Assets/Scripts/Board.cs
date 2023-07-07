using System;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    public TileInfo[,] tiles; // rows, then cols
    public Vector2Int[] startTilePositions;
    public BoardPath[] paths;
    public Vector2Int[] brokenTilePositions;

    public Board(TileInfo[,] tiles, BoardPath[] paths) {
        this.tiles = tiles;
        for(int i = 0; i < tiles.GetLength(0); i++) {
            tiles[i, 0].isEdge = true;
            tiles[0, i].isEdge = true;
            tiles[tiles.GetLength(0) - 1, i].isEdge = true;
            tiles[i, tiles.GetLength(0) - 1].isEdge = true;
        }
        this.paths = paths;
    }

    public TileInfo GetTile(Vector2Int position) {
        return tiles[position.x, position.y];
    }

    public TileInfo GetTile(int x, int y) {
        return tiles[x, y];
    }

    public void SetTile(Vector2Int position, TileInfo tile) {
        tiles[position.x, position.y] = tile;        
    }

    public void SetTile(int x, int y, TileInfo tile) {
        tiles[x, y] = tile;
    }

    bool IsPathValid(BoardPath path) {
        Dictionary<int, Vector2Int> nextTileDirectionLookup = new Dictionary<int, Vector2Int> { // I hate this, please fix it
            { 0, Vector2Int.left },
            { 1, Vector2Int.up },
            { 2, Vector2Int.right },
            { 3, Vector2Int.down }
        };
        // find color we're tracing and direction of path from start tile
        char[] startTileChars = GetTile(path.startPosition).tileArray;
        char colorCode = Array.Find(startTileChars, x => x != 'X');
        int stepDirectionIndex = Array.IndexOf(startTileChars, colorCode);
        
        Vector2Int currentPosition = path.startPosition;

        while (true) {
            // look at next tile
            Vector2Int nextPosition = currentPosition + nextTileDirectionLookup[stepDirectionIndex];
            // if we've reached the end, path is valid
            if(nextPosition == path.endPosition) {
                return true;
            }
            // if there's no next tile, path is invalid
            if(GetTile(nextPosition) == null) {
                return false;
            }
            // if border at "in" direction doesn't match desired color, path is invalid
            int directionFromPreviousIndex = (stepDirectionIndex + 2) % 4;
            if(GetTile(nextPosition).tileArray[directionFromPreviousIndex] != colorCode) {
                return false;
            }
            // otherwise, go to next tile and keep looking
            int directionToNextIndex;
            // for a cross tile (e.g. RRRR), "out" direction is directly opposite "in" direction
            if(GetTile(nextPosition).tileCode == $"{colorCode}{colorCode}{colorCode}{colorCode}") {
                directionToNextIndex = stepDirectionIndex;
            }
            // for any other tile, "out" direction is only match other than "in" direction
            else {
                char[] possibleNextOutputs = (char[]) GetTile(nextPosition).tileArray.Clone();
                possibleNextOutputs[directionFromPreviousIndex] = 'X';
                directionToNextIndex = Array.IndexOf(possibleNextOutputs, colorCode);
                stepDirectionIndex = directionToNextIndex;
            }
            currentPosition = nextPosition;
        }
    }
}
[System.Serializable]
public class TileInfo {
    public string tileCode;
    public char[] tileArray;
    public bool isBroken = false;
    public bool isEdge = false;

    public TileInfo(string tileCode, bool isBroken = false) {
        this.tileCode = tileCode;
        this.tileArray = tileCode.ToCharArray();
        this.isBroken = isBroken;
    }
}

public struct BoardPath {
    // there's no directionality so it doesn't actually matter which one is the start and which is the end, it's just easier to reference it this way
    public Vector2Int startPosition;
    public Vector2Int endPosition;
    public BoardPath(Vector2Int start, Vector2Int end) {
        this.startPosition = start;
        this.endPosition = end;
    }

    public BoardPath(int startX, int startY, int endX, int endY) {
        this.startPosition = new Vector2Int(startX, startY);
        this.endPosition = new Vector2Int(endX, endY);
    }
}