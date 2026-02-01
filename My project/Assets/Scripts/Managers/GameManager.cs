using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject loseMenu, winMenu;
    private void Start()
    {
        PlayerMovement.OnPlayerDeath += OnLose;
    }
    private void OnDisable()
    {
        PlayerMovement.OnPlayerDeath -= OnLose;
    }
    private void OnDestroy()
    {
        PlayerMovement.OnPlayerDeath -= OnLose;
    }
    void OnWin()
    {
        Time.timeScale = 0f;
        winMenu.SetActive(true);
    }
    void OnLose()
    {
        Time.timeScale = 0f;
        loseMenu.SetActive(true);
    }
    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private float delayBeforeLoad = 0.05f;

    public void PlayGame()
    {
        StartCoroutine(PlaySoundAndLoad());
    }

    private IEnumerator PlaySoundAndLoad()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        yield return new WaitForSecondsRealtime(delayBeforeLoad);

        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
