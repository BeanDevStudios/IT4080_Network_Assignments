using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class NetworkHandler : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.OnClientStarted += OnClientStarted;
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;

    }

    private bool hasPrinted = false;
    private void PrintMe()
    {
        if (hasPrinted){
            return;
            }
            
        //Debug.Log("I AM");
        hasPrinted = true;

        if(IsServer){
            Debug.Log($"I AM the Server! {NetworkManager.ServerClientId}");
            hasPrinted = true;
            }
        if(IsHost){
            Debug.Log($"I AM the Host! {NetworkManager.ServerClientId}/{NetworkManager.LocalClientId}");
            hasPrinted = true;
            }
        if(IsClient){
            Debug.Log($"I AM a Client! {NetworkManager.LocalClientId}");
            hasPrinted = true;
            }
        if(!IsServer && !IsClient){
            Debug.Log("... Nothing Yet");
            hasPrinted = false;
            }
    }

    //------ Client Actions ------//
    private void OnClientStarted(){
        Debug.Log("!! Server Started - Client !!");
        NetworkManager.OnClientConnectedCallback += ClientOnClientConnected;
        NetworkManager.OnClientDisconnectCallback += ClientOnClientDicsonnected;
        NetworkManager.OnServerStopped += ClientOnClientStopped;
        PrintMe();
    }

    private void ClientOnClientConnected(ulong clientId){
        PrintMe();
        //Print: I {clientId} have connected to the server
        Debug.Log($"Client{clientId} has connected to the server");

        //handle the case when we are the client running on the host
        //some other client connected
    }

    private void ClientOnClientDicsonnected(ulong clientId){
        //Print: I {clientId} have disconnected to the server
        Debug.Log($"Client {clientId} has disconnected to the server");
    }

    private void ClientOnClientStopped(bool indicator){
        Debug.Log("!! Server Stopped - Client !!");
        hasPrinted = false;
        NetworkManager.OnClientConnectedCallback -= ClientOnClientConnected;
        NetworkManager.OnClientDisconnectCallback -= ClientOnClientDicsonnected;
        NetworkManager.OnServerStopped -= ClientOnClientStopped;}

    //------ Server Actions ------//
    private void OnServerStarted(){
        Debug.Log("!! Server Started - Server!!");
        NetworkManager.OnClientConnectedCallback += ServerOnClientConnected;
        NetworkManager.OnClientDisconnectCallback += ServerOnClientDicsonnected;
        NetworkManager.OnServerStopped += ServerOnServerStopped;
        PrintMe();}

    private void ServerOnClientConnected(ulong clientId){
        Debug.Log($"Client {clientId} connected to the server");}

    private void ServerOnClientDicsonnected(ulong clientId){
        Debug.Log($"Client {clientId} disconnected to the server");}

    private void ServerOnServerStopped(bool indicator){
        Debug.Log("!! Server Stopped - Server !!");
        hasPrinted = false;
        NetworkManager.OnClientConnectedCallback -= ServerOnClientConnected;
        NetworkManager.OnClientDisconnectCallback -= ServerOnClientDicsonnected;
        NetworkManager.OnServerStopped -= ServerOnServerStopped;}
}
