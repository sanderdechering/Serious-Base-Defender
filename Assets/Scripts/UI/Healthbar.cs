using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{

    public float maxHealth;
    public float currentHealth;
    public Gradient gradient;
    public SpriteRenderer fill;
    private Transform bar;
    
    private void Start()
    {
        bar = transform.Find("Bar");
    }

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
        currentHealth = health;
        
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        currentHealth = health;
        
        if (currentHealth > 0 || currentHealth == 0)
        {
            fill.color = gradient.Evaluate(currentHealth/100);
            bar.localScale = new Vector3(currentHealth/100, 1f);
            
        }
    }

}