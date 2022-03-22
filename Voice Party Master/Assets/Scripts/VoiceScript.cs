using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;

public class VoiceScript : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    void Start() 
    {
        actions.Add("forward", Forward);
        actions.Add("up", Up);
        actions.Add("down", Down);
        actions.Add("back", Back);
        actions.Add("end", End);
    
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += CommandRecognized;
        keywordRecognizer.Start();
    }

    private void CommandRecognized(PhraseRecognizedEventArgs phrase) 
    {
        Debug.Log(phrase.text);
        if (actions.ContainsKey(phrase.text)) {
            actions[phrase.text].Invoke();
        }
        
    }

    void Forward() {
        Debug.Log("Forward Command Called!");
    }

    void Back() {
        Debug.Log("Back Command Called!");
    }

    void Up() {
        Debug.Log("Up Command Called!");
    }

    void Down() {
        Debug.Log("Down Command Called!");
    }

    void End() {
        keywordRecognizer.Stop();
        Debug.Log("Voice Commands Deactivated.");
    }


}
