using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Image worldNum, levelNum1, levelNum2;

    public List<Sprite> digitSprites;

    public Image pauseButtonImage;

    public Sprite pauseSprite, playSprite;

    public Button soundsButton, musicButton;
    public Image pauseButtonIcon, resetButtonIcon, musicButtonIcon, soundsButtonIcon;

    public Sprite resetSprite, soundsOnSprite, soundsOffSprite, musicOnSprite, musicOffSprite;

    public SoundEffects sounds;

    void Start()
    {
    }

    void Update()
    {
        var worldIndex = GameManager.Instance.WorldIndex + 1;
        var levelIndex = GameManager.Instance.LevelIndex + 1;

        //update level numbers
        worldNum.sprite = digitSprites[worldIndex % 10];
        levelNum1.sprite = digitSprites[(levelIndex / 10) % 10];
        levelNum2.sprite = digitSprites[levelIndex % 10];

        pauseButtonImage.sprite = GameManager.Instance.Paused ? playSprite : pauseSprite;
        soundsButtonIcon.sprite = Persistence.SoundsEnabled ? soundsOnSprite : soundsOffSprite;
        musicButtonIcon.sprite = Persistence.MusicEnabled ? musicOnSprite : musicOffSprite;

        var isMenuLevel = worldIndex == 0;
        soundsButton.gameObject.SetActive(isMenuLevel);
        musicButton.gameObject.SetActive(isMenuLevel);
    }

    public void SoundsBtnPressed()
    {
        Persistence.SoundsEnabled = !Persistence.SoundsEnabled;
        if (Persistence.SoundsEnabled)
        {
            sounds.PlayEnableSounds();
        }
    }

    public void MusicBtnPressed()
    {
        Persistence.MusicEnabled = !Persistence.MusicEnabled;
        if (Persistence.MusicEnabled)
        {
            sounds.PlayEnableSounds();
        }
    }

    public void ResetBtnPressed()
    {
        GameManager.Instance.ResetLevel();
        sounds.PlayResetLevel();
    }

    public void PauseBtnPressed()
    {
        GameManager.Instance.TogglePause();
        if (GameManager.Instance.Paused)
        {
            sounds.PlayPause();
        }
        else
        {
            sounds.PlayResume();
        }
    }
}