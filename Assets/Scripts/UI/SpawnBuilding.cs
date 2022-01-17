using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SpawnBuilding : MonoBehaviour
{
    [SerializeField] private GameObject buildingPrefab;
    private bool _spawned;
    private GameObject _building;
    private Vector3 _mouseWorldPosition;
    public GameObject agent;
    
    private int _money;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(SpawnAtMousePos);
    }

    // Update is called once per frame
    void Update()
    {
        _mouseWorldPosition = GameManager.mousePositionVector2;

        _mouseWorldPosition.z = 0f;

        if (_spawned)
        {
            FollowAtMousePos();
            CheckMouseClick();
        }
    }

    // Spawn building at current mouse position
    void SpawnAtMousePos()
    {
        _building = Instantiate(buildingPrefab, _mouseWorldPosition, Quaternion.identity);
        gameObject.GetComponent<Button>().interactable = false;
        _spawned = true;
    }

    // Let building object follow cursor
    void FollowAtMousePos()
    {
        _building.transform.position = _mouseWorldPosition;
    }

    // Check if mouse buttons are clicked
    void CheckMouseClick()
    {
        // Left mouse button will place building
        if (Input.GetMouseButtonDown(0))
        {
            _money = MoneyController.GetMoney();
            if (_money >= _building.GetComponent<Building>().Cost) 
            {

                if (_building.GetComponent<Generator>())
                {
                    _spawned = false;
                    _building.GetComponent<Generator>().placed = true;
                    _building.GetComponent<Building>().checkOverlap();
                    
                    // Call function SubtractMoney from Moneycontroller, passing in cost of building
                    MoneyController.SubtractMoney(_building.GetComponent<Building>().Cost);
                    
                    // Spawn x amount of agents. X = capacity of building
                    for (int i = 0; i < _building.GetComponent<Generator>().staffCapacity; i++)
                    {
                        _building.GetComponent<Generator>().SpawnAgent(agent);
                    }
                    
                    PowerController.GetPower();
                }

                if (_building.GetComponent<Building>())
                {
                    _spawned = false;
                    _building.GetComponent<Building>().placed = true;
                    _building.GetComponent<Building>().checkOverlap();

                    MoneyController.SubtractMoney(_building.GetComponent<Building>().Cost);
                    
                    // Spawn x amount of agents. X = capacity of building
                    for (int i = 0; i < _building.GetComponent<Building>().staffCapacity; i++)
                    {
                        _building.GetComponent<Building>().SpawnAgent(agent);
                    }
                    
                    PowerController.UsePower();
                }
                
                gameObject.GetComponent<Button>().interactable = true;
            }
        }

        // Right mouse button will place building and destroy it
        if (Input.GetMouseButtonDown(1))
        {
            MoneyController.AddMoney(_building.GetComponent<Building>().Cost);
            _spawned = false;
            gameObject.GetComponent<Button>().interactable = true;
            Destroy(_building);
        }
    }
}