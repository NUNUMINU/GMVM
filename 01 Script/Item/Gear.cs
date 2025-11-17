using System;
using UnityEngine;

public class Gear : MonoBehaviour {
    public ItemData.ItemType _type;
    public float _rate;

    public void Init(ItemData data) {
      // Basic Set
      name = "Gear " + data._itemId;
      transform.parent = GameManager.Instance.Player.transform;
      transform.localPosition = Vector3.zero;
      
      // Attribute Set
      _type = data._itemType;
      _rate = data._damages[0];
      ApplyGear();
    }

    public void LevelUp(float rate) {
      _rate = rate;
      ApplyGear();
    }

    void ApplyGear() {
      switch (_type) {
        case ItemData.ItemType.Glove:
          RateUp();
          break;
        
        case ItemData.ItemType.Shoe:
          SpeedUp();
          break;
      }
    }

    void RateUp() {
      WeaponParent[] weapons = transform.parent.GetComponentsInChildren<WeaponParent>();

      foreach (WeaponParent weapon in weapons) {
        switch (weapon.ID) {
          case 0:
            weapon.Speed = weapon._meleeSpeed * Character.WeaponSpeed + 
                           (weapon._meleeSpeed * Character.WeaponSpeed * _rate);
            break;
          
          default:
            weapon.Speed = weapon._rangeSpeed * Character.WeaponRate - 
                           (weapon._rangeSpeed * Character.WeaponRate * _rate);
            break;
        }
      }
    }

    void SpeedUp() {
      float basicSpeed = 5 * Character.Speed;
      GameManager.Instance.Player.MovSpeed = basicSpeed + basicSpeed * _rate;
    }
}
