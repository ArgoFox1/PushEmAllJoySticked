using UnityEngine;
using UnityEngine.UI;
public class Cam : MonoBehaviour
{
    public Image wonImage;
    public ParticleSystem confetteFX;
    public Transform player;
    public Vector3 followDistance;  
    private void LateUpdate()
    {
        if (player != null)
        {
            transform.parent = player.transform;
            transform.localPosition = Vector3.Lerp(transform.localPosition, followDistance, Time.deltaTime / 2);
        }
        if(wonImage.gameObject.activeInHierarchy == true)
        { 
            confetteFX.gameObject.SetActive(true);
            confetteFX.Play();
        }
    }
}
