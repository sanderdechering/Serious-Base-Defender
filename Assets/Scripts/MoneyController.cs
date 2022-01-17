using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
    private static int _money;
    private static TextMeshProUGUI _MoneyText;
    
    // Start is called before the first frame update
    void Start()
    {
        _money = 10000;
        _MoneyText = GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _MoneyText.text = "$" + _money;
    }
    
    public static void AddMoney(int amount)
    {
        _money += amount;
    }
    
    public static void SubtractMoney(int amount)
    {
        _money -= amount;
    }
    
    public static int GetMoney()
    {
        return _money;
    }
}
