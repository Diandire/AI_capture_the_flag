using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class SelectorNode : Node { 
    protected List<Node> m_childNodes = new List<Node>(); 
    private int runningNode=0;
    private string m_name="";
    //decides if the selector should lock into a node if it returns running or keep checking from the start every update
    private bool m_lockSelection;

    public SelectorNode(List<Node> nodes,bool lockSelection=true) { 
        m_childNodes = nodes; 
        m_lockSelection=lockSelection;
    }

     public SelectorNode(string name, bool lockSelection=true) { 
        m_childNodes = new List<Node>(); 
        m_name=name;
        m_lockSelection=lockSelection;
    }  
 
    ///<summary>
    ///Ticks the child nodes until it finds one that does not fail.
    ///Keeps ticking that child until it is successfull causing the selector to start checking from the first child again.
    ///Or fails as well and the selector keeps checking the next child
    ///</summary>
    public override NodeStates Tick() {
        if(runningNode>=m_childNodes.Count)runningNode=0;
        //Debug.Log(m_name);
        switch (m_childNodes[runningNode].Tick()) 
        { 
            //on failure set the current node to the next one
            case NodeStates.FAILURE: 
                m_nodeState = NodeStates.FAILURE; 
                //if the current node is not the last one return running
                if(runningNode<(m_childNodes.Count-1))m_nodeState=NodeStates.RUNNING; 
                runningNode++;
                break;     
            //on success return success and reset the current node               
            case NodeStates.SUCCESS: 
                runningNode=0;
                m_nodeState = NodeStates.SUCCESS; 
                return m_nodeState;
            //on running keep the current node if locked in else reset the current node
            case NodeStates.RUNNING: 
                m_nodeState = NodeStates.RUNNING; 
                if(!m_lockSelection)runningNode=0;
                return m_nodeState;
        }
        return m_nodeState; 
    }

    public void AddChildNode(Node child)
    {
        m_childNodes.Add(child);
    } 
}