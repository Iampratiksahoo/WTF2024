using UnityEngine;

[CreateAssetMenu(fileName = "Zombie Data", menuName = "Zombie/Data")]
public class ZombieData : ScriptableObject
{
    public float walkSpeed;
    public float runSpeed;
    public float wanderTimer;
    public float acceptanceRadius;
}
