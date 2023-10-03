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
        NetworkManager.OnClientStarted += OnClientStarted;
        NetworkManager.OnServerStarted += OnServerStarted;
    }

    private void PrintMe()
    {
        if(IsServer){
            NetworkHelper.Log($"I AM a Server! {NetworkManager.ServerClientId}");}
        if(IsHost){
            NetworkHelper.Log($"I AM a Host! {NetworkManager.ServerClientId}/{NetworkManager.LocalClientId}");}
        if(IsClient){
            NetworkHelper.Log($"I AM a Client! {NetworkManager.LocalClientId}");}
        if(!IsServer && !IsClient){
            NetworkHelper.Log("I AM Nothing yet");}
    }

    //------ Client Actions ------//
    //When a client connects...
    private void OnClientStarted(){
        //Display this text
        NetworkHelper.Log("!! Client Connected !!");

        //When a client connects to the server, run ClientOnClientConnected();
        NetworkManager.OnClientConnectedCallback += ClientOnClientConnected;

        //When a client disconnects from the server, run ClientOnClientDisconnected();
        NetworkManager.OnClientDisconnectCallback += ClientOnClientDicsonnected;

        //When the server stops running, run ClientOnClientStopped();
        NetworkManager.OnServerStopped += ClientOnClientStopped;

        //Display needed text
        PrintMe();}

    private void ClientOnClientStopped(bool indicator){
        NetworkHelper.Log("!! Server Stopped - Client !!");
        NetworkManager.OnClientConnectedCallback -= ClientOnClientConnected;
        NetworkManager.OnClientDisconnectCallback -= ClientOnClientDicsonnected;
        NetworkManager.OnServerStopped -= ClientOnClientStopped;
        PrintMe();}
    private void ClientOnClientConnected(ulong clientId){
        NetworkHelper.Log($"I have connected {clientId}");
        
        if(IsHost){
            NetworkHelper.Log($"Client {clientId} connected to the server");
        }
    }
        //FIX THIS METHOD
        //IsClient();
        //IsServer();
        //IsHost();
        //NetworkManager.clientId
        //NetworkManager.LocalClientId
        //NetworkManager.ServerClientId
    private void ClientOnClientDicsonnected(ulong clientId){
        NetworkHelper.Log($"I have disconnected {clientId}");
        if(IsHost){
            NetworkHelper.Log($"Client {clientId} disconnected from the server");
        }
    }
        //FIX THIS METHOD

    
    //------ Server Actions ------//
    //When the server starts...
    private void OnServerStarted(){
        //Display this text
        NetworkHelper.Log("!! Server Started !!");

        //When a client connects to the server, run ServerOnClientConnected();
        NetworkManager.OnClientConnectedCallback += ServerOnClientConnected;

        //When a client disconnects from the server, run ServerOnClientDisconnected();
        NetworkManager.OnClientDisconnectCallback += ServerOnClientDicsonnected;

        //When the server stops running, run ServerOnServerStopped();
        NetworkManager.OnServerStopped += ServerOnServerStopped;

        //Display needed text
        PrintMe();}    
    private void ServerOnServerStopped(bool indicator){
        NetworkHelper.Log("!! Server Stopped - Server !!");
        NetworkManager.OnClientConnectedCallback -= ServerOnClientConnected;
        NetworkManager.OnClientDisconnectCallback -= ServerOnClientDicsonnected;
        NetworkManager.OnServerStopped -= ServerOnServerStopped;
        PrintMe();}

    private void ServerOnClientConnected(ulong clientId){
        NetworkHelper.Log($"Client {clientId} connected to the server");}

    private void ServerOnClientDicsonnected(ulong clientId){
        NetworkHelper.Log($"Client {clientId} disconnected to the server");}
    
}
