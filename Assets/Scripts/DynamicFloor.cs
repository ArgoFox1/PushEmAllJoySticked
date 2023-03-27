using UnityEngine;

public class DynamicFloor : MonoBehaviour
{
    private float y;
    private void Update()
    {
        Spin();
    }
    private void Spin()
    {
        y = transform.rotation.y;
        y += Time.time * 10;
        transform.rotation = Quaternion.Euler(0, y, 0);
    }
}
