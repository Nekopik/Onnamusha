using UnityEngine;

[CreateAssetMenu(fileName = "newStunStateData", menuName = "Data/State Data/Stun State")]
public class D_StunState : ScriptableObject
{
    public float StunTime = 3.0f;
    public float StunKnockbackTime = 0.2f;
    public float StunKnockbackSpeed = 2.0f;

    public Vector2 StunKnockbackAngle;
    
}
