// Ignore Spelling: Habitualize

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Habitualize.Services;
using System.Text.Json.Serialization;

namespace Habitualize.Model
{
    public class Gardening : HabitTemplate
    {
        public bool PlantIsWatered { get; set; }

    }
}
