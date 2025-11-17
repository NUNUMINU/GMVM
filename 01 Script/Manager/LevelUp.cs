using System;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelUp : MonoBehaviour {
  private RectTransform _rect;
  private Item[] _items;

  private void Awake() {
    _rect = GetComponent<RectTransform>();
    _items = GetComponentsInChildren<Item>(true);
  }

  public void Show() {
    _rect.localScale = Vector3.one;
    Next();
    GameManager.Instance.Stop();

    AudioManager.Instance.EffectBgm(true);
    AudioManager.Instance.PlaySfx(AudioManager.SFX.LevelUp);
  }
  
  public void Hide() {
    _rect.localScale = Vector3.zero;
    GameManager.Instance.Resume();
    
    AudioManager.Instance.EffectBgm(false);
    AudioManager.Instance.PlaySfx(AudioManager.SFX.Select);
  }

  public void Select(int i) {
    _items[i].OnClick();
  }

  void Next() {
    // 모든 아이템 비활성화
    foreach (Item item in _items) {
      item.gameObject.SetActive(false);
    }
    
    // 3개 아이템 활성화
    HashSet<int> random = new HashSet<int>();
    
    while (random.Count < 3) {
      random.Add(Random.Range(0, _items.Length));
    }

    foreach (int i in random) {
      Item ranItem = _items[i];
      
      // 만렙 아이템의 경우 소비아이템으로 대체
      if (ranItem._level == ranItem._data._damages.Length) {
        ranItem = _items[Random.Range(4, ranItem._data._damages.Length)];
      } 
      ranItem.gameObject.SetActive(true);
    }
    
  }
}
