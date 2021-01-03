using DefaultNamespace;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;
    private Quaternion angleOffset;

    private Vector3 offset;

    // Start is called before the first frame update
    private void Start()
    {
        offset = target.transform.position - transform.position + new Vector3(0.0f, 0.0f, 0.0f);
        angleOffset = Quaternion.Euler(30, 0, 0);
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (!SharedInfo.InMenu) {
            var angleY = target.transform.eulerAngles.y;
            var angleZ = target.transform.eulerAngles.z;
            var rotation = Quaternion.Euler(0, angleY, angleZ);
            transform.position = target.transform.position - rotation * offset;
            transform.LookAt(target.transform);
            transform.position += new Vector3(0, 2, 0);
            transform.rotation = transform.rotation * angleOffset;
        }
    }
}