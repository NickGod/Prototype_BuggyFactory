using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Management : MonoBehaviour {

  // Use this for initialization
  public GameObject selectSlider;
  [SerializeField] private GameObject gameContent;
  [SerializeField] private hand rightHand;
  [SerializeField] private hand leftHand;
  //[SerializeField] private GameObject leftHandInventory;

	void Start () {
    	selectSlider.GetComponent<VRStandardAssets.Utils.SelectionSlider>().OnBarFilled += hideCanvasGroup;
       	selectSlider.GetComponent<VRStandardAssets.Utils.SelectionSlider>().OnBarFilled += showGameContent;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void hideCanvasGroup() {
    gameObject.GetComponent<CanvasGroup>().alpha = 0f;
  }

  public void showGameContent() {
  	gameContent.SetActive(true);
  	rightHand.enabled = true;
  	leftHand.enabled = true;
  	//leftHandInventory.SetActive(true);

  }
}
