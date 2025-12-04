using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]

public class D_Entity : ScriptableObject
{
    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDistance = 0.4f;
    public float minAggroDistance = 1;
    public float maxAggroDistance = 3;

    public LayerMask whatIsPlayer;
    public LayerMask whatIsGround;
}
