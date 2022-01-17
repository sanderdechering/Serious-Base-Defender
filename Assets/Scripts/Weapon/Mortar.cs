using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Mortar : MonoBehaviour
{

    public float radius = 100;
    public int damage = 100;
    
    private GameObject _background;
    
    private Collider2D[] _objectsInRadius;
    
    private float _distanceFromImpact;
    private float _effectiveness;
    private float _effectiveDamage;
    public float totalBuildingDamage;
    public int buildingCount;
    public int agentCount;
    public float totalAgentDamage;
    // Start is called before the first frame update
    void Start()
    {
        SetLocation();
        DetectObjectsInRadius();
        CalculateDamage();
        
        //DestroySelf();
    }

    // Set the location of the mortar
    private void SetLocation()
    {
        _background = GameObject.Find("Background");
       
        transform.position = new Vector2(UnityEngine.Random.Range(0, _background.transform.position.x), UnityEngine.Random.Range(0, _background.transform.position.y));
    }

    private void ToggleImpactObject()
    {
        transform.Find("impact").gameObject.SetActive(!transform.Find("impact").gameObject.activeSelf);
    }
    
    private void DetectObjectsInRadius()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        _objectsInRadius = Array.FindAll(colliders, x => !x.CompareTag("Background") );
    }

    private void CalculateDamage()
    {
        
        for (int objects = 0; objects < _objectsInRadius.Length; objects++)
        {
            Debug.Log("Object: " + _objectsInRadius[objects].gameObject.name);
            
            _distanceFromImpact = Vector2.Distance(transform.position, _objectsInRadius[objects].gameObject.transform.position);
            Debug.Log("_distanceFromImpact "+_distanceFromImpact);

            _effectiveness = 1 - (_distanceFromImpact / radius);
            Debug.Log("_effectiveness "+_effectiveness);
            if(_effectiveness < 0)
            {
                _effectiveness = 0;
            }
            _effectiveDamage = damage * _effectiveness;
            Debug.Log("_effectiveDamage "+_effectiveDamage);

            DealDamage(_objectsInRadius[objects].gameObject);
            if(_objectsInRadius[objects].GetComponents<Generator>().Length > 0 || _objectsInRadius[objects].GetComponents<Building>().Length > 0)
            {
                if(_effectiveDamage > 0)
                {
                    buildingCount++;
                    Debug.Log(buildingCount);
                    totalBuildingDamage += _effectiveDamage;
                    Debug.Log(totalBuildingDamage);
                }
            }
            if (_objectsInRadius[objects].GetComponents<Agent>().Length > 0 || _objectsInRadius[objects].GetComponents<Medic>().Length > 0)
            {
                if(_effectiveDamage > 0) 
                { 
                agentCount++;
                Debug.Log(agentCount);
                totalAgentDamage += _effectiveDamage;
                Debug.Log(totalAgentDamage);
                }
            }
        }
        
        
    }

    private void DealDamage(GameObject gameObject)
    {
        if (gameObject.gameObject.CompareTag("Agent"))
        {
            gameObject.GetComponent<Agent>().TakeDamage(_effectiveDamage);
        }
        if (gameObject.CompareTag("Building"))
        {
            gameObject.GetComponent<Building>().TakeDamage(_effectiveDamage, 1);
        }
        if(gameObject.GetComponent<Generator>())
        {
            gameObject.GetComponent<Generator>().TakeDamage(_effectiveDamage, 1);
        }

    }
    
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}