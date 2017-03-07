using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Management : MonoBehaviour {

  // Use this for initialization
  public GameObject selectSlider;

	void Start () {
    selectSlider.GetComponent<VRStandardAssets.Utils.SelectionSlider>().OnBarFilled += hideCanvasGroup;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void hideCanvasGroup()
  {
    gameObject.GetComponent<CanvasGroup>().alpha = 0f;
  }
}
