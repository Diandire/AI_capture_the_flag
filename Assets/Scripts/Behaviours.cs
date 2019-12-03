using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviours : AgentActions
{
 public NodeStates MoveToEnemyFlag()
    {
        if(CheckForEnemyFlag())
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
        else{Debug.Log("Flag already taken"); return NodeStates.FAILURE;}
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
        
        Debug.Log(BlackBoard.RedFlagTaken);
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
            case AgentData.Teams.RedTeam : return BlackBoard.RedFlagInBase;

            case AgentData.Teams.BlueTeam : return BlackBoard.BlueFlagInBase;
        }
        return false;
    }

    public bool CheckForEnemyFlag()
    {
        switch(_agentData.EnemyTeam)
        {
            case AgentData.Teams.RedTeam : return !BlackBoard.RedFlagTaken;

            case AgentData.Teams.BlueTeam : return !BlackBoard.BlueFlagTaken;
        }
        return false;
    }

    public NodeStates LookForEnemy()
    {
        List<GameObject> enemiesInView=_agentSenses.GetEnemiesInView();
        if(enemiesInView.Count>0)
        {
            targetToKill=enemiesInView[0];
            return NodeStates.SUCCESS;
        }
        return NodeStates.FAILURE;
    }

    public NodeStates WalkToTarget()
    {
        if(targetToKill==null){Debug.Log("no target found"); return NodeStates.FAILURE;}
        else if(MoveTo(targetToKill)){Debug.Log("Moving to target"); return _agentSenses.IsInAttackRange(targetToKill)?NodeStates.SUCCESS:NodeStates.RUNNING;}
        else return NodeStates.FAILURE;
    }

    public NodeStates TargetEnemyFC()
    {
        Debug.Log("Checking for enemy FC");
        if(CheckForMyFlag()||CheckForEnemyFC()==null)return NodeStates.FAILURE;
        else 
        {
            Debug.Log("Found enemy FC");
            targetToKill=CheckForEnemyFC();
            return NodeStates.SUCCESS;
        }
    }

    public GameObject CheckForEnemyFC()
    {
          switch(_agentData.EnemyTeam)
        {
            case AgentData.Teams.RedTeam : return BlackBoard.RedFlagCarrier;

            case AgentData.Teams.BlueTeam : return BlackBoard.BlueFlagCarrier;
        }
        return null;
    }

    public NodeStates StartFlagReturn()
    {
        if(CheckForMyFlag()||CheckForEnemyFC()!=null)return NodeStates.FAILURE;
        else return NodeStates.SUCCESS;
    }

    public NodeStates AttackTarget()
    {
        return AttackEnemy(targetToKill);
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
        switch(_agentData.FriendlyTeam)
        {
            case AgentData.Teams.RedTeam :
            if(BlackBoard.RedFlagCarrier==null)return NodeStates.FAILURE;
            else if(MoveTo(BlackBoard.RedFlagCarrier))return _agentSenses.IsInAttackRange(BlackBoard.RedFlagCarrier)?NodeStates.SUCCESS:NodeStates.RUNNING;
            else return NodeStates.FAILURE;

             case AgentData.Teams.BlueTeam :
            if(BlackBoard.BlueFlagCarrier==null)return NodeStates.FAILURE;
            else if(MoveTo(BlackBoard.BlueFlagCarrier))return _agentSenses.IsInAttackRange(BlackBoard.BlueFlagCarrier)?NodeStates.SUCCESS:NodeStates.RUNNING;
            else return NodeStates.FAILURE;
        }
        return NodeStates.FAILURE;
    }

    public NodeStates HealMySelf()
    {
        if(_agentData.CurrentHitPoints>(_agentData.MaxHitPoints/2))return NodeStates.FAILURE;
        return UseItem(GameObject.Find(Names.HealthKit));
    }
    public NodeStates MoveToHealthKit()
    {
        if(GameObject.Find(Names.HealthKit)==null||_agentInventory.HasItem(Names.HealthKit)||GameObject.Find(Names.HealthKit).layer==0)return NodeStates.FAILURE;
        else if(MoveTo(GameObject.Find(Names.HealthKit)))
             if(_agentSenses.IsItemInReach(GameObject.Find(Names.HealthKit))&&CollectItem(GameObject.Find(Names.HealthKit))==NodeStates.SUCCESS)return NodeStates.SUCCESS;
             else return NodeStates.RUNNING;
        else return NodeStates.FAILURE;
    }
}
