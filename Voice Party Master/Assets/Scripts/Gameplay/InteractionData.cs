using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionData : MonoBehaviour
{
    public string ObjectName;

    public List<string> InspectionResult;
    public string InteractionResult;

    public bool hasBeenInteractedWith = false;

    public void Inspect(PlayerController pc) {
        string output = "";
        foreach (string s in InspectionResult) {
            output += s + "\n";
        }

        
        Debug.Log(output);

        // Inspection Event
        //////////////////////////////////////////////////////////////////        
        if (InspectionResult[0] == "You found a dusty old key!") {
            // Equip the Key
            pc.Equip("Key");
        }

        else if (InspectionResult[0] == "You found a dull sword!") {
            // Equip the sword
            pc.Equip("Sword");
        }

        else if (InspectionResult[0] == "You found a magic gem!") {
            // Equip the gem
            pc.Equip("Gem");
        } 

        else if (InspectionResult[0] == "You found a pair of gloves!") {
            // Equip the gloves
            pc.Equip("Gloves");
        }

        else if (InspectionResult[0] == "You found an old shield!") {
            // Equip the shield
            pc.Equip("Shield");
        }

    }

    public void Interact() {
        Debug.Log(InteractionResult);
    }
}
