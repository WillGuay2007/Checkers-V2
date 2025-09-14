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

        List<Move> moves = board.GetAllLegalMovesForOpponent();
        if (moves.Count <= 0)
        {
            print("AI lost the game.");
            game.EndGame();
            yield break;
        }

        Move chosen = moves[Random.Range(0, moves.Count)];
        board.MovePiece(chosen);
    }
}