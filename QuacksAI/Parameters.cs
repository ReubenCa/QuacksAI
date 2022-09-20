using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuacksAI
{
    public static class Parameters
    {
        /// <summary>
        /// When true all AIs share a single cache of positions that is persistenet for the entire program
        /// </summary>
        public const  bool SharedCaching = false;
        public const bool Caching = true;
        public const bool AutoBrewWhenCantDie = false;

        //TODO: add a ToString method so that parameters can be included in logs
    }
}
