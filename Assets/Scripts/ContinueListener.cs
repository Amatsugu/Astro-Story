using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class ContinueListener : MonoBehaviour
{
    [SerializeField] LineView lineView;
    private void OnGUI()
    {
        if (Input.GetKeyUp(KeyCode.Q) || Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.Joystick1Button1))
        {
            lineView.OnContinueClicked(); 
        }
    }
}
