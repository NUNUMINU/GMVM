using UnityEngine;

public class Result : MonoBehaviour {
 public GameObject[] _titles;

  public void Lose() {
    _titles[0].SetActive(true);
    _titles[1].SetActive(false);
  }
  
  public void Victory() {
    _titles[1].SetActive(true);
    _titles[0].SetActive(false);
  }
}
