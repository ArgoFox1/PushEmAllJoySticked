using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class DefaultZombie : MonoBehaviour,IRandomDrop
{
    private float distance;
    private bool canDefaultAttack;
    private bool isGrounded;
    public LevelManager levelManager;
    public SpawnManager spawnManager;
    public TweenManager tManager;
    public GameObject box;
    public Transform checker;
    public Rigidbody rb;
    public NavMeshAgent agent;
    public Animation anim;
    public DataManager dataManager;
    public GameObject player;
    private void Awake()
    {

        rb.constraints = RigidbodyConstraints.FreezePositionY;
        agent.speed = dataManager.defaultZombieSpeed;
        agent.angularSpeed = dataManager.defaultTurnSpeed; 
        canDefaultAttack = true;

    }
    private void Update()
    {

        #region Movement
        if(player is not null && agent.enabled == true) { Move(); }
        else { anim.Stop("DefaultZombieRun"); }
        #endregion

        #region CheckingGround
        GroundChecker();
        #endregion

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Log"))
        {
            agent.enabled = false;
            Pushed();
        }
        if (other.gameObject.CompareTag("DeadZone"))
        {
            spawnManager.deadZombieCount++;
            spawnManager.zombies.Remove(gameObject);
            spawnManager.pooledZombies.Add(this.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            agent.enabled = false;
            if(canDefaultAttack == true)
            {
                Attack();
            }
        }       
    }
    private void GroundChecker()
    {
        RaycastHit hit;
        if (Physics.Raycast(checker.position, -checker.transform.up, out hit, dataManager.distance, dataManager.floorMask))
        { 
            rb.constraints = RigidbodyConstraints.FreezePositionY;
            isGrounded = true;
        } 
        else
        {
            rb.constraints = RigidbodyConstraints.None;
            agent.enabled = false;
            anim.Stop();
            isGrounded = false;
        }
    }
    private void Move()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if(distance <= 150)
        {
            agent.SetDestination(player.transform.position);
            anim.Play("DefaultZombieRun");
        }     
    }
    public void Pushed()
    {
        anim.Play("StandingUp");
        rb.velocity = -transform.forward * dataManager.pushForce * 2;
        dataManager.canDrop = Random.value > 0.95f;
        StartCoroutine(nameof(Cooldown4Push));
    }
    IEnumerator Cooldown4Push()
    {
        yield return new WaitForSeconds(3f);
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(7);
        agent.enabled = true;
    }
    public void Attack()
    {
        canDefaultAttack = false;
        anim.PlayQueued("Attack");
        dataManager.playerHealth -= 25;
        StartCoroutine(nameof(Cooldown4Attack));
    }
    IEnumerator Cooldown4Attack()
    {      
        yield return new WaitForSeconds(5f);
        canDefaultAttack = true;
        agent.enabled = true;
    }

    public void RandomDrop()
    {
        dataManager.canDrop = false;
        Vector3 boxPos = new Vector3(transform.position.x, transform.position.y + 25, transform.position.z);
        GameObject _box = Instantiate(box, boxPos, Quaternion.identity);
        _box.transform.DOJump(boxPos, tManager.boxJumpForce, tManager.boxJumpCount, tManager.boxDuration);
        _box.transform.DOShakeScale(tManager.boxShakeDuration, tManager.boxShakeForce, tManager.boxVibration, tManager.boxRandomness);
    }
}
