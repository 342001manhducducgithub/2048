using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    //public Tile[] tiles { get; set; } //
    public Tile tiles; //
    public Tile tilePrefab; //
    public TileRow[] rows { get; private set; }
    public TileCell[] cells { get; private set; }
    public int size => cells.Length;
    public int height => rows.Length;
    public int width => size / height;
    private void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }
    private void Start()
    {
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].cells.Length; x++)
            {
                rows[y].cells[x].coordinates = new Vector2Int(x, y);
            }
        }
    }
    public TileCell GetCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return rows[y].cells[x];
        }
        else
        {
            return null;
        }
    }
    public TileCell GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }
    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        return GetCell(coordinates);
    }
    public TileCell GetRandomEmptyCell()
    {
        int index = Random.Range(0, cells.Length);
        int startingIndex = index;

        while (cells[index].occupied)
        {
            index++;

            if (index >= cells.Length)
            {
                index = 0;
            }

            //tat ca cac o bi chiem
            if (index == startingIndex)
            {
                return null;
            }
        }

        return cells[index];
    }
    public void SpawnTileAt(int tileNumber, int row, int column) //
    {
        if (rows[row].cells[column].tile != null)
        {
            // Nếu ô đã có tile rồi thì không làm gì cả
            return;
        }

        // Tạo một tile mới với giá trị xác định
        Tile newTile = Instantiate(tilePrefab, rows[row].cells[column].transform.position, Quaternion.identity);
        newTile.Initialize(tileNumber, rows[row].cells[column]);
        rows[row].cells[column].tile = newTile;

        // Thêm tile mới vào danh sách các tile
        tiles.Add(newTile);
    }
    public void RemoveTileAt(int row, int column) //
    {
        if (rows[row].cells[column].tile == null)
        {
            // Nếu ô không có tile thì không làm gì cả
            return;
        }

        // Lưu trữ tile hiện có
        Tile currentTile = rows[row].cells[column].tile;

        // Xóa tile khỏi danh sách các tile
        tiles.Remove(currentTile);

        // Xóa tile khỏi ô
        rows[row].cells[column].tile = null;

        // Hủy game object của tile
        Destroy(currentTile.gameObject);
    }
}
