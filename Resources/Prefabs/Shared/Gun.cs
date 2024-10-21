using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] string _SocketName = "GunSocket";
    [SerializeField] protected Transform _Muzzle;

    public Transform MuzzleTransform => _Muzzle;
    public GameObject GunCarrier { get; private set; } = null;

    private void Awake()
    {
        var controller = (PlayerShooterController)GetComponentInParent<ObjectController>();
        if(controller != null)
        {
            controller.PickupGun(this);
            GunCarrier = controller.gameObject;
        }
    }

    public void AttachTo(GameObject character)
    {
        Transform socket = null;
        Transform[] childrenTransforms = character.GetComponentsInChildren<Transform>();
        foreach (var childTransform in childrenTransforms)
        {
            if (childTransform.name.CompareTo(_SocketName) == 0)
            {
                socket = childTransform;
                break;
            }
        }

        if (socket != null)
        {
            transform.parent = socket;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }
}
