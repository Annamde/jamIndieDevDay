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

    [Header("Sueter Variables")]
    public Transform suetersParent; //empty que contiene todos los sueters
    public float startingMinSueterTime, startingMaxSueterTime; //intervalo de tiempo en el que pueden salir los sueters al empezar
    float minSueterTime, maxSueterTime; //intervalo de tiempo que varia a lo largo de la partida
    List<GameObject> suetersList = new List<GameObject>(); //lista de sueters
    List<GameObject> disabledSuetersList = new List<GameObject>(); //lista de sueters desactivados

    [Header("Item Variables")]
    public Transform itemsParent;
    public float startingMinItemTime, startingMaxItemTime;
    float minItemTime, maxItemTime;
    List<GameObject> itemsList = new List<GameObject>();
    List<GameObject> disabledItemsList = new List<GameObject>();

    public float currentTime;
    static public int score;

    void Start()
    {
        Instance = this;

        currentTime = initialTime;

        minSueterTime = startingMinSueterTime;
        maxSueterTime = startingMaxSueterTime;

        minItemTime = startingMinItemTime;
        maxItemTime = startingMaxItemTime;

        foreach (Transform s in suetersParent)
        {
            suetersList.Add(s.gameObject);
            DisableSueter(s.gameObject);
        }

        foreach (Transform i in itemsParent)
        {
            itemsList.Add(i.gameObject);
            DisableItem(i.gameObject);
        }

        int rdm = Random.Range(0, disabledSuetersList.Count);
        disabledSuetersList[rdm].SetActive(true);
        disabledSuetersList.RemoveAt(rdm);

        StartCoroutine(EnableSueter());
        StartCoroutine(EnableItem());
    }

    void Update()
    {
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

            minSueterTime -= minSueterTime * 10 / 100;
            maxSueterTime -= maxSueterTime * 10/100;

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

    IEnumerator EnableItem()
    {
        yield return new WaitForSeconds(Random.Range(minItemTime, maxItemTime));
        if (disabledItemsList.Count > 0)
        {
            int rdm = Random.Range(0, disabledItemsList.Count);
            disabledItemsList[rdm].SetActive(true);
            disabledItemsList.RemoveAt(rdm);

            StartCoroutine(EnableItem());
        }
    }

    public void DisableItem(GameObject item)
    {
        disabledItemsList.Add(item);
        item.SetActive(false);
    }

    void GameOver()
    {
        print("Game Over");
    }
}
