using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    static public class UnitRelations
    {
        //Buffs for attacking army
        static public List<List<float>> AttackerBuffs { get; } = new List<List<float>>() {
                                                          new List<float>() { 0, -0.05f, -0.15f, 0, -0.2f, -0.05f },            //SwordAndShieldBuffs
                                                          new List<float>() { -0.05f, -0.05f, -0.15f, 0, 0.3f, -0.05f },        //SpearAndShieldBuffs
                                                          new List<float>() { -0.1f, -0.15f, -0.25f, -0.2f, 0.35f, 0.05f },      //PikeBuffs
                                                          new List<float>() { -0.1f, -0.1f, 0.1f, 0.1f, 0.35f, 0.05f },         //ArcherBuffs
                                                          new List<float>() { 0.25f, -0.15f, -0.7f, 0.4f, 0f, 0.3f },           //CavalaryBuffs
                                                          new List<float>() { 0.25f, 0.25f, 0.1f, 0.4f, 0.1f, 0.1f },   };      //StrikeBuffs


        //Buffs for defending army
        static public List<List<float>> DefenderBuffs { get; } = new List<List<float>>() {
                                                          new List<float>() { 0, 0.1f, 0.1f, 0.1f, -0.2f, -0.2f },              //SwordAndShieldBuffs
                                                          new List<float>() { 0.05f, 0.05f, 0.1f, 0.1f, 0.3f, -0.25f },         //SpearAndShieldBuffs
                                                          new List<float>() { 0.25f, 0.25f, 0.15f, -0.2f, 0.35f, 0.1f },        //PikeBuffs
                                                          new List<float>() { 0.1f, 0.1f, 0.2f, 0.2f, 0.35f, 0.25f },           //ArcherBuffs
                                                          new List<float>() { 0, -0.15f, -0.7f, 0.1f, 0f, -0.1f },              //CavalaryBuffs
                                                          new List<float>() { 0.1f, 0.05f, 0f, 0.05f, -0.2f, -0.15f },   };     //StrikeBuffs
    }
}
