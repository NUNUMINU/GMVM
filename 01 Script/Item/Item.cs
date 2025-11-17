using System;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {
  public ItemData _data;
  public int _level;
  public WeaponParent _weapon;
  public Gear _gear;

  private Image _icon;
  private Text _textLevel;
  private Text _textName;
  private Text _textDesc;

  void Awake() {
    _icon = GetComponentsInChildren<Image>()[1];
    _icon.sprite = _data._itemIcon;

    Text[] texts = GetComponentsInChildren<Text>();
    _textLevel = texts[0];
    _textName = texts[1];
    _textDesc = texts[2];

    _textName.text = _data._itemName;
  }

  private void OnEnable() {   
    _textLevel.text = $"LV. {_level+1:D2}";

    switch (_data._itemType) {
      case ItemData.ItemType.Melee:
      case ItemData.ItemType.Range:
        _textDesc.text = string.Format(_data._itemDesc, _data._damages[_level] * 100, _data._counts[_level]);
        break;
      
      case ItemData.ItemType.Glove:
      case ItemData.ItemType.Shoe:
        _textDesc.text = string.Format(_data._itemDesc, _data._damages[_level] * 100);
        break;
      
      case ItemData.ItemType.Heal:
        _textDesc.text = _data._itemDesc;
        break;
    }
  }

  public void OnClick() {
    switch (_data._itemType) {
      
      case ItemData.ItemType.Melee:
      case ItemData.ItemType.Range:
        if (_level == 0) {
          GameObject newWeapon = new GameObject();
          _weapon = newWeapon.AddComponent<WeaponParent>();
          _weapon.Init(_data);
        }
        else {
          float nextDamage = _data._baseDamage;
          int nextCount = 0;

          nextDamage += _data._baseDamage * _data._damages[_level];
          nextCount += _data._counts[_level];
          
          _weapon.LevelUp(nextDamage, nextCount);
        }
        
        _level++;
        break;
        
      case ItemData.ItemType.Glove:
      case ItemData.ItemType.Shoe:
        if (_level == 0) {
          GameObject newGear = new GameObject();
          _gear = newGear.AddComponent<Gear>();
          _gear.Init(_data);
        }
        else {
          float nextRate = _data._damages[_level];
          _gear.LevelUp(nextRate);
        }
        
        _level++;
        break;
      
      case ItemData.ItemType.Heal:
        GameManager.Instance._hp = GameManager.Instance._maxHp;
        break;
        
    }

    if (_level == _data._damages.Length) {
      GetComponent<Button>().interactable = false;
    }
  }
}
