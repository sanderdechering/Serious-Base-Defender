using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Agent : MonoBehaviour
{
    //agent variables
    public string agentName;
    public int health;
    public float maxHealth = 100;
    private float healthpercentage;
    public bool gettingHealed;
    public bool incapacitated;
    public Healthbar healthBar;
    private Building spawnBuilding;
    public bool insideBuilding;

    protected TaskController _taskController;
    // task variables
    protected Task currentTask;
    protected float taskTimeStarted;
    protected bool taskCompleted;

    protected Task taskObject;
    //overige variabele
    public float speed;
    public GameObject[] Building;

    
    // task variables
    
    
    
    // Start is called before the first frame update
    protected void Start()
    {
        //get taskcontroller component from camera
        _taskController = Camera.main.GetComponent<TaskController>();
        GetBuildings();

    }
    
    // Update is called once per frame
    protected void Update()
    {
        // check if simulation is running
        if (!GameManager.simulationStarted)
        {
            return; 
        }

        if (GameManager.missionEnded)
        {
            return;
        }
        
        incapacitated = health < 30;

        // If agent is incapacitated, stand still and do nothing
        if (incapacitated)
        {
            return;
        }
        
        // If agent is getting healed, stand still and do nothing
        if (gettingHealed)
        {
            return;
        }

        // If agents health is same or above 100, complete task and set health to 100
        if (health == 100)
        {
            if (taskObject == null)
            {
                getTask();
                return;
            }
            
            if (insideBuilding == false)
            {
                MoveTowardsTaskBuilding();
                return;
            }
            
            DoingTask();

            if (taskCompleted)
            {
                //destroy child object with the object name of _currentTask
                Destroy(gameObject.transform.Find(taskObject.name).gameObject);
                currentTask = null;
                taskObject = null;
                taskCompleted = false;
                insideBuilding = false;
                
                // agent will get a new task when task object is destroyed. no further actioned needed here
            }
        }

        // if agent is healed above 100, set health to 100
        if (health > 100)
        {
            health = 100; 
        }
        
    }

    
    protected void getTask()
    {
        // get task from taskcontroller
        currentTask = _taskController.GetTask(currentTask, gameObject);
        
        // if task is null, do nothing
        if (currentTask == null)
        {
            return;
        }
        
        // create taskobject and instantiate it on the agent
        taskObject = Instantiate(currentTask, transform.position, Quaternion.identity);
        taskObject.name = currentTask.name;
        taskObject.transform.parent = gameObject.transform;
    }
    

    protected void MoveTowardsTaskBuilding()
    {

        // if agent is inside building, set inside to true
        if (Vector3.Distance(transform.position, taskObject.buildingObject.transform.position) < 0.5f)
        {
            insideBuilding = true;
            return;
        }
        
        // if agent is outside building, set inside to false
        if (Vector3.Distance(transform.position, taskObject.buildingObject.transform.position) > 0.5f)
        { 
            transform.position = Vector3.MoveTowards(transform.position, taskObject.buildingObject.transform.position, speed * Time.deltaTime);
            insideBuilding = false;
        }
    }

    protected void DoingTask()
    {
        // if the timer has not started, set the timer to the current time
        if (taskTimeStarted == 0)
        {
            taskTimeStarted = GameManager.currentGameTime;
        }
        
        // if the current time is bigger than the time started plus the task time, complete the task
        if (GameManager.currentGameTime > taskTimeStarted + taskObject.duration)
        {
            taskTimeStarted = 0;
            taskCompleted = true;
        }

    }

    // get all gameobjects with the tag Building
    protected void GetBuildings()
    {
        Building = GameObject.FindGameObjectsWithTag("Building");
    }

    // set building where agent can spawn from
    public void SetSpawnBuilding(Building building)
    {
        spawnBuilding = building;
    }

    public Building GetSpawnBuilding()
    {
        return spawnBuilding;
    }
    
    
    public void Respawn()
    {
        spawnBuilding.SpawnAgent(gameObject);
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= (int) Mathf.Round(damage);
        healthBar.SetHealth(health);
    }
}
