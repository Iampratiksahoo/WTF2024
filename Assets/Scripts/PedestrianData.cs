public enum PedestrianType
{
    Normal,
    Hostile
}

[System.Serializable]
public struct PedestrianData
{
    public PedestrianType mood;
    public float walkSpeed;
    public float runSpeed;
    public float wanderTimer;
    public float acceptanceRadius;

    public void TurnZombieData() {
        walkSpeed = 1.2f;
        runSpeed = 0;
        acceptanceRadius /= 2;
    }
}
