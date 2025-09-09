using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    //Infos: (X,Y) = (1,1) est en haut a gauche
    private Tile SelectedTile;
    private Tile[] UnsortedTiles;
    private List<List<Tile>> Tiles = new List<List<Tile[5]>>;

    void Start()
    {
        UnsortedTiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        InitializeTiles();
        print(Tiles[1][1].CheckIfContainsPiece());
    }

    void Update()
    {
        
    }

    public void UpdateSelectedTile(Tile NewSelectedTile)
    {
        SelectedTile = NewSelectedTile;
    }

    private void GetPossibleMoves()
    {

    }

    private void InitializeTiles()
    {
        foreach (Tile tile in UnsortedTiles)
        {

            tile.xPosition = (int)tile.transform.position.x + 3; // Le +3 est juste un offset parce que de base la premiere tuile a la position de (-2,2) sans l'offset
            tile.yPosition = -(int)tile.transform.position.y + 3;

            Tiles[tile.xPosition][tile.yPosition] = tile;

        }
    }

}
