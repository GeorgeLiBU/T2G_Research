using UnityEngine;

public class PlayerCameraController : ObjectController
{
    [SerializeField] protected GameObject _PlayerTarget;
    [SerializeField] protected Vector3 _ViewOffset = new Vector3(0.0f, 1.7f, 0.0f);
    public void SetTargetByName(string targetName)
    {
        if (string.IsNullOrEmpty("targetName"))
        {
            _PlayerTarget = null;
        }
        else
        {
            _PlayerTarget = GameObject.Find(targetName);
        }
    }

    public void SetViewOffset(Vector3 viewOffset)
    {
        _ViewOffset = viewOffset;
    }

    public bool HasTarget => (_PlayerTarget != null);
}
