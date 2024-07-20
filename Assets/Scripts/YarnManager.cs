using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnManager : MonoBehaviour
{
    static YarnManager _instance;
    public static YarnManager Instance => _instance;

    int[] dialogueProgress = new int[6];

    DialogueRunner dialogueRunner;

    public static void RunDialogue(Planet interlocutor) 
    {
        _instance.dialogueRunner.gameObject.SetActive(true);
        _instance.dialogueRunner.StartDialogue(_instance.GetNodeName(interlocutor));
        _instance.dialogueProgress[(int)interlocutor]++;
    }

    public enum Planet
    { 
        Alpha,
        Beta,
        Gamma,
        Delta,
        Epsilon,
        Zeta
    }

    string GetNodeName(Planet interlocutor) 
    {
        string builtName = PlanetNodeDictionary(interlocutor);
        builtName += dialogueProgress[(int)interlocutor];
        return builtName;
    }

    string PlanetNodeDictionary(Planet interlocutor) 
    { 
        switch (interlocutor) 
        { 
            case Planet.Alpha:
                return "Ava";

            case Planet.Beta:
                return "Rin";

            default:
                return "Error";
        }
    }

    private void Awake()
    {
        _instance = this;
        dialogueRunner = FindFirstObjectByType<DialogueRunner>();
    }

}
