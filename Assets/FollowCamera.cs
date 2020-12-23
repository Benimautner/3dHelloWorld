using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;
    private Quaternion angleOffset;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = target.transform.position - transform.position;
        angleOffset = Quaternion.Euler(30, 0, 0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!SharedInfo.inMenu)
        {
            float angleY = target.transform.eulerAngles.y;
            float angleZ = target.transform.eulerAngles.z;
            Quaternion rotation = Quaternion.Euler(0, angleY, angleZ);
            transform.position = target.transform.position - (rotation * offset);
            transform.LookAt(target.transform);
            transform.position += new Vector3(0, 1, 0);
            transform.rotation = transform.rotation * angleOffset;
        }
    }


}
