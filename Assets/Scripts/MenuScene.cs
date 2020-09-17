using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{

    public Text score;
    public GameObject shopWindow;
    public GameObject settingsWindow;
    public SoundManager sm;

    public GameObject soundBtn;
    public GameObject musicBtn;

    void Start()
    {
        
        if (PlayerPrefs.HasKey("HighScore"))
            score.text = PlayerPrefs.GetInt("HighScore").ToString();
        else
            score.transform.parent.gameObject.SetActive(false);

        sound_and_music_btns();
    }

    void Update()
    {
        
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void OpenShopWindow()
    {
        shopWindow.SetActive(true);
    }

    public void OpenSeetingsWindow()
    {
        settingsWindow.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void CloseShopWindow()
    {
        shopWindow.SetActive(false);
    }

    public void sound_and_music_btns()
    {
        soundBtn.GetComponent<Button>().onClick.AddListener(() => SoundManager.instance.muteAllSounds());
        musicBtn.GetComponent<Button>().onClick.AddListener(() => SoundManager.instance.mutedMusic());

    }

}
