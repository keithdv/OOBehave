﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OOBehave.Portal
{
    public interface ISerializer
    {
        string Serialize(object obj);
        T Deserialize<T>(string obj);
        object Deserialize(Type type, string obj);
        object Deserialize(string json);
        void Populate(string json, object target);
    }
}