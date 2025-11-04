using System.Collections;
using UnityEditor.Build.Content;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public IEnumerator EndGame()
    {
        print("Returning to menu...");
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainMenu");
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


    private void DisplayWinner()
    {

    }

}
