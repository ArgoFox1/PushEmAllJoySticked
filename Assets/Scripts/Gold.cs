using UnityEngine;
using DG.Tweening;
public class Gold : MonoBehaviour
{
    private float y;
    public GameManager gm;
    public TweenManager tManager;
    public SpawnManager spawnManager;
    private void Awake()
    {
        gm = GameManager.instance;
    }
    private void OnEnable()
    {
        transform.DOScale(new Vector3(4, 0.3f, 4), tManager.goldScaleDuration).SetLoops(-1, LoopType.Yoyo);
        transform.DOMoveY(64, tManager.goldMoveDuration).SetLoops(-1, LoopType.Yoyo).From();
    }
    private void Update()
    {
        Rotate();
    }  
    private void Rotate()
    {
        y = transform.rotation.y;
        y += Time.time * 10;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(90, y, 0), Time.deltaTime * 400);
    }
}
