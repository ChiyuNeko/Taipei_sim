using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GodConecter : MonoBehaviourPunCallbacks
{
    public PhotonView _pv;

    public DisasterManager _disasterManager;

    public string roomListText;

    void Start()
    {
        OnStartButtonClick();
    }

    public void OnStartButtonClick()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("connecting...");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("connect OK");
        PhotonNetwork.JoinLobby();
        Debug.Log("joining lobby...");
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby OK");
        PhotonNetwork.JoinRoom("TaipeiSim");
        Debug.Log("joining room --> TaipeiSim...");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("TaipeiSim");
        Debug.Log("create room --> TaipeiSim...");
        PhotonNetwork.JoinRoom("TaipeiSim");
        Debug.Log("joining room --> TaipeiSim...");
    }
    public override void OnJoinedRoom()
    {
        string NickName = "citizen";
        PhotonNetwork.LocalPlayer.NickName = NickName; // Assign random nickname
        Debug.Log("joined room --> FansRoom OK");
        Debug.Log("Name : " + NickName);

        // 更新房間資訊
        UpdateRoomInfo();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} entered the room.");
        UpdateRoomInfo();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName} left the room.");
        UpdateRoomInfo();
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room list updated");
        UpdateRoomList(roomList);
    }
    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        roomListText = "Room List:\n";
        foreach (RoomInfo room in roomList)
        {
            roomListText += room.Name + " (" + room.PlayerCount + "/" + room.MaxPlayers + ")\n";
        }
        print(roomListText);
    }
    private void UpdateRoomInfo()
    {
        roomListText = "Current Room:\n";
        roomListText += PhotonNetwork.CurrentRoom.Name + "\n";
        roomListText += "Players:\n";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            roomListText += player.NickName + "\n";
        }
        print(roomListText);
    }

    [PunRPC]
    public void ReciveFromGodDisaster(Vector3 posistion, string disasterType)
    {
        _disasterManager.DropDisasterByName(posistion, disasterType);
    }
}
