using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNode : Node
{

    private Node m_childNode;
    private string m_name;
    private float m_tickChance;

    public RandomNode(string name,float chance) { 
        m_name=name; 
        m_tickChance=chance;
    } 
 
    ///<summary>
    ///Checks a random number between 0 and 1 against the tick chance.
    ///If it is smaller either Ticks the child node if it has one or returns a success.
    ///If it is higher the Node fails and the child does not get ticked.
    ///</summary>
    public override NodeStates Tick() { 
        Debug.Log(m_name);
        //if the random number is smaller than the tick chance the child node if it exists gets ticked, otherwise returns a success
        if(Random.Range(0,1)<m_tickChance){
            if(m_childNode!=null)return m_childNode.Tick();
            else return NodeStates.SUCCESS;
        }
        //fails if random number is higher than tick chance so node returns failure
        else return NodeStates.FAILURE;
    }

    public void AddChildNode(Node child)
    {
        m_childNode=child;
    }
}
