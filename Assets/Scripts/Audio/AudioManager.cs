using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    // BGM
    [SerializeField] private AudioSource[] startBackgroundMusics;
    private int _startBackgroundIndex = 0;
    [SerializeField] private AudioSource[] upBackgroundMusics;
    private int _upBackgroundIndex = 0;
    [SerializeField] private AudioSource[] downBackgroundMusics;
    private int _downBackgroundIndex = 0;
    [SerializeField] private AudioSource[] bossBackgroundMusics;
    private int _bossBackgroundIndex = 0;

    [SerializeField] private AudioSource[] loseBackgroundMusics;
    private int _loseBackgroundIndex = 0;
    [SerializeField] private AudioSource[] winBackgroundMusics;
    private int _winBackgroundIndex = 0;

    private static AudioSource _curAudio;
    
    // looping sfx
    [SerializeField] private AudioSource[] runningSounds;
    private int _runningSoundIndex = 0;
    
    // single instance sfx
    [SerializeField] private AudioSource[] jumpSounds;
    [SerializeField] private AudioSource beanSound;
    
    [SerializeField] private Transform player;
    
    private bool _isStartBackgroundMusicNull;
    private bool _isUpBackgroundMusicNull;
    private bool _isDownBackgroundMusicNull;
    private bool _isBossBackgroundMusicNull;
    private bool _isLoseBackgroundSoundsNull;
    private bool _isWinBackgroundMusicsNull;
    private bool _isrunningSoundNull;

    private static AudioManager Instance { get; set; }

    private void Awake() => Instance = this;

    private void Start()
    {
        _isStartBackgroundMusicNull = startBackgroundMusics == null || startBackgroundMusics.Length == 0;
        _isUpBackgroundMusicNull = upBackgroundMusics == null || upBackgroundMusics.Length == 0;
        _isDownBackgroundMusicNull = downBackgroundMusics == null || downBackgroundMusics.Length == 0;
        _isBossBackgroundMusicNull = bossBackgroundMusics == null || bossBackgroundMusics.Length == 0;
        _isLoseBackgroundSoundsNull = loseBackgroundMusics == null || loseBackgroundMusics.Length == 0;
        _isWinBackgroundMusicsNull = winBackgroundMusics == null || winBackgroundMusics.Length == 0;
        _isrunningSoundNull = runningSounds == null || runningSounds.Length == 0;
        PlayStartBackground();
    }
    
    private void PlaySound(AudioSource audioSource)
    {
        if (audioSource == null) return;
        var volume = audioSource.volume;
        audioSource.volume = 0f;
        audioSource.Play();
        audioSource.DOFade(volume, 1f);
    }
    
    private void PlayRandomSound(AudioSource[] audioSources)
    {
        if (audioSources == null || audioSources.Length == 0) return;
        var random = Random.Range(0, audioSources.Length);
        var volume = audioSources[random].volume;
        audioSources[random].volume = 0f;
        audioSources[random].Play();
        audioSources[random].DOFade(volume, 1f);
    }
    
    private void StopSound(AudioSource audioSource)
    {
        if (audioSource == null) return;
        var tween = audioSource.DOFade(0, 1f);
        tween.onComplete = () => audioSource.Stop();
    }
    
    private void StopSoundFromArray(AudioSource[] arr, int index)
    {
        if (arr == null || arr.Length == 0 || index >= arr.Length) return;
        StopSound(arr[index]);
    }

    private void EndgameAudio()
    {
        // stop all non-instant sounds
    }
    
    private void WinAudio(object arg0)
    {
        EndgameAudio();
        PlayWinBackground();
    }
    
    private void LoseAudio(object arg0)
    {
        EndgameAudio();
        PlayLoseBackground();
    }
    
    private void PlayStartBackgroundMusic()
    {
        if (_isStartBackgroundMusicNull) return;
        _startBackgroundIndex = Random.Range(0, startBackgroundMusics.Length);
        PlaySound(startBackgroundMusics[_startBackgroundIndex]);
        _curAudio = startBackgroundMusics[_startBackgroundIndex];
    }
    
    private void StopStartBackgroundMusic()
    {
        if (_isStartBackgroundMusicNull) return;
        StopSoundFromArray(startBackgroundMusics, _startBackgroundIndex);
    }
    
    private void PlayUpBackgroundMusic()
    {
        if (_isUpBackgroundMusicNull) return;
        _upBackgroundIndex = Random.Range(0, upBackgroundMusics.Length);
        PlaySound(upBackgroundMusics[_upBackgroundIndex]);
        _curAudio = upBackgroundMusics[_upBackgroundIndex];
    }
    
    private void StopUpBackgroundMusic()
    {
        if (_isUpBackgroundMusicNull) return;
        StopSoundFromArray(upBackgroundMusics, _upBackgroundIndex);
    }
    
    private void PlayDownBackgroundMusic()
    {
        if (_isDownBackgroundMusicNull) return;
        _downBackgroundIndex = Random.Range(0, downBackgroundMusics.Length);
        PlaySound(downBackgroundMusics[_downBackgroundIndex]);
        _curAudio = downBackgroundMusics[_downBackgroundIndex];
    }
    
    private void StopDownBackgroundMusic()
    {
        if (_isDownBackgroundMusicNull) return;
        StopSoundFromArray(downBackgroundMusics, _downBackgroundIndex);
    }
    
    private void PlayBossBackgroundMusic()
    {
        if (_isBossBackgroundMusicNull) return;
        _bossBackgroundIndex = Random.Range(0, bossBackgroundMusics.Length);
        PlaySound(bossBackgroundMusics[_bossBackgroundIndex]);
        _curAudio = bossBackgroundMusics[_bossBackgroundIndex];
    }
    
    private void StopBossBackgroundMusic()
    {
        if (_isBossBackgroundMusicNull) return;
        StopSoundFromArray(bossBackgroundMusics, _bossBackgroundIndex);
    }
    
    private void PlayLoseBackgroundMusic()
    {
        if (_isLoseBackgroundSoundsNull) return;
        _loseBackgroundIndex = Random.Range(0, loseBackgroundMusics.Length);
        PlaySound(loseBackgroundMusics[_loseBackgroundIndex]);
        _curAudio = loseBackgroundMusics[_loseBackgroundIndex];
    }
    
    private void PlayWinBackgroundMusic()
    {
        if (_isWinBackgroundMusicsNull) return;
        _winBackgroundIndex = Random.Range(0, winBackgroundMusics.Length);
        PlaySound(winBackgroundMusics[_winBackgroundIndex]);
        _curAudio = winBackgroundMusics[_winBackgroundIndex];
    }
    
    private void PlayRunningSound()
    {
        if (_isrunningSoundNull) return;
        _runningSoundIndex = Random.Range(0, runningSounds.Length);
        runningSounds[_runningSoundIndex].Play();
    }
    
    private void StopRunningSound()
    {
        if (_isrunningSoundNull || !runningSounds[_runningSoundIndex].isPlaying) return;
        runningSounds[_runningSoundIndex].Stop();
    }
    
    private void PlayJumpSound()
    {
        PlayRandomSound(jumpSounds);
    }
    
    private void PlayBeanHitSound(Vector3 pos)
    {
        beanSound.volume = Mathf.Max(0f, .7f - Mathf.Abs(player.position.x - pos.x) / 12f);
        beanSound.Play();
    }
    
    public static void StopCurrentBGM() => Instance.StopSound(_curAudio);
    public static void PlayStartBackground() => Instance.PlayStartBackgroundMusic();
    public static void StopStartBackground() => Instance.StopStartBackgroundMusic();
    public static void PlayUpBackground() => Instance.PlayUpBackgroundMusic();
    public static void StopUpBackground() => Instance.StopUpBackgroundMusic();
    public static void PlayDownBackground() => Instance.PlayDownBackgroundMusic();
    public static void StopDownBackground() => Instance.StopDownBackgroundMusic();
    public static void PlayBossBackground() => Instance.PlayBossBackgroundMusic();
    public static void StopBossBackground() => Instance.StopBossBackgroundMusic();
    public static void PlayLoseBackground(object arg0=null) => Instance.PlayLoseBackgroundMusic();
    public static void PlayWinBackground(object arg0=null) => Instance.PlayWinBackgroundMusic();
    public static void PlayRunning() => Instance.PlayRunningSound();
    public static void StopRunning() => Instance.StopRunningSound();
    public static void PlayJump() => Instance.PlayJumpSound();

    public static void PlayBeanHitSFX(Vector3 pos) => Instance.PlayBeanHitSound(pos);
    
}
