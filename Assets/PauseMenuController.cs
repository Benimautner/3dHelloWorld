using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
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
        SharedInfo.inMenu = false;
    }

    void ShowMenu()
    {
        pauseMenu.SetActive(true);
        SharedInfo.inMenu = true;
    }
}
