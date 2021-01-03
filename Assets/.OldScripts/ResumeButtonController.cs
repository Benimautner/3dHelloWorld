using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButtonController : MonoBehaviour
{
    [SerializeField] private Button btn;

    private GameObject menuControllerGameObject;

    private PauseMenuController _menuController;
    // Start is called before the first frame update
    void Start()
    {
        menuControllerGameObject = GameObject.Find("PauseMenuControllerObject");
        _menuController = menuControllerGameObject.GetComponent(typeof(PauseMenuController)) as PauseMenuController;
        btn.onClick.AddListener(onClick);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void onClick()
    {
        _menuController.HideMenu();
    }
}
