using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{

    public Button rateGooglePlayButton;
    
	void Start () {
#if UNITY_ANDROID
        rateGooglePlayButton.gameObject.SetActive(true);
#else
        rateGooglePlayButton.gameObject.SetActive(false);
#endif
    }
    
    void Update () {

    }

    public void RateGooglePlay()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=at.impossibru.TIP");
    }
}
