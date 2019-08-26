using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public PlayerScript player;

    [Header("HUD")]
    public Text timeText;
    public Text scoreText;

    [Header("Constants")]
    public float initialTime;

    float currentTime;
    static public int score;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = initialTime; 
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        SetText(timeText, (int)currentTime);
        SetText(scoreText, score);
    }

    void SetText(Text t, float n)
    {
        t.text = n.ToString();
    }
}
