using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree 
{
    Node StartNode;
    // Start is called before the first frame update
    public BehaviourTree(Behaviours behaviours)
    {
        Selector mainBehaviour=new Selector("main behaviour");
        Parallel start=new Parallel("start node"),combat=new Parallel("combat");

        StartNode=start;

        //go to target and attack sequence
        Sequence AttackEnemy=new Sequence("Attack Enemy");
        AttackEnemy.AddChildNode(new ActionNode("walk to target enemy",new ActionNode.ActionNodeDelegate(behaviours.WalkToTarget)));
        AttackEnemy.AddChildNode(new ActionNode("Attack my target",new ActionNode.ActionNodeDelegate(behaviours.AttackTarget)));

        start.AddChildNode(new ActionNode("Healing",new ActionNode.ActionNodeDelegate(behaviours.HealMySelf)));

        Sequence KillAllEnemies=new Sequence("Kill enemies in sight");
        KillAllEnemies.AddChildNode(new ActionNode("Search for enemies",new ActionNode.ActionNodeDelegate(behaviours.LookForEnemy)));
        KillAllEnemies.AddChildNode(AttackEnemy);
        combat.AddChildNode(KillAllEnemies);

        combat.AddChildNode(mainBehaviour);

        start.AddChildNode(combat);
        
        mainBehaviour.AddChildNode(new ActionNode("Move to Healthkit",new ActionNode.ActionNodeDelegate(behaviours.MoveToHealthKit)));


        Selector GetFlags = new Selector("Get Flags");

        Sequence GetEnemyFlag=new Sequence("Get enemy flag");
        GetEnemyFlag.AddChildNode(new ActionNode("Check for enemy flag",new ActionNode.ActionNodeDelegate(behaviours.MoveToEnemyFlag)));
        GetEnemyFlag.AddChildNode(new ActionNode("Pick up enemy flag",new ActionNode.ActionNodeDelegate(behaviours.PickUpEnemyFlag)));
        GetEnemyFlag.AddChildNode(new ActionNode("return to my base",new ActionNode.ActionNodeDelegate(behaviours.ReturnToBase)));
        GetEnemyFlag.AddChildNode(new ActionNode("Drop items",new ActionNode.ActionNodeDelegate(behaviours.DropAllItems)));

        GetFlags.AddChildNode(GetEnemyFlag);

        //go after enemy FC
        Sequence HuntEnemyFC=new Sequence("Hunt enemy FC");
        HuntEnemyFC.AddChildNode(new ActionNode("Target enemy FC",new ActionNode.ActionNodeDelegate(behaviours.TargetEnemyFC)));
        HuntEnemyFC.AddChildNode(AttackEnemy);

        GetFlags.AddChildNode(HuntEnemyFC);
        
        
         

        Sequence ReturnMyFlag=new Sequence("Return my flag sequence");
        ReturnMyFlag.AddChildNode(new ActionNode("check for my flag",new ActionNode.ActionNodeDelegate(behaviours.StartFlagReturn)));
        ReturnMyFlag.AddChildNode(new ActionNode("Move to my flag",new ActionNode.ActionNodeDelegate(behaviours.MoveToFriendlyFlag)));
        ReturnMyFlag.AddChildNode(new ActionNode("pick up my flag",new ActionNode.ActionNodeDelegate(behaviours.PickUpFriendlyFlag)));
        ReturnMyFlag.AddChildNode(new ActionNode("return to my base",new ActionNode.ActionNodeDelegate(behaviours.ReturnToBase)));
        ReturnMyFlag.AddChildNode(new ActionNode("Drop items",new ActionNode.ActionNodeDelegate(behaviours.DropAllItems)));
        GetFlags.AddChildNode(ReturnMyFlag);
        


        //get enemy flag
        
        
        
        mainBehaviour.AddChildNode(GetFlags);

        

         //escort friendly FC
        //Sequence EscortFriendlyFC=new Sequence();
        //EscortFriendlyFC.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.LookForEnemy)));
        //EscortFriendlyFC.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.MoveToMyFC)));
        //EscortFriendlyFC.AddChildNode(AttackEnemy);
        //mainBehaviour.AddChildNode(EscortFriendlyFC);
        mainBehaviour.AddChildNode(new ActionNode("Move to my FC",new ActionNode.ActionNodeDelegate(behaviours.MoveToMyFC)));

        mainBehaviour.AddChildNode(new ActionNode("return to base",new ActionNode.ActionNodeDelegate(behaviours.ReturnToBase)));  
    }

    // Update is called once per frame
    public void Update()
    {
        StartNode.Evaluate();
    }
}
