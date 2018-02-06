using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    public GameObject dragItems, dropItems, continueItems;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		dropItems.SetActive(DragController.DragActive && !continueItems.activeInHierarchy);
        dragItems.SetActive(!DragController.DragActive && ToolCameraController.Instance.HasMoveItems && !continueItems.activeInHierarchy);
	}
}
