using DefaultNamespace;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;
    private Quaternion _angleOffset;
    private Vector3 _offset;

    // Start is called before the first frame update
    private void Start()
    {
        _offset = target.transform.position - transform.position + new Vector3(0.0f, 0.0f, 0.0f);
        _angleOffset = Quaternion.Euler(30, 0, 0);
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (!SharedInfo.InMenu) {
            Vector3 targetEulerAngles = target.transform.eulerAngles;
            var angleY = targetEulerAngles.y;
            var angleZ = targetEulerAngles.x;
            var rotation = Quaternion.Euler(angleZ, angleY, 0);
            transform.position = target.transform.position - rotation * _offset + new Vector3(0, 2, 0);
            transform.LookAt(target.transform);
            transform.rotation *= _angleOffset;
        }
    }
}