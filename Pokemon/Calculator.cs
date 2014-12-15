using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon
{
    public static class Calculator
    {
	///<summary>
	///Calculate the highest possible value for a stat except hp
	///</summary>
        public static int CalculateMax(int BaseStat, int Level)
        {
            int IV = 31;
            int EV = 252;
            double NatureValue = 1.1;

            return CalculateStat(BaseStat, Level, IV, EV, NatureValue);
        }

	///<summary>
	///Calculate the lowest possible value for a stat except hp
	///</summary>
        public static int CalculateMin(int BaseStat, int Level)
        {
            int IV = 0;
            int EV = 0;
            double NatureValue = 0.9;

            return CalculateStat(BaseStat, Level, IV, EV, NatureValue);
        }

	///<summary>
	///Calculate the highest possible value for hp
	///</summary>
        public static int CalculateMaxHP(int BaseStat, int Level)
        {
            int IV = 31;
            int EV = 255;

            return CalculateHP(BaseStat, Level, IV, EV);
        }

	///<summary>
	///Calculate the lowest possible value for hp
	///</summary>
        public static int CalculateMinHP(int BaseStat, int Level)
        {
            int IV = 0;
            int EV = 0;

            return CalculateHP(BaseStat, Level, IV, EV);
        }

	///<summary>
	///Create all possible values for a stat except hp
	///</summary>
        public static List<int> CalculateIV(int BaseStat, int Level, int Total, double Nature, int EV)
        {
            List<int> returnable = new List<int>();

	    //Note: Deze formule staat bij de daadwerkelijk calculation-methods in stukken, vanwege de manier hoe C# prioritizeerd in het berekenen
            //var var1 = Math.Floor(((Math.Ceiling(Total / Nature) - 5) * 100 / Level) - 2 * BaseStat - EV / 4);
            //int test = Convert.ToInt32(var1);

            for (int iv = 0; iv < 32; iv++)
            {
                if (CalculateStat(BaseStat, Level, iv, EV, Nature) == Total)
                    returnable.Add(iv);
            }

            return returnable;
        }

	///<summary>
	///Create all possible values for hp
	///</summary>
        public static List<int> CalculateIVHP(int BaseStat, int Level, int Total, int EV)
        {
            List<int> returnable = new List<int>();

            for (int iv = 0; iv < 32; iv++)
            {
                if (CalculateHP(BaseStat, Level, iv, EV) == Total)
                    returnable.Add(iv);
            }

            return returnable;
        }

	///<summary>
	///Calculate the value for stat under set condition
	///</summary>
        public static int CalculateStat(int BaseStat, int Level, int IV, int EV, double NatureValue)
        {
            var var1 = IV + (2 * BaseStat);
            var var2 = var1 + Math.Floor((double)(EV / 4));
            var var3 = (double)(var2 * Level / 100) + 5;

            int stat = Convert.ToInt32(Math.Floor(Math.Floor(var3) * NatureValue));
            return stat;
        }

	///<summary>
	///Calculate the value for hp under set condition
	///</summary>
        public static int CalculateHP(int BaseStat, int Level, int IV, int EV)
        {
            var var1 = IV + 2 * BaseStat + Math.Floor((double)(EV / 4));
            var var2 = (double)(var1 * Level / 100) + 10 + Level;

            int stat = Convert.ToInt32(Math.Floor(var2));
            return stat;
        }
    }
}
