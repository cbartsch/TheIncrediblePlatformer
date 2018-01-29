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

    public Image pauseButtonIcon, resetButtonIcon;

    public Sprite resetSprite, soundsOnSprite, soundsOffSprite;

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

        var isMenuLevel = worldIndex == 1 && levelIndex == 1;

        if (isMenuLevel)
        {
            resetButtonIcon.sprite = Persistence.SoundsEnabled ? soundsOnSprite : soundsOffSprite;
        }
        else
        {
            resetButtonIcon.sprite = resetSprite;
        }
    }

    public void ResetBtnPressed()
    {
        var isMenuLevel = GameManager.Instance.WorldIndex == 0 && GameManager.Instance.LevelIndex == 0;
        if (isMenuLevel)
        {
            Persistence.SoundsEnabled = !Persistence.SoundsEnabled;
            if (Persistence.SoundsEnabled)
            {
                sounds.PlayEnableSounds();
            }
        }
        else
        {
            GameManager.Instance.ResetLevel();
            sounds.PlayResetLevel();
        }
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