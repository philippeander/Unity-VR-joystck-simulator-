using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class NetworkManager : Photon.PunBehaviour{

	public Text roomView;
	public GameObject cam;

	public 	GameObject m_gameManager;
	private GameManager gameManager;

	void Awake(){
		gameManager = m_gameManager.GetComponent<GameManager>();
	}

	void OnJoinedRoom()
	{
		StartGame();
	}

	IEnumerator OnLeftRoom()
	{
		//Easy way to reset the level: Otherwise we'd manually reset the camera

		//Wait untill Photon is properly disconnected (empty room, and connected back to main server)
		while(PhotonNetwork.room!=null || PhotonNetwork.connected==false)
			yield return 0;

		Application.LoadLevel(Application.loadedLevel);

	}

	void StartGame()
	{
		GameManager.instance.NewPlayer ();
		//PlayerController.instance.NewPlayer ();

		roomView.text = PhotonNetwork.room.name;
		Debug.Log ("Room: " + PhotonNetwork.room.name);
		if(MenuManager.mode == true){
			GameObject player = (GameObject) PhotonNetwork.Instantiate ("Avatar2", new Vector3(50f, 3f, 33f), Quaternion.identity, 0);
			cam.SetActive (false);
		}
	}

	public void Restart()
	{
		if (PhotonNetwork.room == null) return; //Only display this GUI when inside a room

		PhotonNetwork.LeaveRoom();

		SceneManager.LoadScene (0); 

	}

	void OnDisconnectedFromPhoton() 
	{
		Debug.LogWarning("OnDisconnectedFromPhoton");
	}   




}
