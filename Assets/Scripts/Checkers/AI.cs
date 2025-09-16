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

        //Si il ya des captures possibles, changer la liste de moves pour seulement contenir les captures.
        List<Move> Captures = new List<Move>();
        foreach (Move move in moves)
        {
            if (move.IsCapture) Captures.Add(move);
        }
        if (Captures.Count > 0)
        {
            moves = Captures;
        }

        Move chosen = moves[Random.Range(0, moves.Count)];
        board.MovePiece(chosen);
    }
}