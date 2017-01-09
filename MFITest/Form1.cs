using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.IO;

namespace MFITest
{
    public partial class Form1 : Form
    {
        public string LogEachBarValues = @"C:\CustomBackTest\OpimizeTest.csv";
        public double[] closePrice = new double[20];
        public double[] highPrice = new double[28];
        public double[] lowPrice = new double[28];
        public double[] volume = new double[28];

        public Hashtable obj200Values = new Hashtable();
        public double[] arr200Values = new double[200];

        public Form1()
        {
            InitializeComponent();
        }

        private double[] computeStoch(double[] highPriceArray, double[] lowPriceArray, double[] closePriceArray, double[] catchVolume)
        {
            double[] OutPutMFI = new double[15];
            //double[] OutPutAroonUP = new double[25]; // 25
            //double[] OutPutAroonDown = new double[25]; // 25
            double[] OutPutStoch = new double[15];
            int outBegIdx;
            int outNbElement;
            try
            {
                //TicTacTec.TA.Library.Core.Mfi(0, Convert.ToInt32(14), closePriceArray, closePriceArray, closePriceArray, catchVolume, Convert.ToInt32(14), out outBegIdx, out outNbElement, OutPutMFI);
                //TicTacTec.TA.Library.Core.Aroon(0, 24, highPriceArray, lowPriceArray, 24, out outBegIdx, out outNbElement, OutPutAroonDown, OutPutAroonUP);
                //TicTacTec.TA.Library.Core.Stoch(
            }
            catch (Exception ex)
            {
                //WriteLog(ex, "computeADX()");
            }
            return OutPutMFI;
        }

        private double[] computeEMA1(double[] closePriceArray1)
        {
            double[] OutPutEMA1 = new double[closePriceArray1.Length];
            int outBegIdx;
            int outNbElement;
            try
            {
                //TicTacTec.TA.Library.Core.Ema(0, closePriceArray1.Length - 1, closePriceArray1, closePriceArray1.Length, out outBegIdx, out outNbElement, OutPutEMA1);
                TicTacTec.TA.Library.Core.Ema(0, 99, closePriceArray1, 10, out outBegIdx, out outNbElement, OutPutEMA1);
            }
            catch (Exception ex)
            {
                //WriteLog(ex, "computeEMA1()");
            }
            return OutPutEMA1;
        }

        private double[] computeADX(double[] highPriceArray, double[] lowPriceArray, double[] closePriceArray)
        {
            int period4ADX = 14;
            double[] OutPutADX = new double[period4ADX * 2];
            int outBegIdx;
            int outNbElement;

            double[] tempClose = new double[(period4ADX * 2)];
            double[] tempHigh = new double[(period4ADX * 2)];
            double[] tempLow = new double[(period4ADX * 2)];

            //Array.Copy(closePriceArray, (closePriceArray.Length - (GlobalVariables.period4ADX * 2)), tempClose, 0, (GlobalVariables.period4ADX * 2));
            //Array.Copy(highPriceArray, (highPriceArray.Length - (GlobalVariables.period4ADX * 2)), tempHigh, 0, (GlobalVariables.period4ADX * 2));
            //Array.Copy(lowPriceArray, (lowPriceArray.Length - (GlobalVariables.period4ADX * 2)), tempLow, 0, (GlobalVariables.period4ADX * 2));

            Array.Copy(closePriceArray, 0, tempClose, 0, (period4ADX * 2));
            Array.Copy(highPriceArray, 0, tempHigh, 0, (period4ADX * 2));
            Array.Copy(lowPriceArray, 0, tempLow, 0, (period4ADX * 2));

            try
            {
                TicTacTec.TA.Library.Core.Adx(0, (closePriceArray.Length - 1), highPriceArray, lowPriceArray, closePriceArray, period4ADX, out outBegIdx, out outNbElement, OutPutADX);
                //TicTacTec.TA.Library.Core.Adxr(0, (tempClose.Length - 1), tempHigh, tempLow, tempClose, 14, out outBegIdx, out outNbElement, OutPutADX);
                //TicTacTec.TA.Library.Core.Mfi(0, Convert.ToInt32(14), ClosingPrice, ClosingPrice, ClosingPrice, volumes, Convert.ToInt32(MetricPeriod + 1), out outBegIdx, out outNbElement, OutPutMFI);
            }
            catch (Exception ex)
            {
                // WriteLog(ex, "computeADX()");
            }
            return OutPutADX;
        }

