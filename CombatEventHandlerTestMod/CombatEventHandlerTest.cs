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
        if(VcehWrapper.Init(mod.Title))
            VcehWrapper.RegisterOnCalculateAttackDamage(OnCalculateAttackDamage);
    }

    private ResultCalculateAttackDamage OnCalculateAttackDamage(CalculateAttackDamageContext ctx)
    {
        var result = new ResultCalculateAttackDamage
        {
            CalculatedDamage = ctx.CalculatedDamage
        };

        if (ctx.Attacker == null
            || ctx.Target == null
            || !(ctx.Attacker is PlayerEntity attackerAi)
            || !(ctx.Target is EnemyEntity enemy))
            return result;

        result.CalculatedDamage = enemy.MobileEnemy.ID;

        return result;
    }
}
