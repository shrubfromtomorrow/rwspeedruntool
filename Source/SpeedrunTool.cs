using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using System.Reflection;
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
using System.Dynamic;
using HarmonyLib;
using Rewired;
using System.Runtime.Hosting;
using System.IO;
using System.Linq;
using SpeedrunTool;

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
public partial class SpeedrunTool : BaseUnityPlugin
{
    public const string MOD_ID = "CCshrub.SpeedrunTool";
    public const string MOD_NAME = "Speedrun Tool";
    public const string MOD_VERSION = "1.0.0";
    //tracking the instance makes it easier to use separate modules for functionality
    public static SpeedrunTool? instance = null!;
    public static RainWorldGame?  rainWorldGame;
    public SpeedrunToolOptions? options;
    public readonly string ModBasePath = Application.persistentDataPath + Path.DirectorySeparatorChar.ToString() + "SpeedrunTool";

    //Settings
    public static GlobalSettings settings = new GlobalSettings();
    public SaveSettings LocalSaveData { get; set; } = new SaveSettings();

    //Bindable Functions
    internal static Dictionary<string, (string category, Action method)> bindMethods = new();

    //These are all Hk's hooks for loading settings, we'll need to make our own
    /// <summary>
    /// loads global settings
    /// </summary>
    /// <param name="s">GlobalSettings </param>
    public void OnLoadGlobal(GlobalSettings s)
    {
        SpeedrunTool.settings = s;
        if (settings.binds is null)
        {
            settings.binds = new();
            settings.binds = Keybinds.ResetToDefaults(settings.binds);
        }
    }
    public GlobalSettings OnSaveGlobal() => settings;
    public void OnLoadLocal(SaveSettings s) => this.LocalSaveData = s;
    public SaveSettings OnSaveLocal() => this.LocalSaveData;

    /// <summary>
    /// any items spawned tracked here so they can be destroyed, persist determines if we should destroy them
    /// </summary>
    public List<(PhysicalObject obj, bool persist)> trackedObjects = new List<(PhysicalObject, bool)>();

    private bool isInit;

    public void OnEnable()
    {
        instance = this;
        Log.Init(Logger);

        //Clear each method in bindMethods because its static, grab every bindable function
        //HK logs the time it takes, might've been substantial at some point?
        float startTime = Time.realtimeSinceStartup;
        Log.Info("Building Bindable Method Dictionary");
        bindMethods.Clear();
        foreach (MethodInfo method in typeof(BindableFunctions).GetMethods(BindingFlags.Public | BindingFlags.Static))
        {
            object[] attributes = method.GetCustomAttributes(typeof(BindableMethod), false);

            if (attributes.Any())
            {
                BindableMethod attr = (BindableMethod)attributes[0];
                string name = attr.name;
                string cat = attr.category;

                bindMethods.Add(name, (cat, (Action)Delegate.CreateDelegate(typeof(Action), method)));
            }
        }

        Log.Info("Done! Time taken: " + (Time.realtimeSinceStartup - startTime) + "s. Found " + bindMethods.Count + " methods");

        if (settings.FirstRun)
        {
            Log.Info("First run detected, setting default binds");

            settings.FirstRun = false;
            //Keybinds.ResetToDefaults(settings.binds);
        }
        
        if (!Directory.Exists(ModBasePath)) Directory.CreateDirectory(ModBasePath);


        //add delegates
        On.RainWorldGame.ctor += RainWorldGame_ctor;

        //load remix
        options = new SpeedrunToolOptions();
        On.RainWorld.OnModsInit += OnModsInit;


        isInit = true;
        Log.Info("SpeedrunTool Started At "+ DateTime.Now);
    }
    public static void OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld raingame)
    {
        orig(raingame);
        Log.Info("Registering OI");
        if (MachineConnector.GetRegisteredOI(MOD_ID) != null) MachineConnector.SetRegisteredOI(MOD_ID, instance?.options);
    }
    public void OnDisable()
    {
        Log.Info("SpeedrunTool Stopping");
        isInit = false;

        //destroy nonpersisting objects
        foreach (var trackedObject in trackedObjects) if (!trackedObject.persist) trackedObject.obj.Destroy();
        //this should be unnecessary, the instance is destroyed and the list isnt static
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
    public void Update()
    {

        //test function
        //if (Input.GetKeyDown((instance?.options?.Spawn Rarefaction Cell.Value) ?? KeyCode.None)) BindableFunctions.SpawnRarefactionCell();
        (string, Action) methodData;
        string bindName = "Spawn Rarefaction Cell";

        if (Input.GetKeyDown(settings.binds[bindName]))
        {
            bindMethods.TryGetValue(bindName, out methodData);
            methodData.Item2.Invoke();
        }

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

// Shrub todo list
//TODO: Create basic UI panel to house options
// Create basic tabs and add certain options (cycle length suspension, etc.)
//TODO: Create selection mode for timing - shortcuts vs tiles
//TODO: Setup kebinds setting option in ui panel (tie to json?)