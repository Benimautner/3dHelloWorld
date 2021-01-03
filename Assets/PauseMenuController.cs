using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;

#endif

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;

    // Start is called before the first frame update
    private void Start()
    {
        exitButton.onClick.AddListener(ExitGame);
        resumeButton.onClick.AddListener(HideMenu);
        mainMenuButton.onClick.AddListener(BackToMenu);
        pauseMenu.SetActive(false);
        //Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (pauseMenu.activeSelf)
                HideMenu();
            else
                ShowMenu();
        }

        if (pauseMenu.activeSelf) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void HideMenu()
    {
        pauseMenu.SetActive(false);
        SharedInfo.InMenu = false;
    }

    private void ShowMenu()
    {
        pauseMenu.SetActive(true);
        SharedInfo.InMenu = true;
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}