using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    public Button startButton;
    public Button lobbyButton;
    public TMPro.TMP_Text statusLabel;

    void Start()
    {
        NetworkManager.OnClientStarted += OnClientStarted;
        NetworkManager.OnServerStarted += OnServerStarted;
        startButton.onClick.AddListener(OnStartButtonClicked);
        lobbyButton.onClick.AddListener(OnLobbyButtonClicked);

        startButton.gameObject.SetActive(false);
        statusLabel.text = "Start Game";
    }

    private void OnServerStarted() {
        //StartGame();
        startButton.gameObject.SetActive(true);
        statusLabel.text = "Press Start";
        
    }

    private void OnClientStarted() {
        if (!IsHost) {
            statusLabel.text = "Waiting for game to start";
        }
    }

    private void OnStartButtonClicked()
    {
        StartGame();
    }

    private void OnLobbyButtonClicked(){
        GotoLobby();    
    }

    public void GotoLobby() {
        NetworkManager.SceneManager.LoadScene(
            "Lobby",
            UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private void StartGame()
    {
        NetworkManager.SceneManager.LoadScene(
            "Game1",
            UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

}
