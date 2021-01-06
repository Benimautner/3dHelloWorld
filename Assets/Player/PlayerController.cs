using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] public CharacterController characterController;
    [SerializeField] public bool gravityEnabled;
    private Animator _animator;
    public float smoothVelocity;
    public float smoothTime = 0.2f;
    
    public float walkSpeed = 2;
    public float runSpeed = 6;
    private float _currentSpeed;
    public float gravity = -12;
    private float _velocityY;

    private bool _inWater;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        characterController.Move(new Vector3(0, 50, 0));
    }

    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (characterController.isGrounded) {
            if (Input.GetButtonDown("Jump")) {
                float jumpVelocity = Mathf.Sqrt(-2 * gravity * 2);
                _velocityY = jumpVelocity;
            }
        }

        Vector2 inputDir = input.normalized;
        
        if (inputDir != Vector2.zero) {
            transform.eulerAngles += Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg * Vector3.up / 50.0f;
        }
        bool running = Input.GetKey(KeyCode.LeftShift) && !_inWater;
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref smoothVelocity, smoothTime);

        
        if(gravityEnabled) _velocityY += Time.deltaTime * gravity;


        Vector3 vel = transform.forward * _currentSpeed + Vector3.up * _velocityY;
        characterController.Move(vel * Time.deltaTime);
        _currentSpeed = new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;
        if (characterController.isGrounded) {
            _velocityY = 0;
        }
        
        float animationSpeedPercent = ((running) ? _currentSpeed/runSpeed : _currentSpeed/walkSpeed* .5f);
        _animator.SetFloat("speedPercent", animationSpeedPercent,smoothTime, Time.deltaTime);
    }

    public void InWater(bool status)
    {
        _inWater = status;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 1000, 1000), ((int) (1.0f / Time.smoothDeltaTime)).ToString());
    }
}