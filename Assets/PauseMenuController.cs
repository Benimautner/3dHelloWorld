using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    
    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(ExitGame);
        resumeButton.onClick.AddListener(HideMenu);
        mainMenuButton.onClick.AddListener(BackToMenu);
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
                HideMenu();
            else
                ShowMenu();
        }

        if (pauseMenu.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void HideMenu()
    {
        pauseMenu.SetActive(false);
        SharedInfo.InMenu = false;
    }

    void ShowMenu()
    {
        pauseMenu.SetActive(true);
        SharedInfo.InMenu = true;
    }
    
    void ExitGame()
    {
        if (Application.isEditor)
            EditorApplication.isPlaying = false;
        else
            Application.Quit();
    }

    void BackToMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
