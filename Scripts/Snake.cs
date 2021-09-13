using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Snake : MonoBehaviour
{
    [SerializeField] private CameraFollow _cameraFollow;

    [SerializeField] private Transform _snakePartPrefab;
    [SerializeField] private Transform _snakeHadPrefab;
    [SerializeField] private int _partCount = 1;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _partDistance;

    [SerializeField] private SnakeScale _snakeScale;

    private List<Transform> _snakeParts = new List<Transform>();

    private Vector3 _moveDirection = Vector3.right;

    private void Start()
    {
        _snakeParts.Add(Instantiate(_snakeHadPrefab, transform));
        _cameraFollow.SetTarget(_snakeParts[0]);

        for (int i = 0; i < _partCount; i++)
        {
            _snakeParts.Add(Instantiate(_snakePartPrefab, _snakeParts[_snakeParts.Count - 1].position - _moveDirection * _partDistance + Vector3.forward * 0.2f, Quaternion.identity, transform));
        }
        UpdatePartScale();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) AddPart();
        if (Input.GetKeyDown(KeyCode.S)) RemovePart();

    }

    private void FixedUpdate()
    {

        Vector2 _targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if(Vector3.Distance(_snakeParts[0].position, _targetPosition) > 0.2f)
        {
            Vector3 _target = Vector3.RotateTowards(_moveDirection, (_targetPosition - (Vector2)_snakeParts[0].position).normalized, Time.fixedDeltaTime * _rotateSpeed, 0).normalized;
            _moveDirection = _target;
            _snakeParts[0].rotation = Quaternion.LookRotation(_target, Vector3.back);
        }

        _snakeParts[0].position += _moveDirection * Time.fixedDeltaTime * _speed;

        for (int i = 1; i < _snakeParts.Count; i++)
        {
            Vector3 _target = _snakeParts[i - 1].position;
            _target.z = _snakeParts[i].position.z;
            _snakeParts[i].position = Vector3.Lerp(_snakeParts[i].position, _target, (_speed * Time.fixedDeltaTime) / (_partDistance * Mathf.Pow(_snakeScale.NextPartScaleFactor, i)));
            _snakeParts[i].rotation = Quaternion.LookRotation(_target - _snakeParts[i].position, Vector3.back);


        }

    }

    private void UpdatePartScale()
    {
        for (int i = 0; i < _snakeParts.Count; i++) _snakeParts[i].localScale = Vector3.one * CalculatePartScale(i);
    }

    private void AddPart()
    {
        _partCount++;
        _snakeParts.Add(Instantiate(_snakePartPrefab, _snakeParts[_snakeParts.Count - 1].position - _moveDirection * _partDistance + Vector3.forward * 0.2f, Quaternion.identity, transform));
        UpdatePartScale();
    }

    private void RemovePart()
    {
        if (_partCount < 6) return;
        _partCount--;
        Destroy(_snakeParts[_snakeParts.Count - 1].gameObject);
        _snakeParts.RemoveAt(_snakeParts.Count - 1);
        UpdatePartScale();
    }

    private float CalculatePartScale(int _partNumber)
    {
        return _snakeScale.FirstPartScale * Mathf.Pow(_snakeScale.NextPartScaleFactor, _partNumber) * Mathf.Pow(_snakeScale.PartNumberScaleFactor, _partCount);
    }



    [Serializable]
    private class SnakeScale
    {
        public float FirstPartScale;
        public float NextPartScaleFactor;
        public float PartNumberScaleFactor;
    }
}
