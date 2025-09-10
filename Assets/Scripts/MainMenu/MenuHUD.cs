using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHUD : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Checkers");
    }

}
