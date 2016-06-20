using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using System;



public class GameManager : Photon.MonoBehaviour {

	public int myPlayerNumber = 0;

	private PhotonView photonView;
	private Animator animator;


	public float _xMov;
	public float _zMov;
	public float _yRot;
	public float _xRot;

	//========================================================

	//........Singleton Pattern
	public static GameManager instance { get; private set;}

	public void Awake()
	{



		if (instance != null && instance != this)
			Destroy (gameObject);

		instance = this;

		DontDestroyOnLoad (gameObject);


	}
	//........

	public void NewPlayer()
	{
		int playerCount = PhotonNetwork.playerList.Length;

		myPlayerNumber = playerCount-1;

		Debug.Log ("player ID: "+PhotonNetwork.player.ID);
		Debug.Log ("My Number: "+myPlayerNumber);
	}

	//=======================================================

	void Start () {

		photonView = PhotonView.Get (this); 
	} 


	void FixedUpdate ()
	{
		photonView.RPC ("MyGetInputs", PhotonTargets.AllBuffered);

	}

//	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){ 
//	
//		if (stream.isWriting) {
//			stream.SendNext (_xMov);
//			stream.SendNext (_xRot);
//			stream.SendNext (_yRot);
//			stream.SendNext (_zMov);
//		} else {
//			Debug.Log ("Don't Work");
//		}
//	}

	//==================== BOTÕES ================================


	[PunRPC]
	public void MyShoot(){
		animator.SetTrigger ("isShoot"); 
	}

	public void Shoot(){
		photonView.RPC ("MyShoot", PhotonTargets.All );
	}


	//===================== TOUCH ========================================

	[PunRPC]
	public void MyGetInputs(){
		//Calculate movement velocity as a 3D vector
		_xMov = CrossPlatformInputManager.GetAxis("Horizontal");
		_zMov = CrossPlatformInputManager.GetAxis("Vertical");

		_yRot = CrossPlatformInputManager.GetAxisRaw("Mouse X");
		_xRot = CrossPlatformInputManager.GetAxisRaw("Mouse Y");
	}




}