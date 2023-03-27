using UnityEngine;

public class DataManager : MonoBehaviour
{
    public bool canDrop;

    public bool isPickedBladeBox;

    public bool isPickedForceBox;

    public int playerHealth = 100;

    public float playerSpeed; // 10

    public float pushForce; // 10

    public float sensivity; // 2 

    public float defaultTurnSpeed; // 260

    public float defaultZombieSpeed; // 6

    public float oneArmedZombieSpeed; // 8

    public float oneArmedTurnSpeed; // 270

    public float distance; // 1

    public LayerMask floorMask;

}
