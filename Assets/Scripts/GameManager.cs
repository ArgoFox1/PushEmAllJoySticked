using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public bool isFinal;
    public bool isPlayerDead;
    public int goldCount;
    public List<RectTransform> panelRects;
    public GameObject cam;
    public AudioSource soundFolder;
    public AudioClip ambienceClip;
    public TweenManager tManager;
    public DefaultZombie defaultZombie;
    public OneArmedZombie oneArmedZombie;
    public SpawnManager spawnManager;
    public DataManager dataManager;
    [Header("UIs")]
    public Image wonPanel;
    public Image lostPanel;
    public Image pauseMenuImage;
    public Image healthImage;
    public Text healthText;
    public Text goldText;  
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if( _instance == null)
            {
                Debug.LogError("GameManager is Null");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        soundFolder.PlayOneShot(ambienceClip);
        _instance = this;
    }
    private void Start()
    {
        goldCount = 0;
    }
    private void Update()
    {

        #region SoundSettings
        if(Time.timeScale == 0) { soundFolder.Pause(); }
        else { soundFolder.UnPause(); } 
        #endregion

        #region BoxDrop
        if (dataManager.canDrop == true && oneArmedZombie.gameObject.activeInHierarchy == true && defaultZombie.gameObject.activeInHierarchy == true)
        {
            IRandomDrop randomDrop = oneArmedZombie.GetComponent<IRandomDrop>();
            randomDrop = defaultZombie.GetComponent<IRandomDrop>();
            randomDrop.RandomDrop();
        }
        #endregion

        #region UIs Stuff

        PanelTween();

        #region PlayerHealth
        healthText.text = dataManager.playerHealth.ToString();
        healthImage.fillAmount = dataManager.playerHealth / 100;        
        #endregion

        #region Gold
        goldText.text = goldCount.ToString();
        #endregion

        #region Lost
        if (isPlayerDead == true) 
        { 
            lostPanel.gameObject.SetActive(true);
            cam.SetActive(true);
        }
       
        #endregion

        #region Won
        if (isFinal == true) { wonPanel.gameObject.SetActive(true); }    
        #endregion

        #endregion

        #region Players Death
        if (dataManager.playerHealth <= 0) { isPlayerDead = true; }
        #endregion

    }
    public void Settings()
    {
        Time.timeScale = 0;
        pauseMenuImage.gameObject.SetActive(true);
    }
    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenuImage.gameObject.SetActive(false);
    }
    public void PanelTween()
    {
        for (int i = 0; i < panelRects.Count; i++)
        {
            if (panelRects[i].gameObject.activeInHierarchy == true) { panelRects[i].DOAnchorPosY(0, tManager.settingAnchorDuration); }
            else { panelRects[i].DOAnchorPosY(panelRects[i].transform.position.x, 0.1f); }
        }       
    }
}
