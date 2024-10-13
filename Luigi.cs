using MelonLoader;
using BTD_Mod_Helper;
using Luigi;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using UnityEngine;
using HarmonyLib;
using System;
using System.Reflection;
using BTD_Mod_Helper.Api;
using Random = System.Random;
using BTD_Mod_Helper.Api.Display;
using Il2CppSystem;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Api.ModOptions;

[assembly: MelonInfo(typeof(Luigi.LuigiTower), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace Luigi;

public class FireBallDisplay : ModDisplay
{
    public override string BaseDisplay => Generic2dDisplay;      

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        Set2DTexture(node, "FireBallDisplay");
    }   
}
public class LuigiTower : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<LuigiTower>("Luigi loaded!");
    }
    public static ModSettingBool IsPlasma = new ModSettingBool(false)
    {
        displayName = "Plasma Fireballs",
        icon = "LighterFireballs-Icon",
        description = "Makes Luigi's Fireballs use the 'Plasma' damage type, giving him a greater weakness to Purple Bloons. Tier 1+ Luigi's Burn DoT is not affected."
    };
    public static ModSettingBool AltBurnyStuff = new ModSettingBool(false)
    {
        displayName = "Alternate Burn DoT",
        icon = "LighterFireballs-Icon",
        description = "Tweaks Tier 4 Luigi's Burn DoT to have higher damage per tick and slower tick speed (But the same DPS.) Also gives lesser tick damage and speed buffs to Tier 2 and 3 Luigi."
    };
    //public static ModSettingBool AltGroundPound = new ModSettingBool(false)
    //{
        //displayName = "Alternate Ground Pound",
        //icon = "LighterFireballs-Icon",
        //description = "Luigi's Fireballs use a 'Impact' (i.e. Sniper, COBRA) attack that explodes instead of a exploding projectile. "
    //};
    //public static ModSettingBool AltKnockback = new ModSettingBool(false)
    //{
        //displayName = "Alternate Knockback",
        //icon = "LighterFireballs-Icon",
        //description = "Makes Tier 4+ Luigi's Ground Pound use a literal 'KnockbackModel' instead of a 'WindModel' (i.e. Distraction Ninja.) I would not recommend using this one tbh."
    //};

}
public class Luigi : ModTower
{
    public override TowerSet TowerSet => TowerSet.Magic;
    public override string BaseTower => TowerType.DartMonkey;
    public override int Cost => 650;
    public override int TopPathUpgrades => 0;
    public override int MiddlePathUpgrades => 5;
    public override int BottomPathUpgrades => 0;
    public override string Description => "Luigi is here in BTD6.";

    public override bool Use2DModel => true;
    public override string Icon => "LuigiIcon";

