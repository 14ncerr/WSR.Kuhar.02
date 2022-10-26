﻿using System;
using System.Collections.Generic;

namespace Kuhar.Lopushok.Domain.Entities
{
    public partial class AgentPriorityHistory
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public DateTime ChangeDate { get; set; }
        public int PriorityValue { get; set; }

        public virtual Agent Agent { get; set; } = null!;
    }
}
