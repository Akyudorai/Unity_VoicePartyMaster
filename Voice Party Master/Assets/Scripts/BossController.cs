using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
         // COMPONENTS
    ///////////////////////////
    private Rigidbody rb; 
    private NavMeshAgent nmAgent;   
    private Animator animator;

    [SerializeField] GameObject healthPanel;
    [SerializeField] Image healthBar;

    public Animator GetAnimator() { return animator; }

#region Enums

    public enum ACTION { IDLE, MOVE, ENGAGE, ATTACK }
    
    public enum DIRECTION { FORWARD, BACKWARD, LEFT, RIGHT, TARGET }
    
    public enum MOVEMENT { IDLE, WALK }

#endregion

// STATES
    [SerializeField] private ACTION currentAction = ACTION.IDLE; // --------  The current state of the character controller.

// COMBAT SYSTEMS
    public CharacterStats stats = new CharacterStats() 
    {
        // Base Stats
        Movement_Speed = 1.0f,
        Attack_Range = 6.0f,
        
        // Defensive Stats
        Health = 500.0f, 
        Health_Regen = 1.0f,
        Armor = 1,
        Resistance = 1,

        // Physical Stats
        Attack_Power = 5,
        Attack_Speed = 3.0f,

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
    [SerializeField] private float basicAttackTimer; // ------------------------------------  The timer that determines when the character can attack. 
    private RoomData currentRoom;

    // Powerup
    public float powerupCD = 30.0f;
    [SerializeField] private float powerup_timer;    

    // Roar -- Does not have a CD.  Instead, is cast whenever a certain health percentage is met.
    [SerializeField] List<GameObject> spawnPts;
    private int roarCount = 3;

    // Toss 
    public float tossCD = 20.0f;
    [SerializeField] private float toss_timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nmAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        // Initialize Entity
        entity = new Entity(stats.Health);
        entity.SetAnimator(animator);
        entity.healthBar = healthBar;
        entity.healthPanel = healthPanel;
        entity.onDeath += DeathEvent;
    }

    private void DeathEvent()
    {
        currentRoom.RemoveEnemy(this.gameObject);
        nmAgent.SetDestination(transform.position);
        SetAction(ACTION.IDLE);
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

            toss_timer -= Time.deltaTime;
            if (toss_timer < 0) {
                toss_timer = tossCD + 2.875f;   // (2.875 == animation time)
                animator.SetTrigger("TossAttack");   
            }

            powerup_timer -= Time.deltaTime;
            if (powerup_timer < 0) {
                powerup_timer = powerupCD + 2.959f; // (2.959 == animation time) 
                animator.SetTrigger("Powerup");
            }

            if (entity.GetCurrentHealth() / entity.GetMaxHealth() <= 0.7 && roarCount == 3) {
                roarCount--;
                animator.SetTrigger("Roar");
            } else if (entity.GetCurrentHealth() / entity.GetMaxHealth() <= 0.4f && roarCount == 2) {
                roarCount--;
                animator.SetTrigger("Roar");
            } else if (entity.GetCurrentHealth() / entity.GetMaxHealth() <= 0.15f && roarCount == 1) {
                roarCount--;
                animator.SetTrigger("Roar");
            }
        }

        // Declare an attack
        if (currentAction == ACTION.ATTACK) {            
            if (basicAttackTimer <= 0) {
            
                if (Vector3.Distance(transform.position, target.transform.position) > stats.Attack_Range) {
                    SetAction(ACTION.ENGAGE);
                }

                // Damage the target
                //float amount = (character.GetDamageType() == PrimaryDamageType.Physical) ? character.GetStats().Attack_Power * 1.0f : character.GetStats().Spell_Power * 0.35f;                
                //target.GetComponent<PlayerController>().entity.DealDamage(amount);
                basicAttackTimer = 5.0f;
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
                SetMovementSpeed(MOVEMENT.WALK);
                break;

            // When engaging, switch to jog movement.
            case ACTION.ENGAGE: 
                SetMovementSpeed(MOVEMENT.WALK);
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
        SetMovementSpeed(MOVEMENT.WALK);
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
            case MOVEMENT.WALK:
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

    // Can be powered up infinitely, resulting in an inevitible loss if the fight lasts too long
    public void Powerup() 
    {
        stats.Attack_Power += 5;        
    }

    // Spawn 6 Goblins at random positions.
    public void Roar()
    {

        // Randomly Generate Positions
        // ========================================================================
        List<GameObject> randPosList = new List<GameObject>(spawnPts);        
        
        GameObject pos1 = randPosList[Random.Range(0, randPosList.Count - 1)];
        randPosList.Remove(pos1);

        GameObject pos2 = randPosList[Random.Range(0, randPosList.Count - 1)];
        randPosList.Remove(pos2);

        GameObject pos3 = randPosList[Random.Range(0, randPosList.Count - 1)];
        randPosList.Remove(pos3);

        GameObject pos4 = randPosList[Random.Range(0, randPosList.Count - 1)];
        randPosList.Remove(pos4);

        GameObject pos5 = randPosList[Random.Range(0, randPosList.Count - 1)];
        randPosList.Remove(pos5);

        GameObject pos6 = randPosList[Random.Range(0, randPosList.Count - 1)];
        randPosList.Remove(pos6);

        // Spawn 6 Goblins
        // ========================================================================
        GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Goblin"), pos1.transform.position, Quaternion.identity);
        GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Goblin"), pos2.transform.position, Quaternion.identity);
        GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Goblin"), pos3.transform.position, Quaternion.identity);
        GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Goblin"), pos4.transform.position, Quaternion.identity);
        GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Goblin"), pos5.transform.position, Quaternion.identity);
        GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Goblin"), pos6.transform.position, Quaternion.identity);
    }

    public void TossAttack()
    {
        // Generate the ability settings
        AbilitySettings settings;
        settings.owner = entity;
        settings.target = null;

        // Create a Boulder Projectile
        GameObject boulder = GameObject.Instantiate(
            Resources.Load<GameObject>("Prefabs/Boss/Boulder"), 
            transform.position + new Vector3(1.5f, 0.0f, 0.0f),
            transform.rotation
            );

        boulder.GetComponent<Boulder>().Initialize(settings);
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
