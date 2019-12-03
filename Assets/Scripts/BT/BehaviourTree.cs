using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree 
{
    Node StartNode;
    // Start is called before the first frame update
    public BehaviourTree(Behaviours behaviours)
    {
        Selector mainBehaviour=new Selector();
        Parallel start=new Parallel(),combat=new Parallel();

        StartNode=start;

        //go to target and attack sequence
        Sequence AttackEnemy=new Sequence();
        AttackEnemy.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.WalkToTarget)));
        AttackEnemy.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.AttackTarget)));

        start.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.HealMySelf)));

        Sequence KillAllEnemies=new Sequence();
        KillAllEnemies.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.LookForEnemy)));
        KillAllEnemies.AddChildNode(AttackEnemy);
        combat.AddChildNode(KillAllEnemies);

        combat.AddChildNode(mainBehaviour);

        start.AddChildNode(combat);
        
        mainBehaviour.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.MoveToHealthKit)));

        //go after enemy FC
        Sequence HuntEnemyFC=new Sequence();
        HuntEnemyFC.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.TargetEnemyFC)));
        HuntEnemyFC.AddChildNode(AttackEnemy);

        mainBehaviour.AddChildNode(HuntEnemyFC);
        
        
        Sequence ReturnMyFlag=new Sequence();
        ReturnMyFlag.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.StartFlagReturn)));
        ReturnMyFlag.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.MoveToFriendlyFlag)));
        ReturnMyFlag.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.PickUpFriendlyFlag)));
        ReturnMyFlag.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.ReturnToBase)));
        ReturnMyFlag.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.DropAllItems)));
        mainBehaviour.AddChildNode(ReturnMyFlag);
        


        //get enemy flag
        
        Sequence GetEnemyFlag=new Sequence();
        GetEnemyFlag.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.MoveToEnemyFlag)));
        GetEnemyFlag.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.PickUpEnemyFlag)));
        GetEnemyFlag.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.ReturnToBase)));
        GetEnemyFlag.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.DropAllItems)));

        mainBehaviour.AddChildNode(GetEnemyFlag);
        

        

         //escort friendly FC
        Sequence EscortFriendlyFC=new Sequence();
        EscortFriendlyFC.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.LookForEnemy)));
        EscortFriendlyFC.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.MoveToMyFC)));
        EscortFriendlyFC.AddChildNode(AttackEnemy);
        mainBehaviour.AddChildNode(EscortFriendlyFC);

        mainBehaviour.AddChildNode(new ActionNode(new ActionNode.ActionNodeDelegate(behaviours.ReturnToBase)));  
    }

    // Update is called once per frame
    public void Update()
    {
        StartNode.Evaluate();
    }
}
