using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionData : MonoBehaviour
{
    public string ObjectName;

    public List<string> InspectionResult;
    public string InteractionResult;


    public void Inspect() {
        string output = "";
        foreach (string s in InspectionResult) {
            output += s + "\n";
        }

        Debug.Log(output);
    }

    public void Interact() {
        Debug.Log(InteractionResult);
    }
}
