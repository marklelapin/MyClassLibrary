﻿using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    public class SaveAndGetTestUpdateContent : ISaveAndGetTestContent<TestUpdate>
    {


        public SaveAndGetTestUpdateContent()
        {

        }

        public Guid CopyId { get { return TestContent.CopyId; } }

        public List<TestUpdate> getNewUpdates()
        {
           return TestContent.GetNewUpdatesWithoutUpdatedOnServerDates();
        }
    }
}

       
