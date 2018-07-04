using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Facepunch
{
    public static class EventFactory
    {
        static Dictionary<string, EventBase> All;

        static string[] identifiers;
        public static string[] Identifiers { get { Init(); return identifiers; } }

        static string[] displayNames;
        public static string[] DisplayNames { get { Init(); return displayNames; } }


        static void Init()
        {
            if ( All != null )
                return;

            var events = System.AppDomain.CurrentDomain.GetAssemblies()
                                        .SelectMany( x => x.GetTypes().Where( y => y.IsClass && y.BaseType == typeof( EventBase ) ) )
                                        .Select( x => Activator.CreateInstance( x ) as EventBase )
                                        .ToArray();

            identifiers = events.Select( x => x.Identifier ).ToArray();
            displayNames = events.Select( x => x.DisplayName ).ToArray();

            All = events.ToDictionary( x => x.Identifier, x => x );
        }

        internal static EventBase Get( string key )
        {
            Init();

            EventBase eb = null;

            if ( !All.TryGetValue( key, out eb ) )
                return null;

            return eb;
        }
    }

}