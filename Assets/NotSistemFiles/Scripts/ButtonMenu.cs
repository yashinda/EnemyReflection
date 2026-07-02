using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMenu : MonoBehaviour
{
    public void OnNewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
