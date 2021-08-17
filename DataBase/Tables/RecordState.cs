﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.DataBase.Tables
{
    public class RecordState: StateBase
    {
        public List<Record> Records { get; set; } = new List<Record>();
    }
}