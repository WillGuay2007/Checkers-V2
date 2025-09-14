using UnityEngine;

public class Game : MonoBehaviour
{
    private Player CurrentPlayer;
    [SerializeField] private RealPlayer RealPlayer;
    [SerializeField] private AI AIPlayer;

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        CurrentPlayer = RealPlayer;
        CurrentPlayer.MakeMove();
    }

    public void EndGame()
    {

    }

    public void SwitchTurn()
    {
        if (CurrentPlayer == RealPlayer) {
            CurrentPlayer = AIPlayer;
        } else
        {
            CurrentPlayer = RealPlayer;
        }

        CurrentPlayer.MakeMove();

    }

    public void CheckIfWinner()
    {

    }

    private void DisplayWinner()
    {

    }

}
