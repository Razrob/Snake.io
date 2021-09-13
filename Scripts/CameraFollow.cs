using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offcet;

    private void FixedUpdate()
    {
        if (_target != null) transform.position = Vector3.Lerp(transform.position, _target.position, _moveSpeed * Time.fixedDeltaTime) + _offcet;
    }

    public void SetTarget(Transform _cameraTarget) => _target = _cameraTarget;
}
