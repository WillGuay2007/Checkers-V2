using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Player
{

    [SerializeField] private Board board;
    [SerializeField] private Game game;

    public override void MakeMove()
    {
        board.DisableTileSelection();
        StartCoroutine(AIMove());
    }

    private IEnumerator AIMove()
    {
        yield return new WaitForSeconds(0.5f);

        if (board.IsInTerminalPosition(true))
        {
            print("AI lost the game.");
            StartCoroutine(game.EndGame());
            yield break;
        }

        List<Move> moves = board.GetAllLegalMoves(true);

        Move chosen = moves[Random.Range(0, moves.Count)];
        board.MovePiece(chosen);
    }

    //private Move Minimax(int depth, bool isMaximizing)
    //{
        
    //}

}