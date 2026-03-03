using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public void playGame()
    {
        SceneManager.LoadScene(4);
    }
    public void restartGame()
    {
        SceneManager.LoadScene(4);

    }
    public void quitGame()
    {
        Application.Quit();
        
    }

}
