using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAndThirdPersonCameraController : PlayerCameraController
{
    public enum ECameraViewMode
    {
        FirstPersonView,
        ThirdPersonView
    }

    public Vector3 ThirdPersonViewOffset { get; set; }  = new Vector3(0.0f, 3.0f, -5.0f);
    public float TransitionTime { get; set; } = 0.3f;

    private ECameraViewMode _cameraViewMode = ECameraViewMode.ThirdPersonView;
    public ECameraViewMode ViewMode => _cameraViewMode;

    private Vector3 _transitVel = Vector3.zero;

    protected override void Start()
    {
        base.Start();
        ViewOffset = new Vector3(0.0f, 1.7f, 0.0f);
    }

    private void Update()
    {
        var mouseScroll = Input.mouseScrollDelta.y;
        if(mouseScroll < -0.1f && _cameraViewMode == ECameraViewMode.FirstPersonView)
        {
            _cameraViewMode = ECameraViewMode.ThirdPersonView;
        }
        if (mouseScroll > 0.1f && _cameraViewMode == ECameraViewMode.ThirdPersonView)
        {
            _cameraViewMode = ECameraViewMode.FirstPersonView;
        }
    }

    void LateUpdate()
    {
        LocateCamera(_cameraViewMode == ECameraViewMode.FirstPersonView);
    }

    void LocateCamera(bool immediately = false)
    {
        Vector3 target = Vector3.zero;
        Action rotateCamera = null;
        switch (_cameraViewMode)
        {
            case ECameraViewMode.FirstPersonView:
                target = _PlayerTarget.transform.position + ViewOffset;

                rotateCamera = () =>
                {
                    transform.rotation = _PlayerTarget.transform.rotation;
                };
                break;
            case ECameraViewMode.ThirdPersonView:
                target = _PlayerTarget.transform.rotation * ThirdPersonViewOffset + _PlayerTarget.transform.position;
                rotateCamera = () =>
                {
                    transform.LookAt(_PlayerTarget.transform.position + Vector3.up * 2.0f);
                };
                break;
        }

        if (immediately)
        {
            transform.position = target;
            _transitVel = Vector3.zero;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                target,
                ref _transitVel,
                TransitionTime);
        }

        if (rotateCamera != null)
        {
            rotateCamera();
        }
    }

    public void SetViewMode(ECameraViewMode viewMode, bool immediate = false)
    {
        _cameraViewMode = viewMode;
        if (HasTarget)
        {
            LocateCamera(immediate);
        }
    }
}
