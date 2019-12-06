using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNode : Node
{

    private Node m_childNode;
    private string m_name;
    private float m_tickChance;

    //bool used to lock the node into ticking the child node once the random generator succeeds once
    //this prevents it from generating a new random number until it fails
    private bool keepRunning=false;

    public RandomNode(string name,float chance) { 
        m_name=name; 
        m_tickChance=chance;
    } 
 
    ///<summary>
    ///Checks a random number between 0 and 1 against the tick chance.
    ///If it is smaller either Ticks the child node if it has one or returns a success.
    ///If it is higher the Node fails and the child does not get ticked.
    ///</summary>
    public override NodeState Tick() { 
        //Debug.Log(m_name);
        //if the random number is smaller than the tick chance the child node if it exists gets ticked, otherwise returns a success
        float rng=Random.value;
        //Debug.Log(rng);
        if(rng<m_tickChance||keepRunning){
            if(m_childNode!=null)
            {
                m_nodeState = m_childNode.Tick();
                keepRunning=(m_nodeState==NodeState.RUNNING)?true:false;
                return m_nodeState;
            }
            else return NodeState.SUCCESS;
        }
        //fails if random number is higher than tick chance so node returns failure
        else return NodeState.FAILURE;
    }

    public void AddChildNode(Node child)
    {
        m_childNode=child;
    }
}
