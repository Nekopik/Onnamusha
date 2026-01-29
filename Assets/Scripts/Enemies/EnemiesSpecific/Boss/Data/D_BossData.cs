using UnityEngine;

[CreateAssetMenu(fileName = "newStunStateData", menuName = "Data/State Data/Stun State")]

public class D_BossStunData : ScriptableObject
{
    public float playerDetectRange = 15f;
    public float meleeRange = 2f;
    public Transform aggroPoint;
    public LayerMask whatIsPlayer;
}
