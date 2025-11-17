using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Reposition : MonoBehaviour {
    
    private PlayerController _player;
    public Collider2D _coll;
    
    void Start() {
        _player = GameManager.Instance.Player;
        _coll = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!collision.CompareTag("Area")) return;

        Vector3 playerPos = _player.transform.position;
        Vector3 myPos = transform.position;
        
        switch (transform.tag) {
            case "Ground":
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;

                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = MathF.Abs(diffX);
                diffY = MathF.Abs(diffY);

                if (diffX > diffY) {
                    transform.Translate(Vector3.right * dirX * 40);
                } else if (diffX < diffY) {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            
            case "Enemy":
                if (_coll.enabled) {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ranVec = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0f);
                    transform.Translate(ranVec + dist * 2);
                }
                
                break;
        }
    }
}
