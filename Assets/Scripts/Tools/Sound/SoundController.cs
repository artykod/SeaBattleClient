using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

using Action = System.Action;

public class SoundController : SingletonBehaviour<SoundController, SoundController>
{
    private const string PREFS_SOUND_ENABLED = "sound_on";
    private const string PREFS_MUSIC_ENABLED = "music_on";
    private const string PREFS_SOUND_VOLUME = "sound_volume";
    private const string PATH_SOUNDS = "sound/";
    private const string PATH_MUSICS = "music/";

    public const string SOUND_BUTTON_CLICK = "click";
    public const string SOUND_CLOSE = "close";
    public const string SOUND_BUY = "buy";
    public const string SOUND_WIN = "win";
    public const string SOUND_LOSE = "lose";
    public const string SOUND_ERROR = "error";
    public const string SOUND_SHOOT = "shoot";
    public const string SOUND_BEEP = "beep";

    public const string MUSIC = "music";

    private static IEnumerator buttonsClickTracker = null;
    private static Button lastSelectedButton = null;
    private static HashSet<string> activeSounds = new HashSet<string>();

    private AudioSource sourceSound = null;
    private AudioSource sourceMusic = null;

    public static bool IsSoundEnabled
    {
        get
        {
            return PlayerPrefs.GetInt(PREFS_SOUND_ENABLED, 1) > 0;
        }
        set
        {
            PlayerPrefs.SetInt(PREFS_SOUND_ENABLED, value ? 1 : 0);
            PlayerPrefs.Save();

            IsMusicEnabled = value;
        }
    }
    public static bool IsMusicEnabled
    {
        get
        {
            return PlayerPrefs.GetInt(PREFS_MUSIC_ENABLED, 1) > 0;
        }
        set
        {
            var oldValue = IsMusicEnabled;

            PlayerPrefs.SetInt(PREFS_MUSIC_ENABLED, value ? 1 : 0);
            PlayerPrefs.Save();

            if (!value)
            {
                Instance.sourceMusic.Stop();
            }
            else if (!oldValue)
            {
                Instance.sourceMusic.Play();
            }
        }
    }
    public static float SoundVolume
    {
        get
        {
            return PlayerPrefs.GetFloat(PREFS_SOUND_VOLUME, 1f);
        }
        set
        {
            var val = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(PREFS_SOUND_VOLUME, val);
            PlayerPrefs.Save();

            if (Instance != null && Instance.sourceSound != null) Instance.sourceSound.volume = val;
            if (Instance != null && Instance.sourceMusic != null) Instance.sourceMusic.volume = val;
        }
    }

    public static void Sound(string sound)
    {
        Instance.PlaySound(PATH_SOUNDS + sound);
    }
    public static void Music(string music)
    {
        Instance.PlayMusic(PATH_MUSICS + music);
    }

    private void PlaySound(string sound, Action continueCallback = null)
    {
        if (!IsSoundEnabled)
        {
            return;
        }

        StartCoroutine(PlaySoundRoutine(sound, continueCallback));
    }

    private IEnumerator PlaySoundRoutine(string sound, Action continueCallback = null)
    {
        if (activeSounds.Contains(sound))
        {
            yield break;
        }

        var clip = LoadAudio(sound);
        sourceSound.PlayOneShot(clip, SoundVolume);

        if (continueCallback != null)
        {
            StartCoroutine(InvokeAfterDelay(clip.length, continueCallback));
        }

        activeSounds.Add(sound);
        var delay = 0.1f;
        while (delay > 0f)
        {
            delay -= Time.deltaTime;
            yield return null;
        }
        activeSounds.Remove(sound);
    }

    private IEnumerator InvokeAfterDelay(float delay, Action action)
    {
        while (delay > 0f)
        {
            delay -= Time.deltaTime;
            yield return null;
        }
        if (action != null)
        {
            action();
        }
    }

    private void PlayMusic(string music)
    {
        sourceMusic.clip = LoadAudio(music);
        sourceMusic.loop = true;
        sourceMusic.volume = SoundVolume;
        if (IsMusicEnabled)
        {
            sourceMusic.Play();
        }
    }

    private AudioClip LoadAudio(string path)
    {
        return Resources.Load<AudioClip>(path);
    }

    private void Awake()
    {
        sourceSound = gameObject.AddComponent<AudioSource>();
        sourceMusic = gameObject.AddComponent<AudioSource>();
    }

    public static void StartButtonsClickTracker()
    {
        if (buttonsClickTracker != null)
        {
            return;
        }
        Instance.StartCoroutine(buttonsClickTracker = TrackButtonsClick());
    }

    public static void StopButtonsClickTracker()
    {
        if (buttonsClickTracker == null)
        {
            return;
        }

        Instance.StopCoroutine(buttonsClickTracker);
        buttonsClickTracker = null;
        UnsubscribeFromButtonClick();
    }

    private static void SubscribeToButtonClick(Button button)
    {
        UnsubscribeFromButtonClick();
        if (button != null)
        {
            lastSelectedButton = button;
            lastSelectedButton.onClick.AddListener(ButtonClickListener);
        }
    }
    private static void UnsubscribeFromButtonClick()
    {
        if (lastSelectedButton != null)
        {
            lastSelectedButton.onClick.RemoveListener(ButtonClickListener);
            lastSelectedButton = null;
        }
    }

    private static void ButtonClickListener()
    {
        Sound(SOUND_BUTTON_CLICK);
        UnsubscribeFromButtonClick();
    }

    private static IEnumerator TrackButtonsClick()
    {
        while (true)
        {
            var touchId = -1;
            var isTouched = Input.GetMouseButtonDown(0);

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
			isTouched = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
			if (isTouched) {
				touchId = Input.GetTouch(0).fingerId;
			}
#endif

            if (isTouched)
            {
                var eventSystem = EventSystem.current;
                if (eventSystem != null)
                {
                    var obj = eventSystem.currentSelectedGameObject;
                    if (obj != null && eventSystem.IsPointerOverGameObject(touchId))
                    {
                        var button = obj.GetComponent<Button>();
                        if (button != null && button.interactable)
                        {
                            SubscribeToButtonClick(button);
                        }
                    }
                }
            }

            yield return null;
        }
    }
}
