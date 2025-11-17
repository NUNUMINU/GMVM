using System;
using UnityEngine;

public class WeaponParent : MonoBehaviour {
    private int _id;
    private float _damage;
    private int _count;
    private int _prefabID;
    
    public float _speed;
    public float _meleeSpeed = 150f;
    public float _rangeSpeed = 0.5f;
    private float _timer;
    
    private PlayerController _player;
    
    public int ID => _id;
    public float Speed { get => _speed; set => _speed = value; }
    
    private void Awake() {
        _player = GameManager.Instance.Player;
    }
    
    public void Init(ItemData data) {
        // Basic Set
        name = "Weapon " + data._itemId;
        transform.parent = _player.transform;
        transform.localPosition = Vector3.zero;

        // Attribute Set
        _id = data._itemId;
        _damage = data._baseDamage * Character.Damage;
        _count = data._baseCount + Character.Count;

        for (int i = 0; i < GameManager.Instance.PM.Prefabs.Length; i++) {
            if (data._projectTile == GameManager.Instance.PM.Prefabs[i]) {
                _prefabID = i;
                break;
            }
        }
        
        switch (_id) {
            case 0:
                _speed = _meleeSpeed * Character.WeaponSpeed;
                Batch();
                break;
            
            default:
                _speed = _rangeSpeed * Character.WeaponRate;
                break;
        }

        Hand hand = _player._hands[(int)data._itemType];
        hand._spriter.sprite = data._hand;
        hand.gameObject.SetActive(true);
        
        _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void LevelUp(float damage, int count) {
        _damage = damage * Character.Damage;
        _count += count;

        if (_id == 0) {
            Batch();
        }
        
        _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch() {
        for (int i = 0; i < _count; i++) {
            Transform bullet;
            
            if (i < transform.childCount) {
                bullet = transform.GetChild(i);
            }
            else {
                bullet = GameManager.Instance.PM.Get(_prefabID).transform;
                bullet.parent = transform;
            }
            
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / _count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            
            bullet.GetComponent<Bullet>().Init(_damage, -99, Vector3.zero); // -99 is Infinity Per.
        }
    }

    void Fire() {
        if (!_player.Scan._nearestTarget)
            return;

        Vector3 targetPos = _player.Scan._nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.Instance.PM.Get(_prefabID).transform;
        bullet.parent = transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(_damage, _count, dir);
        
        AudioManager.Instance.PlaySfx(AudioManager.SFX.Range);
    }
    
    void Update() {
        if (!GameManager.Instance.IsLive) return;
        
        switch (_id) {
            case 0:
                transform.Rotate(Vector3.back * _speed * Time.deltaTime);
                break;
            
            default:
                _timer += Time.deltaTime;

                if (_timer > _speed) {
                    Fire();
                    _timer -= _speed;
                }
                break;
        }
    }
}
