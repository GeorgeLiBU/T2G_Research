using UnityEngine;

public class AR15IKController : MonoBehaviour
{
    private readonly float MAX_AIM_ANGLE = 60.0f;
    [SerializeField] string _SpineTransformName = "spine";
   
    private Transform _Spine;

    bool FindSpineTransform()
    {
        _Spine = null;
        var parent = transform.parent;
        while(parent != null)
        {
            var grandParent = parent.parent;
            if (parent.name.ToLower().IndexOf(_SpineTransformName) >= 0 &&
                (grandParent == null || grandParent.name.ToLower().IndexOf(_SpineTransformName) < 0))
            {
                _Spine = parent;
                break;
            }
            parent = parent.parent;
        }
        return (_Spine != null);
    }

    public void SetTargetPositionByMousePosition(Vector3 mousePosition)
    {
        if(_Spine == null && !FindSpineTransform())
        {
            return;
        }

        //Normalize the mouse position into the range from -1 to 1 
        Vector2 mousePosNormalized = new Vector2(
             Mathf.Clamp(mousePosition.x / Screen.width, 0.0f, 1.0f) - 0.5f,
             Mathf.Clamp(mousePosition.y / Screen.height, 0.0f, 1.0f) - 0.5f) * 2.0f;

        _Spine.localRotation = Quaternion.Euler(
            -MAX_AIM_ANGLE * mousePosNormalized.y,
            MAX_AIM_ANGLE * mousePosNormalized.x,
            0.0f);
    }

    void OnSetRagdollEnabledMessage(bool ragdallEnabled)
    {
        this.enabled = !ragdallEnabled;
    }
}

