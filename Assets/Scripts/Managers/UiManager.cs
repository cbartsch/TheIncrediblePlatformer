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
    
	void Start () {
		
	}
	
	void Update ()
	{
        //update level numbers
	    worldNum.sprite = digitSprites[GameManager.Instance.WorldNum % 10];
	    levelNum1.sprite = digitSprites[(GameManager.Instance.LevelNum / 10) % 10];
	    levelNum2.sprite = digitSprites[GameManager.Instance.LevelNum % 10];

	    pauseButtonImage.sprite = GameManager.Instance.Paused ? playSprite : pauseSprite;
	}
}
