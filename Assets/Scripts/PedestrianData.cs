using System.Collections.Generic;
using UnityEngine;

public enum PedestrianType
{
    Normal,
    Hostile
}

[CreateAssetMenu(fileName = "Pedestrian Data", menuName = "Pedestrian/Data")]
public class PedestrianData : ScriptableObject
{
    public PedestrianType mood;
    public float walkSpeed;
    public float runSpeed;
    public float wanderTimer;
    public float acceptanceRadius;
}
