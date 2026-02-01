using System.Collections;
using System.Collections.Generic;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
