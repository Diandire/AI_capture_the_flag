using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree 
{
    Node StartNode;
    // Start is called before the first frame update
    public BehaviourTree(Behaviours behaviours)
    {
        SelectorNode mainBehaviour=new SelectorNode("main behaviour",false),completeObjectives=new SelectorNode("complete my objectives");
        ParallelNode start=new ParallelNode("start node");

        StartNode=start;

        //go to target and attack sequence
        SequenceNode AttackEnemy=new SequenceNode("Attack Enemy");
        AttackEnemy.AddChildNode(new ActionNode("walk to target enemy",new ActionNode.ActionNodeDelegate(behaviours.WalkToTarget)));
        AttackEnemy.AddChildNode(new ActionNode("Attack my target",new ActionNode.ActionNodeDelegate(behaviours.AttackTarget)));

        //target closest enemy and attack him
        SequenceNode KillAllEnemies=new SequenceNode("Kill enemies in sight");
        KillAllEnemies.AddChildNode(new ActionNode("Search for enemies",new ActionNode.ActionNodeDelegate(behaviours.LookForEnemy)));
        KillAllEnemies.AddChildNode(new ActionNode("Attack my target",new ActionNode.ActionNodeDelegate(behaviours.AttackTarget)));
        start.AddChildNode(KillAllEnemies);

        //target closest enemy and flee from him
        SequenceNode FleeFromEnemies=new SequenceNode("Flee from enemy");
        FleeFromEnemies.AddChildNode(new ActionNode("Search for enemies",new ActionNode.ActionNodeDelegate(behaviours.LookForEnemy)));
        FleeFromEnemies.AddChildNode(new ActionNode("Run away",new ActionNode.ActionNodeDelegate(behaviours.FleeFromEnemy)));

        //Get e health kit if below 50% health
        ConditionNode HealthCheck=new ConditionNode("check if health is critical",new ConditionNode.ConditionNodeDelegate(behaviours.CriticalHealthCheck));

        SequenceNode GoForHealing=new SequenceNode("go to get some health");
        GoForHealing.AddChildNode(new ActionNode("go to healthkit",new ActionNode.ActionNodeDelegate(behaviours.GetHealthkit)));
        GoForHealing.AddChildNode(new ActionNode("heal myself",new ActionNode.ActionNodeDelegate(behaviours.HealMySelf)));

        HealthCheck.AddChildNode(GoForHealing);


        //return to base if carrying a flag
        ConditionNode DoIHaveAFlag=new ConditionNode("Do I have a flag?",behaviours.CheckForFlags);
        SelectorNode ReturnFlags=new SelectorNode("return flags",false);
        ReturnFlags.AddChildNode(FleeFromEnemies);
        SequenceNode ReturnToBase=new SequenceNode("return flag");
        ReturnToBase.AddChildNode(new ActionNode("return to my base",new ActionNode.ActionNodeDelegate(behaviours.ReturnToBase)));
        ReturnToBase.AddChildNode(new ActionNode("drop Items",new ActionNode.ActionNodeDelegate(behaviours.DropAllItems)));
        ReturnFlags.AddChildNode(ReturnToBase);

        DoIHaveAFlag.AddChildNode(ReturnFlags);
        
        mainBehaviour.AddChildNode(DoIHaveAFlag);
        mainBehaviour.AddChildNode(HealthCheck);
        mainBehaviour.AddChildNode(completeObjectives);


        //go to get the power up
        RandomNode GoForPowerUp = new RandomNode("Go for the PowerUp",0.5f);
        GoForPowerUp.AddChildNode(new ActionNode("Get the PowerUp",new ActionNode.ActionNodeDelegate(behaviours.GetPowerUp)));
        completeObjectives.AddChildNode(GoForPowerUp);

        start.AddChildNode(mainBehaviour);

        //Get the enemy flag and return my flag if it is taken
        //getting the enemy flag has higher priority
        SelectorNode GetFlags = new SelectorNode("Get Flags");

        
        //get the enemy flag
        ConditionNode EnemyFlagAvailable = new ConditionNode("check for enemy flag",behaviours.CheckForEnemyFlag);

        SequenceNode GetEnemyFlag=new SequenceNode("Get enemy flag");
        GetEnemyFlag.AddChildNode(new ActionNode("move to enemy flag",new ActionNode.ActionNodeDelegate(behaviours.MoveToEnemyFlag)));
        GetEnemyFlag.AddChildNode(new ActionNode("Pick up enemy flag",new ActionNode.ActionNodeDelegate(behaviours.PickUpEnemyFlag)));

        EnemyFlagAvailable.AddChildNode(GetEnemyFlag);
        GetFlags.AddChildNode(EnemyFlagAvailable);


        
        ConditionNode MyFlagTaken = new ConditionNode("Is my flag taken?",behaviours.CheckForMyFlag);
        //target the enemy flag carrier
        SequenceNode HuntEnemyFC=new SequenceNode("Hunt enemy FC");
        HuntEnemyFC.AddChildNode(new ActionNode("Target enemy FC",new ActionNode.ActionNodeDelegate(behaviours.TargetEnemyFC)));
        HuntEnemyFC.AddChildNode(AttackEnemy);

        GetFlags.AddChildNode(HuntEnemyFC);
        //get my own flag
        SequenceNode ReturnMyFlag=new SequenceNode("Return my flag sequence");
        ReturnMyFlag.AddChildNode(new ActionNode("Move to my flag",new ActionNode.ActionNodeDelegate(behaviours.MoveToFriendlyFlag)));
        ReturnMyFlag.AddChildNode(new ActionNode("pick up my flag",new ActionNode.ActionNodeDelegate(behaviours.PickUpFriendlyFlag)));

        MyFlagTaken.AddChildNode(ReturnMyFlag);
        GetFlags.AddChildNode(MyFlagTaken);     
        
        
        completeObjectives.AddChildNode(GetFlags);

        // move to my flag carrier
        mainBehaviour.AddChildNode(new ActionNode("Move to my FC",new ActionNode.ActionNodeDelegate(behaviours.MoveToMyFC)));

        //return to my base, this has lowest priority and serves to guard the base if we have both flags
        mainBehaviour.AddChildNode(new ActionNode("return to base",new ActionNode.ActionNodeDelegate(behaviours.ReturnToBase)));  
    }

    // Update is called once per frame
    public void Update()
    {
        StartNode.Tick();
    }
}
