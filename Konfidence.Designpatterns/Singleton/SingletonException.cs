﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Konfidence.DesignPatterns.Singleton
{
    internal class SingletonException : Exception
    {
        public SingletonException(string message) : base(message) { }
    }
}