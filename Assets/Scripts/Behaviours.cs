using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviours : AgentActions
{
    private GameObject targetAgent;
    private BlackBoard blackBoard;

    public override void Start() 
    {
        base.Start();
        blackBoard=GameObject.FindObjectOfType<BlackBoard>();    
    }

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

    public NodeStates PickUpEnemyFlag()
    {
        switch(_agentData.EnemyTeam)
        {
            case AgentData.Teams.RedTeam : return CollectItem(GameObject.Find(Names.RedFlag));

            case AgentData.Teams.BlueTeam : return CollectItem(GameObject.Find(Names.BlueFlag));
            
            default : return NodeStates.FAILURE;
        }
    }

    public NodeStates PickUpFriendlyFlag()
    {
        switch(_agentData.FriendlyTeam)
        {
            case AgentData.Teams.RedTeam : return CollectItem(GameObject.Find(Names.RedFlag));

            case AgentData.Teams.BlueTeam : return CollectItem(GameObject.Find(Names.BlueFlag));
            
            default : return NodeStates.FAILURE;
        }
    }

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

    public bool CheckForEnemyFlag()
    {
        switch(_agentData.EnemyTeam)
        {
            case AgentData.Teams.RedTeam : 
            return (_agentData.HasEnemyFlag||(!BlackBoard.RedFlagTaken&&blackBoard.RedFlagCarrier==null));

            case AgentData.Teams.BlueTeam :
            return (_agentData.HasEnemyFlag||(!BlackBoard.BlueFlagTaken&&blackBoard.BlueFlagCarrier==null));

            default :
            return false;
        }
    }

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
        return NodeStates.FAILURE;
    }

    public NodeStates FleeFromEnemy()
    {
        return Flee(targetAgent);
    }

    public NodeStates WalkToTarget()
    {
        if(targetAgent==null)return NodeStates.FAILURE;
        else if(MoveTo(targetAgent)) return _agentSenses.IsInAttackRange(targetAgent)?NodeStates.SUCCESS:NodeStates.RUNNING;
        else return NodeStates.FAILURE;
    }

    public NodeStates TargetEnemyFC()
    {
        if(CheckForEnemyFC()==null)return NodeStates.FAILURE;
        else 
        {
            targetAgent=CheckForEnemyFC();
            return NodeStates.SUCCESS;
        }
    }

    public GameObject CheckForEnemyFC()
    {
          switch(_agentData.EnemyTeam)
        {
            case AgentData.Teams.RedTeam : return blackBoard.RedFlagCarrier;

            case AgentData.Teams.BlueTeam : return blackBoard.BlueFlagCarrier;
        }
        return null;
    }

    public GameObject CheckForFriendlyFC()
    {
          switch(_agentData.FriendlyTeam)
        {
            case AgentData.Teams.RedTeam : return blackBoard.RedFlagCarrier;

            case AgentData.Teams.BlueTeam : return blackBoard.BlueFlagCarrier;
        }
        return null;
    }

    public NodeStates AttackTarget()
    {
        return AttackEnemy(targetAgent);
    }

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

    public NodeStates MoveToMyFC()
    {
        if(CheckForFriendlyFC()==null)return NodeStates.FAILURE;
        else if(MoveTo(CheckForFriendlyFC()))return _agentSenses.IsInAttackRange(CheckForFriendlyFC())?NodeStates.SUCCESS:NodeStates.RUNNING;
        else return NodeStates.FAILURE;
    }

    public NodeStates HealMySelf()
    {
        if(_agentData.CurrentHitPoints>(_agentData.MaxHitPoints/2))return NodeStates.FAILURE;
        return UseItem(GameObject.Find(Names.HealthKit));
    }
    public NodeStates MoveToHealthKit()
    {
        if(!blackBoard.HealthKitAvailable())return NodeStates.FAILURE;
        else if(MoveTo(GameObject.Find(Names.HealthKit)))
             if(_agentSenses.IsItemInReach(GameObject.Find(Names.HealthKit))&&CollectItem(GameObject.Find(Names.HealthKit))==NodeStates.SUCCESS)return NodeStates.SUCCESS;
             else return NodeStates.RUNNING;
        else return NodeStates.FAILURE;
    }
}
