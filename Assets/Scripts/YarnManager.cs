using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnManager : MonoBehaviour
{
    static YarnManager _instance;
    static public YarnManager Instance => _instance;

    int[] dialogueProgress = new int[6] { 1, 1, 1, 1, 1, 1 };
    [SerializeField] int[] dialogueLimits;

    [SerializeField] DialogueRunner dialogueRunner;

    static public void RunDialogue(Planet interlocutor) 
    {
        _instance.dialogueRunner.gameObject.SetActive(true);
        _instance.dialogueRunner.StartDialogue(_instance.GetNodeName(interlocutor));
        int planetIndex = (int)interlocutor;
        if (_instance.dialogueProgress[planetIndex] < _instance.dialogueLimits[planetIndex]) _instance.dialogueProgress[planetIndex]++;
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
                return "RinFinal";

            case Planet.Gamma:
                return "Sim";

            case Planet.Delta:
                return "Eno";

            case Planet.Epsilon:
                return "Rin";

            default:
                return "Error";
        }
    }

    private void Awake()
    {
        _instance = this;
    }

}
