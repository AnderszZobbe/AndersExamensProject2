﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_layer.DataClasses
{
    public class WorkteamData : Workteam
    {
        internal WorkteamData(string foreman) : base(foreman)
        {
        }
    }
}
