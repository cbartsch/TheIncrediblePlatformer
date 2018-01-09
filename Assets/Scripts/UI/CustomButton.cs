using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public float pressOffset = 15;

    private Button button;
    private float startY, startHeight;
    
	void Start ()
	{
	    button = GetComponent<Button>();
	    startY = transform.position.y;
	    startHeight = (transform as RectTransform).sizeDelta.y;
	}

    public void OnPointerDown(PointerEventData d)
    {
        var t = transform as RectTransform;
        var p = t.position;
        p.y = startY - pressOffset;
        transform.position = p;
        var s = t.sizeDelta;
        s.y = startHeight - pressOffset;
        t.sizeDelta = s;
    }
    public void OnPointerUp(PointerEventData d)
    {
        var t = transform as RectTransform;
        var p = t.position;
        p.y = startY;
        transform.position = p;
        var s = t.sizeDelta;
        s.y = startHeight;
        t.sizeDelta = s;
    }
}
