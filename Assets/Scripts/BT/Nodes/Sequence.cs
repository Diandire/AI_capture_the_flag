using UnityEngine; 
using System.Collections; 
using System.Collections.Generic; 
 
public class Sequence : Node { 
    /** Children nodes that belong to this sequence */ 
    private List<Node> m_nodes = new List<Node>(); 
    private int runningNode=0;
    private bool anyChildRunning = false; 
 
    /** Must provide an initial set of children nodes to work */ 
    public Sequence(List<Node> nodes)
    { 
        m_nodes = nodes; 
    } 

    public Sequence()
    { 
        m_nodes = new List<Node>(); 
    } 
 
    /* If any child node returns a failure, the entire node fails. Whence all  
     * nodes return a success, the node reports a success. */ 
    public override NodeStates Evaluate() 
    {   
            if(runningNode>=m_nodes.Count)runningNode=0;
            switch (m_nodes[runningNode].Evaluate()) 
            { 
                case NodeStates.FAILURE: 
                    m_nodeState = NodeStates.FAILURE; 
                    runningNode=0;
                    return m_nodeState;                     
                case NodeStates.SUCCESS: 
                    runningNode++;
                    break; 
                case NodeStates.RUNNING: 
                    anyChildRunning = true; 
                    break; 
            }
        m_nodeState = anyChildRunning ? NodeStates.RUNNING : NodeStates.SUCCESS; 
        return m_nodeState; 
    } 

    public void AddChildNode(Node child)
    {
        m_nodes.Add(child);
    }
}