using System;
using UnityEngine;

public abstract class DecisionTreeNode
{
    public abstract void Execute(BossController boss);
}

public class DecisionNode : DecisionTreeNode
{
    private Func<BossController, bool> decision;
    private DecisionTreeNode trueNode;
    private DecisionTreeNode falseNode;

    public DecisionNode(Func<BossController, bool> decision, DecisionTreeNode trueNode, DecisionTreeNode falseNode)
    {
        this.decision = decision;
        this.trueNode = trueNode;
        this.falseNode = falseNode;
    }

    public override void Execute(BossController boss)
    {
        if (decision(boss))
        {
            trueNode.Execute(boss);
        }
        else
        {
            falseNode.Execute(boss);
        }
    }
}

public class ActionNode : DecisionTreeNode
{
    private Action<BossController> action;

    public ActionNode(Action<BossController> action)
    {
        this.action = action;
    }

    public override void Execute(BossController boss)
    {
        action(boss);
    }
}
