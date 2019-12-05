using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///This class stores all behaviours and conditions to be bound to the node delegates.
///It extends AgentActions and calls the respective actions with the needed parameters for the intended behaviour.
///</summary>
public class Behaviours : AgentActions
{
    private GameObject targetAgent;
    private BlackBoard blackBoard;

    public override void Start() 
    {
        base.Start();
        blackBoard=GameObject.FindObjectOfType<BlackBoard>();    
    }

///<summary>
///Move to the enemy Flag
///</summary>
 public NodeStates MoveToEnemyFlag()
    {
        switch(_agentData.EnemyTeam)
        {
            case AgentData.Teams.RedTeam : 
             if(MoveTo(GameObject.Find(Names.RedFlag)))
             return _agentSenses.IsItemInReach(GameObject.Find(Names.RedFlag))?NodeStates.SUCCESS:NodeStates.RUNNING;
             else return NodeStates.FAILURE;

            case AgentData.Teams.BlueTeam : 
             if(MoveTo(GameObject.Find(Names.BlueFlag)))
             return _agentSenses.IsItemInReach(GameObject.Find(Names.BlueFlag))?NodeStates.SUCCESS:NodeStates.RUNNING;
             else return NodeStates.FAILURE;
            
            default : return NodeStates.FAILURE;
        }
    }

///<summary>
///Move to my own Flag
///</summary>
    public NodeStates MoveToFriendlyFlag()
    {
        switch(_agentData.FriendlyTeam)
        {
            case AgentData.Teams.RedTeam : 
             if(MoveTo(GameObject.Find(Names.RedFlag)))
             return _agentSenses.IsItemInReach(GameObject.Find(Names.RedFlag))?NodeStates.SUCCESS:NodeStates.RUNNING;
             else return NodeStates.FAILURE;

            case AgentData.Teams.BlueTeam : 
             if(MoveTo(GameObject.Find(Names.BlueFlag)))
             return _agentSenses.IsItemInReach(GameObject.Find(Names.BlueFlag))?NodeStates.SUCCESS:NodeStates.RUNNING;
             else return NodeStates.FAILURE;
            
            default : return NodeStates.FAILURE;
        }
    }

///<summary>
///Pick up the enemy Flag
///</summary>
    public NodeStates PickUpEnemyFlag()
    {
        switch(_agentData.EnemyTeam)
        {
            case AgentData.Teams.RedTeam : return CollectItem(GameObject.Find(Names.RedFlag));

            case AgentData.Teams.BlueTeam : return CollectItem(GameObject.Find(Names.BlueFlag));
            
            default : return NodeStates.FAILURE;
        }
    }

///<summary>
///Pick up my own Flag
///</summary>
    public NodeStates PickUpFriendlyFlag()
    {
        switch(_agentData.FriendlyTeam)
        {
            case AgentData.Teams.RedTeam : return CollectItem(GameObject.Find(Names.RedFlag));

            case AgentData.Teams.BlueTeam : return CollectItem(GameObject.Find(Names.BlueFlag));
            
            default : return NodeStates.FAILURE;
        }
    }

///<summary>
///Check wether my Flag is in my base or I currently carry it
///</summary>
    public bool CheckForMyFlag()
    {
        switch(_agentData.FriendlyTeam)
        {
            case AgentData.Teams.RedTeam : 
            return (!BlackBoard.RedFlagInBase||_agentData.HasFriendlyFlag);

            case AgentData.Teams.BlueTeam :
            return (!BlackBoard.BlueFlagInBase||_agentData.HasFriendlyFlag);

            default :
            return false;
        }
    }

///<summary>
///Check if we have captured the enemy Flag or I currently hold it
///</summary>
    public bool CheckForEnemyFlag()
    {
        switch(_agentData.EnemyTeam)
        {
            case AgentData.Teams.RedTeam : 
            return (_agentData.HasEnemyFlag||(!BlackBoard.RedFlagTaken&&((CheckForFriendlyFC()==null)||CheckForFriendlyFC().GetComponent<Behaviours>()._agentData.FriendlyTeam==_agentData.EnemyTeam)));

            case AgentData.Teams.BlueTeam :
            return (_agentData.HasEnemyFlag||(!BlackBoard.RedFlagTaken&&((CheckForFriendlyFC()==null)||CheckForFriendlyFC().GetComponent<Behaviours>()._agentData.FriendlyTeam==_agentData.EnemyTeam)));

            default :
            return false;
        }
    }

///<summary>
///Targets the closest enemy Agent
///</summary>
    public NodeStates LookForEnemy()
    {
        List<GameObject> enemiesInView=_agentSenses.GetEnemiesInView();
        if(enemiesInView.Count>0)
        {
            float distanceToTarget=Vector3.Distance(enemiesInView[0].transform.position,transform.position);
            targetAgent=enemiesInView[0];
            foreach(GameObject enemy in enemiesInView)
            {
                if(Vector3.Distance(enemiesInView[0].transform.position,transform.position)<distanceToTarget)
                {
                    distanceToTarget=Vector3.Distance(enemiesInView[0].transform.position,transform.position);
                    targetAgent=enemy;
                }
            }
            return NodeStates.SUCCESS;
        }
        targetAgent=null;
        return NodeStates.FAILURE;
    }

///<summary>
///Flee from my current target
///</summary>
    public NodeStates FleeFromEnemy()
    {
        return Flee(targetAgent);
    }

///<summary>
///Walk towards my current target
///</summary>
    public NodeStates WalkToTarget()
    {
        if(targetAgent==null)return NodeStates.FAILURE;
        else if(MoveTo(targetAgent)) return _agentSenses.IsInAttackRange(targetAgent)?NodeStates.SUCCESS:NodeStates.RUNNING;
        else return NodeStates.FAILURE;
    }

///<summary>
///Target the enemy Flag carrier
///</summary>
    public NodeStates TargetEnemyFC()
    {
        if(CheckForEnemyFC()==null)return NodeStates.FAILURE;
        else 
        {
            targetAgent=CheckForEnemyFC();
            return NodeStates.SUCCESS;
        }
    }

///<summary>
///returns the carrier of my Flag
///</summary>
    public GameObject CheckForEnemyFC()
    {
          switch(_agentData.EnemyTeam)
        {
            case AgentData.Teams.RedTeam : return blackBoard.BlueFlagCarrier;

            case AgentData.Teams.BlueTeam : return blackBoard.RedFlagCarrier;
        }
        return null;
    }

///<summary>
///returns the carrier of the enemy flag
///</summary>
    public GameObject CheckForFriendlyFC()
    {
          switch(_agentData.FriendlyTeam)
        {
            case AgentData.Teams.RedTeam : return blackBoard.BlueFlagCarrier;

            case AgentData.Teams.BlueTeam : return blackBoard.RedFlagCarrier;
        }
        return null;
    }

///<summary>
///Attack the current target
///</summary>
    public NodeStates AttackTarget()
    {
        return AttackEnemy(targetAgent);
    }

///<summary>
///return to my base
///</summary>
    public NodeStates ReturnToBase()
    {
         switch(_agentData.FriendlyTeam)
        {
           case AgentData.Teams.RedTeam : 
             if(MoveTo(GameObject.Find(Names.RedBase)))
             return _agentSenses.IsInAttackRange(GameObject.Find(Names.RedBase))?NodeStates.SUCCESS:NodeStates.RUNNING;
             else return NodeStates.FAILURE;

            case AgentData.Teams.BlueTeam : 
             if(MoveTo(GameObject.Find(Names.BlueBase)))
             return _agentSenses.IsInAttackRange(GameObject.Find(Names.BlueBase))?NodeStates.SUCCESS:NodeStates.RUNNING;
             else return NodeStates.FAILURE;
            
            default : return NodeStates.FAILURE;
        }
    }

///<summary>
///move to the carrier of enemy flag
///</summary>
    public NodeStates MoveToMyFC()
    {
        if(CheckForFriendlyFC()==null)return NodeStates.FAILURE;
        else if(MoveTo(CheckForFriendlyFC()))return _agentSenses.IsInAttackRange(CheckForFriendlyFC())?NodeStates.SUCCESS:NodeStates.RUNNING;
        else return NodeStates.FAILURE;
    }

///<summary>
///Try to Heal myself
///</summary>
    public NodeStates HealMySelf()
    {
        return UseItem(GameObject.Find(Names.HealthKit));
    }

///<summary>
///Check if my health is below 50%
///</summary>
    public bool CriticalHealthCheck()
    {
        return (_agentData.CurrentHitPoints<(_agentData.MaxHitPoints/2));
    }

///<summary>
///Move to the Healthkit and pick it up
///</summary>
    public NodeStates GetHealthkit()
    {
        if(!blackBoard.HealthKitAvailable())return NodeStates.FAILURE;
        else if(MoveTo(GameObject.Find(Names.HealthKit)))
             if(_agentSenses.IsItemInReach(GameObject.Find(Names.HealthKit))&&CollectItem(GameObject.Find(Names.HealthKit))==NodeStates.SUCCESS)return NodeStates.SUCCESS;
             else return NodeStates.RUNNING;
        else return NodeStates.FAILURE;
    }

///<summary>
///Move to the PowerUp and pick it up and uses it
///</summary>
    public NodeStates GetPowerUp()
    {
        if(!blackBoard.PowerUpAvailable())return NodeStates.FAILURE;
        else if(MoveTo(GameObject.Find(Names.PowerUp)))
        if(_agentSenses.IsItemInReach(GameObject.Find(Names.PowerUp))&&CollectItem(GameObject.Find(Names.PowerUp))==NodeStates.SUCCESS)return UseItem(GameObject.Find(Names.PowerUp));
        else return NodeStates.RUNNING;
        else return NodeStates.FAILURE;
    }

///<summary>
///Check if I have a flag
///</summary>
    public bool CheckForFlags()
    {
        return (_agentData.HasFriendlyFlag||_agentData.HasEnemyFlag);
    }
}
