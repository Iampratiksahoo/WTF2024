using UnityEngine;
using UnityEngine.UI;

public class GameProgressBar : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float progress;
    [SerializeField] Image progressImage;
    [SerializeField] Image skull;
    [SerializeField] Vector2 skullMinMax;
    

    private void Awake()
    {
        progress = 0;
    }

    public void SetProgress(float progress, float max)
    {
        progressImage.fillAmount = progress/max;

        float skullRange = Mathf.Abs(skullMinMax.x - skullMinMax.y);
        skull.transform.localPosition = new Vector3((skullRange * progressImage.fillAmount) + skullMinMax.x, skull.transform.localPosition.y, skull.transform.localPosition.z);
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