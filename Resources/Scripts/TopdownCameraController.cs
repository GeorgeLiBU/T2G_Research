using UnityEngine;

public class TopdownCameraController : PlayerCameraController
{
    protected override void Start()
    {
        base.Start();
        ViewOffset = new Vector3(0.0f, 5.0f, -8.0f);
    }

    void LateUpdate()
    {
        LocateCamera();
    }

    void LocateCamera()
    {
        transform.position = _PlayerTarget.transform.position + ViewOffset;
        transform.LookAt(_PlayerTarget.transform.position);
    }
}
