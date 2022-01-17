using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TaskController : MonoBehaviour
{
   private Task _sleepingTask;
   private Task _eatingTask;
   private Task _hygieneTask;
   
   private Task _hospitalShiftTask;
   private Task _patrollingShiftTask;
   private Task _commandPostShiftTask;
   private Task _generetorShiftTask;
   
   private Task[] _tasks;
   private Task _task;
   
   private Building[] _buildings;
   private Building[] _buildingsToAssign;
   private Building _tempGo;
   
   private bool _busy;

   private Agent _agent;
   private Medic _medic;
   private int _x;
   private bool _agentNameExists;
   
   // Start is called before the first frame update
    void Start()
    {
        _x = 0;
        _busy = false;
        GetBuildings();
        SetTasks();
        _tasks = new Task[] {_sleepingTask, _eatingTask, _hygieneTask, _hospitalShiftTask, _commandPostShiftTask, _generetorShiftTask};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetTasks()
    { 
        _hygieneTask = GameObject.Find("Hygiene").GetComponent<Task>();
        _eatingTask = GameObject.Find("Eating").GetComponent<Task>();
        _sleepingTask = GameObject.Find("Sleeping").GetComponent<Task>();

        _hospitalShiftTask = GameObject.Find("hospitalShift").GetComponent<Task>();
        _commandPostShiftTask = GameObject.Find("commandPostShift").GetComponent<Task>();
        _generetorShiftTask = GameObject.Find("generatorShift").GetComponent<Task>();
    }
    
    // get all gameObjects with the Building component
    public void GetBuildings()
    {
        _buildings = FindObjectsOfType<Building>();
    }

    public Task GetTask(Task previousTask, GameObject go)
    {
        _x = 0;
        _buildingsToAssign = new Building[10];
        GetBuildings();
        // pick a random task from _tasks
        var task = _tasks[Random.Range(0, _tasks.Length)];

        //check if agent has an agent script
        if (go.GetComponent<Agent>())
        {
            _agent = go.GetComponent<Agent>();
        }
        
        // check if the agent has a medic script
        if (go.GetComponent<Medic>())
        {
            _medic = go.GetComponent<Medic>();
        }
        
        
        // if there is a previous task, pick a random task that is not the same as the previous task
        if (previousTask != null)
        {
            var randomTask = Random.Range(0, _tasks.Length);
    
            while (_tasks[randomTask].id == previousTask.id)
            {
                randomTask = Random.Range(0, _tasks.Length);
            }

            task = _tasks[randomTask];
        }
        
        // if the task is a shift task, check if the name of the agent is in agentNames inside task
        if (task.name == "hospitalShift" || task.name == "patrollingShift" || task.name == "commandPostShift" || task.name == "generatorShift")
        {
            // if the agent is a medic, check if the name is in the task agentName list
            if (_medic)
            {
                _agentNameExists = task.agentNames.Contains(_medic.agentName);
            }
            
            // if the agent is a agent, check if the name is in the task agentName list
            if (_agent)
            {
                _agentNameExists = task.agentNames.Contains(_agent.agentName);
            }

            if (!_agentNameExists)
            {
                return null;
            }
        }
        
        // check if building exists where the task is supposed to be done, then add 1 to x
        foreach (var building in _buildings)
        {
            if (building.buildingName == task.buildingName)
            {
                _x++;
            }
        }
        
        // make an building array with length of x, the amount of buildings that has the same name as the task
        _buildingsToAssign = new Building[_x];
        
        // if there are no buildings found, return null
        if (_x == 0)
        {
            return null;
        }
        
        // same check as above, but this time we check the array length, just to be sure
        if (_buildingsToAssign.Length == 0)
        {
            return null;
        }

        // make x 0 again, so we can add a building from at the beginning of the array
        _x = 0;
        
        foreach (var building in _buildings)
        {
            if (building.buildingName == task.buildingName)
            {
                // add the building to the array
                _buildingsToAssign[_x] = building;
                _x++;
            }
        }
        
        // if array is bigger than 0, pick a random building from the array and return it
        if (_buildingsToAssign.Length > 0)
        {
            task.buildingObject = _buildingsToAssign[Random.Range(0, _buildingsToAssign.Length)];
            return task;
        }
        return null;
    }

}
