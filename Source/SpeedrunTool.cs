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

[BepInPlugin(MOD_ID, MOD_NAME, MOD_VERSION)]
public partial class SpeedrunTool : BaseUnityPlugin {
    public const string MOD_ID = "CCshrub.SpeedrunTool";
    public const string MOD_NAME = "Speedrun Tool";
    public const string MOD_VERSION = "1.0.0";
    //tracking the instance makes it easier to use separate modules for functionality
    public static SpeedrunTool instance = null!;
    public static RainWorldGame? rainWorldGame;

    public SpeedrunToolOptions? options;

    //any items spawned tracked here so they can be destroyed
    public List<PhysicalObject> trackedObjects = new List<PhysicalObject>();

    private bool isInit;


    public void OnEnable() {
        instance = this;
        //this is the way I'm used to logging with BepInEx, not sure if Rain World does it differently?
        Log.Init(Logger);

        //add delegates
        On.RainWorldGame.ctor += RainWorldGame_ctor;

        //load remix
        options = new SpeedrunToolOptions(instance);
        On.RainWorld.OnModsInit += OnModsInit;

        isInit = true;
        Log.Info("SpeedrunTool Started");
    }
    public static void OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld raingame)
    {
        orig(raingame);
        Log.Info("Registering OI");
        MachineConnector.SetRegisteredOI(MOD_ID, instance.options);
    }
    public void OnDisable() {
        Log.Info("SpeedrunTool Stopping");
        isInit = false;

        //clean any spawned object
        foreach (PhysicalObject obj in trackedObjects) obj.Destroy();
        trackedObjects.Clear();

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
        if (Input.GetKeyDown(instance.options.SpawnKey.Value)) rainWorldGame.GetPlayer().GiveItem(MoreSlugcatsEnums.AbstractObjectType.EnergyCell);
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