using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour {
    
    private bool _isLive;
    private float _movSpeed;
    private float _hp;
    private float _maxHp;
    [SerializeField] private RuntimeAnimatorController[] _animCon;
    
    
    private Rigidbody2D _rb;
    private Collider2D _coll;
    private SpriteRenderer _spriter;
    private Animator _anim;
    private WaitForFixedUpdate _wait;
    
    [SerializeField]private Rigidbody2D target;
    private Vector2 _movDir;

    
    void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
        _spriter = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _wait = new WaitForFixedUpdate();
    }

    private void OnEnable() {
      target = GameManager.Instance.Player.GetComponent<Rigidbody2D>();
      _isLive = true;
      _coll.enabled = true;
      _rb.simulated = true;
      _spriter.sortingOrder = 2;
      _anim.SetBool("Dead", false);
      _hp = _maxHp;
    }

    public void Init(SpawnData data) {
      _anim.runtimeAnimatorController = _animCon[data._spriteType];
      _movSpeed = data._movSpeed;
      _hp = _maxHp = data._hp;
    }
    
    void OnTriggerEnter2D(Collider2D collision) {
      if (!_isLive || !collision.CompareTag("Bullet")) return;

      _hp -= collision.GetComponent<Bullet>().Damage;
      StartCoroutine(KnockBack());

      if (_hp > 0) {
        _anim.SetTrigger("Hit");
        
        AudioManager.Instance.PlaySfx(AudioManager.SFX.Hit);
      }
      else {
        _isLive = false;
        _coll.enabled = false;
        _rb.simulated = false;
        _spriter.sortingOrder = 1;
        _anim.SetBool("Dead", true);
        GameManager.Instance._kill++;
        GameManager.Instance.GetExp();
        
        if(GameManager.Instance.IsLive)
          AudioManager.Instance.PlaySfx(AudioManager.SFX.Dead);
      }
    }

    IEnumerator KnockBack() {
      yield return _wait;
      
      Vector3 playerPos = GameManager.Instance.Player.transform.position;
      Vector3 dirVec = transform.position - playerPos;
      _rb.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }
    

    public void Dead() {
      gameObject.SetActive(false);
    }
  

    private void Update() {
      if (!GameManager.Instance.IsLive) return;
      
      _movDir = target.position - _rb.position;
      Rotate();
    }
    
    void Rotate() {
      if (!_isLive) return;
      
      if (_movDir.x != 0) {
        _spriter.flipX = _movDir.x < 0;
      }
    }
  
    void FixedUpdate() {
      if (!GameManager.Instance.IsLive) return;
      
      if (!_isLive || _anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;
      
      Vector2 movVec = _movDir.normalized;
      _rb.MovePosition(_rb.position + movVec * _movSpeed * Time.fixedDeltaTime);
      _rb.linearVelocity = Vector2.zero;
    }
}
