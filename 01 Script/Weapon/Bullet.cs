using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] private float _damage;
    [SerializeField] private int _per;
    public float Damage => _damage;

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir) {
        _damage = damage;
        _per = per;

        if (_per >= 0) {
            rb.linearVelocity = dir * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Enemy") || _per == -99) return;
        

        if (--_per < 0) {
            rb.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
            // Debug.Log("1");
        }
    }

   private void OnTriggerExit2D(Collider2D other) {
       if (!other.CompareTag("Area") || _per == -99) return;
       
           rb.linearVelocity = Vector2.zero;
           gameObject.SetActive(false);
           // Debug.Log("2");
    }
}
