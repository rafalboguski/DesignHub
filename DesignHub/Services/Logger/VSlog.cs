using System;
using System.Diagnostics;

namespace DesignHub.Services.Logger
{
    public class VSlog
    {

        public static void Write(params object[] variables)
        {
            Debug.Write(DateTime.Now);
            foreach (var x in variables)
            {
                Debug.Write("\t"+x);
            }
            Debug.WriteLine("");
        }

    }
}