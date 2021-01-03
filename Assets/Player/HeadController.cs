using DefaultNamespace;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class HeadController : MonoBehaviour
{
    public float mouseMultiplicator;

    // Start is called before the first frame update
    private void Start()
    {
        if (mouseMultiplicator == 0) mouseMultiplicator = 10;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!SharedInfo.InMenu) {
            var movement = new Vector3();
            movement.x = 0;
            if(Mathf.Abs(Input.GetAxis("Mouse X")) > 0.0f || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.0f) {
                movement.y = Input.GetAxis("Mouse X") * mouseMultiplicator;
                movement.z = Input.GetAxis("Mouse Y") * mouseMultiplicator;
            }

            print("X: " + Input.GetAxis("Joystick X"));
            if (Mathf.Abs(Input.GetAxis("Joystick X")) > 0.4f) {
                movement.y += Input.GetAxis("Joystick X");
            }
            
            if (Mathf.Abs(Input.GetAxis("Joystick Y")) > 0.4f ) {
                movement.z += Input.GetAxis("Joystick Y");
            }
            
            transform.eulerAngles += movement;

            LimitRotation();
        }
    }


    private void LimitRotation()
    {
        var loc_rotation = transform.eulerAngles;
        loc_rotation.z -= 90;
        loc_rotation.z = Clamp(loc_rotation.z, 10f, 170f);
        loc_rotation.z += 90;
        transform.eulerAngles = loc_rotation;
    }

    private float Clamp(float value, float min, float max)
    {
        if (value < (double) min)
            value = min;
        else if (value > (double) max) value = max;
        return value;
    }
}