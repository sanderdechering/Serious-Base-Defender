using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Medic : Agent
{
    private GameObject[] Allies;
    Medic[] Medics;
    private GameObject closestMedic;
    public bool allyStatus;
    public bool gettingHealedStatus;
    public float allyHealth;
    public float allyMaxHealth;
    public int healTimer;
    private float medicDistance;

    public int length;
    public int i = 1;
    public List<bool> tasksDone = new List<bool>();
    public GameObject Hospital;
    
    new void Start()
    {
        GetBuildings();

        Allies = GameObject.FindGameObjectsWithTag("Agent");
        _taskController = Camera.main.GetComponent<TaskController>();


    }
    
    private void previousUpdate()
    {
        GetBuildings();
        Hospital = GameObject.Find("Hospital");
        Allies = GameObject.FindGameObjectsWithTag("Agent");
            
        for (int allyCount = 0; allyCount < Allies.Length; allyCount++)
        {
            allyHealth = Allies[allyCount].GetComponent<Agent>().health;
            allyMaxHealth = Allies[allyCount].GetComponent<Agent>().maxHealth;
            if (allyHealth < allyMaxHealth)
            {

            }
        }
    }

}
