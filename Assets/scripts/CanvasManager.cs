using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {


	[SerializeField]private GameObject manager;
	[SerializeField]private GameObject codeRoom;
	[SerializeField]private GameObject controlInputAcess;
	[SerializeField]private GameObject mobileType;
	[SerializeField]private GameObject controlWithGyroscope; 
	[SerializeField]private RectTransform controlWithoutGyroscope;  



	public void ShowManager(){
		manager.SetActive (true);
	}
	public void HideManager(){
		manager.SetActive (false);
	}


	public void ShowCodeRoom(){
		codeRoom.SetActive (true);
	}
	public void HideCodeRoom(){
		codeRoom.SetActive (false);
	}

	public void ShowCtrlIputAccess(){
		controlInputAcess.SetActive (true);
	}
	public void HideCtrlIputAccess(){
		controlInputAcess.SetActive (false);
	}


	public void ShowMobileType(){
		mobileType.SetActive (true);
	}
	public void HideMobileType(){
		mobileType.SetActive (false);
	}


	public void ShowCtrlWithGiroscope(){
		controlWithGyroscope.SetActive (true);
	}
	public void HideCtrlWithGiroscope(){
		controlWithGyroscope.SetActive (false);
	}


	public void ShowCtrlWithoutGiroscope(){
		controlWithoutGyroscope.anchoredPosition = new Vector2 (0, 0);
	}
	public void HideCtrlWithoutGiroscope(){
		controlWithoutGyroscope.anchoredPosition = new Vector2 (0, 500);
	}






}
