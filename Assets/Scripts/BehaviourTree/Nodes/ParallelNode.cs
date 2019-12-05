using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ParallelNode : Node { 
    protected List<Node> m_childNodes = new List<Node>(); 
    private string m_name="";

     public ParallelNode(string name) { 
        m_childNodes = new List<Node>(); 
        m_name=name;
    }  
 
    ///<summary>
    ///Ticks all children every time it is ticked.
    ///Only one child needs to bes successfull to report a success
    ///</summary>
    public override NodeState Tick() {
        m_nodeState = NodeState.FAILURE;  
        foreach (Node node in m_childNodes) { 
            Debug.Log(m_name);
            switch (node.Tick()) { 
                case NodeState.FAILURE: 
                    continue; 
                case NodeState.SUCCESS: 
                    m_nodeState = NodeState.SUCCESS; 
                    continue; 
                case NodeState.RUNNING: 
                    m_nodeState = NodeState.RUNNING; 
                    continue; 
            } 
        }     
        return m_nodeState; 
    }

    public void AddChildNode(Node child)
    {
        m_childNodes.Add(child);
    } 
}
