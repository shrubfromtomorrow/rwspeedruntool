using System;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using RWCustom;
using BepInEx;
using Debug = UnityEngine.Debug;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using IL.JollyCoop;
using BepInEx.Logging;
using HarmonyLib.Tools;

#pragma warning disable CS0618

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

/* SAFE PRACTICES FOR HOT RELOAD
 * 
 * Any delegates added will need to be removed when the mod is unloaded (either OnDisable, or in object destruction code)
 * Allowing things to be instances makes it easier to have clean setup/destruction of data
 * - For example, making sure config stuff gets run, timers get destroyed, etc
 * 
 * If you don't clean up, when you hot reload you'll duplicate delegates/objects
 * 
 */

namespace RWSRTool;

[BepInPlugin("CC+Shrub.SpeedrunTool", "SpeedrunTool", "1.0.0")]
public class RWSRTool : BaseUnityPlugin {
    public static RWSRTool Instance = null!;
    private bool isInit;
    private void OnEnable() {
        Instance = this;
        //this is the way I'm used to logging with BepInEx, not sure if Rain World does it differently?
        Log.Init(Logger);
        Log.Info("RWSRTool Started");
        isInit = true;
    }

    private void OnDisable() {
        Destroy(Instance);
        Instance = null!;
        isInit = false;
    }
}
// CC todo list
//TODO: Setup config and keybinds
//if (ModManager.MSC && base.grasps[0] != null && base.grasps[0].grabbed is EnergyCell && (base.grasps[0].grabbed as EnergyCell).usingTime > 0f && base.grasps[0].grabbed.Submersion != 0f)
//TODO: Rarefaction Cell w/ hotkey (name = EnergyCell)
//Find the instance of a cell the player is holding
//change its state to off and change cooldown to 0
//TODO: Change Rot Cycle time to 2, or to a really big number w/ hotkey
//TODO: Setup Timer Mod
//Create/Destroy collisions with button
//Setup timer logic
//Setup timer display
//save previous times with that collision
//TODO: MiniSavestates
//Save current position, and room
//Save this information to a json
//Teleport player to position/room
//TODO: Spawn items w/ hotkey
//Spawn rock, spear, grapple worm