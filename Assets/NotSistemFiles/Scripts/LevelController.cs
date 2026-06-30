using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum State
{
    Playing,
    Failed,
    Paused
}

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    
    [Header("UI References")]
    [Tooltip("Pause Panel")]
    public GameObject pausePanel;
    [Tooltip("Failed Panel")]
    public GameObject failedPanel;
    
    private State _currentState = State.Playing;
    public State CurrentState => _currentState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void TogglePause()
    {
        if (_currentState == State.Playing)
            EnterPauseState();
        else if (_currentState == State.Paused)
            EnterPlayingState();
    }

    private void EnterPauseState()
    {
        _currentState = State.Paused;

        Time.timeScale = 0;
        ShowCursor(true);
        
        pausePanel?.SetActive(true);
    }

    private void EnterPlayingState()
    {
        _currentState = State.Playing;
        
        Time.timeScale = 1;
        ShowCursor(false);
        
        pausePanel?.SetActive(false);
    }
    
    private void ShowCursor(bool show)
    {
        Cursor.visible = show;
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void GameOver()
    {
        if (_currentState == State.Failed)
            return;
        
        _currentState = State.Failed;
        
        failedPanel?.SetActive(true);
        StartCoroutine(RestartLevel());
        MusicManager.Instance.PlayMusic(MusicType.Failed);
    }

    private IEnumerator RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentSceneIndex);

        asyncLoad.allowSceneActivation = false;

        yield return new WaitForSecondsRealtime(7.68f);
        
        Time.timeScale = 1;
        
        asyncLoad.allowSceneActivation = true;
    }
}
