using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    // Variables used in the game
    public string buildingName;
    public float health;
    public int staffCapacity;
    public int capacity;
    public float resilience;
    public string material;
    public bool placed;
    public int powerUsed;
    public int Cost = 100;

    // The armor upgrade changes the damage calculation in TakeDamage(), reducing all incoming damage by 25%.
    public bool armorUpgrade;

    // Sprites and Rendering
    public Sprite defaultSprite;
    public Sprite destroyedSprite;

    public List<Agent> agentsInside = new List<Agent>();

    // Start is called before the first frame update
    public void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {

    }

    // Take damage. Takes as input the base damage and a modifier based on the weapon's destructiveness.
    // Also takes resilience of the building and armor upgrade into account.
    public void TakeDamage(float dmg, float destructiveness)
    {

        float totaldmg;
        if (this.armorUpgrade)
        {
            totaldmg = dmg * destructiveness * resilience * 0.75f;
        }
        else
        {
            totaldmg = dmg * destructiveness * resilience;
        }
        
        health -= totaldmg;

        if (health <= 0)
        {
            Destroy();
        }
    }

    protected virtual void Destroy()
    {
        this.GetComponent<SpriteRenderer>().sprite = destroyedSprite;
        this.capacity = 0;
        this.health = 0;

        // Deal random damage to each agent inside.
        for (int i = 0; i < this.agentsInside.Count; i++)
        {
            if (UnityEngine.Random.Range(0.0f, 10.0f) < 5.0f)
            {
                agentsInside[i].TakeDamage(100.0f);
            }
            else
                agentsInside[i].TakeDamage(UnityEngine.Random.Range(0.0f, 100.0f) + 1.0f);
        }
    }

    // Add an agent to the building.
    public bool Enter(Agent agent)
    {
        // If the building is not full, place agent inside and return true.
        if (this.agentsInside.Count < this.capacity)
        {
            this.agentsInside.Add(agent);
            agent.insideBuilding = true;
            return true;
        }
        else
            return false;
    }


    // Used to respawn agents when a building is placed on top of them.
    public void checkOverlap()
    {

        var collider = this.GetComponent<BoxCollider2D>();
        Bounds boxBounds = collider.bounds;
        Vector2 topRight = new Vector2(boxBounds.center.x + boxBounds.extents.x, boxBounds.center.y + boxBounds.extents.y);
        Vector2 bottomLeft = new Vector2(boxBounds.center.x - boxBounds.extents.x, boxBounds.center.y - boxBounds.extents.y);

        Collider2D[] colliders = Physics2D.OverlapAreaAll(bottomLeft, topRight);
        colliders = Array.FindAll(colliders, x => x.tag == "Agent");

        // For each agent that was found, respawn the agent.
        for (int i = 0; i < colliders.Length; i++)
        {
            var agentAgent = colliders[i].gameObject.GetComponent<Agent>();
            agentAgent.Respawn();
        }
    }

    // Spawn agent around the building.
    public void SpawnAgent(GameObject agentPrefab)
    {
        var collider = this.GetComponent<BoxCollider2D>();
        var agent = Instantiate(agentPrefab, transform.position, Quaternion.identity);
        var agentTransform = agent.GetComponent<Transform>();
        var agentAgent = agent.GetComponent<Agent>();
        var agentCollider = agent.GetComponent<Rigidbody2D>();

        agentAgent.SetSpawnBuilding(this);

        agentTransform.position = new Vector3(transform.position.x + UnityEngine.Random.Range(-100.0f, 100.0f), transform.position.y + UnityEngine.Random.Range(-100.0f, 100.0f), 0);

        // Get all Colliders within range and then filter them to just buildings.
        Collider2D[] colliders = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - 100, transform.position.y - 100), new Vector2(transform.position.x + 100, transform.position.y + 100));
        colliders = Array.FindAll(colliders, x => x.tag == "Building");


        // Try to move an agent to a valid position, max 100 tries.
        var checker = true;
        var tries = 0;
        while (checker == true && tries < 100)
        {
            checker = false;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].bounds.Contains(agentTransform.position))
                {
                    checker = true;
                }
            }
            if (checker == true)
            {
                agentTransform.position = new Vector3(transform.position.x + UnityEngine.Random.Range(-100.0f, 100.0f), transform.position.y + UnityEngine.Random.Range(-100.0f, 100.0f), 0);
                tries++;
            }

            if (tries == 1000)
            {
                // Failed to spawn. Destroy the agent and move on.
                Destroy(agent);
            }
        }
    }

    // Change the building's material and resilience.
    public void ChangeMaterial(string material)
    {
        switch (material)
        {
            case "Material 1":
                this.resilience = 1.2f;
                break;
            case "Material 2":
                this.resilience = 1.0f;
                break;
            case "Material 3":
                this.resilience = 0.8f;
                break;
            case "Material 4":
                this.resilience = 0.6f;
                break;
            case "Material 5":
                this.resilience = 0.4f;
                break;
            case "Material 6":
                this.resilience = 0.2f;
                break;
        }
    }
}
