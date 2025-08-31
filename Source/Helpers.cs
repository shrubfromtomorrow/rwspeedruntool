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

namespace SpeedrunTool {
    internal static class Helpers {
        /// <summary>
        /// Spawns an item, tracks it, and returns the realized version
        /// </summary>
        /// <param name="room">Room to spawn in</param>
        /// <param name="pos">Pos to spawn at</param>
        /// <param name="type">Enum type of object to spawn</param>
        /// <returns>PhysicalObject</returns>
        public static PhysicalObject SpawnItem(Room room, Vector2 pos, AbstractPhysicalObject.AbstractObjectType type) {
            try {
                RainWorldGame game = SpeedrunTool.rainWorldGame;
                World world = game.world;
                AbstractPhysicalObject _item = new AbstractPhysicalObject(world, type, null, room.GetWorldCoordinate(pos), game.GetNewID());
                _item.RealizeInRoom();
                PhysicalObject item = _item.realizedObject;
                SpeedrunTool.instance.trackedObjects.Add(item);
                return item;
            }
            catch (Exception e) {
                Log.Error(e.ToString());
                return null;
            }
        }
    }
}
