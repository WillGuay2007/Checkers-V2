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

        if (transform == null)
            return; // Je crois que je dois faire ca pour la classe de position vu que celle-ci est juste du data et pas de visuel
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
