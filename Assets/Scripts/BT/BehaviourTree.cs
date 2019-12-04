using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree 
{
    Node StartNode;
    // Start is called before the first frame update
    public BehaviourTree(Behaviours behaviours)
    {
        SelectorNode mainBehaviour=new SelectorNode("main behaviour");
        ParallelNode start=new ParallelNode("start node"),combat=new ParallelNode("combat");

        StartNode=start;

        //go to target and attack sequence
        SequenceNode AttackEnemy=new SequenceNode("Attack Enemy");
        AttackEnemy.AddChildNode(new ActionNode("walk to target enemy",new ActionNode.ActionNodeDelegate(behaviours.WalkToTarget)));
        AttackEnemy.AddChildNode(new ActionNode("Attack my target",new ActionNode.ActionNodeDelegate(behaviours.AttackTarget)));

        start.AddChildNode(new ActionNode("Healing",new ActionNode.ActionNodeDelegate(behaviours.HealMySelf)));

        SequenceNode KillAllEnemies=new SequenceNode("Kill enemies in sight");
        KillAllEnemies.AddChildNode(new ActionNode("Search for enemies",new ActionNode.ActionNodeDelegate(behaviours.LookForEnemy)));
        KillAllEnemies.AddChildNode(AttackEnemy);
        combat.AddChildNode(KillAllEnemies);

        combat.AddChildNode(mainBehaviour);

        start.AddChildNode(combat);
        
        //mainBehaviour.AddChildNode(new ActionNode("Move to Healthkit",new ActionNode.ActionNodeDelegate(behaviours.MoveToHealthKit)));


        SelectorNode GetFlags = new SelectorNode("Get Flags");

        

        ConditionNode EnemyFlagAvailable = new ConditionNode("check for enemy flag",behaviours.CheckForEnemyFlag);

        SequenceNode GetEnemyFlag=new SequenceNode("Get enemy flag");
        GetEnemyFlag.AddChildNode(new ActionNode("move to enemy flag",new ActionNode.ActionNodeDelegate(behaviours.MoveToEnemyFlag)));
        GetEnemyFlag.AddChildNode(new ActionNode("Pick up enemy flag",new ActionNode.ActionNodeDelegate(behaviours.PickUpEnemyFlag)));
        GetEnemyFlag.AddChildNode(new ActionNode("return to my base",new ActionNode.ActionNodeDelegate(behaviours.ReturnToBase)));
        GetEnemyFlag.AddChildNode(new ActionNode("Drop items",new ActionNode.ActionNodeDelegate(behaviours.DropAllItems)));

        EnemyFlagAvailable.AddChildNode(GetEnemyFlag);
        GetFlags.AddChildNode(EnemyFlagAvailable);

        ConditionNode MyFlagTaken = new ConditionNode("Is my flag taken?",behaviours.CheckForMyFlag);

        SequenceNode HuntEnemyFC=new SequenceNode("Hunt enemy FC");
        HuntEnemyFC.AddChildNode(new ActionNode("Target enemy FC",new ActionNode.ActionNodeDelegate(behaviours.TargetEnemyFC)));
        HuntEnemyFC.AddChildNode(AttackEnemy);

        GetFlags.AddChildNode(HuntEnemyFC);

        SequenceNode ReturnMyFlag=new SequenceNode("Return my flag sequence");
        ReturnMyFlag.AddChildNode(new ActionNode("Move to my flag",new ActionNode.ActionNodeDelegate(behaviours.MoveToFriendlyFlag)));
        ReturnMyFlag.AddChildNode(new ActionNode("pick up my flag",new ActionNode.ActionNodeDelegate(behaviours.PickUpFriendlyFlag)));
        ReturnMyFlag.AddChildNode(new ActionNode("return to my base",new ActionNode.ActionNodeDelegate(behaviours.ReturnToBase)));
        ReturnMyFlag.AddChildNode(new ActionNode("Drop items",new ActionNode.ActionNodeDelegate(behaviours.DropAllItems)));
        MyFlagTaken.AddChildNode(ReturnMyFlag);
        GetFlags.AddChildNode(MyFlagTaken);
       

        //go after enemy FC
       
        
        
         

        
        


        //get enemy flag
        
        
        
        mainBehaviour.AddChildNode(GetFlags);

        

         //escort friendly FC
        //Sequence EscortFriendlyFC=new Sequence();
        //EscortFriendlyFC.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.LookForEnemy)));
        //EscortFriendlyFC.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.MoveToMyFC)));
        //EscortFriendlyFC.AddChildNode(AttackEnemy);
        //mainBehaviour.AddChildNode(EscortFriendlyFC);
        //mainBehaviour.AddChildNode(new ActionNode("Move to my FC",new ActionNode.ActionNodeDelegate(behaviours.MoveToMyFC)));

        mainBehaviour.AddChildNode(new ActionNode("return to base",new ActionNode.ActionNodeDelegate(behaviours.ReturnToBase)));  
    }

    // Update is called once per frame
    public void Update()
    {
        StartNode.Tick();
    }
}
