using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    static public class UnitRelations
    {
        static List<List<float>> AttackerBuffs { get; } = new List<List<float>>() {
                                                          new List<float>() { 0, -0.05f, -0.15f, 0, -0.2f, -0.05f },
                                                          new List<float>() { -0.05f, -0.05f, -0.15f, 0, 0.3f, -0.05f },
                                                          new List<float>() { -0.1f, -0.15f, -0.25f, -0.2f, 0.7f, 0.05f },
                                                          new List<float>() { -0.1f, -0.1f, 0.1f, 0.1f, 0.7f, 0.05f },

                                                                                                                                };
    }
}
