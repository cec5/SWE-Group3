using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public void playGame()
    {
        SceneManager.LoadScene(2);
    }
    public void restartGame()
    {
        SceneManager.LoadScene(2);

    }
    public void quitGame()
    {
        Application.Quit();
        
    }

}
