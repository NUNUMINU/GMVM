using System;
using System.Collections;
using UnityEngine;

public class ArchiveManager : MonoBehaviour {
  public GameObject[] _lockCharacter;
  public GameObject[] _unlockCharacter;
  public GameObject _noticeUI;
  
  enum Archive { UnlockPotato, UnlockBean }
  private Archive[] _archives;
  private WaitForSecondsRealtime _wait;
  
  void Awake() {
    _archives = (Archive[])Enum.GetValues(typeof(Archive));

    if (!PlayerPrefs.HasKey("MyData")) {
      Init();
    }

    _wait = new WaitForSecondsRealtime(5f);
  }

  void Init() {
    PlayerPrefs.SetInt("MyData", 1);

    foreach (Archive archive in _archives) {
      PlayerPrefs.SetInt(archive.ToString(), 0);
    }
  }

  void Start() {
    UnlockCharacter();
  }

  void UnlockCharacter() {
    for (int i = 0; i < _lockCharacter.Length; i++) {
      string archiveName = _archives[i].ToString();
      bool isUnlock = PlayerPrefs.GetInt(archiveName) == 1;
      _lockCharacter[i].SetActive(!isUnlock);
      _unlockCharacter[i].SetActive(isUnlock);
    }
  }

  private void LateUpdate() {
    foreach (Archive archive in _archives) {
      CheckArchive(archive);
    }
  }

  void CheckArchive(Archive archive) {
    bool isArchive = false;

    switch (archive) {
      case Archive.UnlockPotato:
        isArchive = GameManager.Instance._kill >= 10;
        break;
      
      case Archive.UnlockBean:
        isArchive = Math.Abs(GameManager.Instance._currentTime - GameManager.Instance._maxGameTime) < 0.001f;
        break;
    }

    if (isArchive && PlayerPrefs.GetInt(archive.ToString()) == 0) {
      PlayerPrefs.SetInt(archive.ToString(), 1);

      for (int i = 0; i < _noticeUI.transform.childCount; i++) {
        bool isActive = (i == (int)archive);
        _noticeUI.transform.GetChild(i).gameObject.SetActive(isActive);
      }

      StartCoroutine(NoticeRoutine());
    }
  }

  IEnumerator NoticeRoutine() {
    _noticeUI.SetActive(true);
    AudioManager.Instance.PlaySfx(AudioManager.SFX.LevelUp);
    
    yield return _wait;
    
    _noticeUI.SetActive(false);
  }
}
