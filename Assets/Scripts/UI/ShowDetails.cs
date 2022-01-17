using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowDetails : MonoBehaviour
{
    private Vector2 _mousePositionVector2;
    private RaycastHit2D _raycast;
    private GameObject _gameObject;
    private Agent _agentClass; 
    
    private TextMeshProUGUI _namePlaceholder;
    private TextMeshProUGUI _healthPlaceholder;
    private TextMeshProUGUI _capacityPlaceholder;
    private TextMeshProUGUI _materialPlaceholder;

    private GameObject _buildingHolder;
    private GameObject _agentHolder;
    
    // Start is called before the first frame update
    private void Start()
    {
        // Define all TextMeshProUGUI placeholders
        _namePlaceholder = GameObject.Find("NamePlaceholder").GetComponent<TextMeshProUGUI>();
        _healthPlaceholder = GameObject.Find("HealthPlaceholder").GetComponent<TextMeshProUGUI>();
        _materialPlaceholder = GameObject.Find("MaterialPlaceholder").GetComponent<TextMeshProUGUI>();
        _capacityPlaceholder = GameObject.Find("CapacityPlaceholder").GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    private void Update()
    {
        // Get current mouse position form GameManager
        _mousePositionVector2 = GameManager.mousePositionVector2;

        // Check if left mouse button is being pressed
        if (Input.GetMouseButtonDown(0))
        {
            // Create a raycast
            CreateRaycast();
            
            // Check if raycast has hit an object or has hit the background object specifically
            // Empty the TextMeshProUGUI objects if so
            if (_raycast.collider == null || _gameObject.name == "Background")
            {
                SetEmptyDetail();
                return;
            }

            // Check if the object has been placed. When spawning a building, the object has to be placed first before the user can inspect the building.
            if (_gameObject.GetComponent<Building>().placed == false)
            {
                return;
            }
            
            // Check if _gameObject has Agent tag and has Agent Class
            if (_gameObject.CompareTag("Agent") && _gameObject.GetComponent<Agent>())
            {
                SetAgentDetail();
                return;
            }
        
            // Check if _gameObject has Building tag and has Building Class
            if (_gameObject.CompareTag("Building") && _gameObject.GetComponent<Building>())
            {
                SetBuildingDetail();
                return;
            }
        }

        // Check if right mouse button is being pressed
        if (Input.GetMouseButtonDown(1))
        {
            SetEmptyDetail();
        }
        
    }

    // Create a raycast
    private void CreateRaycast()
    {
        // Check if raycast hit gameObject
        if (Physics2D.Raycast(_mousePositionVector2, Vector2.zero))
        {
            // Put raycast info in private _raycast
            _raycast = Physics2D.Raycast(_mousePositionVector2, Vector2.zero);
            

            // Put gameObject that was hit by raycast in private _gameObject
            _gameObject = _raycast.collider.gameObject;
        }
    }

    private void SetEmptyDetail()
    {
        _namePlaceholder.text = "";
        _healthPlaceholder.text = "";
        _capacityPlaceholder.text = "";
        _materialPlaceholder.text = "";
    }

    private void SetAgentDetail()
    {
        
    }
    
    // Change TextMeshProUGUI to match values of _gameObject and _gameObject Building Class
    private void SetBuildingDetail()
    {   
        _namePlaceholder.text = _gameObject.GetComponent<Building>().buildingName;
        _healthPlaceholder.text = _gameObject.GetComponent<Building>().health.ToString();
        _materialPlaceholder.text = _gameObject.GetComponent<Building>().material;
        _capacityPlaceholder.text = _gameObject.GetComponent<Building>().capacity.ToString();
    }
}