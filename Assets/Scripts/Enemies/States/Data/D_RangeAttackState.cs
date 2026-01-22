using UnityEngine;

[CreateAssetMenu(fileName = "newRangeAttackStateData", menuName = "Data/State Data/Range Attack State")]
public class D_RangeAttackState : ScriptableObject
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public int attackDamage = 10;
    public LayerMask whatIsPlayer;
}
