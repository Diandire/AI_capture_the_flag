using System; 
using UnityEngine; 
using System.Collections; 
 
public class ConditionNode : Node { 
    /* Method signature for the action. */ 
    public delegate bool ConditionNodeDelegate(); 
 
    /* The delegate that is called to evaluate this node */ 
    public ConditionNodeDelegate m_condition; 
    private Node m_childNode;
    private string m_name;
    /* Because this node contains no logic itself, 
     * the logic must be passed in in the form of  
     * a delegate. As the signature states, the action 
     * needs to return a NodeStates enum */ 
    public ConditionNode(string name,ConditionNodeDelegate condition) { 
        m_condition = condition;
        m_name=name; 
    } 
 
    /* Evaluates the node using the passed in delegate and  
     * reports the resulting state as appropriate */ 
    public override NodeStates Tick() { 
        Debug.Log(m_name);
        if(m_condition()){
            if(m_childNode!=null)return m_childNode.Tick();
            else return NodeStates.SUCCESS;
        }
        else return NodeStates.FAILURE;
    }

    public void AddChildNode(Node child)
    {
        m_childNode=child;
    }
    
}
