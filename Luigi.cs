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
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using UnityEngine.Assertions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;

[assembly: MelonInfo(typeof(Luigi.LuigiTowerAlt), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
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
public class LuigiTowerAlt : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<LuigiTowerAlt>("Alt Luigi loaded!");
    }
}
public class LuigiAlt : ModTower
{
    public override TowerSet TowerSet => TowerSet.Magic;
    public override string BaseTower => TowerType.DartMonkey;
    public override int Cost => 650;
    public override int TopPathUpgrades => 5;
    public override int MiddlePathUpgrades => 3;
    public override int BottomPathUpgrades => 4;
    public override string Description => "Luigi is here in BTD6";

    public override bool Use2DModel => true;
    public override string Icon => "LuigiIcon";

    public override string Portrait => "LuigiIcon";

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        //towerModel.GetAttackModel().weapons[0].projectile = Game.instance.model.GetTower(TowerType.MonkeySub).GetAttackModel().weapons[0].projectile.Duplicate();
        var attackModel = towerModel.GetBehavior<AttackModel>();
        towerModel.GetAttackModel().weapons[0].projectile = Game.instance.model.GetTower(TowerType.WizardMonkey).GetAttackModel().weapons[0].projectile.Duplicate();
        towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple; // CAN POP LEAD
        towerModel.GetAttackModel().weapons[0].projectile.ApplyDisplay<FireBallDisplay>();
        
    }

    public override string Get2DTexture(int[] tiers)
    {
        return "LuigiDisplay";
    }
}
public class GuidedFireballs : ModUpgrade<LuigiAlt>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => TOP;
    public override int Tier => 1;
    public override int Cost => 150;

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Pair'"

    // public override string Description => "Fireballs now light bloons on fire upon landing";
    public override string Description => "Fireballs now seek out Bloons";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var attackModel = towerModel.GetAttackModel();
        var projectile = attackModel.weapons[0].projectile;
        attackModel.weapons[0].projectile.collisionPasses = new int[] { -1, 0, 1 };
        attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("WizardMonkey-100").GetAttackModel().weapons[0].projectile.GetBehavior<TrackTargetModel>().Duplicate());
        //foreach (var beh in Game.instance.model.GetTowerFromId("WizardMonkey-100").GetAttackModel().weapons[0].projectile.GetBehaviors<AddBehaviorToBloonModel>())
        //{
        //    attackModel.weapons[0].projectile.AddBehavior(beh.Duplicate());
        //}
        //towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
    }
}
public class FlameBlast : ModUpgrade<LuigiAlt>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => TOP;
    public override int Tier => 2;
    public override int Cost => 500;

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Pair'"

    // public override string Description => "Fireballs now light bloons on fire upon landing";
    public override string Description => "Luigi's fireballs now light Bloons on fire upon landing";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        //
        //var FireL = Game.instance.model.GetTower(TowerType.MortarMonkey, 0, 0, 2).GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>();
        //towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(FireL);
        // towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<DamageOverTimeModel>().Interval = 0.4f;
        // attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().Interval = 0.4f;
        // towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new[] { -1, 0 };
        //towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new[] { 1, 0 };
        //

        foreach (var weaponModel in towerModel.GetWeapons())
        {
            var fire = Game.instance.model.GetTower(TowerType.MortarMonkey, 0, 0, 2).GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>();
            //var fire = Game.instance.model.GetTowerFromId("MortarMonkey-002").Duplicate<TowerModel>().GetBehavior<AttackModel>().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>();
            // AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
            //var projectile = attackModel.weapons[0].projectile;
            //towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(fire);
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(fire);
            //towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.collisionPasses = new int[] { 0, -1 };
            towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new[] { 1, 0 };
        }
    }
}

public class HotterFireballs : ModUpgrade<LuigiAlt>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => TOP;
    public override int Tier => 3;
    public override int Cost => 1200;

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Pair'"

    // public override string Description => "Fireballs now light bloons on fire upon landing";
    public override string Description => "Luigi's fireballs now deal more damage";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        //
        //var FireL = Game.instance.model.GetTower(TowerType.MortarMonkey, 0, 0, 2).GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>();
        //towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(FireL);
        // towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<DamageOverTimeModel>().Interval = 0.4f;
        // attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().Interval = 0.4f;
        // towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new[] { -1, 0 };
        //towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new[] { 1, 0 };
        //

        foreach (var weaponModel in towerModel.GetWeapons())
        {
            var fire = Game.instance.model.GetTower(TowerType.MortarMonkey, 3, 0, 2).GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>();
            //var fire = Game.instance.model.GetTowerFromId("MortarMonkey-002").Duplicate<TowerModel>().GetBehavior<AttackModel>().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>();
            // AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
            //var projectile = attackModel.weapons[0].projectile;
            //towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(fire);
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(fire);
            //towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.collisionPasses = new int[] { 0, -1 };
            towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new[] { -1, 0 };
            towerModel.GetBehavior<AttackModel>().weapons[0].projectile.GetDamageModel().damage += 1;
        }
        
        //{
        //    var fire = Game.instance.model.GetTower(TowerType.MortarMonkey, 3, 0, 2).GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>();
        //    //var fire = Game.instance.model.GetTowerFromId("MortarMonkey-302").Duplicate<TowerModel>().GetBehavior<AttackModel>().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>();
        //    AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
        //    var projectile = attackModel.weapons[0].projectile;
        //    projectile.GetDamageModel().damage += 1;
        //    attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(fire);
        //    attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.collisionPasses = new int[] { 0, -1 };
        //}

    }
}

