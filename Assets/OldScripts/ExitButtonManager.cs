using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ExitButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Button btn;
    
    void Start()
    {
        btn.onClick.AddListener(onClick);
    }

    // Update is called once per frame
    void Update()
    {
        }

    void onClick()
    {
        if (Application.isEditor)
            EditorApplication.isPlaying = false;
        else
            Application.Quit();    
    }

}
