﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityProject.ViewModels
{
    public class QualificationVM
    {
        public int QualificationID { get; set; }
        public string QualificationName { get; set; }
        public bool Assigned { get; set; }
    }
}