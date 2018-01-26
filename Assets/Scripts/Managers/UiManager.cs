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
    
	void Start () {
		
	}
	
	void Update ()
	{
        //update level numbers
	    worldNum.sprite = digitSprites[GameManager.Instance.WorldNum % 10];
	    levelNum1.sprite = digitSprites[(GameManager.Instance.LevelNum / 10) % 10];
	    levelNum2.sprite = digitSprites[GameManager.Instance.LevelNum % 10];

	    pauseButtonImage.sprite = GameManager.Instance.Paused ? playSprite : pauseSprite;

	    var isMenuLevel = GameManager.Instance.WorldNum == 1 && GameManager.Instance.LevelNum == 1;
        
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
        var isMenuLevel = GameManager.Instance.WorldNum == 1 && GameManager.Instance.LevelNum == 1;
        if (isMenuLevel)
        {
            Persistence.SoundsEnabled = !Persistence.SoundsEnabled;
        }
        else
        {
            GameManager.Instance.ResetLevel();
        }
    }

    public void PauseBtnPressed()
    {
        GameManager.Instance.TogglePause();
    }
}
