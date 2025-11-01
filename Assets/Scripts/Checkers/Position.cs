using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{

    //C'est pour l'algorithme Minimax c'est comme un helper tool on va dire
    //Techniquement, une Position est un board mais j'ai rajout� quelques fonctions pour g�rer les sous-positions
    //Je sais pas si on peut considerer ca comme de l'heritage

    [SerializeField] private Board VisualGameBoard;
    private Board PositionBoardData;

    //J'ai decid� de travailler avec un genre d'arbre. Chaque position a une liste de positions enfants et un parent (Sauf dans des cas ou c'est le debut ou la fin de l'arbre)
    private List<Position> ChildPositions = new List<Position>();
    private Position ParentPosition;

    private bool IsTerminalPosition;
    private bool IsAIToPlay = true; //White = AI. De base c'est toujours l'AI qui commence vu que le player va pas minimax. Cette variable existe pour gerer les sub positions

    private int PositionEvaluationScore;
    private int PawnValue = 1;
    private int KingValue = 3;

    void Start()
    {
        PasteOriginalBoardPosition();
        //StartCoroutine(PrintBothBoards());
    }

    IEnumerator PrintBothBoards()
    {
        while (true)
        {
            print("Number of pieces in the original board: " + VisualGameBoard.UnsortedPieces.Count);
            print("Number of pieces in the copied board: " + PositionBoardData.UnsortedPieces.Count);
            yield return new WaitForSeconds(1);
        }
    }


    //Pour quand on cr�� la position de base
    public void PasteOriginalBoardPosition()
    {
        //PositionBoardData = ;
    }

    //Parent = la position que cette fonction a �t� appel�. Chaque child est une variation possible de la position parente
    //Chaque child va continuer de generer jusqu'a la depth max ou bien une position terminale
    public void GenerateChilds()
    {
        List<Move> LegalMoves = PositionBoardData.GetAllLegalMoves(IsAIToPlay);

        //Cr�er et set la position enfant pour chaque move l�gal puis continuer r�cursivement
        foreach (Move move in LegalMoves)
        {
            Position NewChildPosition = new Position(); //Cr��r une position enfant
            NewChildPosition.ParentPosition = this;
            //NewChildPosition.PositionBoardData =  //Copier le board de la position parente
            NewChildPosition.IsAIToPlay = !IsAIToPlay; //Changer le tour

            //Appliquer le move a la position enfant
            NewChildPosition.PositionBoardData.MovePiece(move);

            NewChildPosition.GenerateChilds(); //R�cursivit� (continuer tant que c'est pas une position terminale)
            ChildPositions.Add(NewChildPosition); //Garder ces positions en m�moire dans le tableau d'enfants
        }

        if (LegalMoves.Count == 0)
        {
            IsTerminalPosition = true;
        }

    }

    public void EvaluatePosition()
    {

    }

    

}
