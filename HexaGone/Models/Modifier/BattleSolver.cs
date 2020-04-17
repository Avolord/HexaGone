using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models.Modifier
{
    static public class BattleSolver
    {
        static public  string test = "";
        static Random RandomGenerator = new Random(Guid.NewGuid().GetHashCode());
        static public void Solve(ref Army defenderArmy, ref Army attackerArmy)
        {
            //update defender stats
            float TotalAttackDefender = 0;
            float TotalRangedAttackDefender = 0;
            float TotalDefenseDefender = 0;
            int TotalManCountDefender = defenderArmy.GetTotalManCount();
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
                if (n.IsRangedUnit)
                {
                    TotalRangedAttackDefender += n.MenCount * n.AttackStat * (1 + totalBuff);
                }
            }
            float OriginalDefenseDefender = TotalDefenseDefender;

            //update attacker stats
            float TotalAttackAttacker = 0;
            float TotalRangedAttackAttacker = 0;
            float TotalDefenseAttacker = 0;
            int TotalManCountAttacker = attackerArmy.GetTotalManCount();
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
                if (n.IsRangedUnit)
                {
                    TotalRangedAttackAttacker += n.MenCount * n.AttackStat * (1 + totalBuff);
                }
            }
            float OriginalDefenseAttacker = TotalDefenseAttacker;

            //Let the Ranged Units get one free attack
            TotalDefenseDefender -= TotalRangedAttackAttacker * RandomGenerator.Next(85, 105) / 100;
            TotalDefenseAttacker -= TotalRangedAttackDefender * RandomGenerator.Next(85, 105) / 100;
            //Calculate Winner with RNG
            while (TotalDefenseAttacker > 0 && TotalDefenseDefender > 0)
            {
                TotalDefenseDefender -= TotalAttackAttacker * RandomGenerator.Next(85, 115) / 100;
                TotalDefenseAttacker -= TotalAttackDefender * RandomGenerator.Next(85, 115) / 100; 
            }

            float SurvivingDefendersPercentage = 0;
            float SurvivingAttackersPercentage = 0;

            //Defender lost
            if (TotalDefenseDefender < TotalDefenseAttacker)
            {
                test += "Attacker won ";
                //Attacker is also almost destroyed
                if (TotalDefenseAttacker < 0)
                {
                    SurvivingAttackersPercentage = 0.05f;
                }
                //Attacker won easily
                else
                {
                    SurvivingAttackersPercentage = TotalDefenseAttacker / OriginalDefenseAttacker + 0.1f;
                    if (SurvivingDefendersPercentage > 1)
                    {
                        SurvivingDefendersPercentage = 1;
                    }
                }
                //Apply the losses
                defenderArmy.Destroy();
                foreach (Unit n in attackerArmy.Units)
                {
                    n.MenCount = (int)(n.MenCount * SurvivingAttackersPercentage);
                }
                test += "and lost " + (TotalManCountAttacker - attackerArmy.GetTotalManCount()).ToString() + " \n";
            }
            //Attacker lost
            else
            {
                test += "Defender won ";
                //Defender is also almost destroyed
                if (TotalDefenseDefender < 0)
                {
                    SurvivingDefendersPercentage = 0.05f;
                }
                //Defender won easily
                else
                {
                    SurvivingDefendersPercentage = TotalDefenseDefender / OriginalDefenseDefender + 0.1f;
                    if (SurvivingDefendersPercentage > 1)
                    {
                        SurvivingDefendersPercentage = 1;
                    }
                }
                //Apply the losses
                attackerArmy.Destroy();
                foreach (Unit n in defenderArmy.Units)
                {
                    n.MenCount = (int)(n.MenCount * SurvivingDefendersPercentage);
                }
                test += "and lost " + (TotalManCountDefender - defenderArmy.GetTotalManCount()).ToString() + " \n";
            }
        }
    }
}
