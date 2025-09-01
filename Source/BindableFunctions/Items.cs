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


namespace SpeedrunTool
{
    public static partial class BindableFunctions
    {

        [BindableMethod(name = "Spawn Rarefaction Cell", category = "Items")]
        public static void SpawnRarefactionCell()
        {
            SpeedrunTool.rainWorldGame.GetPlayer().GiveItem(MoreSlugcatsEnums.AbstractObjectType.EnergyCell);
        }

        public class GlobalSettings
        {
        }

    }
}
