using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMe : MonoBehaviour
{
    public float timeToDisable;
    float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if(timer>timeToDisable)
        {
            timer = 0;
            GameManager.Instance.DisableItem(this.gameObject);
        }
    }
}
