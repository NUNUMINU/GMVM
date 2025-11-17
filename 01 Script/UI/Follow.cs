using UnityEngine;

public class Follow : MonoBehaviour {
    
    private RectTransform _rect;
    void Awake() {
        _rect = GetComponent<RectTransform>();
    }

    void FixedUpdate() {
        _rect.position = Camera.main.WorldToScreenPoint(GameManager.Instance.Player.transform.position);
    }
}
