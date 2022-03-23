using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class VoiceController : MonoBehaviour
{   
    GameManager gm;
    InterfaceManager im;

    public PlayerController selectedCharacter = null;
    public PlayerController warrior, rogue, archer, mage, priest;

    // Initialize the Controller
    private void Start() 
    {
        // Find Game Manager
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gm == null) {
            Debug.LogError("GameManager script not found!");
        }

        // Find Interface Manager
        im = GameObject.Find("UI").GetComponent<InterfaceManager>();
        if (im == null) {
            Debug.LogError("InterfaceManager script not found!");
        }   
        
        // Initialize IM;
        im.Warrior = warrior.entity;
        im.Rogue = rogue.entity;
        im.Archer = archer.entity;
        im.Mage = mage.entity;
        im.Priest = priest.entity;
        im.Initialize();

        // Initialize Voice Commands
        // Motion Controls        
        VoiceCommands.Commands.Add("idle", new Action<int, string>(Idle));
        VoiceCommands.Commands.Add("walk", new Action<int, string>(Walk));
        VoiceCommands.Commands.Add("move", new Action<int, string>(Move));
        VoiceCommands.Commands.Add("sprint", new Action<int, string>(Sprint));
        VoiceCommands.Commands.Add("run", new Action<int, string>(Sprint));      

        VoiceCommands.Commands.Add("middle", new Action<int,string>(MiddleRoom));
        VoiceCommands.Commands.Add("boss", new Action<int,string>(BossRoom));
        VoiceCommands.Commands.Add("cellar", new Action<int,string>(CellarRoom));
        VoiceCommands.Commands.Add("armory", new Action<int,string>(ArmoryRoom));
        VoiceCommands.Commands.Add("butcher", new Action<int,string>(ButcherRoom));
        VoiceCommands.Commands.Add("library", new Action<int,string>(LibraryRoom));
        VoiceCommands.Commands.Add("treasure", new Action<int,string>(TreasureRoom));
        VoiceCommands.Commands.Add("greenhouse", new Action<int,string>(GreenhouseRoom));
        VoiceCommands.Commands.Add("gallery", new Action<int,string>(GalleryRoom));

        // Selection Input
        VoiceCommands.Commands.Add("warrior", new Action<int,string>(SelectWarrior));
        VoiceCommands.Commands.Add("rogue", new Action<int,string>(SelectRogue));
        VoiceCommands.Commands.Add("archer", new Action<int,string>(SelectArcher));
        VoiceCommands.Commands.Add("mage", new Action<int,string>(SelectMage));
        VoiceCommands.Commands.Add("priest", new Action<int,string>(SelectPriest));

        // Combat Input
        VoiceCommands.Commands.Add("attack", new Action<int,string>(Engage));
        VoiceCommands.Commands.Add("engage", new Action<int,string>(Engage));

        // Other Input
        VoiceCommands.Commands.Add("room info", new Action<int,string>(RoomInfo));
        VoiceCommands.Commands.Add("target", new Action<int,string>(Target));
        VoiceCommands.Commands.Add("inspect", new Action<int,string>(Inspect));
        VoiceCommands.Commands.Add("interact", new Action<int,string>(Interact));
        VoiceCommands.Commands.Add("stop", new Action<int,string>(Stop));    

        VoiceCommands.Commands.Add("test", new Action<int,string>(Test));
        VoiceCommands.Commands.Add("debug", new Action<int, string>(DebugCommands));   

        SelectWarrior(1, "");
    }

    private void DebugCommands(int order, string prevKeyword)
    {
       foreach (KeyValuePair<string, Delegate> pair in VoiceCommands.Commands) {
            Debug.Log(pair.Key);
        }
    }

    private void Test(int order, string prevKeyword) 
    {
        Debug.Log("Order: " + order + " | Previous Command: " + prevKeyword);
        if (order == 1) {
            Debug.Log("I'm the first in the command chain");
        }

        else if (order == 2) {
            Debug.Log("I'm the second in the command chain");
        }

        if (prevKeyword == "warrior")
        {
            Debug.Log("I'm responding to the warrior keyword!");    
        }
        
    }

    private void RoomInfo(int order, string prevKeyword) 
    {
        Debug.Log(selectedCharacter.GetRoom().GetRoomInfo());    
    }

    private void Inspect(int order, string prevKeyword) 
    {        
        selectedCharacter.Inspect();
    }

    private void Interact(int order, string prevKeyword) 
    {
        if (order == 1) {

        }
        selectedCharacter.Interact();
    }

    private void Target(int order, string prevKeyword) {
        selectedCharacter.SetAction(PlayerController.ACTION.TARGETING);
    }

    private void Engage(int order, string prevKeyword) {
        selectedCharacter.SetAction(PlayerController.ACTION.ENGAGE);
    }

    private void SelectWarrior(int order, string prevKeyword) {

        if (selectedCharacter.GetAction() == PlayerController.ACTION.TARGETING) {
            selectedCharacter.SetTarget(warrior.gameObject); 
            selectedCharacter.SetAction(PlayerController.ACTION.IDLE);
        } else {
            selectedCharacter.GetCharacter().isSelected = false;
            selectedCharacter = warrior;
            im.SelectCharacter(warrior);
            selectedCharacter.GetCharacter().isSelected = true;
        }
        
    }

    private void SelectPriest(int order, string prevKeyword) {
        if (selectedCharacter.GetAction() == PlayerController.ACTION.TARGETING) {
            selectedCharacter.SetTarget(priest.gameObject); 
            selectedCharacter.SetAction(PlayerController.ACTION.IDLE);
        } else {
            selectedCharacter.GetCharacter().isSelected = false;
            selectedCharacter = priest;
            im.SelectCharacter(priest);
            selectedCharacter.GetCharacter().isSelected = true;
        }
    }

    private void SelectRogue(int order, string prevKeyword) {
        if (selectedCharacter.GetAction() == PlayerController.ACTION.TARGETING) {
            selectedCharacter.SetTarget(rogue.gameObject); 
            selectedCharacter.SetAction(PlayerController.ACTION.IDLE);
        } else {
            selectedCharacter.GetCharacter().isSelected = false;
            selectedCharacter = rogue;
            im.SelectCharacter(rogue);
            selectedCharacter.GetCharacter().isSelected = true;
        }
    }

    private void SelectMage(int order, string prevKeyword) {
        if (selectedCharacter.GetAction() == PlayerController.ACTION.TARGETING) {
            selectedCharacter.SetTarget(mage.gameObject); 
            selectedCharacter.SetAction(PlayerController.ACTION.IDLE);
        } else {
            selectedCharacter.GetCharacter().isSelected = false;
            selectedCharacter = mage;
            im.SelectCharacter(mage);
            selectedCharacter.GetCharacter().isSelected = true;
        }
    }

    private void SelectArcher(int order, string prevKeyword) {
        if (selectedCharacter.GetAction() == PlayerController.ACTION.TARGETING) {
            selectedCharacter.SetTarget(archer.gameObject); 
            selectedCharacter.SetAction(PlayerController.ACTION.IDLE);
        } else {
            selectedCharacter.GetCharacter().isSelected = false;
            selectedCharacter = archer;
            im.SelectCharacter(archer);
            selectedCharacter.GetCharacter().isSelected = true;
        }
    }

    private void Idle(int order, string prevKeyword) {
        selectedCharacter.SetMovementSpeed(PlayerController.MOVEMENT.IDLE);
    }

    private void Walk(int order, string prevKeyword) {
        selectedCharacter.SetMovementSpeed(PlayerController.MOVEMENT.WALK);
    }

    private void Move(int order, string prevKeyword) {       
        selectedCharacter.SetMovementSpeed(PlayerController.MOVEMENT.RUN);
    }

    private void Sprint(int order, string prevKeyword) {
        selectedCharacter.SetMovementSpeed(PlayerController.MOVEMENT.SPRINT);
    }

    private void BossRoom(int order, string prevKeyword) {
        if (order > 1)
        {
            selectedCharacter.SetDestinationTarget(gm.roomNav["boss"]);
            selectedCharacter.SetAction(PlayerController.ACTION.MOVE);
        }
        
    }

    private void MiddleRoom(int order, string prevKeyword) {
        if (order > 0)
        {
            selectedCharacter.SetDestinationTarget(gm.roomNav["middle"]);
            selectedCharacter.SetAction(PlayerController.ACTION.MOVE);
        }
        
    }

    private void CellarRoom(int order, string prevKeyword) {
        
        // Can only set destination after specifying either a movement command or a target command 
        if (order > 1) {
            selectedCharacter.SetDestinationTarget(gm.roomNav["cellar"]);
            selectedCharacter.SetAction(PlayerController.ACTION.MOVE);
        }
        
    }

    private void ArmoryRoom(int order, string prevKeyword) {
        if (order > 1)
        {
            selectedCharacter.SetDestinationTarget(gm.roomNav["armory"]);
            selectedCharacter.SetAction(PlayerController.ACTION.MOVE);
        }        
    }

    private void GalleryRoom(int order, string prevKeyword) {
        
        if (order > 1) {
            selectedCharacter.SetDestinationTarget(gm.roomNav["gallery"]);
            selectedCharacter.SetAction(PlayerController.ACTION.MOVE);
        }
    }

    private void LibraryRoom(int order, string prevKeyword) {
        if (order > 1)
        {
            selectedCharacter.SetDestinationTarget(gm.roomNav["library"]);
            selectedCharacter.SetAction(PlayerController.ACTION.MOVE);
        }
        
    }

    private void TreasureRoom(int order, string prevKeyword) {
        if (order > 1)
        {
            selectedCharacter.SetDestinationTarget(gm.roomNav["treasure"]);
            selectedCharacter.SetAction(PlayerController.ACTION.MOVE);
        }
        
    }

    private void ButcherRoom(int order, string prevKeyword) {
        if (order > 1)
        {
            selectedCharacter.SetDestinationTarget(gm.roomNav["butcher"]);
            selectedCharacter.SetAction(PlayerController.ACTION.MOVE);
        }
        
    }

    private void GreenhouseRoom(int order, string prevKeyword) {
        if (order > 1)
        {
            selectedCharacter.SetDestinationTarget(gm.roomNav["greenhouse"]);
            selectedCharacter.SetAction(PlayerController.ACTION.MOVE);
        }
        
    }

    private void Stop(int order, string prevKeyword) {       
        selectedCharacter.SetAction(PlayerController.ACTION.IDLE);             
    }
}
