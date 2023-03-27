using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour 
{
    public bool isGrounded;
    public bool canJump;
    public LayerMask floorMask;
    public Transform checker;
    public GameManager gameManager;
    public LevelManager levelManager;
    public AudioSource soundFolder;
    public RectTransform goldImageRect;
    public AudioClip goldCollectClip;
    public AudioClip breakingWoodenBox;
    public SpawnManager spawnManager;
    public TweenManager tManager;
    public Transform log2;
    public Animation anim;
    public Rigidbody rb;
    public DataManager dataManager;
    public FixedJoystick fixedJoystick;
    private bool canThrow;
    private enum Logs
    {
        Default,
        Forced,
        Blade
    }
    private Logs logs;
    private void Awake()
    {
        gameManager = GameManager.instance;
        canThrow = true;
        dataManager.isPickedBladeBox = false;
        dataManager.isPickedForceBox = false;
    }
    private void LateUpdate()
    {

        #region LookDirs
        if(isGrounded == true)
        {
            if (fixedJoystick.Horizontal != 0 || fixedJoystick.Vertical != 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rb.velocity), Time.deltaTime * dataManager.sensivity);
                anim.Play("mixamo.com");
            }
            else
            {
                anim.Stop();
            }
        }     
        #endregion

    }
    private void FixedUpdate()
    {

        #region Movement
        if (Input.GetMouseButton(0))
        {
            Movement();
        }
        #endregion     

    }
    private void Update()
    {

        #region LogThrow

        if (canThrow == true && dataManager.isPickedBladeBox != true && dataManager.isPickedForceBox != true) { logs = Logs.Default; }
        else if (dataManager.isPickedBladeBox == true && canThrow == true) { logs = Logs.Blade; }
        else if (dataManager.isPickedForceBox == true && canThrow == true) { logs = Logs.Forced; }

        #endregion

        #region Jump
        if(canJump == true) 
        {
            rb.AddForce(Vector3.up * 80);
        }
        #endregion

        #region GroundCheck
        GroundChecker();
        #endregion

    }
    private void OnTriggerEnter(Collider other)
    {     
        if (other.gameObject.CompareTag("BladeBox"))
        {
            soundFolder.PlayOneShot(breakingWoodenBox);
            dataManager.isPickedBladeBox = true;
            Destroy(other.gameObject);
        }   
        if (other.gameObject.CompareTag("ForceBox"))
        {
            soundFolder.PlayOneShot(breakingWoodenBox);
            dataManager.isPickedForceBox = true;
            Destroy (other.gameObject);
        }
        if (other.gameObject.CompareTag("Gold"))
        {
            other.gameObject.SetActive(false);
            soundFolder.PlayOneShot(goldCollectClip);
            spawnManager.golds.Remove(other.gameObject);
            spawnManager.pooledGolds.Add(other.gameObject);
            goldImageRect.DOScale(1.25f,0.2f).SetLoops(2,LoopType.Yoyo).OnComplete(() => { gameManager.goldCount++; goldImageRect.DOScale(1f, 0.2f); });
        }
        if (other.gameObject.CompareTag("DeadZone"))
        {
            gameManager.isPlayerDead = true;
            gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("JumpPad"))
        {
            canJump = true;
            transform.rotation = Quaternion.identity;
        }
    }   
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("JumpPad")) 
        { 
            if(isGrounded == true) { canJump = false; }
        }
    }
    private void GroundChecker()
    {
        RaycastHit hit;
        if (Physics.Raycast(checker.position, -checker.transform.up, out hit,10f, floorMask))
        {
            Debug.DrawRay(checker.position, -checker.transform.up * hit.distance, Color.red);
            isGrounded = true;
        }
        else 
        {
            canJump = false;
            isGrounded = false;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Quaternion.Euler(0, 0, 0).eulerAngles), Time.deltaTime * dataManager.sensivity);
        }
    }
    private void Movement()
    {
        float horizontal = fixedJoystick.Horizontal * dataManager.playerSpeed;
        float vertical = fixedJoystick.Vertical * dataManager.playerSpeed;
        rb.velocity = new Vector3(horizontal, rb.velocity.y, vertical);
    }
    public void ThrowLog()
    {
        switch (logs)
        {
            case Logs.Default:
                DefaultLog();
                break;
            case Logs.Blade:
                BladeLog();
                break;
            case Logs.Forced:
                ForcedLog();
                break;
        }
    }
    private void DefaultLog()
    {
        if(canThrow == true)
        {
            canThrow = false;
            log2.DOLocalMoveZ(tManager.logThrowDistance, tManager.logThrowDuration).OnComplete(() => { log2.DOLocalMoveZ(0.615f, tManager.logThrowDuration); });
            StartCoroutine(nameof(Cooldown4DefaultLog));
        }
    }
    private void BladeLog()
    {
        if(dataManager.isPickedBladeBox == true)
        {
            dataManager.isPickedBladeBox = false;
            log2.DOLocalMoveZ(tManager.logThrowDistance * 2, tManager.logThrowDuration).OnComplete(() => { log2.DOLocalMoveZ(0.615f, tManager.logThrowDuration); });
            log2.DOLocalRotate(Quaternion.Euler(90, 270, 0).eulerAngles, tManager.logThrowDuration).OnComplete(() => { log2.DOLocalRotate(Quaternion.Euler(90, 90, 0).eulerAngles, tManager.logThrowDuration); });
            StartCoroutine(nameof(Cooldown4BladeLog));
        }      
    }
    private void ForcedLog()
    {
        if(dataManager.isPickedForceBox == true)
        {
            dataManager.isPickedForceBox = false;
            dataManager.pushForce = dataManager.pushForce * 2;
            log2.DOLocalMoveZ(tManager.logThrowDistance * 2, tManager.logThrowDuration).OnComplete(() => { log2.DOLocalMoveZ(0.615f, tManager.logThrowDuration); });
            StartCoroutine(nameof(Cooldown4ForcedLog));
        }     
    }   
    IEnumerator Cooldown4DefaultLog()
    {
        yield return new WaitForSeconds(tManager.logThrowDistance);
        canThrow = true;
    }
    IEnumerator Cooldown4ForcedLog()
    {
        yield return new WaitForSeconds(tManager.logThrowDistance * 2);
        dataManager.pushForce = dataManager.pushForce / 2;
    }
    IEnumerator Cooldown4BladeLog()
    {
        yield return new WaitForSeconds(tManager.logThrowDistance);
    }
}
