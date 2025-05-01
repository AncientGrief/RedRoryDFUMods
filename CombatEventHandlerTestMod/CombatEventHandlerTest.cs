using System;
using System.Collections;
using System.Collections.Generic;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using UnityEngine;
using VanillaCombatEventHandler;

public class CombatEventHandlerTest : MonoBehaviour
{
    static Mod mod;

    [Invoke(StateManager.StateTypes.Start, 0)]
    public static void Init(InitParams initParams)
    {
        mod = initParams.Mod;
        var go = new GameObject(mod.Title);
        go.AddComponent<CombatEventHandlerTest>();
    }

    private void Awake()
    {
        VcehWrapper.Init(mod.Title);
        VcehWrapper.RegisterOnAttackDamage(OnAttackDamage);
    }

    private ResultAttackDamageCalculated OnAttackDamage(AttackDamageCalculatedContext ctx)
    {
        var result = new ResultAttackDamageCalculated
        {
            AttackDamage = ctx.CalculatedDamage
        };

        if (ctx.Attacker == null
            || ctx.Target == null
            || !(ctx.Attacker is PlayerEntity attackerAi)
            || !(ctx.Target is EnemyEntity enemy))
            return result;

        result.AttackDamage = enemy.MobileEnemy.ID;

        return result;
    }
}