    public override string Portrait => "LuigiIcon";

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        towerModel.GetAttackModel().weapons[0].projectile = Game.instance.model.GetTower(TowerType.MonkeySub).GetAttackModel().weapons[0].projectile.Duplicate();
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.None;
        if (LuigiTower.IsPlasma) { towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = Il2Cpp.BloonProperties.Purple; }
        //towerModel.GetAttackModel().weapons[0].projectile.ApplyDisplay<FireBallDisplay>();
        
    }

    public override string Get2DTexture(int[] tiers)
    {
        return "LuigiDisplay";
    }
}
public class FlameBlast : ModUpgrade<Luigi>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => MIDDLE;
    public override int Tier => 1;
    public override int Cost => 400;

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Pair'"

    public override string Description => "Fireballs now light bloons on fire on hit.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {

        var Fire = Game.instance.model.GetTower(TowerType.MortarMonkey, 0, 0, 2).GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>();
        Fire.GetBehavior<DamageOverTimeModel>().Interval = 1.3f;
        towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(Fire);
        towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new[] { -1, 0 };
    }
}
public class LighterFireballs : ModUpgrade<Luigi>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => MIDDLE;
    public override int Tier => 2;
    public override int Cost => 1000;

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Pair'"

    public override string Description => "Lighter fireballs allows for much faster firing.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetAttackModel().weapons[0].Rate *= .4f;

        foreach (var attacks in towerModel.GetAttackModels())
        {
            if (!attacks.name.Contains("Ground") && LuigiTower.AltBurnyStuff)
            {
                attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().Interval = 1.225f;
                attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage = 2f;
            }
        }
    }
}
public class SuperThump : ModUpgrade<Luigi>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => MIDDLE;
    public override int Tier => 3;
    public override int Cost => 2000;//3500

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Super Thump'"

    public override string Description => "Luigi gains a stunning ground pound attack.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var projectile = Game.instance.model.GetTower(TowerType.BombShooter, 4, 0, 0).GetAttackModel().weapons[0].projectile.Duplicate();
        projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage *= 25;
        projectile.display = new() { guidRef = "" };
        var weapon = towerModel.GetAttackModel().Duplicate();
        weapon.weapons[0].projectile = projectile;
        weapon.weapons[0].Rate = 8;
        weapon.name = "GroundPound";
        towerModel.AddBehavior(weapon);

        foreach (var attacks in towerModel.GetAttackModels())
        {
            if (!attacks.name.Contains("Ground") && LuigiTower.AltBurnyStuff)
            {
                attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().Interval = 1.215f;
                attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage = 4f;
            }
        }
    }
}
public class Shockwaves : ModUpgrade<Luigi>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => MIDDLE;
    public override int Tier => 4;
    public override int Cost => 5000;//7000

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Pair'"

    public override string Description => "Luigi's ground pound now knocks bloons back. Attack speed and Burn DoT is improved.";
    //public override string Description => "Luigi's ground pound now shockwaves knocking back bloons. Attack speed is improved.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {


        foreach (var attacks in towerModel.GetAttackModels())
        {
            if (attacks.name.Contains("Ground"))
            {
                attacks.weapons[0].Rate *= .4f;
                //if (LuigiTower.AltKnockback)
                //{
                    //var wind = Game.instance.model.GetTower(TowerType.SuperMonkey, 0, 0, 4).GetAttackModel().weapons[0].projectile.GetBehavior<KnockbackModel>();
                    //wind.lightMultiplier += 1;
                    //wind.heavyMultiplier += 1;
                    //wind.moabMultiplier = 0;
                    //wind.lifespan = wind.Lifespan = 3;
                    //attacks.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(wind);
                //}
                //else
                //{
                    var wind = Game.instance.model.GetTower(TowerType.NinjaMonkey, 0, 2, 0).GetAttackModel().weapons[0].projectile.GetBehavior<WindModel>();
                    wind.distanceMax = 20;
                    wind.distanceMin = 2;
                    attacks.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(wind);
                //}
            }
            else
            {
                if (LuigiTower.AltBurnyStuff)
                {
                    attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().Interval = 1.2f;
                    attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage = 18f;
                }
                else 
                {

                    attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().Interval = .2f;
                    attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage = 3f;
                }
                
                attacks.weapons[0].Rate *= .5f;
            }
        }
    }
}
public class Superuigi : ModUpgrade<Luigi>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => MIDDLE;
    public override int Tier => 5;
    public override int Cost => 8500000;

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Pair'"

    public override string Description => "Luigi has grown to unstoppable powers, his fireballs can do thousands of damage per tick and his stomps now PERMANANTLY stun bloons";

    public override void ApplyUpgrade(TowerModel towerModel)
    {


        foreach (var attacks in towerModel.GetAttackModels())
        {
            if (attacks.name.Contains("Ground"))
            {
                attacks.weapons[0].Rate *= .001f;
                attacks.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = 9000000;

                //if (LuigiTower.AltKnockback)
                //{
                    //var knockback = attacks.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<KnockbackModel>();

                    //knockback.lightMultiplier = 1; //technically a reduction,
                    //knockback.heavyMultiplier = 1; //but w/o this the bloon would move in reverse PERMANANTLY,
                    //knockback.moabMultiplier = 1f; //which isnt ideal imo.
                    //knockback.lifespan += 999999;
                //}
                //else
                {
                    var wind = attacks.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<WindModel>();
                    wind.affectMoab = true;
                    wind.speedMultiplier = 0; //technically blowing-back at 0% speed, never reaching the end...
                }
            }
            else
            {
                attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().Interval = 0.001f;
                attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage = 200000f;
                attacks.weapons[0].Rate *= .015f;
                attacks.weapons[0].projectile.pierce *= 30;
                attacks.weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed *= 2;
                attacks.weapons[0].projectile.GetDamageModel().damage = 200000;
            }
            attacks.range = 150;
        }
        towerModel.range = 150;
        towerModel.AddBehavior(new OverrideCamoDetectionModel("camooo", true));
    }
}