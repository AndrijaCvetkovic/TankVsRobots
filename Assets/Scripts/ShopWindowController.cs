using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindowController : MonoBehaviour
{

    public GameObject hulls;
    public GameObject tracks;
    public GameObject towers;

    public GameObject btnPrefab;

    public TankParts tp;

    public Text coins;

    public SpriteRenderer track1;
    public SpriteRenderer track2;
    public SpriteRenderer hull;
    public SpriteRenderer tower;

    public string[] boughtTracks;
    public string[] boughtHulls;
    public string[] boughtTowers;

    // Start is called before the first frame update
    void Start()
    {
        
        InitParts();
        coins.text = PlayerPrefs.GetInt("Coins").ToString();
        boughtPartsInit();
    }

    // Update is called once per frame
    void Update()
    {
        coins.text = PlayerPrefs.GetInt("Coins").ToString();
    }

    public void boughtPartsInit()
    {
        if (PlayerPrefs.HasKey("purchesedTracks"))
            boughtTracks = PlayerPrefs.GetString("purchesedTracks").Split(' ');
        if (PlayerPrefs.HasKey("purchesedHulls"))
            boughtHulls = PlayerPrefs.GetString("purchesedHulls").Split(' ');
        if (PlayerPrefs.HasKey("purchesedTowers"))
            boughtTowers = PlayerPrefs.GetString("purchesedTowers").Split(' ');
    }

    public void InitParts()
    {
        int counter = 0;
        foreach(Tracks t in tp.tracksParts)
        {
            int indexForItem = counter;
            GameObject g = Instantiate(btnPrefab);
            g.transform.GetChild(0).GetComponent<Image>().sprite = t.sprite;
            g.transform.GetChild(1).GetComponent<Text>().text = t.cost.ToString();
            g.GetComponent<Button>().onClick.AddListener(() => buyTrack(indexForItem));
            g.transform.SetParent(tracks.transform.GetChild(0).transform.GetChild(0).transform);
            g.transform.localScale = new Vector3(1, 1, 1);
            counter++;
        }

        counter = 0;
        foreach (Hull h in tp.hullParts)
        {
            int indexForItem = counter;
            GameObject g = Instantiate(btnPrefab);
            g.transform.GetChild(0).GetComponent<Image>().sprite = h.sprite;
            g.transform.GetChild(1).GetComponent<Text>().text = h.cost.ToString();
            g.GetComponent<Button>().onClick.AddListener(() => buyHull(indexForItem));
            g.transform.SetParent(hulls.transform.GetChild(0).transform.GetChild(0).transform);
            g.transform.localScale = new Vector3(1, 1, 1);
            counter++;
        }

        counter = 0;
        foreach (Tower t in tp.towerPars)
        {
            int indexForItem = counter;
            GameObject g = Instantiate(btnPrefab);
            g.transform.GetChild(0).GetComponent<Image>().sprite = t.sprite;
            g.transform.GetChild(1).GetComponent<Text>().text = t.cost.ToString();
            g.GetComponent<Button>().onClick.AddListener(() => buyTower(indexForItem));
            g.transform.SetParent(towers.transform.GetChild(0).transform.GetChild(0).transform);
            g.transform.localScale = new Vector3(1, 1, 1);
            counter++;
        }

        //------------------------------------------------------------------------------------------------------

        if (PlayerPrefs.HasKey("trackId") == false)
        {
            Tracks tracksPart = tp.tracksParts[0];
            track1.sprite = tracksPart.sprite;
            track2.sprite = tracksPart.sprite;
        }
        else
        {
            int partId = PlayerPrefs.GetInt("trackId");
            Tracks tracksPart = tp.tracksParts[partId];
            track1.sprite = tracksPart.sprite;
            track2.sprite = tracksPart.sprite;

        }

        if (PlayerPrefs.HasKey("hullId") == false)
        {
            Hull hullPart = tp.hullParts[0];
            hull.sprite = hullPart.sprite;
          
        }
        else
        {
            int partId = PlayerPrefs.GetInt("hullId");
            Hull hullPart = tp.hullParts[partId];
            hull.sprite = hullPart.sprite;
        }

        if (PlayerPrefs.HasKey("towerId") == false)
        {
            int partId = 0;
            Tower towerPart = tp.towerPars[partId];
            tower.sprite = towerPart.sprite;
            
        }
        else
        {
            int partId = PlayerPrefs.GetInt("towerId");
            Tower towerPart = tp.towerPars[partId];
            tower.sprite = towerPart.sprite;
        }


    }

    public void OpenHulls()
    {
        hulls.SetActive(true);
        towers.SetActive(false);
        tracks.SetActive(false);
    }

    public void OpenTracks()
    {
        hulls.SetActive(false);
        towers.SetActive(false);
        tracks.SetActive(true);
    }
    public void OpenTowers()
    {
        hulls.SetActive(false);
        towers.SetActive(true);
        tracks.SetActive(false);
    }

    public void buyTrack(int id)
    {
        Debug.Log("ID tracks " + id);
        int coins = PlayerPrefs.GetInt("Coins");
        Tracks trackPurchesed = tp.tracksParts[id];

        bool part_already_bought = false;
        foreach (string s in boughtTracks)
            if (s == id.ToString())
                part_already_bought = true;

        if(part_already_bought == true)
        {
            PlayerPrefs.SetInt("trackId", id);
            track1.sprite = trackPurchesed.sprite;
            track2.sprite = trackPurchesed.sprite;
        }
        else if (coins > trackPurchesed.cost)
        {
            PlayerPrefs.SetInt("trackId", id);
            track1.sprite = trackPurchesed.sprite;
            track2.sprite = trackPurchesed.sprite;

            if (part_already_bought == false)
            {
                Debug.Log("BOUGHT");
                string tracks = PlayerPrefs.GetString("purchesedTracks");
                PlayerPrefs.SetString("purchesedTracks", tracks + id + " ");
                PlayerPrefs.SetInt("Coins", coins - trackPurchesed.cost);
                boughtPartsInit();
            }
        }
    }

    public void buyHull(int id)
    {
        Debug.Log("ID hull " + id);
        int coins = PlayerPrefs.GetInt("Coins");
        Hull hullPurchesed = tp.hullParts[id];

        bool part_already_bought = false;
        foreach (string s in boughtHulls)
            if (s == id.ToString())
                part_already_bought = true;

        if(part_already_bought == true)
        {
            PlayerPrefs.SetInt("hullId", id);
            hull.sprite = hullPurchesed.sprite;
        }
        else if (coins > hullPurchesed.cost)
        {
           PlayerPrefs.SetInt("hullId",id);
           hull.sprite = hullPurchesed.sprite;

            if (part_already_bought == false)
            {
                Debug.Log("BOUGHT");
                string hulls = PlayerPrefs.GetString("purchesedHulls");
                PlayerPrefs.SetString("purchesedHulls", hulls + id + " ");
                PlayerPrefs.SetInt("Coins", coins - hullPurchesed.cost);
                boughtPartsInit();
            }
        }
    }

    public void buyTower(int id)
    {
        Debug.Log("ID tower " + id);
        int coins = PlayerPrefs.GetInt("Coins");
        Tower towerPurchesed = tp.towerPars[id];

        bool part_already_bought = false;
        foreach (string s in boughtTowers)
            if (s == id.ToString())
                part_already_bought = true;

        if (part_already_bought)
        {
            PlayerPrefs.SetInt("towerId", id);
            tower.sprite = towerPurchesed.sprite;
        }
        else if (coins > towerPurchesed.cost)
        {
            PlayerPrefs.SetInt("towerId", id);
            tower.sprite = towerPurchesed.sprite;
            

            if (part_already_bought == false)
            {
                Debug.Log("BOUGHT");
                string towers = PlayerPrefs.GetString("purchesedTowers");
                PlayerPrefs.SetString("purchesedTowers", towers + id + " ");
                PlayerPrefs.SetInt("Coins", coins - towerPurchesed.cost);
                boughtPartsInit();
            }
        }
        
        
    }

}
