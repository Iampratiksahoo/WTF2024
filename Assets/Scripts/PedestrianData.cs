using System.Collections.Generic;

[System.Serializable]
public struct PedestrianData
{
    public float walkSpeed;
    public float runSpeed;
    public float wanderTimer;
    public float acceptanceRadius;
    
    public void TurnZombieData() {
        walkSpeed = 1.2f;
    }
}
