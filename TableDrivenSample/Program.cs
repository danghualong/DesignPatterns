using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableDrivenSample
{
    /****
     保险公司计算个人保费，通常会参考以下几个因素
     * 性别：男、女
     * 婚姻：单身、已婚、丧偶、离异
     * 吸烟史：吸烟、不吸烟
     * 年龄：不同性别，年龄要求不一样
     */
    public enum GenderEnum{
        Male,
        Famale
    }
    public enum MaritalEnum{
        Single,
        Married,
        Divorce,
        Widowed
    }
    public enum SmokeEnum{
        Smoking,
        NonSmoking
    }
    public class InsuranceCalculator
    {
        /// <summary>
        /// 费率表
        /// </summary>
        private IDictionary<byte, List<Tuple<int, int, double>>> ratesTable = null;
        public InsuranceCalculator()
        {
            ratesTable = new Dictionary<byte, List<Tuple<int, int, double>>>();
            /*  因为篇幅问题，没有把婚姻的另外两种状态写进来 */
            #region Male
            byte type = ((byte)GenderEnum.Male << 6) | ((byte)MaritalEnum.Married << 2) | (byte)SmokeEnum.Smoking;
            var rates = new List<Tuple<int, int, double>>();
            rates.Add(new Tuple<int, int, double>(0, 20, 200));
            rates.Add(new Tuple<int, int, double>(20, 40, 300));
            rates.Add(new Tuple<int, int, double>(40, 60, 500));
            rates.Add(new Tuple<int, int, double>(60, int.MaxValue, 1000));
            ratesTable.Add(type, rates);

            type = ((byte)GenderEnum.Male << 6) | ((byte)MaritalEnum.Married << 2) | (byte)SmokeEnum.NonSmoking;
            rates = new List<Tuple<int, int, double>>();
            rates.Add(new Tuple<int, int, double>(0, 20, 100));
            rates.Add(new Tuple<int, int, double>(20, 40, 200));
            rates.Add(new Tuple<int, int, double>(40, 60, 400));
            rates.Add(new Tuple<int, int, double>(60, int.MaxValue, 800));
            ratesTable.Add(type, rates);

            type = ((byte)GenderEnum.Male << 6) | ((byte)MaritalEnum.Single << 2) | (byte)SmokeEnum.NonSmoking;
            rates = new List<Tuple<int, int, double>>();
            rates.Add(new Tuple<int, int, double>(0, 30, 300));
            rates.Add(new Tuple<int, int, double>(30, 60, 600));
            rates.Add(new Tuple<int, int, double>(60, int.MaxValue, 1500));
            ratesTable.Add(type, rates);

            type = ((byte)GenderEnum.Male << 6) | ((byte)MaritalEnum.Single << 2) | (byte)SmokeEnum.Smoking;
            rates = new List<Tuple<int, int, double>>();
            rates.Add(new Tuple<int, int, double>(0, 30, 400));
            rates.Add(new Tuple<int, int, double>(30, 60, 800));
            rates.Add(new Tuple<int, int, double>(60, int.MaxValue, 2000));
            ratesTable.Add(type, rates);
            #endregion
            #region Female
            type = ((byte)GenderEnum.Famale << 6) | ((byte)MaritalEnum.Single << 2) | (byte)SmokeEnum.NonSmoking;
            rates = new List<Tuple<int, int, double>>();
            rates.Add(new Tuple<int, int, double>(0, 25, 300));
            rates.Add(new Tuple<int, int, double>(25, 50, 600));
            rates.Add(new Tuple<int, int, double>(50, int.MaxValue, 1200));
            ratesTable.Add(type, rates);

            type = ((byte)GenderEnum.Famale << 6) | ((byte)MaritalEnum.Single << 2) | (byte)SmokeEnum.Smoking;
            rates = new List<Tuple<int, int, double>>();
            rates.Add(new Tuple<int, int, double>(0, 20, 400));
            rates.Add(new Tuple<int, int, double>(20, 60, 1200));
            rates.Add(new Tuple<int, int, double>(60, int.MaxValue, 1600));
            ratesTable.Add(type, rates);

            type = ((byte)GenderEnum.Famale << 6) | ((byte)MaritalEnum.Married << 2) | (byte)SmokeEnum.NonSmoking;
            rates = new List<Tuple<int, int, double>>();
            rates.Add(new Tuple<int, int, double>(0, 30, 200));
            rates.Add(new Tuple<int, int, double>(30, 50, 500));
            rates.Add(new Tuple<int, int, double>(50, int.MaxValue, 1000));
            ratesTable.Add(type, rates);

            type = ((byte)GenderEnum.Famale << 6) | ((byte)MaritalEnum.Married << 2) | (byte)SmokeEnum.Smoking;
            rates = new List<Tuple<int, int, double>>();
            rates.Add(new Tuple<int, int, double>(0, 30, 300));
            rates.Add(new Tuple<int, int, double>(30, 50, 700));
            rates.Add(new Tuple<int, int, double>(50, int.MaxValue, 1500));
            ratesTable.Add(type, rates);
            #endregion
        }

        /*
        通过该方法，可以看出有大量的重复内容，并且一长篇都是If else ，可读性、维护性很差
        为此，建议大家尝试表驱动法处理该问题
        */
        public double GetRateByCondition(GenderEnum gender, MaritalEnum marital, SmokeEnum smoke, int age)
        {
            if (gender == GenderEnum.Male)
            {
                if (marital == MaritalEnum.Married)
                {
                    if (smoke == SmokeEnum.Smoking)
                    {
                        if (age < 20)
                        {
                            return 200;
                        }
                        else if (age < 40)
                        {
                            return 300;
                        }
                        else if (age < 60)
                        {
                            return 500;
                        }
                        else
                        {
                            return 1000;
                        }
                    }
                    else
                    {
                        //to do 
                    }
                }
                else if (marital == MaritalEnum.Single)
                {
                    //to do
                }
                else if (marital == MaritalEnum.Divorce)
                {
                    //to do
                }
                else
                {
                    //to do
                }
            }
            return 0;
        }
        /// <summary>
        /// 利用表驱动法，读取费率
        /// </summary>
        /// <param name="gender"></param>
        /// <param name="marital"></param>
        /// <param name="smoke"></param>
        /// <param name="age"></param>
        /// <returns></returns>
        public double GetRateByTable(GenderEnum gender, MaritalEnum marital, SmokeEnum smoke, int age )
        {
            byte type =(byte)(((byte)gender << 6) | ((byte)marital << 2) | (byte)smoke);
            var rates=ratesTable[type];
            double result = 0;
            foreach (var rate in rates)
            {
                if (age >= rate.Item1 && age < rate.Item2)
                {
                    result= rate.Item3;
                    break;
                }
            }
            return result;
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            InsuranceCalculator ic = new InsuranceCalculator();
            var rate=ic.GetRateByTable(GenderEnum.Male, MaritalEnum.Single, SmokeEnum.Smoking, 35);
            Console.WriteLine("GenderEnum.Male, MaritalEnum.Single, SmokeEnum.Smoking, 35---->" + rate);
            rate = ic.GetRateByTable(GenderEnum.Famale, MaritalEnum.Married, SmokeEnum.NonSmoking, 42);
            Console.WriteLine("GenderEnum.Famale, MaritalEnum.Married, SmokeEnum.NonSmoking, 42---->" + rate);
            Console.ReadKey();
        }
    }
}
