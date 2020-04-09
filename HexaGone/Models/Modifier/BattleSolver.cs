using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models.Modifier
{
    static public class BattleSolver
    {
        static Random RandomGenerator = new Random(Guid.NewGuid().GetHashCode());

        static public String test { get; set; }
        static public void Solve(ref Army defenderArmy, ref Army attackerArmy)
        {
            //update defender stats
            float TotalAttackDefender = 0;
            float TotalDefenseDefender = 0;
            foreach (Unit n in defenderArmy.Units)
            {
                int CurrentUnitClass = n.UnitClass;
                float totalBuff = 0;
                foreach (Unit m in attackerArmy.Units)
                {
                    totalBuff += UnitRelations.DefenderBuffs[CurrentUnitClass][m.UnitClass] / attackerArmy.Units.Count;
                }
                TotalAttackDefender += n.MenCount * n.AttackStat * (1 + totalBuff);
                TotalDefenseDefender += n.MenCount *  n.DefenseStat * (1 + totalBuff);
                
            }
            float OriginalDefenseDefender = TotalDefenseDefender;

            //update attacker stats
            float TotalAttackAttacker = 0;
            float TotalDefenseAttacker = 0;
            foreach (Unit n in attackerArmy.Units)
            {
                int CurrentUnitClass = n.UnitClass;
                float totalBuff = 0;
                foreach (Unit m in defenderArmy.Units)
                {
                    totalBuff += UnitRelations.AttackerBuffs[CurrentUnitClass][m.UnitClass] / defenderArmy.Units.Count;
                }
                TotalAttackAttacker += n.MenCount * n.AttackStat * (1 + totalBuff);
                TotalDefenseAttacker += n.MenCount * n.DefenseStat * (1 + totalBuff);
            }
            float OriginalDefenseAttacker = TotalDefenseAttacker;

            //Calculate Winner with RNG
            while (TotalDefenseAttacker > 0 && TotalDefenseDefender > 0)
            {
                test += TotalDefenseDefender + " Defense Defender\n";
                test += TotalDefenseAttacker + " Defense Attacker\n";
                TotalDefenseDefender -= TotalAttackAttacker * RandomGenerator.Next(95, 105) / 100;
                TotalDefenseAttacker -= TotalAttackDefender * RandomGenerator.Next(95, 105) / 100; 
            }

            float SurvivingDefendersPercentage = 0;
            float SurvivingAttackersPercentage = 0;
            //Defender lost
            if (TotalDefenseDefender < TotalDefenseAttacker)
            {
                //Attacker is also almost destroyed
                if (TotalDefenseAttacker < 0)
                {
                    SurvivingAttackersPercentage = 0.05f;
                }
                //Attacker won easily
                else
                {
                    SurvivingAttackersPercentage = (TotalDefenseAttacker + (OriginalDefenseAttacker - TotalDefenseAttacker) * 0.1f) / OriginalDefenseAttacker;
                }
                //Apply the losses
                defenderArmy.Destroy();
                foreach (Unit n in attackerArmy.Units)
                {
                    n.MenCount = (int)(n.MenCount * SurvivingAttackersPercentage); 
                }
                test += "Attacker Won! \n" ;
            }
            //Attacker lost
            else
            {
                //Defender is also almost destroyed
                if (TotalDefenseDefender < 0)
                {
                    SurvivingDefendersPercentage = 0.05f;
                }
                //Defender won easily
                else
                {
                    SurvivingDefendersPercentage = (TotalDefenseDefender + (OriginalDefenseDefender - TotalDefenseDefender) * 0.1f) / OriginalDefenseDefender;
                   
                }
                //Apply the losses
                attackerArmy.Destroy();
                foreach (Unit n in defenderArmy.Units)
                {
                    n.MenCount = (int)(n.MenCount * SurvivingDefendersPercentage);
                }
                test += "Defender Won! \n" ;
            }
            /*
            foreach (Unit n in defenderArmy.Units)
            {
                n.MenCount = n.MaxMenCount;
            }
            foreach (Unit n in attackerArmy.Units)
            {
                n.MenCount = n.MaxMenCount;
            }*/
        }
    }
}
