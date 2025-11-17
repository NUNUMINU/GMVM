using System;
using UnityEngine;

public class Hand : MonoBehaviour {
  public bool _isLeft;
  public SpriteRenderer _spriter;

  public SpriteRenderer _player;

  private Vector3 _rightPos = new Vector3(0.35f, -0.15f, 0);
  private Vector3 _rightPosRevers = new Vector3(-0.15f, -0.15f, 0);
  Quaternion _leftRot = Quaternion.Euler(0, 0, -35);
  Quaternion _leftRotRevers = Quaternion.Euler(0, 0, -135);


  void Awake() {
    _player = GetComponentsInParent<SpriteRenderer>()[1];
  }

  private void LateUpdate() {
    bool isRevers = _player.flipX;

    if (_isLeft) { // 근거리무기
      transform.localRotation = isRevers ? _leftRotRevers : _leftRot;
      _spriter.flipY = isRevers;
      _spriter.sortingOrder = isRevers ? 4 : 6;
    }
    else { // 원거리무기
      transform.localPosition = isRevers ? _rightPosRevers : _rightPos;
      _spriter.flipX = isRevers;
      _spriter.sortingOrder = isRevers ? 6 : 4;
    }
  }
}
