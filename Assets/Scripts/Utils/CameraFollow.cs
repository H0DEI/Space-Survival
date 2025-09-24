using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollowIso : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.15f;
    [SerializeField] private bool findPlayerByTag = true;

    private Vector3 _vel;
    private Vector3 _offset;
    private bool _hasOffset;

    void Start()
    {
        if (!target && findPlayerByTag)
            target = GameObject.FindWithTag("Player")?.transform;

        TryInitOffset();
    }

    void LateUpdate()
    {
        if (!target)
            return;

        if (!_hasOffset)
            TryInitOffset();

        Vector3 desired = target.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref _vel, smoothTime);
    }

    private void TryInitOffset()
    {
        if (target == null) return;
        _offset = transform.position - target.position;
        _hasOffset = true;
    }

    public void SnapToTarget()
    {
        if (!target) return;
        if (!_hasOffset) TryInitOffset();
        transform.position = target.position + _offset;
        _vel = Vector3.zero;
    }
}
