using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private float _gameTime;
    private float _mortarImpactTime;
    private Mortar _mortar;
    
    private Camera _camera;
    private static Vector3 _mousePositionVector3;
    public static Vector2 mousePositionVector2;
    
    public GameObject mortarPrefab;

    private float _timeOfStartSimulation;
    public static float currentGameTime;

    public static bool missionStarted;
    private static GameObject _startMissionButton;
    private static GameObject _startMissionButtonText;
    private GameObject _briefingPanel;
    
    public static bool simulationStarted;
    private static GameObject _startSimulationButton;
    private static GameObject _startSimulationButtonText;

    private static GameObject _endSimulationButton;
    private static GameObject _endSimulationButtonText;
    private static TextMeshProUGUI _briefingPanelTitleText;
    private static TextMeshProUGUI _briefingPanelText;
    private GameObject MortarObject;
    private float DamageDealtToBuildings;
    private int BuildingsHit;
    private float DamageDealtToAgent;
    private int AgentsHit;
    public static bool missionEnded;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        currentGameTime = 0;
        
        simulationStarted = false;
        _startSimulationButton = GameObject.Find("StartSimulationButton");
        _startSimulationButtonText = GameObject.Find("StartSimulationButtonText");
        _endSimulationButton = GameObject.Find("EndSimulationButton");
        _endSimulationButtonText = GameObject.Find("EndSimulationButtonText");
        _startMissionButton = GameObject.Find("StartMissionButton");

    }

    // Update is called once per frame
    void Update()
    {
        currentGameTime += Time.deltaTime;

        GetMousePosition();
        SimulationReadyCheck();
        
        if (!simulationStarted)
        {
            return;
        }
        if(_mortarImpactTime < currentGameTime)
        {
            endSimulationReadyCheck();
        }
        // Start alarm
        
        SpawnMortar();
    }
    
    // Get current mouse position
    private void GetMousePosition()
    {
        _mousePositionVector3 = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePositionVector2 = new Vector2(_mousePositionVector3.x, _mousePositionVector3.y);
    }

    private void SimulationReadyCheck()
    {
        // Check if there is a power outage
        if (PowerController.powerOutage)
        {
            Debug.Log(PowerController.powerOutage);
            _startSimulationButton.GetComponent<Button>().interactable = false;
            _startSimulationButtonText.GetComponent<TextMeshProUGUI>().text = "Power Outage";
            _startSimulationButton.GetComponent<Image>().color = new Color(255, 0, 0);
            return;
        }
        
        _startSimulationButton.GetComponent<Button>().interactable = true;
        _startSimulationButtonText.GetComponent<TextMeshProUGUI>().text = "Start simulation";
        _startSimulationButton.GetComponent<Image>().color = new Color(0, 255, 0);
    }

    // Start mission; This removes the briefing panel
    public void StartMission()
    {
        missionStarted = true;
        _briefingPanel = GameObject.Find("Briefing");
        _briefingPanel.SetActive(false);
    }

    private void endSimulationReadyCheck()
    {
        
        _endSimulationButton.GetComponent<Button>().interactable = true;
        _endSimulationButtonText.GetComponent<TextMeshProUGUI>().text = "End simulation";
        _endSimulationButton.GetComponent<Image>().color = new Color(0, 255, 0);
        return;
    }
    public void endMission()
    {
        _briefingPanel.SetActive(true);
        _briefingPanelTitleText = GameObject.Find("BriefingTitle").GetComponent<TextMeshProUGUI>();
        _briefingPanelTitleText.text = "Debriefing";
        
        _startMissionButton.SetActive(false);
        
        MortarObject = GameObject.Find("Mortar(Clone)");
        BuildingsHit = MortarObject.GetComponent<Mortar>().buildingCount;
        DamageDealtToBuildings = MortarObject.GetComponent<Mortar>().totalBuildingDamage;
        DamageDealtToAgent = MortarObject.GetComponent<Mortar>().totalAgentDamage;
        AgentsHit = MortarObject.GetComponent<Mortar>().agentCount;
        
        _briefingPanelText = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
        _briefingPanelText.text = BuildingsHit + " Buildings have been hit for " + Math.Round(DamageDealtToBuildings) + " damage. \n" + AgentsHit + " Agents have been hit for " + Math.Round(DamageDealtToAgent) + " damage.";


        missionEnded = true;
    }
    // Start simulation;
    public void StartSimulation()
    {
       
        // check if simulation is already started
        if (simulationStarted)
        {
            return;
        }
        
        _timeOfStartSimulation = currentGameTime;
        simulationStarted = true;
        
        SetMortarImpactTime();
        
    }
    
    // Set impact time of mortar
    private void SetMortarImpactTime()
    {
        _mortarImpactTime = Random.Range(_timeOfStartSimulation + 5, _timeOfStartSimulation + 15);
    }

    // Spawn mortar
    private void SpawnMortar()
    {
        // Chek if mortar is ready to be spawned
        if (currentGameTime < _mortarImpactTime)
        {
            return;
        }

        // spawn mortar if not already spawned
        if (_mortar == null)
        {
            _mortar = Instantiate(mortarPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Mortar>();
        }
    }
}