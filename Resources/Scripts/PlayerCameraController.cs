using UnityEngine;

public class PlayerCameraController : ObjectController
{
    public string TargetName = string.Empty;
    public Vector3 ViewOffset = new Vector3(0.0f, 1.7f, 0.0f);

    protected GameObject _PlayerTarget;

    protected virtual void Start()
    {
        _PlayerTarget = GameObject.Find(TargetName);
    }

    public bool HasTarget => (_PlayerTarget != null);

    public void SetTargetByName(string targetName)
    {
        if(string.IsNullOrEmpty(targetName))
        {
            return;
        }
        
        GameObject foundObj = GameObject.Find(targetName);
        if (foundObj != null)
        {
            TargetName = targetName;
            _PlayerTarget = foundObj;
        }
    }
}
