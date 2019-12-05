using System; 
using UnityEngine; 
using System.Collections; 
 
public class ActionNode : Node { 
    public delegate NodeStates ActionNodeDelegate(); 

    public ActionNodeDelegate m_action; 
    private string m_name;

    public ActionNode(string name,ActionNodeDelegate action) { 
        m_action = action;
        m_name=name; 
    } 
 
   ///<summary>
   ///Invokes the Behaviour bound to the action delegate
   ///</summary>
    public override NodeStates Tick() { 
        //Debug.Log(m_name);
        //return whatever the delegate returns
        switch (m_action()) { 
            case NodeStates.SUCCESS: 
                m_nodeState = NodeStates.SUCCESS; 
                return m_nodeState; 
            case NodeStates.FAILURE: 
                m_nodeState = NodeStates.FAILURE; 
                return m_nodeState; 
            case NodeStates.RUNNING: 
                m_nodeState = NodeStates.RUNNING; 
                return m_nodeState; 
            default: 
                m_nodeState = NodeStates.FAILURE; 
                return m_nodeState; 
        } 
    }
    
}