using UnityEngine; 
using System.Collections; 
using System.Collections.Generic; 
 
public class SequenceNode : Node { 
    /** Children nodes that belong to this sequence */ 
    private List<Node> m_nodes = new List<Node>(); 
    private int runningNode=0;
    
    private string m_name;
    /** Must provide an initial set of children nodes to work */ 
    public SequenceNode(List<Node> nodes)
    { 
        m_nodes = nodes; 
    } 

    public SequenceNode(string name)
    { 
        m_nodes = new List<Node>();
        m_name=name; 
    } 
 
    /* If any child node returns a failure, the entire node fails. Whence all  
     * nodes return a success, the node reports a success. */ 
    public override NodeStates Tick() 
    {   
            //bool anyChildRunning = false; 
            if(runningNode>=m_nodes.Count)runningNode=0;
            //Debug.Log(m_name);
            switch (m_nodes[runningNode].Tick()) 
            { 
                case NodeStates.FAILURE: 
                    m_nodeState = NodeStates.FAILURE; 
                    runningNode=0;
                    return m_nodeState;                     
                case NodeStates.SUCCESS: 
                    runningNode++;
                    break; 
                case NodeStates.RUNNING: 
                    //anyChildRunning = true; 
                    break; 
            }
        m_nodeState = (runningNode==(m_nodes.Count-1))? NodeStates.SUCCESS : NodeStates.RUNNING; 
        return m_nodeState; 
    } 

    public void AddChildNode(Node child)
    {
        m_nodes.Add(child);
    }
}