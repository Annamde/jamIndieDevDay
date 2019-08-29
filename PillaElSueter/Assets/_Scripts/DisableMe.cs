using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMe : MonoBehaviour
{
    public enum ItemType
    {
        TV,
        Telephone,
        EnergyDrink
    }

    public ItemType myType;

    public float timeToDisable;
     public float timer = 0;

    void Update()
    {
        if (myType != ItemType.Telephone || GameManager.Instance.isPhoneRinging)
        {

            timer += Time.deltaTime;
            if (timer > timeToDisable)
            {
                timer = 0;
                GameManager.Instance.DisableItem(myType.ToString(), this.gameObject);
            }
        }
    }
}
