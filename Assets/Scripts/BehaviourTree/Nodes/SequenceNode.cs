using UnityEngine; 
using System.Collections; 
using System.Collections.Generic; 
 
public class SequenceNode : Node { 
    private List<Node> m_childNodes = new List<Node>(); 
    private int runningNode=0;
    
    private string m_name;

    public SequenceNode(string name)
    { 
        m_childNodes = new List<Node>();
        m_name=name; 
    } 
 
    ///<summary>
    ///Ticks the children one after another in order.
    ///Fails if one child fails.
    ///Returns success if all children are successful.
    ///Otherwise returns running.
    ///</summary>
    public override NodeState Tick() 
    {   
            if(runningNode>=m_childNodes.Count)runningNode=0;
            //Debug.Log(m_name);
            switch (m_childNodes[runningNode].Tick()) 
            { 
                //return failure and set the running child to 0 so the sequence failed and the next time it is ticked it starts over
                case NodeState.FAILURE: 
                    m_nodeState = NodeState.FAILURE; 
                    runningNode=0;
                    return m_nodeState;      

                //on success set the running node to the next child               
                case NodeState.SUCCESS: 
                    runningNode++;
                    break; 

                //keep ticking the current child until it returns either success or failure
                case NodeState.RUNNING: 
                    break; 
            }
        //return running if not all of the child nodes have been successfull
        m_nodeState = (runningNode==(m_childNodes.Count-1))? NodeState.SUCCESS : NodeState.RUNNING; 
        return m_nodeState; 
    } 

    public void AddChildNode(Node child)
    {
        m_childNodes.Add(child);
    }
}