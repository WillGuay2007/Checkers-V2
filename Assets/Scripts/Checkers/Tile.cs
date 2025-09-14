using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public bool PlayerCanSelect;
    private Board Board;
    private Color DarkTileColor = new Color32(217, 175, 81, 255);
    private Color LightTileColor = new Color32(212, 203, 137, 255);
    private Color LegalMoveColor = new Color32(0, 255, 0, 180);

    public Piece OccupyingPiece;
    public int xPosition;
    public int yPosition;


    //Ca commence a 1, finit a 5.
    //Cette fonction permet de remettre la couleur originale de la tuile si elle avait été changée avant.
    public void ResetColor()
    {
        if (xPosition % 2 == 0) // Si le x est pair
        {
            if (yPosition % 2 == 0) // Si le y est pair lorsque le x est pair
            {
                gameObject.GetComponent<SpriteRenderer>().color = DarkTileColor;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().color = LightTileColor;
            }
        }
        else
        {
            if (yPosition % 2 == 0) // Si le y est pair lorsque le x est impair
            {
                gameObject.GetComponent<SpriteRenderer>().color = LightTileColor;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().color = DarkTileColor;
            }
        }
    }

    public void HighlightAsLegalMove()
    {
        gameObject.GetComponent<SpriteRenderer>().color = LegalMoveColor;
    }

    public void SetPiece(Piece piece)
    {
        OccupyingPiece = piece;
    }

    public void ClearPiece()
    {
        OccupyingPiece = null;
    }
    
    public bool HasPiece()
    {
        return OccupyingPiece != null;
    }

    public void SetBoard(Board BoardToSet)
    {
        Board = BoardToSet;
    }

    private void OnMouseUp()
    {
        if (!PlayerCanSelect) return;

        //la variable move va te retourner le move si c'est une case légale qui a ete cliquee (de la selected tile du board)
        Move? move = Board.GetMoveForDestination(this);
        if (move.HasValue)
        {
            Board.MovePiece(move.Value);
        }
        else
        {
            //Ca va update la selected tile du board si c'est pas un move legal qui a ete cliquee.
            //La nouvelle selected tile peut etre null ou une tile avec une piece.
            Board.UpdateSelectedTile(this);
        }
    }
}
