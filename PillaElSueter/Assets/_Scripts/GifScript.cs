using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GifScript : MonoBehaviour
{

    public Texture2D[] frames;
    public float framesPorSegundo = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float index = Time.time * framesPorSegundo;
        index = index % frames.Length;
        GetComponent<Renderer>().material.mainTexture = frames[(int)index];
    }
}
