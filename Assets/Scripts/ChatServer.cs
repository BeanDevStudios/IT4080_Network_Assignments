using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ChatServer : NetworkBehaviour
{
    public ChatUi chatUi;
    const ulong SYSTEM_ID = ulong.MaxValue;
    private ulong[] dmClientIds = new ulong[2];
    private ulong[] playerClientId = new ulong[1];

    void Start()
    {
        chatUi.printEnteredText = false;
        chatUi.MessageEntered += OnChatUiMessageEntered;

        if(IsServer)
        {
            NetworkManager.OnClientConnectedCallback += ServerOnClientConnected;
            NetworkManager.OnClientDisconnectCallback += ServerOnClientDisconnected;
            if(IsHost)
            {
                DisplayMessageLocally(SYSTEM_ID, $"You are the host AND client {NetworkManager.LocalClientId}");
            }
            else
            {
                DisplayMessageLocally(SYSTEM_ID, "You are the server");
            }
        }
        else
        {
            DisplayMessageLocally(SYSTEM_ID, $"You are a client {NetworkManager.LocalClientId}");
        }
    }

    private void ServerOnClientConnected(ulong clientId)
    {
        ClientRpcParams rpcParams = default;
        playerClientId[0] = clientId;
        rpcParams.Send.TargetClientIds = playerClientId;
        RecieveChatMessageClientRpc($"I ({NetworkManager.LocalClientId}) see you ({clientId}) have connected to the server, well done", SYSTEM_ID, rpcParams);
        SendChatMessageServerRpc($"New client ({clientId}) has connected to the server. Say hello!");
    }

    private void ServerOnClientDisconnected(ulong clientId)
    {
        //notify all when client disconnects
        SendChatMessageServerRpc($"Client ({clientId}) has disconnected from the server.");
    }

    private void DisplayMessageLocally(ulong from, string message)
    {
        string fromStr = $"Player {from}";
        Color textColor = chatUi.defaultTextColor;

        if(from == NetworkManager.LocalClientId)
        {
            fromStr = "you";
            textColor = Color.magenta;
        }
        else if(from == SYSTEM_ID)
        {
            fromStr = "SYS";
            textColor = Color.green;
        }

        chatUi.addEntry(fromStr, message, textColor);
    }

    private void OnChatUiMessageEntered(string message)
    {
        SendChatMessageServerRpc(message);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendChatMessageServerRpc(string message, ServerRpcParams serverRpcParams = default)
    {
        if (message.StartsWith("@"))
        {
            string[] parts = message.Split(" ");
            string clientIdStr = parts[0].Replace("@","");
            ulong toClientId;
            if(ulong.TryParse(clientIdStr, out toClientId))
            {
                ServerSendDirectMessage(message, serverRpcParams.Receive.SenderClientId, toClientId);
            }
            else
            {
                ClientRpcParams rpcParams = default;
                playerClientId[0] = serverRpcParams.Receive.SenderClientId;
                rpcParams.Send.TargetClientIds = playerClientId;
                RecieveChatMessageClientRpc($"client Id ({clientIdStr}) is not a valid client id.", SYSTEM_ID, rpcParams);
            }
        }
        else
        {
            RecieveChatMessageClientRpc(message,serverRpcParams.Receive.SenderClientId);
        }
    }

    [ClientRpc]
    public void RecieveChatMessageClientRpc(string message, ulong from, ClientRpcParams clientRpcParams = default)
    {
        DisplayMessageLocally(from, message);
    }

    private void ServerSendDirectMessage (string message, ulong from, ulong to)
    {
        bool connected = false;
        ClientRpcParams rpcParams = default;
        foreach(ulong clientId in NetworkManager.ConnectedClientsIds)
        {
            if(clientId == to)
            {
                connected = true;
                break;
            }
        }

        if(connected)
        {
            dmClientIds[0] = from;
            dmClientIds[1] = to;
            rpcParams.Send.TargetClientIds = dmClientIds;

             RecieveChatMessageClientRpc($" {message}", from, rpcParams);
        }
        else
        {
            playerClientId[0] = from;
            rpcParams.Send.TargetClientIds = playerClientId;
            RecieveChatMessageClientRpc($"client Id ({to}) is not currently connected.", SYSTEM_ID, rpcParams);
        }


    }
}
