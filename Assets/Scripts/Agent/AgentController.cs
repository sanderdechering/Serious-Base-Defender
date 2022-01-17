using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    private GameObject[] Allies;
    Medic[] Medics;
    private GameObject closestMedic;
    private float medicDistance;
    public GameObject Hospital;
    public float allyHealth;
    public float allyMaxHealth;
    public float speed;
    public int healTimer;
    private int allycounter = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Medics = GameObject.FindObjectsOfType<Medic>();
        Hospital = GameObject.Find("Hospital");
        Allies = GameObject.FindGameObjectsWithTag("Agent");
    }

    // Update is called once per frame
    void Update()
    {
        getWounded();
    }


    private void getWounded()
    {

        for (int allyCount = 0; allyCount < Allies.Length; allyCount++)
        {
            //Debug.Log(allyCount);
           // allycounter++;
            allyHealth = Allies[allyCount].GetComponent<Agent>().health;
            allyMaxHealth = Allies[allyCount].GetComponent<Agent>().maxHealth;
            if (allyHealth < allyMaxHealth)
            {
                if(Allies[allyCount].GetComponent<Agent>().gettingHealed == false) 
                { 
                    getClosestMedic(allyCount);
                }
            }
        }
    }
    
    private void getClosestMedic(int allyCount)
    {
        //Debug.Log(allyCount);
        medicDistance = Vector2.Distance(Medics[0].transform.position, Allies[0].transform.position);
        
        foreach (var medic in Medics)
        {
            if (Vector2.Distance(medic.transform.position, Allies[allyCount].transform.position) < medicDistance)
            {
                closestMedic = medic.gameObject;
                medicDistance = Vector2.Distance(medic.transform.position, Allies[allyCount].transform.position);
            }
        }
        
        closestMedic.transform.position = Vector2.MoveTowards(closestMedic.transform.position, Allies[allyCount].transform.position, speed * Time.deltaTime);

        if (Vector2.Distance(closestMedic.transform.position, Allies[allyCount].transform.position) < 0.2f)
        {
            Allies[allyCount].GetComponent<Agent>().gettingHealed = true;

            closestMedic.transform.position = Vector2.MoveTowards(closestMedic.transform.position, Hospital.transform.position, speed * Time.deltaTime);
            Allies[0].transform.position = Vector2.MoveTowards(Allies[allyCount].transform.position, new Vector2(0, 0), speed * Time.deltaTime);

            //Allies[allyCount].transform.position = new Vector2(0, 0);
            
            if (Vector2.Distance(closestMedic.transform.position, Hospital.transform.position) < 0.2f)
            {
                if(Vector2.Distance(Allies[allyCount].transform.position, Hospital.transform.position) < 0.2f)
                {
                    //healAgent(allyCount);
                    Debug.Log("bij het ziekenhuis");

                }
            }
        }
    }
    private void healAgent(int allyCount)
    {
        allyHealth = Allies[allyCount].GetComponent<Agent>().health;
        while (allyHealth < 100)
        {
            healTimer++;
            if (healTimer % 50 == 0)
            {
                allyHealth = Allies[allyCount].GetComponent<Agent>().health;
                allyHealth += 10;
            }
        }
    }
}
