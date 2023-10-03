using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(Application.isEditor) 
        {
            //Add code here
        }
        else
        {
            //Turns off the stack trace for Debug.log to cut down on log noise
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
        }
    }
    private void OnGUI()
    {
        NetworkHelper.GUILayoutNetworkControls();
    }
}
