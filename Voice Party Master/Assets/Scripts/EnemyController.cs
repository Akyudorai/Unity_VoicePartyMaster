using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyController : MonoBehaviour
{
        // COMPONENTS
    ///////////////////////////
    private Rigidbody rb; 
    private NavMeshAgent nmAgent;   
    private Animator animator;

    [SerializeField] GameObject healthpanel;
    [SerializeField] Image healthbar;

    public Animator GetAnimator() { return animator; }

#region Enums

    public enum ACTION { IDLE, MOVE, ENGAGE, ATTACK }
    
    public enum DIRECTION { FORWARD, BACKWARD, LEFT, RIGHT, TARGET }
    
    public enum MOVEMENT { IDLE, RUN }

#endregion

// STATES
    [SerializeField] private ACTION currentAction = ACTION.IDLE; // --------  The current state of the character controller.

// COMBAT SYSTEMS
    public CharacterStats stats = new CharacterStats() 
    {
        // Base Stats
        Movement_Speed = 1.0f,
        Attack_Range = 4.0f,
        
        // Defensive Stats
        Health = 100.0f, 
        Health_Regen = 1.0f,
        Armor = 1,
        Resistance = 1,

        // Physical Stats
        Attack_Power = 5,
        Attack_Speed = 1.0f,

        // Magical Stats
        Spell_Power = 1,
        Mana = 100.0f, 
        Mana_Regen = 1.0f,

        // Offensive Stats
        Critical_Rate = 0.1f,
        Critical_Damage = 2.0f,
    };

    public Entity entity; // ----------------------------------------------  Manages Health
    public GameObject target; // ------------------  The Current Target of the Controller
    private float basicAttackTimer; // ------------------------------------  The timer that determines when the character can attack. 

    private RoomData currentRoom;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nmAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        // Initialize Entity
        entity = new Entity(stats.Health);
        entity.SetAnimator(animator);
        entity.healthPanel = healthpanel;
        entity.healthBar = healthbar;
        entity.onDeath += DeathEvent;
    }

    private void DeathEvent() {
        currentRoom.RemoveEnemy(this.gameObject);
        nmAgent.SetDestination(transform.position);
        SetAction(ACTION.IDLE);
    }

    private void Update()
    {
        if (entity.IsDead) return;

        // HANDLE UI
        ///////////////////////////////////////
        healthpanel.transform.LookAt(Camera.main.transform.position);

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
            if (Vector3.Distance(transform.position, nmAgent.destination) > stats.Attack_Range) 
            {
                nmAgent.SetDestination(target.transform.position);
            } 
            
            // When in range, switch to attack action.
            else if (Vector3.Distance(transform.position, nmAgent.destination) <= stats.Attack_Range) 
            {
                SetAction(ACTION.ATTACK);   
            }
        }
            // HANDLE COMBAT
        ///////////////////////////////////////

        // Look for players
        if (currentRoom.PlayersInRoom.Count > 0) 
        {
            if (target == null || target.GetComponent<PlayerController>().entity.IsDead) 
            {
                // Find closest player
                GameObject pc = null;

                for (int i = 0; i < currentRoom.PlayersInRoom.Count; i++)
                {
                    if (pc == null) {
                        pc = currentRoom.PlayersInRoom[i];
                    }

                    else {
                        float currDist = Vector3.Distance(transform.position, pc.transform.position);
                        float indexDist = Vector3.Distance(transform.position, currentRoom.PlayersInRoom[i].transform.position);

                        if (indexDist < currDist) { 
                            pc = currentRoom.PlayersInRoom[i];
                        }
                    }
                }

                // Engage It
                SetTarget(pc);
                SetDestinationTarget(pc.gameObject.transform.position);
                SetAction(ACTION.ENGAGE);
            }
        }

        // Attack Speed Timer
        if (basicAttackTimer > 0) { 
            basicAttackTimer -= Time.deltaTime;
        }

        // Look At Target
        if (currentAction == ACTION.ENGAGE || currentAction == ACTION.ATTACK) {
            transform.LookAt(transform.position + GetDirection(DIRECTION.TARGET));
        }

        // Declare an attack
        if (currentAction == ACTION.ATTACK) {     

            if (Vector3.Distance(transform.position, target.transform.position) > stats.Attack_Range) {
                SetAction(ACTION.ENGAGE);
            }


            if (basicAttackTimer <= 0) {
                // Damage the target
                //float amount = (character.GetDamageType() == PrimaryDamageType.Physical) ? character.GetStats().Attack_Power * 1.0f : character.GetStats().Spell_Power * 0.35f;                
                //target.GetComponent<PlayerController>().entity.DealDamage(amount);
                basicAttackTimer = stats.Attack_Speed;
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
                SetMovementSpeed(MOVEMENT.RUN);
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
    
        currentAction = ACTION.MOVE;
        SetMovementSpeed(MOVEMENT.RUN);
    }

    // Set the type of movement being made
    ///////////////////////////////////////////
    public void SetMovementSpeed(MOVEMENT mov) 
    {
        if (entity.IsDead) return;

        switch(mov) {
            case MOVEMENT.IDLE:                
                nmAgent.speed = 0.0f;          
                break;
            case MOVEMENT.RUN:
                nmAgent.speed = 3.5f;
                break;
        }

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

    ////////////////////////////////////////////////////
    // UTILITIES
    ////////////////////////////////////////////////////

    public void SetRoom(RoomData room) 
    {   
        currentRoom = room;
    }

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
        if (stats != null) {
            // Attack Range
            Gizmos.DrawWireSphere(transform.position, stats.Attack_Range);
        }
        
        if (Application.isPlaying)
        {   
            // nmAgent Destination Point
            Gizmos.DrawSphere(nmAgent.destination, 0.4f);
        }
    }
}
