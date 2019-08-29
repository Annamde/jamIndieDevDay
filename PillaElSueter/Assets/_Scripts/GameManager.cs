using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }

    [Header("Player")]
    public PlayerScript player;

    [Header("HUD")]
    public Text timeText;
    public Text scoreText;

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
    public float startingMinEnergyItemTime, startingEnergyMaxItemTime;
    float minEnergyItemTime, maxEnergyItemTime;
    List<GameObject> energyItemsList = new List<GameObject>();
    List<GameObject> disabledEnergyItemsList = new List<GameObject>();

    [HideInInspector] public float currentTime, score;

    void Start()
    {
        Instance = this;

        currentTime = initialTime;

        minSueterTime = startingMinSueterTime;
        maxSueterTime = startingMaxSueterTime;

        minEnergyItemTime = startingMinEnergyItemTime;
        maxEnergyItemTime = startingEnergyMaxItemTime;

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

        StartCoroutine(EnableSueter());
        StartCoroutine(EnableEnergyItem());
    }

    void Update()
    {
        print(minSueterTime + " min");
        print(maxSueterTime + " max");


        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
            GameOver();

        SetText(timeText, (int)currentTime);
        SetText(scoreText, score);
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
            disabledSuetersList[rdm].SetActive(true);
            disabledSuetersList.RemoveAt(rdm);
            if (minSueterTime >= 1 && maxSueterTime >= 3)
            {
                minSueterTime -= minSueterTime * enabledSueterTime / 100;
                maxSueterTime -= maxSueterTime * enabledSueterTime / 100;
            }

            StartCoroutine(EnableSueter());
        }

        else
            StartCoroutine(EnableSueter());
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

    public void EnableTelephone()
    {

    }

    public void DisableTelephone()
    {

    }
    public void EnableTV()
    {

    }

    public void DisableTV()
    {

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
        print("Game Over");
    }
}
