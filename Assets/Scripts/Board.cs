using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    //Infos: (X,Y) = (1,1) est en haut a gauche
    private Tile SelectedTile;
    private Tile[] UnsortedTiles;
    private Piece[] UnsortedPieces;

    private Tile[,] Tiles = new Tile[6, 6]; // [x,y] de 1 à 5 inclus (L'index 0 est ignoré pour raisons personnelles)

    void Start()
    {
        UnsortedTiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        UnsortedPieces = FindObjectsByType<Piece>(FindObjectsSortMode.None);
        InitializeTiles();
        InitializePieces();
    }

    void Update()
    {

    }

    public void UpdateSelectedTile(Tile NewSelectedTile)
    {
        SelectedTile = NewSelectedTile;
        ResetAllTiles();
        ShowLegalMoves();
    }

    private void ResetAllTiles()
    {
        foreach (Tile tile in UnsortedTiles)
        {
            tile.ResetColor();
        }
    }

    private void ShowLegalMoves()
    {
        if (SelectedTile == null || !SelectedTile.HasPiece()) {
            return;
        } //Vérifier qu'il ya bien une piece sur la tuile sélectionée

        Piece Piece = SelectedTile.OccupyingPiece;
        int X = SelectedTile.xPosition;
        int Y = SelectedTile.yPosition;

        int direction = Piece.IsOpponent ? 1 : -1; //L'adversaire va vers le bas alors que nous allons vers le haut.

        if (X - 1 >= 1 && Y + direction >= 1 && Y + direction <= 5)
        {
            Tile targetTile = Tiles[X - 1, Y + direction];
            if (!targetTile.HasPiece())
            {
                targetTile.HighlightAsLegalMove();
            }
        }

        if (X + 1 <= 5 && Y + direction >= 1 && Y + direction <= 5)
        {
            Tile targetTile = Tiles[X + 1, Y + direction];
            if (!targetTile.HasPiece())
            {
                targetTile.HighlightAsLegalMove();
            }
        }

    }

    private void InitializeTiles()
    {
        foreach (Tile tile in UnsortedTiles)
        {
            // Convertir position monde -> indices de la grille
            tile.xPosition = (int)tile.transform.position.x + 3;
            tile.yPosition = -(int)tile.transform.position.y + 3;
            tile.SetBoard(this);

            Tiles[tile.xPosition, tile.yPosition] = tile;
        }
    }

    private void InitializePieces()
    {
        foreach (Piece piece in UnsortedPieces)
        {
            int x = (int)piece.transform.position.x + 3;
            int y = -(int)piece.transform.position.y + 3;

            Tile tile = Tiles[x, y];
            piece.SetTile(tile);
        }
    }

}
