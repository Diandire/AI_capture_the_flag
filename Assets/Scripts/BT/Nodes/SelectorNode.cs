using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class SelectorNode : Node { 
    /** The child nodes for this selector */ 
    protected List<Node> m_nodes = new List<Node>(); 
    private int runningNode=0;
    private string m_name="";
    /** The constructor requires a lsit of child nodes to be  
     * passed in*/ 
    public SelectorNode(List<Node> nodes) { 
        m_nodes = nodes; 
    }

     public SelectorNode(string name) { 
        m_nodes = new List<Node>(); 
        m_name=name;
    }  
 
    /* If any of the children reports a success, the selector will 
     * immediately report a success upwards. If all children fail, 
     * it will report a failure instead.*/ 
    public override NodeStates Tick() {
        if(runningNode>=m_nodes.Count)runningNode=0;
        //Debug.Log(m_name);
        switch (m_nodes[runningNode].Tick()) 
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
        if(runningNode<(m_nodes.Count-1))m_nodeState=NodeStates.RUNNING; 
        return m_nodeState; 
    }

    public void AddChildNode(Node child)
    {
        m_nodes.Add(child);
    } 
}