using System;

namespace Tub.InputOutput
{
    [System.Serializable]
    public class Output
    {
        public OutputTarget[] OutputTargets;

        internal void StateChanged( bool bstate )
        {
            if ( OutputTargets == null ) return;

            foreach ( var target in OutputTargets )
            {
                target.OnOutputState( bstate );
            }
        }
    }

}