using UnityEngine;

public class Tile : MonoBehaviour
{

    private Board Board;
    private Color DarkTileColor = new Color32(217, 175, 81, 255);
    private Color LightTileColor = new Color32(212, 203, 137, 255);
    private Color LegalMoveColor = new Color32(0, 255, 0, 180);

    public Piece OccupyingPiece;
    public int xPosition;
    public int yPosition;

    void Start()
    {  
    
    }

    void Update()
    {
        
    }

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

    private void OnMouseDown()
    {
        
    }

    private void OnMouseUp()
    {
        Board.UpdateSelectedTile(this);
    }
}
