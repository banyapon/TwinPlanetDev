using UnityEngine;

public class BulletFacingCam : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.current.transform.position, Vector3.up);
    }
}
