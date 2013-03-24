using System;

namespace Konfidence.DesignPatterns.Singleton
{
    internal class SingletonException : Exception
    {
        public SingletonException(string message) : base(message) { }
    }
}
