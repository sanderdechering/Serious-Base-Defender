using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerController : MonoBehaviour
{
    
    private static int _powerCapacity;
    private static int _powerUsed;
    public static bool powerOutage;
    
    private static TextMeshProUGUI _powerCapacityText;
    private static TextMeshProUGUI _powerUsedText;
    
    // Start is called before the first frame update
    void Start()
    {
        _powerUsedText = GameObject.Find("PowerInUse").GetComponent<TextMeshProUGUI>();
        _powerCapacityText = GameObject.Find("PowerCapacity").GetComponent<TextMeshProUGUI>();
        
        GetPower();
        UsePower();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Get all generators and add their power to the total power
    public static void GetPower()
    {
        _powerCapacity = 0;
 
        foreach (Generator generator in FindObjectsOfType<Generator>())
        {
            _powerCapacity += generator.powerGenerated;
        }

        powerOutage = _powerUsed > _powerCapacity;
        
        _powerCapacityText.text = _powerCapacity.ToString();
    }
    
    // Get all generators and subtract their power from the total power
    public static void UsePower()
    {
        _powerUsed = 0;
        
        foreach (Building building in FindObjectsOfType<Building>())
        {
            _powerUsed += building.powerUsed;
        }
        
        powerOutage = _powerUsed > _powerCapacity;
        
        _powerUsedText.text = _powerUsed.ToString();
    }
}
