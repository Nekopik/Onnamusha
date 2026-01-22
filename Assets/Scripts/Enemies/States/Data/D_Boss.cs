using UnityEngine;

public class D_Boss : ScriptableObject
{
    public float playerDetectRange = 15f;
    public float meleeRange = 2f;
    public Transform aggroPoint;
    public LayerMask whatIsPlayer;
}
