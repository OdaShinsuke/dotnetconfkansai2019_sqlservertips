using System;
using System.Data.SqlClient;
using System.Transactions;

namespace _1_ConnectionStrings
{
    class Program
    {
        static void Main(string[] args)
        {
            // 同じ接続文字列でpoolsize2();
            // 同じ接続文字列でpoolsize1();
            // 別の文字列でpoolsize1();
            // 同じ接続文字列でTransactionScope();
            // 別の接続文字列でTransactionScope();

            Console.WriteLine("終わり");
        }

        static void 同じ接続文字列でpoolsize2()
        {
            var connstr1 = @"Data Source=MSI;Integrated Security=True;Connect Timeout=30;Encrypt=False; Max Pool Size=2";

            using (var conn1 = new SqlConnection(connstr1))
            {
                Console.WriteLine("1個め");
                conn1.Open();
                using (var conn2 = new SqlConnection(connstr1))
                {
                    Console.WriteLine("2個め");
                    conn2.Open();
                }
                Console.WriteLine("2個め閉じた");
            }
            Console.WriteLine("1個め閉じた");
        }
        static void 同じ接続文字列でpoolsize1()
        {
            try
            {
                // 待ってられないのでタイムアウトの時間を短く！
                var connstr1 = @"Data Source=MSI;Integrated Security=True;Connect Timeout=5;Encrypt=False; Max Pool Size=1";

                using (var conn1 = new SqlConnection(connstr1))
                {
                    Console.WriteLine("1個め");
                    conn1.Open();
                    using (var conn2 = new SqlConnection(connstr1))
                    {
                        Console.WriteLine("2個め");
                        conn2.Open();
                    }
                    Console.WriteLine("2個め閉じた");
                }
                Console.WriteLine("1個め閉じた");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        static void 別の文字列でpoolsize1()
        {
            var connstr1 = @"Data Source=MSI;Integrated Security=True;Connect Timeout=30;Encrypt=False; Max Pool Size=1";
            var connstr2 = @"Data Source=MSI;Integrated Security=True;Connect Timeout=30; Encrypt=False; Max Pool Size=1";

            using (var conn1 = new SqlConnection(connstr1))
            {
                Console.WriteLine("1個め");
                conn1.Open();
                using (var conn2 = new SqlConnection(connstr2))
                {
                    Console.WriteLine("別の文字列で1個め");
                    conn2.Open();
                }
                Console.WriteLine("別の文字列で1個め閉じた");
            }
            Console.WriteLine("1個め閉じた");
        }
        static void 同じ接続文字列でTransactionScope()
        {
            var connstr1 = @"Data Source=MSI;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
            
            using (var scope = new TransactionScope())
            {
                using (var conn1 = new SqlConnection(connstr1))
                {
                    Console.WriteLine("1個め");
                    conn1.Open();
                }
                Console.WriteLine("1個め閉じた");
                using (var conn2 = new SqlConnection(connstr1))
                {
                    Console.WriteLine("2個め");
                    conn2.Open();
                }
                Console.WriteLine("2個め閉じた");
            }
        }
        static void 別の接続文字列でTransactionScope()
        {
            try
            {
                var connstr1 = @"Data Source=MSI;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
                var connstr2 = @"Data Source=MSI;Integrated Security=True;Connect Timeout=30; Encrypt=False;";

                using (var scope = new TransactionScope())
                {
                    using (var conn1 = new SqlConnection(connstr1))
                    {
                        Console.WriteLine("1個め");
                        conn1.Open();
                    }
                    Console.WriteLine("1個め閉じた");
                    using (var conn2 = new SqlConnection(connstr2))
                    {
                        Console.WriteLine("別の文字列で1個め");
                        conn2.Open();
                    }
                    Console.WriteLine("別の文字列で1個め閉じた");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
