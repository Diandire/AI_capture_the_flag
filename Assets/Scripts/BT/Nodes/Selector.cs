using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Selector : Node { 
    /** The child nodes for this selector */ 
    protected List<Node> m_nodes = new List<Node>(); 
    private int runningNode=0;
 
    /** The constructor requires a lsit of child nodes to be  
     * passed in*/ 
    public Selector(List<Node> nodes) { 
        m_nodes = nodes; 
    }

     public Selector() { 
        m_nodes = new List<Node>(); 
    }  
 
    /* If any of the children reports a success, the selector will 
     * immediately report a success upwards. If all children fail, 
     * it will report a failure instead.*/ 
    public override NodeStates Evaluate() {
        if(runningNode>=m_nodes.Count)runningNode=0;
        switch (m_nodes[runningNode].Evaluate()) 
        { 
            case NodeStates.FAILURE: 
                m_nodeState = NodeStates.FAILURE; 
                runningNode++;
                break;                    
            case NodeStates.SUCCESS: 
                runningNode=0;
                m_nodeState = NodeStates.SUCCESS; 
                return m_nodeState;
            case NodeStates.RUNNING: 
                m_nodeState = NodeStates.RUNNING; 
                return m_nodeState;
        }
        return m_nodeState; 
    }

    public void AddChildNode(Node child)
    {
        m_nodes.Add(child);
    } 
}