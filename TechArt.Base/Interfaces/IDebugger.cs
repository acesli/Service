using System;

namespace TechArt.Base.Interfaces
{
    public interface IDebugger
    {
        void AddMsg(string message);

        void AddError(Exception exception);
    }
}