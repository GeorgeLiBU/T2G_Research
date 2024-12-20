using UnityEngine;

public class ThirdPersonCameraController : PlayerCameraController
{
    [SerializeField] private float TransitionTime = 0.3f;

    private Vector3 _transitVel = Vector3.zero;

    private Transform _firstPersonViewTransform;
    private Transform _gunMuzzleTransform;
    private Transform _gunCrosshairTransform;

    protected override void Start()
    {
        base.Start();
        ViewOffset = new Vector3(0.0f, 3.0f, -5.0f);
    }

    void LateUpdate()
    {
        LocateCamera();
    }

   void LocateCamera()
    {
        Vector3 target = Vector3.zero;
        target = _PlayerTarget.transform.rotation * ViewOffset + _PlayerTarget.transform.position;

        transform.position = Vector3.SmoothDamp(
                transform.position,
                target,
                ref _transitVel,
                TransitionTime);

        transform.LookAt(_PlayerTarget.transform.position + Vector3.up* 2.0f);
    }
}
