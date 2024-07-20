using UnityEngine;
public class LightFlickerEffect : MonoBehaviour 
{
    public new Light light;
    public Vector2 timeOn = new Vector2(0.1f, 0.1f);
    public Vector2 timeOff = new Vector2(0.5f, 0.5f);
    float changeTime = 0f;
    private void Awake()
    {
        light = GetComponent<Light>();
    }

    void Update() {
        if (Time.time > changeTime)
        {
            light.enabled = !light.enabled;
            if (light.enabled)
            {
                changeTime = Time.time + Random.Range(timeOn.x, timeOn.y);
            }
            else
            {
                changeTime = Time.time + Random.Range(timeOff.x, timeOff.y);
            }
        }
    }

}