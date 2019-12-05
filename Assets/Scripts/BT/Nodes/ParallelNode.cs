using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ParallelNode : Node { 
    protected List<Node> m_childNodes = new List<Node>(); 
    private string m_name="";
 
    public ParallelNode(List<Node> nodes) { 
        m_childNodes = nodes; 
    }

     public ParallelNode(string name) { 
        m_childNodes = new List<Node>(); 
        m_name=name;
    }  
 
    ///<summary>
    ///Ticks all children every time it is ticked.
    ///Only one child needs to bes successfull to report a success
    ///</summary>
    public override NodeStates Tick() {
        m_nodeState = NodeStates.FAILURE;  
        foreach (Node node in m_childNodes) { 
            Debug.Log(m_name);
            switch (node.Tick()) { 
                case NodeStates.FAILURE: 
                    continue; 
                case NodeStates.SUCCESS: 
                    m_nodeState = NodeStates.SUCCESS; 
                    continue; 
                case NodeStates.RUNNING: 
                    m_nodeState = NodeStates.RUNNING; 
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
