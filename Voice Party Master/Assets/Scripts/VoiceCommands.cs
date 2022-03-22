using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoiceCommands
{
    public static Dictionary<string, Delegate> Commands = new Dictionary<string, Delegate>();

    public static void Initialize() 
    {
           
        // Quit the game (from any scene);
        Commands.Add("quit", new Action<int, string>(GameManager.Quit));
    }

}
