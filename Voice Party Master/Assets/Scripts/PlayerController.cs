using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    // COMPONENTS
    ///////////////////////////
    private Rigidbody rb; 
    private NavMeshAgent nmAgent;   
    private Animator animator;

    public Animator GetAnimator() { return animator; }

#region Enums

    public enum ACTION { IDLE, MOVE, TARGETING, ENGAGE, ATTACK, INTERACT, INSPECT }
    
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

    // GAME SYSTEMS
    [SerializeField] private RoomData currentRoom = null;

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

        // Load Character Actions to Commands
        foreach (KeyValuePair<string, Delegate> pair in character.Actions) {
            VoiceCommands.Commands.Add(pair.Key, pair.Value);
        }
    }

    private async void Update() 
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
            if (Vector3.Distance(transform.position, target.transform.position) > character.GetStats().Attack_Range) 
            {
                nmAgent.SetDestination(target.transform.position);
            } 
            
            // When in range, switch to attack action.
            else if (Vector3.Distance(transform.position, target.transform.position) <= character.GetStats().Attack_Range) 
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
        }

        // Declare an attack
        if (currentAction == ACTION.ATTACK) {            
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

        nmAgent.SetDestination(target);
        currentAction = ACTION.MOVE;
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
            nmAgent.speed = 1.5f;
            currentMovementSpeed = MOVEMENT.WALK;
            animator.SetInteger("MoveType", (int)1);
            return;
        }

        switch(mov) {
            case MOVEMENT.IDLE:                
                nmAgent.speed = 0.0f;          
                break;
            case MOVEMENT.WALK:
                nmAgent.speed = 1.5f;
                break;
            case MOVEMENT.RUN:
                nmAgent.speed = 3.5f;
                break;
            case MOVEMENT.SPRINT:
                nmAgent.speed = 5.5f;
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
            target.GetComponent<InteractionData>().Inspect();
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
        
    }
    
    
}
