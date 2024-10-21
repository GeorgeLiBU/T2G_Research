using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR15Controller : Gun
{
    [SerializeField] private Transform _AimCrosshair;
    [SerializeField] private Transform _FirstPersonViewpoint;
    [SerializeField] private GameObject _BulletPrefab;
    [SerializeField] private float _FiringSpeed = 3.0f;    //Fire bullets per second 

    private Camera _camera;
    private float _firingInterval;
    private bool _canFire = true;
    private AR15IKController _gunIKController;

    private void Start()
    {
        _gunIKController = GetComponent<AR15IKController>();
        _firingInterval = _FiringSpeed > 0.0f ? 1.0f / _FiringSpeed : 0.5f;
        _camera = FindFirstObjectByType<Camera>();
    }

    public void OnDrawGizmos()
    {
        //Draw aiming line
        Debug.DrawLine(_Muzzle.position, _AimCrosshair.position, Color.magenta);
        var dir = (_AimCrosshair.position - _Muzzle.position) * 30.0f;
        Debug.DrawLine(_AimCrosshair.position, _AimCrosshair.position + dir, Color.gray);
    }

    void LateUpdate()
    {
        if (_canFire && Input.GetMouseButton(0))
        {
            StartCoroutine(Fire());
        }

        Vector3 mousePos = Input.mousePosition;
        _gunIKController.SetTargetPositionByMousePosition(mousePos);

        _AimCrosshair.LookAt(_camera.transform.position, -Vector3.up);
    }

    IEnumerator Fire()
    {
        _canFire = false;
        yield return new WaitForSeconds(_firingInterval);
        //TODO: Need to use a bullet pool
        GameObject bullet = Instantiate(_BulletPrefab, _Muzzle.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(_Muzzle.forward * 3000.0f);
        _canFire = true;
    }

    public Transform GetCrosshairTransform()
    {
        return _AimCrosshair;
    }

    public Transform GetFirstPersonViewTransform()
    {
        return _FirstPersonViewpoint;
    }

    void OnSetRagdollEnabledMessage(bool ragdallEnabled)
    {
        this.enabled = !ragdallEnabled;
    }
}