        private void computeMACD(double[] highPriceArray, double[] lowPriceArray, double[] closePriceArray)
        {
            try
            {
                double[] OutPutMACD = new double[closePriceArray.Length];
                double[] OutPutMACDSignal = new double[closePriceArray.Length];
                double[] OutPutMACDHist = new double[closePriceArray.Length];
                int outBegIdx = 34;
                int outNbElement;

                //TicTacTec.TA.Library.Core.MacdExt(0, 99, closePriceArray, 12, TicTacTec.TA.Library.Core.MAType.Ema, 26, TicTacTec.TA.Library.Core.MAType.Ema, 9, TicTacTec.TA.Library.Core.MAType.Ema, out outBegIdx, out outNbElement, OutPutMACD, OutPutMACDSignal, OutPutMACDHist);
                TicTacTec.TA.Library.Core.Macd(0, closePriceArray.Length - 1, closePriceArray, 12, 26, 9, out outBegIdx, out outNbElement, OutPutMACD, OutPutMACDSignal, OutPutMACDHist);

                string record = "";
                for (int i = 0; i < closePriceArray.Length; i++)
                {
                    if (i < 26)
                    {
                        record = i.ToString() + "," + closePriceArray[i] + "," + 0 + "," + 0 + "," + 0;
                        WriteValuesEachBar(record);
                    }
                    //else if (i >= 11 && i < 25)
                    //{
                    //    record = i.ToString() + "," + closePriceArray[i] + "," + OutPutMACD[i - 11] + "," + 0 + "," + 0;
                    //    WriteValuesEachBar(record);
                    //}
                    else if (i > 25 && i < 34)
                    {
                        record = i.ToString() + "," + closePriceArray[i] + "," + OutPutMACD[i - 26] + "," + 0 + "," + 0;
                        WriteValuesEachBar(record);
                    }
                    else if (i > 33)
                    {
                        record = i.ToString() + "," + closePriceArray[i] + "," + OutPutMACD[i - 33] + "," + OutPutMACDSignal[i - 33] + "," + OutPutMACDHist[i - 33];
                        WriteValuesEachBar(record);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private double[] BBands(double[] closePriceArray)
        {
            int outBegIdx = 20;
            int outNbElement;

            double[] upperBB = new double[closePriceArray.Length];
            double[] lowerBB = new double[closePriceArray.Length];
            double[] middleBB = new double[closePriceArray.Length];

            try
            {
                TicTacTec.TA.Library.Core.Bbands(0, closePriceArray.Length - 1, closePriceArray, 20, 2, 2, TicTacTec.TA.Library.Core.MAType.Ema, out outBegIdx, out outNbElement, upperBB, middleBB, lowerBB);
            }
            catch (Exception ex)
            {
            }
            return upperBB;
        }

        private void btnStartBackTest_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            //for (int i = 0; i <= 27; i++)
            //{
            //    volume[i] = rnd.Next(10000, 50000);
            //}
            //for (int j = 0; j <= 27; j++)
            //{
            //    highPrice[j] = rnd.Next(110, 115);
            //}
            //for (int k = 0; k <= 27; k++)
            //{
            //    lowPrice[k] = rnd.Next(100, 109);
            //}
            for (int z = 0; z < 20; z++)
            {
                closePrice[z] = rnd.Next(107, 114);
            }


            //double[] MFI = computeAroon(highPrice, lowPrice, closePrice, volume);
            //double[] EMA1 = computeEMA1(closePrice); // EMA for 3 period.
            //double[] Stoch = computeStoch(highPrice, lowPrice, closePrice, volume);
            //double[] ADX = computeADX(highPrice, lowPrice, closePrice);
            //computeMACD(highPrice, lowPrice, closePrice);
            double[] BB = BBands(closePrice);
        }

        private void btnCheckLoopPerformance_Click(object sender, EventArgs e)
        {
            try
            {
                testFunction();
                timerTestPerformance.Start();
            }
            catch (Exception ex)
            {

            }
        }

        private void DisplayMessage(string message)
        {
            try
            {
                //Delegate Invoker to display File path to GUI textbox on the FLY.
                if (txtErr.InvokeRequired)
                {
                    MethodInvoker invoker = new MethodInvoker(delegate()
                    {
                        txtErr.Text += "\r\n" + message + " " + DateTime.Now.ToString("HH:mm:ss.fff") + " \r\n";
                    });
                    Invoke(invoker);
                }
                else
                {
                    txtErr.Text += "\r\n" + message + " " + DateTime.Now.ToString("HH:mm:ss.fff") + " \r\n";
                }
            }
            catch (Exception ex)
            {
                //WriteLog(ex, "DisplayMessage()");
            }
        }

        private void InsertValuesIntoDataBase(double[] acceptArr)
        {
            try
            {
                DAL dal;
                string ConString = System.Configuration.ConfigurationManager.AppSettings["CONSTRING"];
                dal = new DAL(ConString);


                for (int i = 0; i <= (acceptArr.Count() - 1); i++)
                {
                    SqlCommand cmdInsertValue = new SqlCommand();
                    cmdInsertValue.Parameters.AddWithValue("@value ", acceptArr[i]);
                    cmdInsertValue.CommandText = "TestInsertValue";
                    cmdInsertValue.CommandType = CommandType.StoredProcedure;

                    dal.ExecuteNonQuery(cmdInsertValue);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void testFunction()
        {
            int j = 0;
            int iCount = 100;
            double sma = 0;

            //double EMAManualValue = 2.0/ (50.0 + 1.0);
            try
            {
                //DisplayMessage("start inserting 200 values in hashtable");
                for (int i = 1; i <= (arr200Values.Count() - 1); i++)
                {
                    obj200Values.Add(i, iCount);
                    iCount = iCount + 10;
                }
                //DisplayMessage("end of 200 values insertion in hashtable");


                //DisplayMessage("start inserting 200 values in array");
                foreach (DictionaryEntry k in obj200Values)
                {
                    arr200Values[j] = Convert.ToDouble(k.Value);
                    j = j + 1;
                }
                //DisplayMessage("end of 200 values insertion in array");


                //DisplayMessage("start time to calculate SMA");
                for (int y = 0; y <= (arr200Values.Count() - 1); y++)
                {
                    sma = (sma + arr200Values[y]) / (y + 1);
                }
                //DisplayMessage("end time to calculate SMA");
                //DisplayMessage("SMA = " + sma + "");

                // insert into database
                //DisplayMessage("start inserting values in DB");
                InsertValuesIntoDataBase(arr200Values);
                //DisplayMessage("end of values insertion in DB");
            }
            catch (Exception ex)
            {

            }

            finally
            {
                //sma = null;
                //iCount = null;
            }
        }
        private void timerTestPerformance_Tick(object sender, EventArgs e)
        {
            try
            {
                DateTime dtInitiated = DateTime.Now;
                //DisplayMessage("Timer Initiated : ");
                obj200Values.Clear();
                //arr200Values = null;
                testFunction();
                TimeSpan dtMillDiff = DateTime.Now - dtInitiated;
                DisplayMessage("Timer end====> " + dtMillDiff.Milliseconds + " <===");

            }
            catch (Exception ex)
            {

            }
        }

        private void txtErr_TextChanged(object sender, EventArgs e)
        {
            txtErr.SelectionStart = txtErr.Text.Length;
            txtErr.ScrollToCaret();
            //txtErr.ScrollToCar
            txtErr.Refresh();
        }

        private void WriteValuesEachBar(string comment)
        {
            StreamWriter sw;
            try
            {
                if (!File.Exists(LogEachBarValues))
                {
                    sw = new StreamWriter(LogEachBarValues, true);
                    sw.Write("TickCount,");
                    //sw.Write("Open,");
                    //sw.Write("High,");
                    //sw.Write("Low,");
                    sw.Write("Close,");
                    //sw.Write("Volume,");
                    //sw.Write("TickTime,");
                    //sw.Write("ADX,");
                    sw.Write("MACD,");
                    sw.Write("MACDSignal,");
                    sw.Write("MACDHist,");
                    //sw.Write("RSI,");
                    //sw.Write("StochasticFastK,");
                    //sw.Write("StochasticFastD,");
                    //sw.Write("StochasticSlowK,");
                    //sw.Write("StochasticSlowD,");
                    //sw.Write("EMA2,");
                    //sw.Write("EMA1,");
                    //sw.Write("BBStdDevSetting,");
                    //sw.Write("BBUpperBand,");
                    //sw.Write("BBMiddleBand,");
                    //sw.Write("BBLowerBand,");
                    //sw.Write("BBBandwidth,");
                    //sw.Write("MFIOverBought,");
                    //sw.Write("MFIOverSold,");
                    //sw.Write("MFI,");

                    sw.WriteLine(" ");

                    sw.WriteLine(comment);
                    sw.Flush();
                    sw.Close();
                }
                else
                {
                    sw = File.AppendText(LogEachBarValues);

                    sw.WriteLine(comment);

                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                //WriteLog(ex, "WriteLogStrikes()");
            }
        }

        private void form_load(object sender, EventArgs e)
        {
            // on jan 09. 11:12 AM IST
            //trying to sync on githut using githut windows

            //edited on 9 jan 11:26 AM IST
            MessageBox.Show("check updated code on Github directly");
            int a = 6 + 4;
            if (a > 10)
            {
            }
        }
    }
}
