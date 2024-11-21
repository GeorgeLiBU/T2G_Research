using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FirstPersonCameraController : PlayerCameraController
{
    protected override void Start()
    {
        base.Start();
        ViewOffset = new Vector3(0.0f, 1.7f, 0.0f);
    }


    void LateUpdate()
    {
        if (_PlayerTarget != null)
        {
            transform.position = _PlayerTarget.transform.position + ViewOffset;
            transform.rotation = _PlayerTarget.transform.rotation;
        }
    }
}
