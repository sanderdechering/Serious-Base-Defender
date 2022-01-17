using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Building
{
    public int powerGenerated;
    
    // Start is called before the first frame update
    new void Start()
    {
        powerGenerated = 50;
    }

    // Update is called once per frame
    new void Update()
    {
        
    }
    
    protected override void Destroy()
    {
        this.GetComponent<SpriteRenderer>().sprite = destroyedSprite;
        capacity = 0;
        health = 0;
        powerGenerated = 0;

        PowerController.GetPower();
        
        foreach (var t in agentsInside)
        {
            if (Random.Range(0.0f, 10.0f) < 5.0f)
            {
                t.TakeDamage(100.0f);
            }
            else
                t.TakeDamage(Random.Range(0.0f, 100.0f) + 1.0f);
        }
    }
}