using TechArt.Base.Interfaces;

namespace TechArt.Base
{
    public class TFramework
    {
        //
        public void Initialize()
        {

        }

        public static IConfig Config { get; set; }

        public static IDebugger Debugger { get; set; }
    }
}