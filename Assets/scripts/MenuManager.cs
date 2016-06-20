using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasManager))]
public class MenuManager : MonoBehaviour {

	[SerializeField]private Text passRoom;
	private string passCharacteres = "ABCDEFGHIJKLMNOPQRSTUVXYWZ1234567890";

	private string roomName = "myRoom"; 

	private CanvasManager screens;
	public static bool deviceType = false;  

	public static bool mode = false;

	void Awake () {
		screens = GetComponent<CanvasManager> ();


		if (!PhotonNetwork.connected)
			PhotonNetwork.ConnectUsingSettings("v1.0");

		PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1, 9999)); 


	}

	void Start(){
		HideAllScreens ();
		screens.ShowManager ();
	}

	// Update is called once per frame
	void Update () {


		if (!PhotonNetwork.connected)
		{
			return;   //Wait for a connection
		}


		if (PhotonNetwork.room != null)
			return; //Only when we're not in a Room

	}

	public void GenerateCode(){
		passRoom.text = "";
		for (int i = 0; i < 4; i++) {
			passRoom.text += passCharacteres[UnityEngine.Random.Range(0, passCharacteres.Length)];
		}
		Debug.Log (passRoom.text);
	}



	//============ BUTTONS ===================

	public void HideAllScreens(){
		screens.HideManager ();
		screens.HideCodeRoom ();
		screens.HideCtrlIputAccess ();
		screens.HideMobileType ();
		screens.HideCtrlWithGiroscope ();
		screens.HideCtrlWithoutGiroscope ();
	}

	public void ComManager(){
		HideAllScreens();
		screens.ShowCodeRoom();
		mode = true;

		GenerateCode ();
		roomName = passRoom.text;
		PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { maxPlayers = 5 }, TypedLobby.Default);


		//PhotonNetwork.JoinRoom(roomName);
		//roomView.text = PhotonNetwork.room.name;
	}
		

	public void PlayerManager(){ 
		HideAllScreens();
		screens.ShowCtrlIputAccess();

		roomName = passRoom.text;
		PhotonNetwork.JoinRoom(roomName);
	}



	public void SetRoomAndPlay(Text room){

		roomName = room.text;
		PhotonNetwork.JoinRoom(roomName);

		HideAllScreens ();
		screens.ShowMobileType ();

		//roomView.text = PhotonNetwork.room.name;
	}

	//========================================================


	public void OnConnectedToMaster()
	{
		// this method gets called by PUN, if "Auto Join Lobby" is off.
		// this demo needs to join the lobby, to show available rooms!

		PhotonNetwork.JoinLobby();  // this joins the "default" lobby
	}

	//Go Game to Controll
	public void GoGame(bool mobileType){
		deviceType = mobileType;
		if (deviceType) {
			HideAllScreens ();
			screens.ShowCtrlWithGiroscope ();
		} else {
			HideAllScreens ();
			screens.ShowCtrlWithoutGiroscope ();
		}
	}

	public void GoGameVR(){
		HideAllScreens ();  
	}


}
