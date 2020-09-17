using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    public AudioSource soundExplosion;
    public AudioSource soundShoot;
    public AudioSource music;
    public AudioSource btnSound;
    public AudioSource boost;
    public AudioSource shellHit;
    public bool muteMusic = false;
    public bool muteSound = false;
    //public bool muteMusic = true;

    public GameObject soundBtn;
    public GameObject musicBtn;

    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        if(instance == null)
            instance = this;

        DontDestroyOnLoad(this.gameObject);


        if (PlayerPrefs.GetInt("Sound") == 1)
            muteAllSounds();

        if(music != null)
            music.Play();
        if (PlayerPrefs.GetInt("Music") == 1)
            mutedMusic();
        //play_music();
    }

    public void play_SoundExplosion()
    {
        if (soundExplosion == null)
        {
            Debug.Log("NULL");
            soundExplosion = GameObject.Find(FindObjectOfType<SoundManager>().name + "/Explosion").GetComponent<AudioSource>();
            if (muteSound == false)
                soundExplosion.Play();
        }
        else if (muteSound == false)
            soundExplosion.Play();
    }

    public void play_soundShoot()
    {
        if(soundShoot == null)
        {
            Debug.Log("NULL");
            soundShoot = GameObject.Find(FindObjectOfType<SoundManager>().name + "/Shoot").GetComponent<AudioSource>();
            if (muteSound == false)
                soundShoot.Play();
        }
        else if (muteSound == false)
        {
            soundShoot.Play();
        }
    }

    public void play_music()
    {

        if (muteMusic == false)
        {
            music.mute = false;
        }
        else
        {
            music.mute = true;
        }
    }

    public void play_btnSound()
    {
        if (btnSound == null)
        {
            Debug.Log("NULL");
            btnSound = GameObject.Find(FindObjectOfType<SoundManager>().name + "/Btn").GetComponent<AudioSource>();
            if (muteSound == false)
                btnSound.Play();
        }
        else if (muteSound == false)
            btnSound.Play();
    }

    public void play_boost()
    {
        if (boost == null)
        {
            Debug.Log("NULL");
            boost = GameObject.Find(FindObjectOfType<SoundManager>().name + "/Boost").GetComponent<AudioSource>();
            if (muteSound == false)
                boost.Play();
        }
        else if (muteSound == false)
            boost.Play();
    }

    public void play_shellHit()
    {
        if (shellHit == null)
        {
            Debug.Log("NULL");
            shellHit = GameObject.Find(FindObjectOfType<SoundManager>().name + "/ShellHit").GetComponent<AudioSource>();
            if (muteSound == false)
                shellHit.Play();
        }
        else if (muteSound == false)
            shellHit.Play();
    }

    public void muteAllSounds()
    {
        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
            soundBtn = Camera.main.GetComponent<MenuScene>().soundBtn;
            muteSound = !muteSound;
            int muted = 0;
            soundBtn.transform.GetChild(0).gameObject.SetActive(true);
            soundBtn.transform.GetChild(1).gameObject.SetActive(false);
            if (muteSound == true)
            {
                muted = 1;
                soundBtn.transform.GetChild(0).gameObject.SetActive(false);
                soundBtn.transform.GetChild(1).gameObject.SetActive(true);
            }
            //play_music();
            PlayerPrefs.SetInt("Sound", muted);
        }
    }

    public void mutedMusic()
    {
        if(music == null)
            music = GameObject.Find(FindObjectOfType<SoundManager>().name + "/BGM").GetComponent<AudioSource>();
        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
            musicBtn = Camera.main.GetComponent<MenuScene>().musicBtn;


            muteMusic = !muteMusic;
            int muted = 0;
            musicBtn.transform.GetChild(0).gameObject.SetActive(true);
            musicBtn.transform.GetChild(1).gameObject.SetActive(false);
            if (muteMusic == true)
            {
                muted = 1;
                musicBtn.transform.GetChild(0).gameObject.SetActive(false);
                musicBtn.transform.GetChild(1).gameObject.SetActive(true);
            }
            music.mute = muteMusic;
            PlayerPrefs.SetInt("Music", muted);
        }
    }

}
