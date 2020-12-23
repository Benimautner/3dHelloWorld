using System.Collections;
using System.Collections.Generic;
using System.Xml;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;

public class HeadController : MonoBehaviour
{
    public float mouseMultiplicator;

    // Start is called before the first frame update
    void Start()
    {
        if (mouseMultiplicator == 0)
        {
            mouseMultiplicator = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!SharedInfo.inMenu)
        {
            Vector3 movement = new Vector3(0, Input.GetAxis("Mouse X") * mouseMultiplicator,
                Input.GetAxis("Mouse Y") * mouseMultiplicator);

            transform.eulerAngles += movement;
            limitRotation();
        }
    }
    

    private void limitRotation()
    {
        Vector3 loc_rotation = transform.eulerAngles;
        loc_rotation.z -= 90;
        loc_rotation.z = Clamp(loc_rotation.z, 10f, 170f);
        loc_rotation.z += 90;
        transform.eulerAngles = loc_rotation;
    }

    private float Clamp(float value, float min, float max)
    {
        if ((double) value < (double) min)
        {
            value = min;
        }else if ((double) value > (double) max)
        {
            value = max;
        }
        return value;
    }
}
