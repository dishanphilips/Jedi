﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jedi
{
    public class JediFieldInfo
    {
        public FieldInfo Info { get; }

        public JediFieldInfo(FieldInfo info)
        {
            Info = info;
        }

        public void Set(object value)
        { 
        
        }
    }
}
