using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private SpriteRenderer _sprite;
    private Rigidbody2D _rb;
    private Animator _anim;
    private Scanner _scan;
    public Hand[] _hands;
    [SerializeField] private RuntimeAnimatorController[] _animCon;
    
    
    private Vector2 _moveInput;
    // private bool _isLive = true;
    public Vector2 MoveInput => _moveInput;
    public Scanner Scan => _scan;

    
    [SerializeField] private float _movSpeed = 3.0f;

    public float MovSpeed {
        get => _movSpeed;
        set => _movSpeed = value;
    }
    
    void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _scan = GetComponent<Scanner>();
        _hands = GetComponentsInChildren<Hand>(true);

    }

    private void OnEnable() {
        _movSpeed *= Character.Speed;
        _anim.runtimeAnimatorController = _animCon[GameManager.Instance._playerId];
    }

    void OnMove(InputValue value) {
        _moveInput = value.Get<Vector2>();
    }
    
    private void FixedUpdate() {
        if (!GameManager.Instance.IsLive) return;
        
        _rb.MovePosition(_rb.position + _moveInput * _movSpeed * Time.fixedDeltaTime);
    }

    void Update() {
        if (!GameManager.Instance.IsLive) return;
        
        SetAnim();
        Rotate();
    }

    void SetAnim() {
        if (_moveInput.magnitude > 0) {
            _anim.SetBool("IsRun", true);
        }
        else {
            _anim.SetBool("IsRun", false);
        }
    }
    
    void Rotate() {
        if (_moveInput.x != 0) {
            _sprite.flipX = _moveInput.x < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (!GameManager.Instance.IsLive) return;
        
        GameManager.Instance._hp -= Time.deltaTime * 10;

        if (GameManager.Instance._hp <= 0) {
            for (int i = 2; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            
            _anim.SetTrigger("IsDead");
            GameManager.Instance.GameOver();
        }
    }
}
