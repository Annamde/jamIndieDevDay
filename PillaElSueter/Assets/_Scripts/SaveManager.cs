using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public void Load()
    {
        GameManager.Instance.highScore = PlayerPrefs.GetInt("High Score");
    }

    public void Save()
    {
        if(GameManager.Instance.score >= PlayerPrefs.GetInt("High Score"))
        {
            PlayerPrefs.SetInt("High Score", GameManager.Instance.score);
            PlayerPrefs.Save();
        }
    }

    public void ResetSaving()
    {
        PlayerPrefs.DeleteAll();
        Save();
    }
}
