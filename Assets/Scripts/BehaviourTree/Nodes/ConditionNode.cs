using System; 
using UnityEngine; 
using System.Collections; 
 
public class ConditionNode : Node { 
    public delegate bool ConditionNodeDelegate(); 
 

    public ConditionNodeDelegate m_condition; 
    private Node m_childNode;
    private string m_name;

    public ConditionNode(string name,ConditionNodeDelegate condition) { 
        m_condition = condition;
        m_name=name; 
    } 
 
    ///<summary>
    ///Checks the condition bound to the condition delegate.
    ///On success either Ticks the child node if it has one or returns a success.
    ///If the condition fails the Node fails and the child does not get ticked.
    ///</summary>
    public override NodeState Tick() { 
        //Debug.Log(m_name);
        //if the condition returns true the child node if it exists gets ticked, otherwise returns a success
        if(m_condition()){
            if(m_childNode!=null)return m_childNode.Tick();
            else return NodeState.SUCCESS;
        }
        //condition fails so node returns failure
        else return NodeState.FAILURE;
    }

    public void AddChildNode(Node child)
    {
        m_childNode=child;
    }
    
}
