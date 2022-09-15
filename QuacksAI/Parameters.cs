using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuacksAI
{
    internal static class Parameters
    {
        /// <summary>
        /// When true all AIs share a single cache of positions that is persistenet for the entire program
        /// </summary>
        static public bool SharedCaching = true;
    }
}
