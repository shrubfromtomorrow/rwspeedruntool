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

    //TODO: Fix this launching the player on room exit if not regrasped
    //      Maybe this should be renamed changed object?
    /// <summary>
    /// Replaces an item in hand with another item.
    /// </summary>
    /// <param name="player">Player Instance</param>
    /// <param name="type">Enum Type of AbstractObject</param>
    /// <param name="graspIndex">index of held objects</param>
    /// <returns>PhysicalObject</returns>
    public static PhysicalObject GiveItem(this Player player, AbstractPhysicalObject.AbstractObjectType type, int graspIndex = 0) {
        PhysicalObject item = Helpers.SpawnItem(player.room, player.mainBodyChunk.pos, type);
        player.grasps[graspIndex].grabbed.Destroy();
        player.grasps[graspIndex].grabbed = item;
        return player.grasps[graspIndex].grabbed;
    }
}


