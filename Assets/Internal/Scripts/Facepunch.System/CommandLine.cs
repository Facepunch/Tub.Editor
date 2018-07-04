using System.Collections.Generic;
using Facepunch.Extend;

namespace Facepunch
{
    public static class CommandLine
    {
        private static bool initialized = false;
        private static string commandline = "";
        private static Dictionary<string,string> switches = new Dictionary<string, string>( System.StringComparer.OrdinalIgnoreCase );

        /// <summary>
        /// Returns the full command line, reconstructed from System.Environment.GetCommandLineArgs
        /// </summary>
        public static string Full
        {
            get { Initalize(); return commandline; }
        }

        /// <summary>
        /// Sometimes you want to force a command line instead of using the actual command line.
        /// To do that just call this function with the values you want "+my values -etc"
        /// </summary>
        public static void Force( string val )
        {
            commandline = val;
            initialized = false;
        }

        //
        // 
        //
        private static void Initalize()
        {
            if ( initialized ) return;
            initialized = true;

            //
            // If we already have a command line it's probably been set by Force()
            //
            if ( commandline == "" )
            {
                //
                // We can't use System.Environment.CommandLine because it removes quotes
                // (we could parse straight from this but we want Force to be passed in as a string)
                //
                string[] args = System.Environment.GetCommandLineArgs();
                foreach ( string arg in args )
                {
                    commandline += "\"" + arg + "\" ";
                }
            }

            // No command line - we can bail
            if ( commandline == "" ) return;

            //
            // Parses the command line
            //
            string strKey = "";
            var parts = commandline.SplitQuotesStrings();

            //
            // For each part of the command line
            //
            foreach ( string var in parts )
            {
                if ( var.Length == 0 ) continue;

                //
                // if it starts with + or - it's a switch
                //
                if ( var[0] == '-' || var[0] == '+' )
                {
                    //
                    // We have a key with no switch
                    //
                    if ( strKey != "" && !switches.ContainsKey( strKey ) )
                    {
                        switches.Add( strKey, "" );
                    }

                    strKey = var;
                    continue;
                }

                //
                // We are holding a switch and have a key - so fill it in.
                //
                if ( strKey != "" )
                {
                    if ( !switches.ContainsKey( strKey ) )
                    {
                        switches.Add( strKey, var );
                    }

                    strKey = "";
                }
            }

            //
            // The command line ended with a switch with no value, so add it.
            //
            if ( strKey != "" && !switches.ContainsKey( strKey ) )
            {
                switches.Add( strKey, "" );
            }
        }


        /// <summary>
        /// if ( HasSwitch( "-console" ) ) EnableConsole();
        /// </summary>
        public static bool HasSwitch( string strName )
        {
            Initalize();

            return switches.ContainsKey( strName );
        }

        //
        // map = GetSwitch( "+map", "de_dust" );
        //
        public static string GetSwitch( string strName, string strDefault )
        {
            Initalize();

            string strValue = "";
            if ( !switches.TryGetValue( strName, out strValue ) )
            {
                return strDefault;
            }

            return strValue;
        }

        //
        // maxplayers = GetSwitchInt( "+maxplayers", 32 );
        //
        public static int GetSwitchInt( string strName, int iDefault )
        {
            Initalize();

            string strValue = "";
            if ( !switches.TryGetValue( strName, out strValue ) )
            {
                return iDefault;
            }

            int outval = iDefault;
            if ( !int.TryParse( strValue, out outval ) )
            {
                return iDefault;
            }

            return outval;
        }

        /// <summary>
        /// Returns all command line values
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetSwitches()
        {
            Initalize();

            return switches;
        }
    }
}