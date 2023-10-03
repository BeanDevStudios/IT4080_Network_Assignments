using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
        public Button startButton;
        public TMPro.TMP_Text statusLabel;

        void Start()
        {
            startButton.gameObject.SetActive(false);
            statusLabel.text = "Start Game";

            startButton.onClick.AddListener(OnStartButtonClicked);

            NetworkManager.OnClientStarted += OnClientStarted;
            NetworkManager.OnServerStarted += OnServerStarted;
        }

        private void OnServerStarted()
        {   
            //StartGame();
            //Removing start button for now
            startButton.gameObject.SetActive(true);
            statusLabel.text = "Press Start";
        }

        private void OnClientStarted()
        {
            if(!IsHost)
            {
                statusLabel.text = "Waiting for game to start";
            }
        }

        private void OnStartButtonClicked()
        {
            StartGame();
        }

        public void StartGame()
        {
            NetworkManager.SceneManager.LoadScene("Game1",UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
