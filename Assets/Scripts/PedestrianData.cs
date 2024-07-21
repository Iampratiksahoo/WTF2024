using System.Collections.Generic;

[System.Serializable]
public struct PedestrianData
{
    public List<WayPoint> _policeWayPoints;
    public float walkSpeed;
    public float runSpeed;
    public float wanderTimer;
    public float acceptanceRadius;
    public float attackRadius;
    
    public void TurnZombieData() {
        walkSpeed = 1.2f;
    }
}
