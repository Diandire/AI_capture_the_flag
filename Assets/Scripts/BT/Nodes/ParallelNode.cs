using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ParallelNode : Node { 
    /** The child nodes for this selector */ 
    protected List<Node> m_nodes = new List<Node>(); 
    private string m_name="";
 
    /** The constructor requires a lsit of child nodes to be  
     * passed in*/ 
    public ParallelNode(List<Node> nodes) { 
        m_nodes = nodes; 
    }

     public ParallelNode(string name) { 
        m_nodes = new List<Node>(); 
        m_name=name;
    }  
 
    /* If any of the children reports a success, the selector will 
     * immediately report a success upwards. If all children fail, 
     * it will report a failure instead.*/ 
    public override NodeStates Tick() {
        m_nodeState = NodeStates.FAILURE;  
        foreach (Node node in m_nodes) { 
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
        m_nodes.Add(child);
    } 
}
