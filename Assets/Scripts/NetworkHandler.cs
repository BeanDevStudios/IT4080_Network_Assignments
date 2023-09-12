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

    //When a client connects...
    private void OnClientStarted(){
        //Display this text
        Debug.Log("!! Client Connected !!");

        //When a client connects to the server, run ClientOnClientConnected();
        NetworkManager.OnClientConnectedCallback += ClientOnClientConnected;

        //When a client disconnects from the server, run ClientOnClientDisconnected();
        NetworkManager.OnClientDisconnectCallback += ClientOnClientDicsonnected;

        //When the server stops running, run ClientOnClientStopped();
        NetworkManager.OnServerStopped += ClientOnClientStopped;

        //Display needed text
        PrintMe();
    }

    private void ClientOnClientConnected(ulong clientId){
        PrintMe();
        //Print: I {clientId} have connected to the server
        Debug.Log($"I {clientId} have connected to the server");
    }

    private void ClientOnClientDicsonnected(ulong clientId){
        //Print: I {clientId} have disconnected to the server
        Debug.Log($"I {clientId} have disconnected from the server");
    }

    private void ClientOnClientStopped(bool indicator){
        Debug.Log("!! Server Stopped - Client !!");
        hasPrinted = false;
        NetworkManager.OnClientConnectedCallback -= ClientOnClientConnected;
        NetworkManager.OnClientDisconnectCallback -= ClientOnClientDicsonnected;
        NetworkManager.OnServerStopped -= ClientOnClientStopped;}

    //------ Server Actions ------//
    
    //When the server starts...
    private void OnServerStarted(){
        //Display this text
        Debug.Log("!! Server Started !!");

        //When a client connects to the server, run ServerOnClientConnected();
        NetworkManager.OnClientConnectedCallback += ServerOnClientConnected;

        //When a client disconnects from the server, run ServerOnClientDisconnected();
        NetworkManager.OnClientDisconnectCallback += ServerOnClientDicsonnected;

        //When the server stops running, run ServerOnServerStopped();
        NetworkManager.OnServerStopped += ServerOnServerStopped;

        //Display needed text
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

    //------ Host Actions ------//

    //When the host starts...
    private void OnHostStarted(){
        //Display this text
        Debug.Log("!! Host Started !!");

        //When a client connectes to the host, run HostOnClientConnected;
        NetworkManager.OnClientConnectedCallback += HostOnClientConnected;

        //When a client disconnectes from the host, run HostOnClientDisconnected;
        NetworkManager.OnClientConnectedCallback += HostOnClientDisconnected;

        //When the server stopps running, run HostOnHostStopped();
        NetworkManager.OnServerStopped += HostOnHostStopped;

        //Display needed text
        PrintMe();
    }
    
    private void HostOnClientConnected(ulong clientId){
        Debug.Log($"Client {clientId} connected to the server");
    }

    private void HostOnClientDisconnected(ulong clientId){
        Debug.Log($"Client {clientId} disconnected to the server");
    }

    private void HostOnHostStopped(bool indicator){
        Debug.Log("!! Server Stopped - Server !!");
        hasPrinted = false;
        NetworkManager.OnClientConnectedCallback -= HostOnClientConnected;
        NetworkManager.OnClientDisconnectCallback -= HostOnClientDisconnected;
        NetworkManager.OnServerStopped -= HostOnHostStopped;
    }
}
