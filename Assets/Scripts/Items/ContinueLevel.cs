using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueLevel : MonoBehaviour
{
    public GameObject continueItems;

    public SpriteRenderer worldNum, levelNum1, levelNum2;
    public List<Sprite> digitSprites;

	// Use this for initialization
	void Start () {
		continueItems.SetActive(Persistence.HasLevelData);
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var worldNum = Persistence.WorldNum + 1;
        int levelNum = Persistence.LevelNum + 1;

	    this.worldNum.sprite = digitSprites[worldNum];
	    levelNum1.sprite = digitSprites[levelNum / 10];
	    levelNum2.sprite = digitSprites[levelNum % 10];
	}
}
