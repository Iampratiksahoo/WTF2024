using UnityEngine;
using UnityEngine.UI;
public class MainMenuButton : MonoBehaviour
{
    public Button play;
    public Button credits;
    public Button quit;

    private void Awake()
    {
        play.onClick.AddListener(play_OnClick);
        credits.onClick.AddListener(credits_OnClick);
        quit.onClick.AddListener(quit_OnClick);
    }

    private void play_OnClick()
    {

    }
    private void credits_OnClick()
    {

    }
    private void quit_OnClick()
    {
        Application.Quit(); 
    }
}
