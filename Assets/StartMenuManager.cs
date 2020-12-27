using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;

public class StartMenuManager : MonoBehaviour
{
    public Button  startGameButton;

    public Button exitGameButton;
    
    [SerializeField] private Text loadingText;

    // Start is called before the first frame update
    void Start()
    {
        loadingText.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        startGameButton.onClick.AddListener(LaunchGame);
        exitGameButton.onClick.AddListener(EndGame);
    }

    void LaunchGame()
    {
        loadingText.enabled = true;
        print("Launching Game");
        SceneManager.LoadScene("SampleScene");
    }

    void EndGame()
    {
        if (Application.isEditor)
            EditorApplication.isPlaying = false;
        else
            Application.Quit();
    }
}
