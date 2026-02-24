using UnityEngine;

[CreateAssetMenu(fileName = "newHurtStateData", menuName = "Data/State Data/Hurt State")]
public class D_HurtState : ScriptableObject
{
    public float hurtTime = 0.4f;

    public float hurtKnockbackSpeed = 1.5f;
    public Vector2 hurtKnockbackAngle = new Vector2(1f, 1f);
    public float hurtKnockbackTime = 0.2f;
}
