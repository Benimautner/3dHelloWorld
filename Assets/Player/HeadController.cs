using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class HeadController : MonoBehaviour
{
    public float mouseMultiplicator = 10;

    public float maxRotation = 50;

    public float minRotation = 320;
    // Start is called before the first frame update
    private void Start()
    {
        if (mouseMultiplicator == 0) mouseMultiplicator = 10;
        transform.localEulerAngles += new Vector3(90, 0, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!SharedInfo.InMenu) {
            var movement = new Vector3();
            movement.x = 0;
            if(Mathf.Abs(Input.GetAxis("Mouse X")) > 0.0f || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.0f) {
                movement.y = Input.GetAxis("Mouse X") * mouseMultiplicator;
                movement.x = Input.GetAxis("Mouse Y") * mouseMultiplicator;
            }

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
        var locRotation = transform.eulerAngles;
        
        if (locRotation.x < 180) {
            if (locRotation.x > maxRotation) locRotation.x = maxRotation;
        }
        else {
            if (locRotation.x < minRotation) locRotation.x = minRotation;
        }

        transform.eulerAngles = locRotation;
    }
    
    public static float DebugClamp(float value, float min, float max)
    {
        if (value > 180) return value;
        if ((double) value < (double) min) {
            print("clamped at min " + value);
            value = min;
        }else if ((double) value > (double) max) {
            print("clamped at max " + value);
            value = max;
        }
        return value;
    }

    // ONLY WORKING TO SOME EXTENT

        
}