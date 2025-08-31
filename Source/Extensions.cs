using System;
using System.Linq;
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
using IL.MoreSlugcats;
using On.JollyCoop;
using DevInterface;
using JetBrains.Annotations;
using IL.DevInterface;

namespace SpeedrunTool;

/// <summary>
/// This class contains a library of various game class extensions
/// </summary>
internal static class Extensions {

    //All methods in this class are extensions for game objects

    /// <summary>
    /// RainWorldGame Extension - returns a Player with given index
    /// </summary>
    /// <param name="game">Instance of game</param>
    /// <param name="playerIndex">Index of player, default is 0</param>
    /// <returns>Player</returns>
    public static Player GetPlayer(this RainWorldGame game, int playerIndex = 0) =>
        game.Players[playerIndex].realizedCreature as Player;

    /// <summary>
    /// Player Extension - returns a PhysicalObject[] array of held items
    /// </summary>
    /// <param name="player">Player Instance</param>
    /// <returns>PhysicalObject[]</returns>
    public static PhysicalObject[] GetHeldObjects(this Player player) => player.grasps
        .Where(g => g != null)
        .Select(g => g.grabbed)
        .ToArray();

    /// <summary>
    /// Player Extension - returns a PhysicalObject thats held at index
    /// </summary>
    /// <param name="player">Player Instance</param>
    /// <param name="graspIndex">Index of held objects</param>
    /// <returns>PhysicalObject</returns>
    public static PhysicalObject GetHeldObject(this Player player, int graspIndex) => player.grasps[graspIndex].grabbed;

    /// <summary>
    /// Player Extension = returns first PhysicalObject held of Type T
    /// </summary>
    /// <typeparam name="T">Type Of Object</typeparam>
    /// <param name="player">Player Instance</param>
    /// <returns>(T)PhysicalObject</returns>
    public static T? GetHeldObject<T>(this Player player) where T : PhysicalObject {
        foreach (PhysicalObject x in player.GetHeldObjects()) {
            if (x.GetType() == typeof(T)) 
                return (T)x;
        }
        return null;
    }

    /// <summary>
    /// Places item in free hand, drops items until hand is free
    /// </summary>
    /// <param name="player">Player Instance</param>
    /// <param name="type">Enum Type of AbstractObject</param>
    /// <param name="graspIndex">Index of preferred grasp</param>
    /// <param name="persist">Should SpeedrunTool remove it OnDisable</param>
    /// <param name="handSwapGlitch">Cause handSwapGlitch</param>
    /// <returns>PhysicalObject</returns>
    public static PhysicalObject GiveItem(this Player player, AbstractPhysicalObject.AbstractObjectType type, bool handSwapGlitch = false, int graspIndex = -1, bool persist = false) {
        try {
            int i = -1;
            int g = -1;
            PhysicalObject item = Helpers.SpawnItem(player.room, player.mainBodyChunk.pos, type, persist);
            if (player.HeavyCarry(item) && !handSwapGlitch) {
                for (int j = 0; j < player.grasps.Length; j++) {
                    player.grasps[j].Release();
                }
            }

            else while (player.FreeHand() < 0) {
                if (graspIndex > -1 && player.grasps[graspIndex].grabbed != null) {
                    player.grasps[graspIndex].Release();
                }
                else {
                    i++;
                    player.grasps[i].Release();
                }
            }

            if (graspIndex > -1) g = graspIndex;
            else g = player.FreeHand();
            player.SlugcatGrab(item, g);
            return player.grasps[g].grabbed;
        }
        catch (Exception e) {
            Log.Error(e.ToString());
            return null;
        }
    }
}


