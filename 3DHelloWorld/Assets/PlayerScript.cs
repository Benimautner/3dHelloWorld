using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private CharacterController body;

    private Vector3 move;
    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.AddComponent<CharacterController>();
        move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    // Update is called once per frame
    void Update()
    {
        body.Move(move);
    }

}
