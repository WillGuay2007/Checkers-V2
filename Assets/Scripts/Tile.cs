using UnityEngine;

public class Tile : MonoBehaviour
{

    [SerializeField] private Board board;
    private Color DarkTileColor = new Color32(217, 175, 81, 255);
    private Color LightTileColor = new Color32(212, 203, 137, 255);
    private Color SelectedTileColor = new Color32(0, 200, 0, 255);
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
    private void SetTileOriginalColor()
    {
        if (transform.position.x % 2 == 0) // Si le x est pair
        {
            if (transform.position.y % 2 == 0) // Si le y est pair lorsque le x est pair
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
            if (transform.position.y % 2 == 0) // Si le y est pair lorsque le x est impair
            {
                gameObject.GetComponent<SpriteRenderer>().color = LightTileColor;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().color = DarkTileColor;
            }
        }
    }

    public bool CheckIfContainsPiece()
    {
        return transform.childCount > 0;
    }

    private void OnMouseEnter()
    {
        print("(X: " + xPosition + ", Y: " + yPosition + ")");
        print(CheckIfContainsPiece());
        gameObject.GetComponent<SpriteRenderer>().color = SelectedTileColor;
    }  

    private void OnMouseExit()
    {
        SetTileOriginalColor();
    }

    private void OnMouseDown()
    {
        gameObject.GetComponent<SpriteRenderer>().color = SelectedTileColor * 0.7f;
    }

    private void OnMouseUp()
    {
        gameObject.GetComponent<SpriteRenderer>().color = SelectedTileColor;
    }
}
