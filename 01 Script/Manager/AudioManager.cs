using UnityEngine;

public class AudioManager : MonoBehaviour {
  
  private static AudioManager _instance;

  [Header("# BGM")]
  public AudioClip bgmClip;
  public float bgmVolume;
  AudioSource bgmPlayer;
  private AudioHighPassFilter bgmEffect;
  
  [Header("# SFX")]
  public AudioClip[] sfxClips;
  public float sfxVolume;
  public int channels;
  AudioSource[] sfxPlayers;
  int channelIndex;
  
  public enum SFX {
    Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Select, Win
  }
  
  
  public static AudioManager Instance => _instance;
  
  private void Awake() {
    _instance = this;
    Init();
  }

  void Init() {
    // 배경음 플레이어 초기화
    GameObject bgmObject = new GameObject("BgmPlayer");
    bgmObject.transform.parent = transform;
    bgmPlayer = bgmObject.AddComponent<AudioSource>();
    bgmPlayer.playOnAwake = false;
    bgmPlayer.loop = true;
    bgmPlayer.volume = bgmVolume;
    bgmPlayer.clip = bgmClip;
    bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

    // 효과음 플레이어 초기화
    GameObject sfxObject = new GameObject("sfxPlayer");
    sfxObject.transform.parent = transform;
    sfxPlayers = new AudioSource[channels];
    for (int i = 0; i < channels; i++) {
      sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
      sfxPlayers[i].playOnAwake = false;
      sfxPlayers[i].volume = sfxVolume;
      sfxPlayers[i].bypassListenerEffects = true;
    }
  }
  
  public void PlayBgm(bool isPlay) {
    if (isPlay) {
      bgmPlayer.Play();
    }
    else {
      bgmPlayer.Stop();
    }
  }

  public void EffectBgm(bool isPlay) {
    bgmEffect.enabled = isPlay;
  }

  public void PlaySfx(SFX sfx) {
    for (int i = 0; i < channels; i++) {
      int loopIndex = (i + channelIndex) % channels;

      if (sfxPlayers[loopIndex].isPlaying) {
        continue;
      }
      else {
        int ranIndex = 0;
        
        if (sfx == SFX.Hit || sfx == SFX.Melee) {
          ranIndex = Random.Range(0, 2);
        }
        
        channelIndex = loopIndex;
        sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];
        sfxPlayers[loopIndex].Play();
        break;
      }
    }
  }
}
