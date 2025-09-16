using UnityEngine;

public class Piece : MonoBehaviour
{

    public bool IsOpponent;
    public bool IsKing;
    private Tile currentTile;

    [SerializeField] private GameObject KingSprite;

    private void Start()
    {
        IsOpponent = transform.tag == "Opponent" ? true : false;
    }

    public void SetTile(Tile tile)
    {
        if (currentTile != null)
            currentTile.ClearPiece();

        currentTile = tile;
        tile.SetPiece(this);

        transform.SetParent(tile.transform);
        transform.localPosition = Vector3.zero;
    }

    public Tile GetTile()
    {
        return currentTile;
    }

    public void PromoteToKing()
    {
        IsKing = true;
        GameObject KingSpriteClone = Instantiate(KingSprite, Vector2.zero, Quaternion.identity);
        KingSpriteClone.transform.SetParent(transform);
        KingSpriteClone.transform.localPosition = Vector2.zero;
    }
}
