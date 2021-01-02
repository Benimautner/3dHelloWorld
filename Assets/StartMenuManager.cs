using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField] private Button startGameButton;

    [SerializeField] private Button exitGameButton;
    
    [SerializeField] private Text loadingText;

    // Start is called before the first frame update
    void Start()
    {
        loadingText.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        startGameButton.onClick.AddListener(LaunchGame);
        exitGameButton.onClick.AddListener(EndGame);
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void LaunchGame()
    {
        loadingText.enabled = true;
        print("Launching Game");
        SceneManager.LoadScene("SampleScene");
    }

    void EndGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
