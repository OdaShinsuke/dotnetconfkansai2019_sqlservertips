using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;

namespace _4_Retry_NETFramework
{
    class Program
    {
        // 検証のためコネクションタイムアウトは5秒にしてる
        static string connstr = @"Data Source=MSI;Initial Catalog=dotnetconf2019;Integrated Security=True;Connect Timeout=5;Encrypt=False;";

        static void Main(string[] args)
        {
            リトライの設定デフォルト();
            // Open中にサービス再起動_リトライ0回();
            // Open中にサービス再起動_リトライ5回インターバル5秒();
            Console.WriteLine("終わり");
        }

        static void リトライの設定デフォルト()
        {
            using (var conn = new SqlConnection(connstr))
            {
                var type = typeof(SqlConnection);
                Console.WriteLine($"ConnectRetryInterval：{type.GetProperty("ConnectRetryInterval", BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.Instance).GetValue(conn)}");
                Console.WriteLine($"_connectRetryCount：{type.GetField("_connectRetryCount", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(conn)}");
            }
        }

        static void Open中にサービス再起動_リトライ0回()
        {
            try
            {
                using (var conn = new SqlConnection(connstr + "ConnectRetryCount=0;ConnectRetryInterval=5;"))
                using (var cmd = new SqlCommand(@"select count(*) from Hoge", conn))
                {
                    conn.Open();

                    Console.WriteLine($"件数：{cmd.ExecuteScalar()}");

                    Console.WriteLine("Service 止める");
                    Console.ReadKey();
                    Console.WriteLine("Service止まった！");

                    Console.WriteLine($"件数：{cmd.ExecuteScalar()}");

                    Console.WriteLine("Service起動したら出力される");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void Open中にサービス再起動_リトライ5回インターバル5秒()
        {
            Stopwatch s = null;
            try
            {
                using (var conn = new SqlConnection(connstr + "ConnectRetryCount=5;ConnectRetryInterval=5;"))
                using (var cmd = new SqlCommand(@"select count(*) from Hoge", conn))
                {
                    conn.Open();

                    Console.WriteLine($"件数：{cmd.ExecuteScalar()}");

                    Console.WriteLine("Service 止める");
                    Console.ReadKey();
                    Console.WriteLine("Service止まった！");
                    s = Stopwatch.StartNew();

                    Console.WriteLine($"件数：{cmd.ExecuteScalar()}");

                    Console.WriteLine("Service起動したら出力される");
                }
            }
            catch (Exception ex)
            {
                s.Stop();
                Console.WriteLine(ex.Message);
                Console.WriteLine(s.ElapsedMilliseconds);
            }
        }
    }
}
