using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class VoiceController : MonoBehaviour
{   
    GameManager gm;
    InterfaceManager im;

    public PlayerController selectedCharacter = null;
    public PlayerController warrior, rogue, archer, mage, priest;
    public bool selectingEveryone = false;

    private int secretCount = 0;

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

        // Action Commands
        VoiceCommands.Commands.Add("search", new Action<int, string>(Investigate));
        VoiceCommands.Commands.Add("investigate", new Action<int, string>(Investigate));
        VoiceCommands.Commands.Add("target", new Action<int,string>(Target));
        VoiceCommands.Commands.Add("stop", new Action<int,string>(Stop));    
        VoiceCommands.Commands.Add("attack", new Action<int,string>(Engage));
        VoiceCommands.Commands.Add("engage", new Action<int,string>(Engage));
        VoiceCommands.Commands.Add("trade", new Action<int, string>(Trade));

        // Selection Input
        VoiceCommands.Commands.Add("warrior", new Action<int,string>(SelectWarrior));
        VoiceCommands.Commands.Add("rogue", new Action<int,string>(SelectRogue));
        VoiceCommands.Commands.Add("archer", new Action<int,string>(SelectArcher));
        VoiceCommands.Commands.Add("mage", new Action<int,string>(SelectMage));
        VoiceCommands.Commands.Add("priest", new Action<int,string>(SelectPriest));
        VoiceCommands.Commands.Add("camera", new Action<int, string>(SelectCamera));
        VoiceCommands.Commands.Add("everyone", new Action<int, string>(SelectEveryone));

        // Room Commands
        VoiceCommands.Commands.Add("hallway", new Action<int,string>(HallwayRoom));
        VoiceCommands.Commands.Add("boss", new Action<int,string>(BossRoom));
        VoiceCommands.Commands.Add("cellar", new Action<int,string>(CellarRoom));
        VoiceCommands.Commands.Add("armory", new Action<int,string>(ArmoryRoom));
        VoiceCommands.Commands.Add("bedroom", new Action<int,string>(BedRoom));
        VoiceCommands.Commands.Add("library", new Action<int,string>(LibraryRoom));
        VoiceCommands.Commands.Add("dining", new Action<int,string>(DiningRoom));
        VoiceCommands.Commands.Add("garden", new Action<int,string>(GardenRoom));
        VoiceCommands.Commands.Add("kitchen", new Action<int,string>(KitchenRoom));

        // Other Input
        VoiceCommands.Commands.Add("test", new Action<int,string>(Test));
        VoiceCommands.Commands.Add("debug", new Action<int, string>(DebugCommands));   
        VoiceCommands.Commands.Add("up", new Action<int, string>(Up));
        VoiceCommands.Commands.Add("down", new Action<int,string>(Down));
        VoiceCommands.Commands.Add("left", new Action<int,string>(Left));
        VoiceCommands.Commands.Add("right", new Action<int,string>(Right));
        VoiceCommands.Commands.Add("A", new Action<int, string>(A));
        VoiceCommands.Commands.Add("B", new Action<int,string>(B));
        VoiceCommands.Commands.Add("AB", new Action<int,string>(AB));
        VoiceCommands.Commands.Add("start", new Action<int,string>(Start));

        SelectCharacter("Warrior", 1, "");
        SelectCharacter("Warrior", 2, "camera");

        // DEBUGGING
        mage.Equip("Gem");
        rogue.Equip("Gloves");
    }

    private void Update()
    {
        if (warrior.entity.IsDead && rogue.entity.IsDead && archer.entity.IsDead &&
            mage.entity.IsDead && priest.entity.IsDead)
        {
            gm.LoseEvent();        
        }
    }

    private void DebugCommands(int order, string prevKeyword)
    {
       foreach (KeyValuePair<string, Delegate> pair in VoiceCommands.Commands) {
            Debug.Log(pair.Key);
        }
    }

    private void Up(int order, string prevKeyword)
    {
        if (order == 1 || secretCount == 1 && prevKeyword == "up")
        {
            secretCount++;
        } 
    }

    private void Down(int order, string prevKeyword)
    {
        if (secretCount == 2 && prevKeyword == "up" || secretCount == 3 && prevKeyword == "down")
        {
            secretCount++;
        } 
    }

    private void Left(int order, string prevKeyword)
    {
        if (secretCount == 4 && prevKeyword == "down" || secretCount == 6 && prevKeyword == "right")
        {
            secretCount++;
        }     
    }

    private void Right(int order, string prevKeyword)
    {
        if (secretCount == 5 && prevKeyword == "left" || secretCount == 7 && prevKeyword == "left")
        {
            secretCount++;
        }     
    }
    private void A(int order, string prevKeyword)
    {
        if (secretCount == 8 && prevKeyword == "right")
        {
            secretCount++;
        }     
    }
    private void B(int order, string prevKeyword)
    {
        if (secretCount == 9 && prevKeyword == "a")
        {
            secretCount++;
        }
    }

    private void AB(int order, string prevKeyword)
    {
        if (secretCount == 8 && prevKeyword == "right")
        {
            secretCount += 2;
        }
    }

    private void Start(int order, string prevKeyword) 
    {
        if (secretCount == 10 && prevKeyword == "AB")
        {
            secretCount++;
            SceneManager.LoadScene("SecretScene");
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

 
    
#region Selection Commands

    private void SelectCharacter(string name, int order, string prevKeyword) 
    {    
        if (order == 1) 
        {
            selectedCharacter.GetCharacter().isSelected = false;
            
            switch (name) {
                default:
                case "Warrior":
                    selectedCharacter = warrior;
                    im.SelectCharacter(warrior);                    
                    break;
                case "Priest":
                    selectedCharacter = priest;
                    im.SelectCharacter(priest);
                    break;
                case "Rogue":
                    selectedCharacter = rogue;
                    im.SelectCharacter(rogue);
                    break;
                case "Mage":
                    selectedCharacter = mage;
                    im.SelectCharacter(mage);
                    break;
                case "Archer":
                    selectedCharacter = archer;
                    im.SelectCharacter(archer);
                    break;                
            }

            selectedCharacter.GetCharacter().isSelected = true;
            SelectCharacter(name, 2, "camera");

            // Updates Room Data
            GameObject.Find("UI").GetComponent<InterfaceManager>().UpdateRoomData();
        }

        else if (order > 1 && prevKeyword == "target") 
        {
            switch (name) {
                default:
                case "Warrior":
                    selectedCharacter.SetTarget(warrior.gameObject); 
                    break;
                case "Priest":
                    selectedCharacter.SetTarget(priest.gameObject); 
                    break;
                case "Rogue":
                    selectedCharacter.SetTarget(rogue.gameObject); 
                    break;
                case "Mage":
                    selectedCharacter.SetTarget(mage.gameObject); 
                    break;
                case "Archer":
                    selectedCharacter.SetTarget(archer.gameObject); 
                    break;                
            }
            
            selectedCharacter.SetAction(PlayerController.ACTION.IDLE);
        }

        else if (order > 1 && prevKeyword == "camera") 
        {
            // Set Camera Position to Room Overview
            switch (name) {
                default:
                case "Warrior":
                    Camera.main.GetComponent<CameraController>().targetCharacter = warrior.gameObject;
                    break;
                case "Priest":
                    Camera.main.GetComponent<CameraController>().targetCharacter = priest.gameObject;
                    break;
                case "Rogue":
                    Camera.main.GetComponent<CameraController>().targetCharacter = rogue.gameObject;
                    break;
                case "Mage":
                    Camera.main.GetComponent<CameraController>().targetCharacter = mage.gameObject;
                    break;
                case "Archer":
                    Camera.main.GetComponent<CameraController>().targetCharacter = archer.gameObject;
                    break;                
            }

            Camera.main.GetComponent<CameraController>().tMode = CameraController.TargetMode.FollowTarget;            
        }
    
        else if (order > 1 && prevKeyword == "trade") 
        {
            // Can't trade with self, silly.
            if (selectedCharacter.name == name) return;

            string item1  = selectedCharacter.currentItem;            
            string item2 = "";
            selectedCharacter.Unequip();
            
            switch (name) {
                default:
                case "Warrior":
                    item2 = warrior.currentItem;
                    warrior.Unequip();
                    warrior.Equip(item1);

                    selectedCharacter.Equip(item2);                     
                    break;
                case "Priest":
                    item2 = priest.currentItem;
                    priest.Unequip();
                    priest.Equip(item1);

                    selectedCharacter.Equip(item2); 
                    break;
                case "Rogue":
                    item2 = rogue.currentItem;
                    rogue.Unequip();
                    rogue.Equip(item1);

                    selectedCharacter.Equip(item2); 
                    break;
                case "Mage":
                    item2 = mage.currentItem;
                    mage.Unequip();
                    mage.Equip(item1);

                    selectedCharacter.Equip(item2); 
                    break;
                case "Archer":
                    item2 = archer.currentItem;
                    archer.Unequip();
                    archer.Equip(item1);

                    selectedCharacter.Equip(item2); 
                    break;                
            }
        }
    }

    private void SelectWarrior(int order, string prevKeyword) {
        SelectCharacter("Warrior", order, prevKeyword);
    }

    private void SelectPriest(int order, string prevKeyword) {
        SelectCharacter("Priest", order, prevKeyword);
    }

    private void SelectRogue(int order, string prevKeyword) {
       SelectCharacter("Rogue", order, prevKeyword);
    }

    private void SelectMage(int order, string prevKeyword) {
        SelectCharacter("Mage", order, prevKeyword);
    }

    private void SelectArcher(int order, string prevKeyword) {
        SelectCharacter("Archer", order, prevKeyword);
    }
    
    private void SelectCamera(int order, string prevKeyword)
    { }

    private void SelectEveryone(int order, string prevKeyword)
    { 
        selectingEveryone = true;
    }

#endregion

#region Movement Commands
    private void Idle(int order, string prevKeyword) {

        if (prevKeyword == "everyone") {
            warrior.SetMovementSpeed(PlayerController.MOVEMENT.IDLE);
            priest.SetMovementSpeed(PlayerController.MOVEMENT.IDLE);
            mage.SetMovementSpeed(PlayerController.MOVEMENT.IDLE);
            rogue.SetMovementSpeed(PlayerController.MOVEMENT.IDLE);
            archer.SetMovementSpeed(PlayerController.MOVEMENT.IDLE);
        }

        else {
            selectedCharacter.SetMovementSpeed(PlayerController.MOVEMENT.IDLE);
        }
        
    }

    private void Walk(int order, string prevKeyword) {

        if (prevKeyword == "everyone") {
            warrior.SetMovementSpeed(PlayerController.MOVEMENT.WALK);
            priest.SetMovementSpeed(PlayerController.MOVEMENT.WALK);
            mage.SetMovementSpeed(PlayerController.MOVEMENT.WALK);
            rogue.SetMovementSpeed(PlayerController.MOVEMENT.WALK);
            archer.SetMovementSpeed(PlayerController.MOVEMENT.WALK);
        }

        else {
            selectedCharacter.SetMovementSpeed(PlayerController.MOVEMENT.WALK);
        }
        
    }

    private void Move(int order, string prevKeyword) {       

        if (prevKeyword == "everyone") {
            warrior.SetMovementSpeed(PlayerController.MOVEMENT.RUN);
            priest.SetMovementSpeed(PlayerController.MOVEMENT.RUN);
            mage.SetMovementSpeed(PlayerController.MOVEMENT.RUN);
            rogue.SetMovementSpeed(PlayerController.MOVEMENT.RUN);
            archer.SetMovementSpeed(PlayerController.MOVEMENT.RUN);
        }

        else {
            selectedCharacter.SetMovementSpeed(PlayerController.MOVEMENT.RUN);
        }
        
    }

    private void Sprint(int order, string prevKeyword) {
        
        if (prevKeyword == "everyone") {
            warrior.SetMovementSpeed(PlayerController.MOVEMENT.SPRINT);
            priest.SetMovementSpeed(PlayerController.MOVEMENT.SPRINT);
            mage.SetMovementSpeed(PlayerController.MOVEMENT.SPRINT);
            rogue.SetMovementSpeed(PlayerController.MOVEMENT.SPRINT);
            archer.SetMovementSpeed(PlayerController.MOVEMENT.SPRINT);
        }

        else {
            selectedCharacter.SetMovementSpeed(PlayerController.MOVEMENT.SPRINT);
        }
        
    }

#endregion

#region Action Commands

    private void Trade(int order, string prevKeyword)
    { }

    private void Target(int order, string prevKeyword) {

        if (prevKeyword == "everyone") {
            warrior.SetAction(PlayerController.ACTION.TARGETING);
            priest.SetAction(PlayerController.ACTION.TARGETING);
            mage.SetAction(PlayerController.ACTION.TARGETING);
            rogue.SetAction(PlayerController.ACTION.TARGETING);
            archer.SetAction(PlayerController.ACTION.TARGETING);

            selectingEveryone = false;
        }

        else {
            selectedCharacter.SetAction(PlayerController.ACTION.TARGETING);
        }
        
    }

    private void Engage(int order, string prevKeyword) {
        
        if (prevKeyword == "everyone") {
            warrior.SetAction(PlayerController.ACTION.ENGAGE);
            priest.SetAction(PlayerController.ACTION.ENGAGE);
            mage.SetAction(PlayerController.ACTION.ENGAGE);
            rogue.SetAction(PlayerController.ACTION.ENGAGE);
            archer.SetAction(PlayerController.ACTION.ENGAGE);
        
            selectingEveryone = false;
        }

        else {
            selectedCharacter.SetAction(PlayerController.ACTION.ENGAGE);
        }
        
    }


    private void Investigate(int order, string prevKeyword)
    {
        selectedCharacter.SetAction(PlayerController.ACTION.INVESTIGATING);
    }

    private void Stop(int order, string prevKeyword) {    
        
        if (prevKeyword == "everyone") {
            warrior.SetAction(PlayerController.ACTION.IDLE);
            priest.SetAction(PlayerController.ACTION.IDLE);
            mage.SetAction(PlayerController.ACTION.IDLE);
            rogue.SetAction(PlayerController.ACTION.IDLE);
            archer.SetAction(PlayerController.ACTION.IDLE);
        }

        else {
            selectedCharacter.SetAction(PlayerController.ACTION.IDLE);
        }
                     
    }

#endregion

#region Room Commands

    private void SelectRoom(string name, int order, string prevKeyword)
    {
        if (order > 1 && prevKeyword == "move" ||
            order > 1 && prevKeyword == "sprint" || 
            order > 1 && prevKeyword == "walk" || 
            order > 1 && prevKeyword == "run"
            )
        {
            Vector3 destination = Vector3.zero;

            switch (name) {                
                case "Hallway": destination = gm.roomNav["hallway"]; break;
                case "Boss": destination = gm.roomNav["boss"]; break;
                case "Armory": destination = gm.roomNav["armory"]; break;
                case "Dining": destination = gm.roomNav["dining"]; break;
                case "Kitchen": destination = gm.roomNav["kitchen"]; break;
                case "Cellar": destination = gm.roomNav["cellar"]; break;
                case "Bedroom": destination = gm.roomNav["bedroom"]; break;
                case "Library": destination = gm.roomNav["library"]; break;
                case "Garden": destination = gm.roomNav["garden"]; break;                
            }

            if (destination != Vector3.zero) {
                if (selectingEveryone) {
                    warrior.SetDestinationTarget(destination);
                    warrior.SetAction(PlayerController.ACTION.MOVE);
                    priest.SetDestinationTarget(destination);
                    priest.SetAction(PlayerController.ACTION.MOVE);
                    mage.SetDestinationTarget(destination);
                    mage.SetAction(PlayerController.ACTION.MOVE);
                    rogue.SetDestinationTarget(destination);
                    rogue.SetAction(PlayerController.ACTION.MOVE);
                    archer.SetDestinationTarget(destination);
                    archer.SetAction(PlayerController.ACTION.MOVE);

                    selectingEveryone = false;
                }

                else {
                    selectedCharacter.SetDestinationTarget(destination);
                    selectedCharacter.SetAction(PlayerController.ACTION.MOVE);
                }
            }
        }

        if (order > 1 && prevKeyword == "camera") 
        {
            // Set Camera Position to Room Overview
            Camera.main.GetComponent<CameraController>().tMode = CameraController.TargetMode.TargetPosition;

            switch (name) {
                default:
                case "Hallway":
                    Camera.main.GetComponent<CameraController>().targetPosition = gm.GetRoom("Hallway").cameraOverviewPos;
                    break;
                case "Boss":
                    Camera.main.GetComponent<CameraController>().targetPosition = gm.GetRoom("Boss").cameraOverviewPos;
                    break;
                case "Armory":
                    Camera.main.GetComponent<CameraController>().targetPosition = gm.GetRoom("Armory").cameraOverviewPos;
                    break;
                case "Dining":
                    Camera.main.GetComponent<CameraController>().targetPosition = gm.GetRoom("Dining").cameraOverviewPos;
                    break;
                case "Kitchen":
                    Camera.main.GetComponent<CameraController>().targetPosition = gm.GetRoom("Kitchen").cameraOverviewPos;
                    break;
                case "Cellar":
                    Camera.main.GetComponent<CameraController>().targetPosition = gm.GetRoom("Cellar").cameraOverviewPos;
                    break;
                case "Bedroom":
                    Camera.main.GetComponent<CameraController>().targetPosition = gm.GetRoom("Bedroom").cameraOverviewPos;
                    break;
                case "Library":
                    Camera.main.GetComponent<CameraController>().targetPosition = gm.GetRoom("Library").cameraOverviewPos;
                    break;
                case "Garden":
                    Camera.main.GetComponent<CameraController>().targetPosition = gm.GetRoom("Garden").cameraOverviewPos;
                    break;                
            }

        
        }
    }

    private void BossRoom(int order, string prevKeyword) {
        SelectRoom("Boss", order, prevKeyword);        
    }

    private void HallwayRoom(int order, string prevKeyword) {
        SelectRoom("Hallway", order, prevKeyword); 
    }

    private void CellarRoom(int order, string prevKeyword) {
        SelectRoom("Cellar", order, prevKeyword); 
    }

    private void ArmoryRoom(int order, string prevKeyword) {
        SelectRoom("Armory", order, prevKeyword);     
    }

    private void BedRoom(int order, string prevKeyword) {
        SelectRoom("Bedroom", order, prevKeyword); 
    }

    private void LibraryRoom(int order, string prevKeyword) {
        SelectRoom("Library", order, prevKeyword); 
    }

    private void DiningRoom(int order, string prevKeyword) {
        SelectRoom("Dining", order, prevKeyword); 
    }

    private void GardenRoom(int order, string prevKeyword) {
        SelectRoom("Garden", order, prevKeyword); 
    }

    private void KitchenRoom(int order, string prevKeyword) {
        SelectRoom("Kitchen", order, prevKeyword); 
    }

#endregion
    
}
