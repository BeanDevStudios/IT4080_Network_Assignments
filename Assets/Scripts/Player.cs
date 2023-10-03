using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    public float movementSpeed = 50f;
    public float rotationSpeed = 130f;
    public NetworkVariable<Color> playerColor = new NetworkVariable<Color>(Color.red);

    private Camera playerCamera;
    private GameObject cube;

    private void NetworkInit(){
        cube = transform.Find("ObjectObject").gameObject;
        playerCamera = transform.Find("Camera").GetComponent<Camera>();
        playerCamera.enabled = IsOwner;
        playerCamera.GetComponent<AudioListener>().enabled = IsOwner;
        ApplyPlayerColor();
        playerColor.OnValueChanged += OnPlayerColorChanged;
        }

    void Awake(){
         NetworkHelper.Log(this, "Awake");
         }

    void Start(){
        NetworkHelper.Log(this, "Start");
        }

    public override void OnNetworkSpawn(){
        NetworkHelper.Log(this, "OnNetworkSpawn");
        NetworkInit();
        base.OnNetworkSpawn();
        }

    void Update(){
        if (IsOwner){
        OwnerHandleInput();}
        }

    public void OnPlayerColorChanged(Color previous, Color current){
        ApplyPlayerColor();
        }

    private void OwnerHandleInput(){
        
        Vector3 movement = CalcMovement();
        Vector3 rotation = CalcRotation();
        if(movement != Vector3.zero || rotation != Vector3.zero){
            MoveServerRpc(CalcMovement(), CalcRotation());
        }   
        }

    private void ApplyPlayerColor(){
        NetworkHelper.Log(this, $"Applying color{playerColor.Value}");
        cube.GetComponent<MeshRenderer>().material.color = playerColor.Value;
    }

    [ServerRpc]
    private void MoveServerRpc(Vector3 movement, Vector3 rotation){
        transform.Translate(movement);
        transform.Rotate(rotation);
    }

    // Rotate around the y axis when shift is not pressed
    private Vector3 CalcRotation() {
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        Vector3 rotVect = Vector3.zero;
        if (!isShiftKeyDown) {
            rotVect = new Vector3(0, Input.GetAxis("Horizontal"), 0);
            rotVect *= rotationSpeed * Time.deltaTime;
        }
        return rotVect;
    }

    // Move up and back, and strafe when shift is pressed
    private Vector3 CalcMovement() {
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float x_move = 0.0f;
        float z_move = Input.GetAxis("Vertical");

        if (isShiftKeyDown) {
            x_move = Input.GetAxis("Horizontal");
        }

        Vector3 moveVect = new Vector3(x_move, 0, z_move);
        moveVect *= movementSpeed * Time.deltaTime;

        return moveVect;
    }
}
