﻿using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

/// <summary>
/// This class provides an interface for all the actions the AI agent can take in the world
/// These actions are detailed in the brief and the AI.cs file. Actions that require a target
/// or an object take a GameObject as a parameter. The GameObject should be aqcuired from the
/// AI agents senses
/// </summary>
public class AgentActions : MonoBehaviour
{
    // The name of the animation for the sword swing
    private const string AttackAnimationTrigger = "Attack";

    protected AgentData _agentData;
    // Gives access to the agent senses
    protected Sensing _agentSenses;
    // gives access to the agents inventory
    protected InventoryController _agentInventory;

    private UnityEngine.AI.NavMeshAgent _navAgent;
    private Animator _swordAnimator;

    // Show the AI mood
    private AiMoodIconController _agentMoodIndicator;
    public AiMoodIconController AiMoodIndicator
    {
        get { return _agentMoodIndicator; }
    }

    // Use this for initialization, get references to all the component scripts we'll need
    public virtual void Start()
    {
        _agentData = GetComponent<AgentData>();
        _agentSenses = GetComponentInChildren<Sensing>();
        _agentSenses.Start();
        _agentInventory = GetComponentInChildren<InventoryController>();
        _navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _swordAnimator = GetComponentInChildren<Animator>();
        _agentMoodIndicator = GetComponentInChildren<AiMoodIconController>();
    }

    /// <summary>
    /// Utility method to test for a valid destination on the navmesh
    /// </summary>
    /// <param name="destinationToTest">The location we are testing</param>
    /// <param name="destination">A valid destination</param>
    /// <returns>true if a destination was found, false otherwise</returns>
    private bool TestDestination(Vector3 destinationToTest, out Vector3 destination)
    {
        // Check we can move there
        UnityEngine.AI.NavMeshHit navHit;
        if (UnityEngine.AI.NavMesh.SamplePosition(destinationToTest, out navHit, Vector3.Distance(transform.position, destinationToTest), AgentData.AgentLayerMask))
        {
            destination = navHit.position;
            return true;
        }

        destination = Vector3.zero;
        return false;
    }

