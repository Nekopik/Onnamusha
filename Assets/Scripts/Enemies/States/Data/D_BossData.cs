using UnityEngine;

[CreateAssetMenu(fileName = "newBossData", menuName = "Data/Enemy/Boss Data")]

public class D_BossData : ScriptableObject
{
    public float playerDetectRange = 15f;
    public float meleeRange = 2f;
    public Transform aggroPoint;
    public LayerMask whatIsPlayer;
}
