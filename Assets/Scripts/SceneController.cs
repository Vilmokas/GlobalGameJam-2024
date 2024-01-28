using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] GameObject creditsWindow;
    [SerializeField] GameObject gameEndWindow;
    [SerializeField] TextMeshProUGUI gameEndText;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowCredits()
    {
        creditsWindow.SetActive(!creditsWindow.active);
    }

    public void ShowGameEndWindow(bool gameWon)
    {
        gameEndText.text = gameWon ? "You won!" : "You lost!";
        gameEndWindow.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
