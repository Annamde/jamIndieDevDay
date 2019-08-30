using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }

    [Header("Player")]
    public PlayerScript player;

    [Header("HUD")]
    public Text timeText;
    public Text scoreText;
    public Text highScoreText;
    public GameObject gameOverPanel;

    [Header("Constants")]
    public float initialTime;
    public float enabledSueterTime;

    [Header("Sueter Variables")]
    public Transform suetersParent; //empty que contiene todos los sueters
    public float startingMinSueterTime, startingMaxSueterTime; //intervalo de tiempo en el que pueden salir los sueters al empezar
    float minSueterTime, maxSueterTime; //intervalo de tiempo que varia a lo largo de la partida
    List<GameObject> suetersList = new List<GameObject>(); //lista de sueters
    List<GameObject> disabledSuetersList = new List<GameObject>(); //lista de sueters desactivados

    [Header("Enery Item Variables")]
    public Transform energyItemsParent;
    public float startingMinEnergyItemTime, startingMaxEnergyItemTime;
    float minEnergyItemTime, maxEnergyItemTime;
    List<GameObject> energyItemsList = new List<GameObject>();
    List<GameObject> disabledEnergyItemsList = new List<GameObject>();

    [Header("Telephone Variables")]
    public GameObject telephone;
    public float startingMinPhoneTime, startingMaxPhoneTime;
    float minPhoneTime, maxPhoneTime;

    [Header("TV Variables")]
    public GameObject television;
    public Material offMaterial, onMaterial;
    public float startingMinTVTime, startingMaxTVTime;
    float minTVTime, maxTVTime;

    [HideInInspector] public float currentTime;
    [HideInInspector] public int score, highScore;

    [HideInInspector] public bool isPhoneRinging = false, isTVOn = false;
    int lastPos = -1;
    [HideInInspector] public bool stopGame = false;

    SaveManager saveManager;

    void Start()
    {
        Instance = this;

        saveManager = GetComponent<SaveManager>();

        saveManager.Load();

        currentTime = initialTime;

        minSueterTime = startingMinSueterTime;
        maxSueterTime = startingMaxSueterTime;

        minEnergyItemTime = startingMinEnergyItemTime;
        maxEnergyItemTime = startingMaxEnergyItemTime;

        minPhoneTime = startingMinPhoneTime;
        maxPhoneTime = startingMaxPhoneTime;

        minTVTime = startingMinTVTime;
        maxTVTime = startingMaxTVTime;

        foreach (Transform s in suetersParent)
        {
            suetersList.Add(s.gameObject);
            DisableSueter(s.gameObject);
        }

        foreach (Transform e in energyItemsParent)
        {
            energyItemsList.Add(e.gameObject);
            DisableEnergyItem(e.gameObject);
        }

        int rdm = Random.Range(0, disabledSuetersList.Count);
        disabledSuetersList[rdm].SetActive(true);
        disabledSuetersList.RemoveAt(rdm);

        SetText(highScoreText, highScore);

        StartCoroutine(EnableSueter());
        // StartCoroutine(EnableEnergyItem());
        StartCoroutine(EnableTelephone());
        StartCoroutine(EnableTV());
    }

    void Update()
    {
        //print(minSueterTime + " min");
        //print(maxSueterTime + " max");
        if (!stopGame)
        {
            if (!isTVOn)
                currentTime -= Time.deltaTime;
            else
                currentTime -= Time.deltaTime * 2;

            if (currentTime <= 0)
                GameOver();

            SetText(timeText, (int)currentTime);
            SetText(scoreText, score);

            //RESET SAVING
            if (Input.GetKeyDown(KeyCode.O))
            {
                saveManager.ResetSaving();
            }

        }
    }

    void SetText(Text t, float n)
    {
        t.text = n.ToString();
    }

    public void AddCurrentTime(float time)
    {
        currentTime += time;
        SetText(timeText, (int)currentTime);
    }

    IEnumerator EnableSueter()
    {
        yield return new WaitForSeconds(Random.Range(minSueterTime, maxSueterTime));
        if (disabledSuetersList.Count > 0)
        {
            int rdm = Random.Range(0, disabledSuetersList.Count);

            while (rdm == lastPos)
            {
                rdm = Random.Range(0, disabledSuetersList.Count);
            }

            lastPos = rdm;

            disabledSuetersList[rdm].SetActive(true);
            disabledSuetersList.RemoveAt(rdm);
            if (minSueterTime >= 2 && maxSueterTime >= 3)
            {
                minSueterTime -= minSueterTime * enabledSueterTime / 100;
                maxSueterTime -= maxSueterTime * enabledSueterTime / 100;
            }

            StartCoroutine(EnableSueter());
        }

        else
            GameOver();
    }

    public void DisableSueter(GameObject sueter)
    {
        disabledSuetersList.Add(sueter);
        sueter.SetActive(false);
    }

    IEnumerator EnableEnergyItem()
    {
        yield return new WaitForSeconds(Random.Range(minEnergyItemTime, maxEnergyItemTime));
        if (disabledEnergyItemsList.Count > 0 && !player.hasEnergy)
        {
            int rdm = Random.Range(0, disabledEnergyItemsList.Count);
            disabledEnergyItemsList[rdm].SetActive(true);
            disabledEnergyItemsList.RemoveAt(rdm);

        }
        StartCoroutine(EnableEnergyItem());
    }

    public void DisableEnergyItem(GameObject item)
    {
        disabledEnergyItemsList.Add(item);
        item.SetActive(false);
    }

    IEnumerator EnableTelephone()
    {
        yield return new WaitForSeconds(Random.Range(minPhoneTime, maxPhoneTime));
        //particulas
        //sonido
        isPhoneRinging = true;
        print("RING RING");
        StartCoroutine(EnableTelephone());
    }

    public void DisableTelephone()
    {
        if (isPhoneRinging)
        {
            //particulas
            //sonido
            isPhoneRinging = false;
            telephone.GetComponent<DisableMe>().timer = 0;
            print("TELF DESACTIVADO");

        }
    }
    IEnumerator EnableTV()
    {
        yield return new WaitForSeconds(Random.Range(minTVTime, maxTVTime));
        if (!isTVOn)
        {
            //cambiar textura
            //sonido
            isTVOn = true;
            print("TV ON");
        }
        StartCoroutine(EnableTV());
    }

    public void DisableTV()
    {
        print("TV OFF");
        //cambiar textura
        //sonido
        isTVOn = false;
    }

    public void DisableItem(string type, GameObject item)
    {
        switch (type)
        {
            case "TV":
                DisableTV();
                break;
            case "Telephone":
                DisableTelephone();
                break;
            case "EnergyDrink":
                DisableEnergyItem(item);
                break;
        }
    }

    void GameOver()
    {
        saveManager.Save();
        gameOverPanel.SetActive(true);
        stopGame = true;
    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