public class PerishingFireballs : ModUpgrade<LuigiAlt>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => TOP;
    public override int Tier => 4;
    public override int Cost => 2000;

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Pair'"

    // public override string Description => "Fireballs now light bloons on fire upon landing";
    public override string Description => "Luigi's fireballs now deal more damage";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var weaponModel in towerModel.GetWeapons())
        {
            var fire = Game.instance.model.GetTower(TowerType.MortarMonkey, 4, 0, 2).GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>();
            //var fire = Game.instance.model.GetTowerFromId("MortarMonkey-002").Duplicate<TowerModel>().GetBehavior<AttackModel>().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>();
            // AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
            //var projectile = attackModel.weapons[0].projectile;
            //towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(fire);
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(fire);
            //towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.collisionPasses = new int[] { 0, -1 };
            towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new[] { -1, 0 };
            //towerModel.GetBehavior<AttackModel>().weapons[0].projectile.GetDamageModel().damage += 1;

        }
        //foreach (var weaponModel in towerModel.GetWeapons())
        //{
            //weaponModel.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        //}
        
        // attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("NinjaMonkey-020").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate());
        // attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("NinjaMonkey-020").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModifierForTagModel>().Duplicate());
        //foreach (var beh in Game.instance.model.GetTower(TowerType.Alchemist, 0, 2, 0).GetAttackModel().weapons[0].projectile.GetBehaviors<AddBehaviorToBloonModel>())
        //{
        //    attackModel.weapons[0].projectile.AddBehavior(beh.Duplicate());
        //}
        //towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
    }
}
public class BlooncineratingFireballs : ModUpgrade<LuigiAlt>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => TOP;
    public override int Tier => 5;
    public override int Cost => 50000;

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Pair'"

    // public override string Description => "Fireballs now light bloons on fire upon landing";
    public override string Description => "Luigi's fireballs now deal more damage";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var weaponModel in towerModel.GetWeapons())
        {
            var fire = Game.instance.model.GetTower(TowerType.MortarMonkey, 0, 0, 5).GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>();
            //var fire = Game.instance.model.GetTowerFromId("MortarMonkey-002").Duplicate<TowerModel>().GetBehavior<AttackModel>().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>();
            // AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
            //var projectile = attackModel.weapons[0].projectile;
            //towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(fire);
            //towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(fire);
            //towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.collisionPasses = new int[] { 0, -1 };
            //towerModel.GetAttackModel().weapons[0].projectile.collisionPasses = new[] { -1, 0 };
            towerModel.GetBehavior<AttackModel>().weapons[0].projectile.GetDamageModel().damage += 3;
        }
    }
}
public class LighterFireballs : ModUpgrade<LuigiAlt>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => MIDDLE;
    public override int Tier => 1;
    public override int Cost => 1000;

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Pair'"

    public override string Description => "Lighter fireballs allow much faster firing";

    public override void ApplyUpgrade(TowerModel towerModel)
    {

        towerModel.GetAttackModel().weapons[0].Rate *= .4f;
    }
}
public class LuigiSense : ModUpgrade<LuigiAlt>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => BOTTOM;
    public override int Tier => 2;
    public override int Cost => 1000;

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Pair'"

    public override string Description => "Luigi see Camo Bloons";

    public override void ApplyUpgrade(TowerModel towerModel)
    {

        towerModel.AddBehavior(new OverrideCamoDetectionModel("camooo", true));
        towerModel.range += 5f;
    }
}
public class ShimmeringFireballs : ModUpgrade<LuigiAlt>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => BOTTOM;
    public override int Tier => 3;
    public override int Cost => 1000;

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Pair'"

    public override string Description => "Luigi's fireballs strip camo properties from Bloons and can hit more Bloons";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var weaponModel in towerModel.GetWeapons())
        {
            //weaponModel.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        }
        var attackModel = towerModel.GetAttackModel();
        var projectile = attackModel.weapons[0].projectile;
        attackModel.weapons[0].projectile.collisionPasses = new int[] { -1, 0, 1 };
        attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("NinjaMonkey-020").GetAttackModel().weapons[0].projectile.GetBehavior<RemoveBloonModifiersModel>().Duplicate());
        // attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("NinjaMonkey-020").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate());
        // attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("NinjaMonkey-020").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModifierForTagModel>().Duplicate());
        foreach (var beh in Game.instance.model.GetTowerFromId("NinjaMonkey-020").GetAttackModel().weapons[0].projectile.GetBehaviors<AddBehaviorToBloonModel>())
        {
            attackModel.weapons[0].projectile.AddBehavior(beh.Duplicate());
        }
        towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

        //if (towerModel.tier < 4)
        //{
        //    projectile.ApplyDisplay<PeppermintDisplayPurple>();
        //
        //}
    }
}
public class GoldenFireballs : ModUpgrade<LuigiAlt>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => BOTTOM;
    public override int Tier => 4;
    public override int Cost => 0;

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Pair'"

    public override string Description => "Placeholder";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var weaponModel in towerModel.GetWeapons())
        {
            //weaponModel.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
        }
        var attackModel = towerModel.GetAttackModel();
        var projectile = attackModel.weapons[0].projectile;
        attackModel.weapons[0].projectile.collisionPasses = new int[] { -1, 0, 1 };
        attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("NinjaMonkey-020").GetAttackModel().weapons[0].projectile.GetBehavior<RemoveBloonModifiersModel>().Duplicate());
        // attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("NinjaMonkey-020").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate());
        // attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("NinjaMonkey-020").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModifierForTagModel>().Duplicate());
        foreach (var beh in Game.instance.model.GetTowerFromId("NinjaMonkey-020").GetAttackModel().weapons[0].projectile.GetBehaviors<AddBehaviorToBloonModel>())
        {
            attackModel.weapons[0].projectile.AddBehavior(beh.Duplicate());
        }
        towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
    }

}
public class LongRangeLuigi : ModUpgrade<LuigiAlt>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => BOTTOM;
    public override int Tier => 1;
    public override int Cost => 1000;

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Pair'"

    public override string Description => "Longer range";

    public override void ApplyUpgrade(TowerModel towerModel)
    {

        towerModel.range += 25f;
        //attackModel.range += 25f;
    }
}
public class SuperThump : ModUpgrade<LuigiAlt>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => MIDDLE;
    public override int Tier => 2;
    public override int Cost => 3900;//3500

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Pair'"

    public override string Description => "Luigi gains a ground pound attack";

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
    }
}
public class Shockwaves : ModUpgrade<LuigiAlt>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => MIDDLE;
    public override int Tier => 3;
    public override int Cost => 7000;

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Pair'"

    public override string Description => "Luigis ground pound now shockwaves knocking back bloons";

    public override void ApplyUpgrade(TowerModel towerModel)
    {


        foreach (var attacks in towerModel.GetAttackModels())
        {
            if (attacks.name.Contains("Ground"))
            {
                
                attacks.weapons[0].Rate *= .4f;
                var wind = Game.instance.model.GetTower(TowerType.NinjaMonkey, 0, 2, 0).GetAttackModel().weapons[0].projectile.GetBehavior<WindModel>();
                wind.distanceMax = 20;
                wind.distanceMin = 2;   
                attacks.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(wind);
            }
           // else
            //{
                //attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().Interval = .2f;
                //attacks.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage = 3f;
                //attacks.weapons[0].Rate *= .5f;
           // }
        }
    }
}
/*
public class Superuigi : ModUpgrade<LuigiAlt>
{
    // public override string Portrait => "Don't need to override this, using the default of Pair-Portrait.png";
    // public override string Icon => "Don't need to override this, using the default of Pair-Icon.png";
    public override string Portrait => "LuigiIcon";
    public override int Path => TOP;
    public override int Tier => 4; // 5
    public override int Cost => 8500000;

    // public override string DisplayName => "Don't need to override this, the default turns it into 'Pair'"

    public override string Description => "Luigi has grown to unstoppable powers, his fireball can do thousands of damage per tick and his stomps now PERMANANTLY stun bloons";

    public override void ApplyUpgrade(TowerModel towerModel)
    {


        foreach (var attacks in towerModel.GetAttackModels())
        {
            if (attacks.name.Contains("Ground"))
            {
                attacks.weapons[0].Rate *= .001f;
                attacks.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = 9000000;
                //if (attacks.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce == 0000) // 53X
                //{
                //    var wind = attacks.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<WindModel>();
                //    wind.affectMoab = true;
                //    wind.speedMultiplier = 0;
                //}
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
*/