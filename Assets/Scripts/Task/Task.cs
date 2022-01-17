using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Task : MonoBehaviour
{
    public int id;
    public string taskName;
    public int duration;
    public string buildingName;
    public Building buildingObject;
    public string[] agentNames;
    private string _error;
}
