using UnityEngine;
using UnityEngine.UI;

public class GameProgressBar : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float progress;
    [SerializeField] Image progressImage;
    [SerializeField] Transform skull;
    [SerializeField] Vector2 skullMinMax;
    [SerializeField] Transform ear;
    [SerializeField] Vector2 earMinMax;
    

    private void Awake()
    {
        progress = 0;
    }

    public void SetProgress(float progress, float max)
    {
        progressImage.fillAmount = progress/max;

        float skullRange = Mathf.Abs(skullMinMax.x - skullMinMax.y);
        skull.localPosition = new Vector3((skullRange * progressImage.fillAmount) + skullMinMax.x, skull.localPosition.y, skull.localPosition.z);
        
        float earRange = Mathf.Abs(earMinMax.x - earMinMax.y);
        ear.localPosition = new Vector3((earRange * progressImage.fillAmount) + earMinMax.x, ear.localPosition.y, ear.localPosition.z);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(progressImage != null && skull != null) 
        {
            SetProgress(progress, 1f);
        }
    }
#endif
}