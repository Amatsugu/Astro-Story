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
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
        {
            lineView.OnContinueClicked(); 
        }
    }
}
