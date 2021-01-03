using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartMenuManager : MonoBehaviour
{
    [SerializeField] private Button startGameButton;

    [SerializeField] private Button exitGameButton;

    [SerializeField] private Text loadingText;

    // Start is called before the first frame update
    private void Start()
    {
        loadingText.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        startGameButton.onClick.AddListener(LaunchGame);
        exitGameButton.onClick.AddListener(EndGame);
    }

    // Update is called once per frame
    private void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LaunchGame()
    {
        loadingText.enabled = true;
        print("Launching Game");
        SceneManager.LoadScene("SampleScene");
    }

    private void EndGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}