﻿using Blaise.Case.Nisra.Processor.Tests.Behaviour.Enums;

namespace Blaise.Case.Nisra.Processor.Tests.Behaviour.Models
{
    public class CaseModel
    {
        public CaseModel(string primaryKey, string outcome, ModeType mode)
        {
            PrimaryKey = primaryKey;
            Outcome = outcome;
            Mode = mode;
        }

        public string PrimaryKey { get; set; }

        public string Outcome { get; set; }

        public ModeType Mode { get; set; }
    }
}