    /// <summary>
    /// Move towards the position of the target object
    /// </summary>
    /// <param name="target">the GameObject to move to</param>
    /// <returns>true if we can move there, false otherwise</returns>
    public bool MoveTo(GameObject target)
    {
        if (target != null)
        {
            // Check we can move there
            Vector3 destination;
            if (TestDestination(target.transform.position, out destination))
            {
                _navAgent.destination = destination;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Move towards a target location
    /// </summary>
    /// <param name="target">Location to move to as a Vector3</param>
    /// <returns>true if we can move there, false otherwise</returns>
    public bool MoveTo(Vector3 target)
    {
        // Check we can move there
        Vector3 destination;
        if (TestDestination(target, out destination))
        {
            _navAgent.destination = destination;
            return true;
        }

        return false;;
    }

    /// <summary>
    /// Move to a nearby random location
    /// </summary>
    public bool MoveToRandomLocation()
    {
        // Choose a new direction
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * _agentData.Speed;
        randomDirection += transform.position;

        // Check we can move there
        Vector3 destination;
        if (TestDestination(randomDirection, out destination))
        {
            _navAgent.destination = destination;
            return true;
        }
        else return false;
    }

    /// <summary>
    /// Pick up a collectable item and put it in the inventory
    /// A collected item is no longer visible to other AIs with the exception of the flag
    /// </summary>
    /// <param name="item">The item to pick up</param>
    public NodeState CollectItem(GameObject item)
    {
        if (item != null&&item.layer!=0)
        {
            if (_agentSenses.IsItemInReach(item))
            {
                // If its collectable add it to the inventory
                if (item.GetComponent<Collectable>() != null)
                {
                    item.GetComponent<Collectable>().Collect(_agentData);
                    if(_agentInventory.AddItem(item)) return NodeState.SUCCESS;                  
                }
            }
        }
        return NodeState.FAILURE;
    }

    /// <summary>
    /// Use an item stored in the inventory if it is stored there
    /// </summary>
    /// <param name="item">The item GameObject</param>
    /// <returns>true if the item was successfully used, false otherwise</returns>
    public NodeState UseItem(GameObject item)
    {
        if (item != null)
        {
            // Check we actually have it and it's usable
            if (_agentInventory.HasItem(item.name) && item.GetComponent<IUsable>() != null)
            {
                _agentInventory.RemoveItem(item.name);
                item.GetComponent<IUsable>().Use(_agentData);
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;
    }

    /// <summary>
    /// Drop an item stored in the inventory onto the ground
    /// A dropped item becomes visible and collectable
    /// </summary>
    /// <param name="item">The item to drop</param>
    public NodeState DropItem(GameObject item)
    {
            // Check we actually have it and its collectable
            if (_agentInventory.HasItem(item.name) && item.GetComponent<Collectable>() != null)
            {
                // Check just in front of us that we're not dropping inside an obstacle
                Vector3 targetPoint = gameObject.transform.position + gameObject.transform.forward;
                // Make sure we're testing a position on the ground
                targetPoint.y = 1.0f;

                Vector3 dropPosition;
                if (TestDestination(targetPoint, out dropPosition))
                {
                    // Make sure we keep the original y position of the item
                    dropPosition.y = item.transform.position.y;
                    _agentInventory.RemoveItem(item.name);

                    item.GetComponent<Collectable>().Drop(_agentData, dropPosition);
                    return NodeState.SUCCESS;
                }
            }
        return NodeState.FAILURE;
    }

    /// <summary>
    /// Drop every item in the inventory
    /// </summary>
    public NodeState DropAllItems()
    {
        // Get a list of all the items in the inventory by key
        string[] inventoryKeys = _agentInventory.Items.Keys.ToArray();

        // go through every key in the key list removing each item from the inventory
        foreach (var key in inventoryKeys)
        {
            if (_agentInventory.HasItem(key))
            {
                GameObject item = _agentInventory.GetItem(key);
                DropItem(item);
            }
        }
        return NodeState.SUCCESS;
    }

    /// <summary>
    /// Attack an enemy AI if it is within range, a powerup will increase the damage done
    /// </summary>
    /// <param name="target">The target to attack</param>
    public NodeState AttackEnemy(GameObject target)
    {
        try
        {
        // Only attack the enemy
        if (target.CompareTag(_agentData.EnemyTeamTag))
        {
            // Only do damage if we're within attack range
            if(_agentSenses.IsInAttackRange(target))
            {
                // Swing the sword
                _swordAnimator.SetTrigger(AttackAnimationTrigger);

                // We may not always hit
                if (UnityEngine.Random.value < _agentData.HitProbability)
                {
                    int actualDamage = _agentData.NormalAttackDamage;

                    // Tell the enemy we hit them
                    if (_agentData.IsPoweredUp)
                    {
                        actualDamage *= _agentData.PowerUpAmount;
                    }
                    target.GetComponent<AgentData>().TakeDamage(actualDamage);
                }
                return NodeState.SUCCESS;
            }
        }
        }
        catch(MissingReferenceException){return NodeState.FAILURE;}
        return NodeState.FAILURE;
    }

    /// <summary>
    /// Flee from an object by moving in the opposite direction
    /// </summary>
    /// <param name="enemy">The object to flee from (expected to be an enemy AI)</param>
    public NodeState Flee(GameObject enemy)
    {
        if(enemy==null)return NodeState.FAILURE;
        // Turn away from the threat
        transform.rotation = Quaternion.LookRotation(transform.position - enemy.transform.position);
        Vector3 runTo = transform.position + transform.forward * _navAgent.speed;

        //So now we've got a Vector3 to run to and we can transfer that to a location on the NavMesh with samplePosition.
        // stores the output in a variable called hit
        UnityEngine.AI.NavMeshHit navHit;

        // Check for a point to flee to
        UnityEngine.AI.NavMesh.SamplePosition(runTo, out navHit, _agentData.Speed, 1 << UnityEngine.AI.NavMesh.GetAreaFromName("Walkable"));
        _navAgent.SetDestination(navHit.position);
        return NodeState.SUCCESS;
    }
   

}