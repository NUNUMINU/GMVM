using System;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
  public enum InfoType { Exp, Level, Kill, Time, Hp }

  public InfoType _type;
  private Text _myText;
  private Slider _mySlider;

  void Awake() {
    _myText = GetComponent<Text>();
    _mySlider = GetComponent<Slider>();
  }

  private void LateUpdate() {
    switch (_type) {
      case InfoType.Exp:
        float curExp = GameManager.Instance._exp;
        float maxExp = GameManager.Instance._nextEXP[Mathf.Min(GameManager.Instance._level, GameManager.Instance._nextEXP.Length-1)];
        _mySlider.value = curExp / maxExp;
        break;
      
      case InfoType.Level:
        _myText.text = $"LV.{GameManager.Instance._level:F0}";
        break;
      
      case InfoType.Kill:
        _myText.text = $"{GameManager.Instance._kill:F0}";
        break;
      
      case InfoType.Time:
        float remainTime = GameManager.Instance._maxGameTime - GameManager.Instance._currentTime;
        int min = Mathf.FloorToInt(remainTime / 60);
        int second = Mathf.FloorToInt(remainTime % 60);
        _myText.text = $"{min:D2}: {second:D2}";
        break;
      
      case InfoType.Hp:
        float curHp = GameManager.Instance._hp;
        float maxHp = GameManager.Instance._maxHp;
        _mySlider.value = curHp / maxHp;
        break;
        
    }
  }
}

