using System; 
using UnityEngine; 
using System.Collections; 
 
public class ActionNode : Node { 
    public delegate NodeState ActionNodeDelegate(); 

    public ActionNodeDelegate m_action; 
    private string m_name;

    public ActionNode(string name,ActionNodeDelegate action) { 
        m_action = action;
        m_name=name; 
    } 
 
   ///<summary>
   ///Invokes the Behaviour bound to the action delegate
   ///</summary>
    public override NodeState Tick() { 
        //Debug.Log(m_name);
        //return whatever the delegate returns
        switch (m_action()) { 
            case NodeState.SUCCESS: 
                m_nodeState = NodeState.SUCCESS; 
                return m_nodeState; 
            case NodeState.FAILURE: 
                m_nodeState = NodeState.FAILURE; 
                return m_nodeState; 
            case NodeState.RUNNING: 
                m_nodeState = NodeState.RUNNING; 
                return m_nodeState; 
            default: 
                m_nodeState = NodeState.FAILURE; 
                return m_nodeState; 
        } 
    }
    
}