using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // COMPONENTS
    ///////////////////////////
    private Rigidbody rb; 
    private NavMeshAgent nmAgent;   
    private Animator animator;

    public Animator GetAnimator() { return animator; }

#region Enums

    public enum ACTION { IDLE, MOVE, TARGETING, ENGAGE, ATTACK, INTERACT, INSPECT, INVESTIGATING, SEARCHING }
    
    public enum DIRECTION { FORWARD, BACKWARD, LEFT, RIGHT, TARGET }
    
    public enum MOVEMENT { IDLE, WALK, RUN, SPRINT }
    

#endregion

    // CLASSIFICATION
    public CharacterClass characterClass;
    private Character character;
    public Character GetCharacter() { return character; }

    // STATES
    [SerializeField] private ACTION currentAction = ACTION.IDLE; // --------  The current state of the character controller.
    
    // MOTION SYSTEMS
    [SerializeField] private MOVEMENT currentMovementSpeed = MOVEMENT.RUN;
     
    // COMBAT SYSTEMS
    public Entity entity; // ----------------------------------------------  Manages Health
    public GameObject target; // ------------------  The Current Target of the Controller
    private float basicAttackTimer; // ------------------------------------  The timer that determines when the character can attack. 
    public Delegate abilityBuffer;

    // INVENTORY SYSTEM
    public string currentItem;
    public Image inventoryPanel;
    public Image inventoryIcon;

    // GAME SYSTEMS
    [SerializeField] private RoomData currentRoom = null;

    private void DeathEvent() {
        currentRoom.RemovePlayer(this.gameObject);
    }

    ////////////////////////////////////////////////////
    // MONOBEHAVIOR
    ////////////////////////////////////////////////////
    private void Awake() {
        rb = GetComponent<Rigidbody>();
        nmAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        // Initialize the Character
        character = Character.Load(characterClass);
        character.SetAnimator(animator);
        character.SetController(this);

        // Initialize Entity
        entity = new Entity(character.GetStats().Health);
        entity.SetOwner(this);
        entity.SetAnimator(animator);
        entity.onDeath += DeathEvent;

        // Load Character Actions to Commands
        foreach (KeyValuePair<string, Delegate> pair in character.Actions) {
            VoiceCommands.Commands.Add(pair.Key, pair.Value);
        }
    }

    private void Update() 
    {    
        if (entity.IsDead) return;

        // HANDLE MOTION
        ///////////////////////////////////////
        if (currentAction == ACTION.MOVE) 
        {                               
            // Set movement to idle when destination has been reached
            if (nmAgent.remainingDistance <= nmAgent.stoppingDistance && nmAgent.velocity.sqrMagnitude == 0f) 
            {   
                SetAction(ACTION.IDLE);         
            }         
        
        } else if (currentAction == ACTION.ENGAGE) 
        {       
            // While out of range, move towards the target     
            if (Vector3.Distance(transform.position, nmAgent.destination) > character.GetStats().Attack_Range) 
            {
                nmAgent.SetDestination(target.transform.position);
            } 
            
            // When in range, switch to attack action.
            else if (Vector3.Distance(transform.position, nmAgent.destination) <= character.GetStats().Attack_Range) 
            {
                if (abilityBuffer != null) {

                    // Cast the Ability
                    abilityBuffer.DynamicInvoke(0, "");

                    // Set Ability Buffer to null after done casting
                    abilityBuffer = null;
                }

                SetAction(ACTION.ATTACK);              
            }

            // While out of range, move towards the target
            // if (Vector3.Distance(transform.position, target.transform.position) > character.GetStats().Attack_Range) {
            //     rb.velocity = GetDirection(moveDirection) * nmAgent.speed * Time.deltaTime;
            // } else if (Vector3.Distance(transform.position, target.transform.position) < character.GetStats().Attack_Range) {
            //     SetAction(ACTION.ATTACK);
            // }            
        }    

        else if (currentAction == ACTION.INVESTIGATING)
        {
            // Set a new investigation target
            if (target.GetComponent<InteractionData>() != null) {

                // When in range, begin investigation.
                if (Vector3.Distance(transform.position, nmAgent.destination) <= 1.5f) 
                {
                    SetAction(ACTION.SEARCHING);
                    animator.SetTrigger("Search");
                }
            }
        } 
                
        
        // HANDLE COMBAT
        ///////////////////////////////////////

        // Attack Speed Timer
        if (basicAttackTimer > 0) { 
            basicAttackTimer -= Time.deltaTime;
        }

        // Ability Cooldowns        
        foreach (KeyValuePair<string, AbilityData> pair in character.Abilities.ToList())
        {            
            AbilityData data = pair.Value;            

            if (data.currCD > 0) {
                data.currCD -= Time.deltaTime;    
            } else if (data.currCD < 0) {
                data.currCD = 0;
            }      

            character.Abilities[pair.Key] = data;
        }

        // Look At Target
        if (currentAction == ACTION.ENGAGE || currentAction == ACTION.ATTACK) {
            transform.LookAt(transform.position + GetDirection(DIRECTION.TARGET));           

            // If the current target dies, either go Idle or Search for Other Enemies
            if (target.GetComponent<EnemyController>() != null) {
                if (target.GetComponent<EnemyController>().entity.IsDead) {
                    // Look for Enemies
                    if (currentRoom.EnemiesInRoom.Count > 0) 
                    {
                        // Find closest enemy
                        GameObject ec = null;

                        for (int i = 0; i < currentRoom.EnemiesInRoom.Count; i++)
                        {
                            if (ec == null) {
                                ec = currentRoom.EnemiesInRoom[i];
                            }

                            else {
                                float currDist = Vector3.Distance(transform.position, ec.transform.position);
                                float indexDist = Vector3.Distance(transform.position, currentRoom.EnemiesInRoom[i].transform.position);

                                if (indexDist < currDist) { 
                                    ec = currentRoom.EnemiesInRoom[i];
                                }
                            }
                        }

                        // Engage It
                        SetTarget(ec);
                        SetDestinationTarget(ec.gameObject.transform.position);
                        SetAction(ACTION.ENGAGE);
                    }

                    // Go Idle
                    else {
                        SetAction(ACTION.IDLE);
                    }
                }
            }

            else if (target.GetComponent<BossController>() != null) {
                if (target.GetComponent<BossController>().entity.IsDead) {
                    // Look for Enemies
                    if (currentRoom.EnemiesInRoom.Count > 0) 
                    {
                        // Find closest enemy
                        GameObject ec = null;

                        for (int i = 0; i < currentRoom.EnemiesInRoom.Count; i++)
                        {
                            if (ec == null) {
                                ec = currentRoom.EnemiesInRoom[i];
                            }

                            else {
                                float currDist = Vector3.Distance(transform.position, ec.transform.position);
                                float indexDist = Vector3.Distance(transform.position, currentRoom.EnemiesInRoom[i].transform.position);

                                if (indexDist < currDist) { 
                                    ec = currentRoom.EnemiesInRoom[i];
                                }
                            }
                        }

                        // Engage It
                        SetTarget(ec);
                        SetAction(ACTION.ENGAGE);
                    }

                    // Go Idle
                    else {
                        SetAction(ACTION.IDLE);
                    }
                }
            } 
        }

        // Declare an attack
        if (currentAction == ACTION.ATTACK) { 

            if (Vector3.Distance(transform.position, target.transform.position) > character.GetStats().Attack_Range) {
                SetAction(ACTION.ENGAGE);
            }

            if (basicAttackTimer <= 0) {
                // Damage the target
                //float amount = (character.GetDamageType() == PrimaryDamageType.Physical) ? character.GetStats().Attack_Power * 1.0f : character.GetStats().Spell_Power * 0.35f;                
                //target.GetComponent<PlayerController>().entity.DealDamage(amount);
                basicAttackTimer = character.GetStats().Attack_Speed;
                animator.SetTrigger("Attack");                
            }
        }

    }

    ////////////////////////////////////////////////////
    // STATE MANAGEMENT
    ////////////////////////////////////////////////////

    // Set Action
    public void SetAction(ACTION action) {

        if (entity.IsDead) return;

        currentAction = action;
        animator.SetInteger("ActionType", (int)currentAction);

        switch (currentAction) {
            default:
            case ACTION.IDLE: // Do Nothing.  
                SetMovementSpeed(MOVEMENT.IDLE);  
                break;

            // Begin Motion
            case ACTION.MOVE:
                SetMovementSpeed(currentMovementSpeed);
                break;

            // When engaging, switch to jog movement.
            case ACTION.ENGAGE: 
                SetMovementSpeed(MOVEMENT.RUN);
                if (target == null) {
                    Debug.Log("I don't have a target.");
                    SetAction(ACTION.IDLE);
                }                
                break;

            // When attacking, switch to idle movement.
            case ACTION.ATTACK:
                SetMovementSpeed(MOVEMENT.IDLE);
                break;   

            // Stop and look at surroundings
            case ACTION.INSPECT:
                SetMovementSpeed(MOVEMENT.IDLE);                                
                break;

            case ACTION.INTERACT:
                SetMovementSpeed(MOVEMENT.IDLE);
                break;     
            case ACTION.INVESTIGATING:
                
                // Select a new target
                int undiscoverd = currentRoom.GetUndiscoveredObjects().Count;
                if (undiscoverd > 0) {
                    int rand = UnityEngine.Random.Range(0, undiscoverd - 1);
                    target = currentRoom.GetUndiscoveredObjects()[rand];

                    Vector3 destination = target.transform.position - (target.transform.position - transform.position).normalized * 1.5f;
                    nmAgent.SetDestination(destination);

                    SetMovementSpeed(MOVEMENT.WALK);
                } else if (currentRoom.GetUndiscoveredObjects().Count == 0) {
                    Debug.Log("No more objects to investigate in this room.");
                    SetAction(ACTION.IDLE);
                }
                break;    
            case ACTION.SEARCHING:
                transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
                SetMovementSpeed(MOVEMENT.IDLE);
                break;
        }
    }

    public ACTION GetAction() {
        return currentAction;
    }

    ////////////////////////////////////////////////////
    // MOTION CONTROLS
    ////////////////////////////////////////////////////
    
    // Set NavMeshAgent target destination
    ///////////////////////////////////////////
    public void SetDestinationTarget(Vector3 target) {

        if (entity.IsDead) return;

        Vector3 destination = target - (target - transform.position).normalized * 1.5f;
        nmAgent.SetDestination(destination);
        
        currentAction = (currentAction == ACTION.IDLE) ? ACTION.MOVE : currentAction;
        SetMovementSpeed(currentMovementSpeed);
    }

    // Set the type of movement being made
    ///////////////////////////////////////////
    public void SetMovementSpeed(MOVEMENT mov) 
    {
        if (entity.IsDead) return;

        // Rogue Stealth Override
        if (characterClass == CharacterClass.Rogue && animator.GetBool("Stealthed"))
        {
            nmAgent.speed = 2.25f;
            currentMovementSpeed = MOVEMENT.WALK;
            animator.SetInteger("MoveType", (int)1);
            return;
        }

        switch(mov) {
            case MOVEMENT.IDLE:                
                nmAgent.speed = 0.0f;          
                break;
            case MOVEMENT.WALK:
                nmAgent.speed = 2.25f;
                break;
            case MOVEMENT.RUN:
                nmAgent.speed = 4.5f;
                break;
            case MOVEMENT.SPRINT:
                nmAgent.speed = 6.5f;
                break;
        }

        currentMovementSpeed = mov;
        animator.SetInteger("MoveType", (int)mov);
    }

    ////////////////////////////////////////////////////
    // COMBAT CONTROLS
    ////////////////////////////////////////////////////

    // Set Object Target
    ///////////////////////////////////////////
    public void SetTarget(GameObject obj) 
    {
        if (entity.IsDead) return;

        target = obj;             
    }


    // Buffer an ability to be cast when conditions are met.    
    public void BufferAbility(Delegate ability)
    {
        abilityBuffer = ability;
    }

    ////////////////////////////////////////////////////
    // GAMEPLAY CONTROLS
    ////////////////////////////////////////////////////

    // Set and Load Current Room Data
    public void SetRoom(RoomData room) 
    {   
        currentRoom = room;
    }

    // Get the current room
    public RoomData GetRoom() 
    {
        return currentRoom;
    }

    public void Inspect() 
    {
        if (entity.IsDead) return;

        if (target == null) {
            Debug.Log("Need a target first.");
            return;
        }

        if (target.GetComponent<InteractionData>() != null) 
        {
            target.GetComponent<InteractionData>().Inspect(this);
        }
    }

    public void Interact()
    {
        if (entity.IsDead) return;

        if (target == null) {
            Debug.Log("Need a target first.");
            return;
        }

        if (target.GetComponent<InteractionData>() != null) 
        {
            target.GetComponent<InteractionData>().Interact();
        } 
    }

    public void Investigate()
    {
        if (target.GetComponent<InteractionData>() != null) 
        {
            target.GetComponent<InteractionData>().Inspect(this);
        }   
    }

    public void Equip(string item)
    {
        switch (item)
        {
            case "Key":
                currentItem = item;
                inventoryPanel.enabled = true;
                inventoryIcon.enabled = true;
                inventoryIcon.sprite = Resources.Load<Sprite>("Icons/Items/skeleton-key");
                break;
            case "Sword":
                currentItem = item;
                inventoryPanel.enabled = true;
                inventoryIcon.enabled = true;
                inventoryIcon.sprite = Resources.Load<Sprite>("Icons/Items/broadsword");

                // Update Power Stat
                character.GetStats().Attack_Power += 3;
                break;
            case "Shield":
                currentItem = item;
                inventoryPanel.enabled = true;
                inventoryIcon.enabled = true;
                inventoryIcon.sprite = Resources.Load<Sprite>("Icons/Items/round-shield");
                // Update Defense Stat
                character.GetStats().Armor += 5;
                break;
            case "Gem":
                currentItem = item;
                inventoryPanel.enabled = true;
                inventoryIcon.enabled = true;
                inventoryIcon.sprite = Resources.Load<Sprite>("Icons/Items/rupee");
                // Update Power Stat
                character.GetStats().Spell_Power += 3;
                break;
            case "Gloves":
                currentItem = item;
                inventoryPanel.enabled = true;
                inventoryIcon.enabled = true;
                inventoryIcon.sprite = Resources.Load<Sprite>("Icons/Items/gloves");
                // Update Attack Speed Stat
                character.GetStats().Attack_Speed -= 0.2f;
                break;
        }
    }

    public void Unequip()
    {
        switch (currentItem)
        {
            case "Key":
                
                break;
            case "Sword":
                // Update Power Stat
                character.GetStats().Attack_Power -= 3;
                break;
            case "Shield":
                // Update Defense Stat
                character.GetStats().Armor -= 5;
                break;
            case "Gem":
                // Update Power Stat
                character.GetStats().Spell_Power -= 3;
                break;
            case "Gloves":
                // Update Attack Speed Stat
                character.GetStats().Attack_Speed += 0.2f;
                break;
        }

        inventoryPanel.enabled = false;
        inventoryIcon.enabled = false;
        inventoryIcon.sprite = null;
        currentItem = "";
    }

    ////////////////////////////////////////////////////
    // UTILITIES
    ////////////////////////////////////////////////////

    // Set Directional Input

    // Get a relative direction based on target direction
    private Vector3 GetDirection(DIRECTION direction) {
        switch (direction) {
            default:
            case DIRECTION.FORWARD:
                return transform.forward;
            case DIRECTION.BACKWARD:
                return -transform.forward;
            case DIRECTION.RIGHT:
                return transform.right;
            case DIRECTION.LEFT: 
                return -transform.right;
            case DIRECTION.TARGET:
                if (target == null) return transform.forward;
                return (target.transform.position - transform.position).normalized;
        }
    }

    private void OnDrawGizmos() {
        if (character != null) {
            Gizmos.DrawWireSphere(transform.position, character.GetStats().Attack_Range);
        }

        if (Application.isPlaying)
        {
            Gizmos.DrawSphere(nmAgent.destination, 0.2f);
        }
        
        
    }

    private void OnTriggerEnter(Collider other) {
        
        if (other.name == "Entry Point" && other.tag != currentRoom.RoomName)             
        {
            // Stop  
            SetAction(PlayerController.ACTION.IDLE); 

            // Set the new room
            currentRoom = other.gameObject.GetComponentInParent<RoomData>();

            // Set Camera Position to Room Overview
            Camera.main.GetComponent<CameraController>().tMode = CameraController.TargetMode.TargetPosition;
            Camera.main.GetComponent<CameraController>().targetPosition = other.gameObject.GetComponentInParent<RoomData>().cameraOverviewPos;
        
            // Look for Enemies
            if (currentRoom.EnemiesInRoom.Count > 0) 
            {
                // Find closest enemy
                GameObject ec = null;

                for (int i = 0; i < currentRoom.EnemiesInRoom.Count; i++)
                {
                    if (ec == null) {
                        ec = currentRoom.EnemiesInRoom[i];
                    }

                    else {
                        float currDist = Vector3.Distance(transform.position, ec.transform.position);
                        float indexDist = Vector3.Distance(transform.position, currentRoom.EnemiesInRoom[i].transform.position);

                        if (indexDist < currDist) { 
                            ec = currentRoom.EnemiesInRoom[i];
                        }
                    }
                }

                // Engage It
                SetTarget(ec);
                SetAction(ACTION.ENGAGE);
            }
        }


    }
    
    
}
