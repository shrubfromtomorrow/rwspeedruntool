using System;
using System.Collections.Generic;
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
using IL;
using On;
using System.Data.SqlClient;
using MoreSlugcats;

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

namespace SpeedrunTool;

[BepInPlugin("RainWorld.SpeedrunTool", "SpeedrunTool", "1.0.0")]
public partial class SpeedrunTool : BaseUnityPlugin {

    //tracking the instance makes it easier to use separate modules for functionality
    public static SpeedrunTool instance = null!;
    public static RainWorldGame rainWorldGame;

    /// <summary>
    /// any items spawned tracked here so they can be destroyed, persist determines if we should destroy them
    /// </summary>
    public List<(PhysicalObject obj, bool persist)> trackedObjects = new List<(PhysicalObject, bool)>();

    private bool isInit;

    public void OnEnable() {
        instance = this;
        //this is the way I'm used to logging with BepInEx, not sure if Rain World does it differently?
        Log.Init(Logger);

        //add delegates
        On.RainWorldGame.ctor += RainWorldGame_ctor;

        isInit = true;
        Log.Info("SpeedrunTool Started");
    }
    public void OnDisable() {
        Log.Info("SpeedrunTool Stopping");
        isInit = false;

        //destroy nonpersisting object
        foreach (var trackedObject in trackedObjects) if (!trackedObject.persist) trackedObject.obj.Destroy(); 

        //remove delegates
        On.RainWorldGame.ctor -= RainWorldGame_ctor;

        Destroy(instance);
    }
    private void RainWorldGame_ctor(On.RainWorldGame.orig_ctor orig, RainWorldGame self, ProcessManager manager)
    {
        orig(self, manager);
        rainWorldGame = self;
    }
    public void Update() {

        //test function
        if (Input.GetKeyDown(KeyCode.Alpha3)) rainWorldGame.GetPlayer().GiveItem(MoreSlugcatsEnums.AbstractObjectType.EnergyCell);
    }
}
// CC todo list
//TODO: Setup config and keybinds
//TODO: Rarefaction Cell w/ hotkey
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