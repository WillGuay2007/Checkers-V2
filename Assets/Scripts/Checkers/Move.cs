using UnityEngine;

public struct Move
{
    public Piece Piece;
    public Tile Destination;
    public bool IsCapture;
    public Piece CapturedPiece;

    public Move(Piece piece, Tile destination, bool isCapture = false, Piece capturedPiece = null)
    {
        Piece = piece;
        Destination = destination;
        IsCapture = isCapture;
        CapturedPiece = capturedPiece;
    }
}
