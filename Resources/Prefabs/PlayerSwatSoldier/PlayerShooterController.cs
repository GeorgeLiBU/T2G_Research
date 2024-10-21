using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerShooterController : ObjectController
{
    private readonly string _animparam_Velocity = "Velocity";
    private readonly string _animparam_Strafe = "Strafe";

    [SerializeField] private float _WalkSpeed = 1.5f;
    [SerializeField] private float _RunSpeed = 3.2f;
    [SerializeField] private float _StrafeSpeed = 3.0f;
    [SerializeField] private float _RotationSpeed = 5.0f;
    
    private bool _IsRagdoll = false;
    private bool _AcceptPlayerInput = false;
    private Collider[] _ragdollColliders;
    private Rigidbody[] _ragdollRigidBodies;

    private Rigidbody _rigidBody;
    private Animator _animator;
    private Collider _collider;

    [HideInInspector] public bool IsRunning = true;

    public Gun Gun { get; private set; } = null;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponent<Collider>();

        _ragdollColliders = GetComponentsInChildren<Collider>();
        _ragdollRigidBodies = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < _ragdollColliders.Length; ++i)
        {
            if (_ragdollColliders[i].name.CompareTo(_rigidBody.name) == 0)
            {
                continue;
            }
            Physics.IgnoreCollision(_collider, _ragdollColliders[i]);
            Physics.IgnoreCollision(_ragdollColliders[i], _collider);
        }
    }

    void Start()
    {
        SetRagdollEnabled(_IsRagdoll);
    }

    void SetRagdollEnabled(bool enabled)
    {
        _AcceptPlayerInput = !enabled;
        _animator.enabled = _collider.enabled = !enabled;
        _rigidBody.isKinematic = enabled;

        gameObject.BroadcastMessage("OnSetRagdollEnabledMessage", enabled, SendMessageOptions.RequireReceiver);
        var cameraController = FindFirstObjectByType<PlayerCameraController>();
        if (cameraController != null)
        {
            cameraController.SetTargetByName(enabled ? null : "PlayerCharacter");
        }

        for (int i = 0; i < _ragdollColliders.Length; ++i)
        {
            if (_ragdollColliders[i].name.CompareTo(_rigidBody.name) == 0)
            {
                continue;
            }
            _ragdollColliders[i].enabled = enabled;
            _ragdollRigidBodies[i].useGravity = enabled;
            _ragdollRigidBodies[i].isKinematic = !enabled;
        }
    }

    private void Update()
    {
        Vector2 axisInputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (axisInputs == Vector2.zero)
        {
            _animator.SetFloat(_animparam_Velocity, 0.0f);
        }
        else
        {
            MovePlayer(axisInputs);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            IsRunning = !IsRunning;
        }
    }

    public void MovePlayer(in Vector2 axisMove)
    {
        Vector3 trans = axisMove.y * _rigidBody.transform.forward + axisMove.x * _rigidBody.transform.right;
        Vector3 vel = trans.normalized;
        if (axisMove.y == 0.0f)      //Strafe
        {
            _rigidBody.linearVelocity = new Vector3(vel.x * _StrafeSpeed, _rigidBody.linearVelocity.y, vel.z * _StrafeSpeed);
            _animator.SetFloat(_animparam_Velocity, 0.0f);
            _animator.SetFloat(_animparam_Strafe, axisMove.x);
        }
        else
        {
            float speed = IsRunning ? _RunSpeed : _WalkSpeed;
            _rigidBody.linearVelocity = new Vector3(vel.x * speed, _rigidBody.linearVelocity.y, vel.z * speed);
            if (axisMove.x != 0.0f)
            {
                _rigidBody.transform.rotation = Quaternion.Lerp(_rigidBody.transform.rotation,
                    Quaternion.LookRotation(trans),
                    Time.deltaTime * _RotationSpeed);
            }
            _animator.SetFloat(_animparam_Velocity, _rigidBody.linearVelocity.magnitude * Mathf.Sign(axisMove.y));
            _animator.SetFloat(_animparam_Strafe, 0.0f);
        }
    }

    public void PickupGun(Gun gun)
    {
        DropGun();
        if (gun != null)
        {
            gun.AttachTo(gameObject);
            Gun = gun;
        }
    }

    public void DropGun()
    {
        if(Gun != null)
        {
            Gun.transform.parent = null;
            Gun = null;
        }
    }

    public void Hit()
    {

    }

    public void Kill()
    {

    }
}
