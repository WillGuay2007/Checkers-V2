using System.Collections;
using UnityEngine;

public class RealPlayer : Player
{
    [SerializeField] private Board board;

    public override void MakeMove()
    {
        StartCoroutine(EnableAfterDelay());
    }

    private IEnumerator EnableAfterDelay()
    {
        yield return new WaitForSeconds(0.25f);
        board.EnableTileSelection();
    }
}