using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]

public class D_Entity : ScriptableObject
{
    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDistance = 0.4f;
    public float minAggroDistance = 1f;
    public float maxAggroDistance = 3f;
    public float closeRangeActionDistance = 1f;

    public LayerMask whatIsPlayer;
    public LayerMask whatIsGround;
}
