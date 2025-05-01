using System;
using System.Collections;
using System.Collections.Generic;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using UnityEngine;
using VanillaCombatEventHandler;

public class CombatEventHandlerTest2 : MonoBehaviour
{
    static Mod mod;
    private VcehWrapper vcehWrapper;

    [Invoke(StateManager.StateTypes.Start, 0)]
    public static void Init(InitParams initParams)
    {
        mod = initParams.Mod;
        var go = new GameObject(mod.Title);
        go.AddComponent<CombatEventHandlerTest2>();
    }

    private void Awake()
    {
        vcehWrapper = new VcehWrapper();
        if(vcehWrapper.Init(mod.Title))
            vcehWrapper.RegisterOnCalculateAttackDamage(OnCalculateAttackDamage);
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

        Debug.Log($"[VCEH] TestMod 2: damage = {result.CalculatedDamage}/4 = {result.CalculatedDamage/4}");
        result.CalculatedDamage /= 4;

        return result;
    }
}
