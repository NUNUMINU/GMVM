using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject {
  public enum ItemType {
    Melee, Range, Glove, Shoe, Heal
  }
  
  [Header("# Main Info")] 
  public ItemType _itemType;
  public int _itemId;
  public string _itemName;
  [TextArea]
  public string _itemDesc;
  public Sprite _itemIcon;

  [Header("# Level Data")]
  public float _baseDamage;
  public int _baseCount;
  public float[] _damages;
  public int[] _counts;

  [Header("# Weapon")] 
  public GameObject _projectTile;
  public Sprite _hand;
}
