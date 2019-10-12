using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Transactions;

namespace _3_BulkCopy
{
    class Program
    {
        static string connstr = @"Data Source=MSI;Initial Catalog=dotnetconf2019;Integrated Security=True;Connect Timeout=30;Encrypt=False;";

        static void Main(string[] args)
        {
            // INSERT_1件ずつ();
            // INSERT_10件ずつ();
            // BULK_INSERT_DataTable();
            // BULK_INSERT_FastMember();
            Console.WriteLine("終わり");
        }

        static void INSERT_1件ずつ()
        {
            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"truncate table BulkInsertTarget", conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            Stopwatch s;
            using (var tran = new TransactionScope())
            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"insert into BulkInsertTarget (Id, Content) values (@id, @content)", conn))
            {
                conn.Open();
                cmd.Parameters.Add("id", SqlDbType.BigInt);
                cmd.Parameters.Add("content", SqlDbType.NVarChar, 50);
                s = Stopwatch.StartNew();
                for (int i = 0; i < 1000; i++)
                {
                    cmd.Parameters[0].Value = i + 1;
                    cmd.Parameters[1].Value = $"Content_{i}";

                    cmd.ExecuteNonQuery();
                }

                tran.Complete();
            }

            s.Stop();

            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"select count(*) from BulkInsertTarget", conn))
            {
                conn.Open();
                Console.WriteLine($"件数：{cmd.ExecuteScalar()}");
            }

            Console.WriteLine($"{nameof(INSERT_1件ずつ)} - {s.ElapsedMilliseconds}ミリ秒");
        }
        static void INSERT_10件ずつ()
        {
            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"truncate table BulkInsertTarget", conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            Stopwatch s;
            using (var tran = new TransactionScope())
            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"insert into BulkInsertTarget (Id, Content) values 
(@id1, @content1), (@id2, @content2), (@id3, @content3), (@id4, @content4), (@id5, @content5), 
(@id6, @content6), (@id7, @content7), (@id8, @content8), (@id9, @content9), (@id10, @content10)", conn))
            {
                conn.Open();
                for (int i = 1; i <= 10; i++)
                {
                    cmd.Parameters.Add($"id{i}", SqlDbType.BigInt);
                    cmd.Parameters.Add($"content{i}", SqlDbType.NVarChar, 50);
                }                
                s = Stopwatch.StartNew();
                for (int i = 0; i < 1000; i+=10)
                {
                    var tmp = i;
                    cmd.Parameters[0].Value = tmp + 1;
                    cmd.Parameters[1].Value = $"Content_{tmp}";
                    cmd.Parameters[2].Value = ++tmp + 1;
                    cmd.Parameters[3].Value = $"Content_{tmp}";
                    cmd.Parameters[4].Value = ++tmp + 1;
                    cmd.Parameters[5].Value = $"Content_{tmp}";
                    cmd.Parameters[6].Value = ++tmp + 1;
                    cmd.Parameters[7].Value = $"Content_{tmp}";
                    cmd.Parameters[8].Value = ++tmp + 1;
                    cmd.Parameters[9].Value = $"Content_{tmp}";
                    cmd.Parameters[10].Value = ++tmp + 1;
                    cmd.Parameters[11].Value = $"Content_{tmp}";
                    cmd.Parameters[12].Value = ++tmp + 1;
                    cmd.Parameters[13].Value = $"Content_{tmp}";
                    cmd.Parameters[14].Value = ++tmp + 1;
                    cmd.Parameters[15].Value = $"Content_{tmp}";
                    cmd.Parameters[16].Value = ++tmp + 1;
                    cmd.Parameters[17].Value = $"Content_{tmp}";
                    cmd.Parameters[18].Value = ++tmp + 1;
                    cmd.Parameters[19].Value = $"Content_{tmp}";

                    cmd.ExecuteNonQuery();
                }

                tran.Complete();
            }

            s.Stop();

            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"select count(*) from BulkInsertTarget", conn))
            {
                conn.Open();
                Console.WriteLine($"件数：{cmd.ExecuteScalar()}");
            }

            Console.WriteLine($"{nameof(INSERT_10件ずつ)} - {s.ElapsedMilliseconds}ミリ秒");
        }
        static void BULK_INSERT_DataTable()
        {
            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"truncate table BulkInsertTarget", conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            Stopwatch s;
            using (var tran = new TransactionScope())
            using (var conn = new SqlConnection(connstr))
            using (var copy = new SqlBulkCopy(conn) { BatchSize = 1000, DestinationTableName = "BulkInsertTarget" })
            {
                conn.Open();
                var table = new DataTable();
                table.Columns.Add("Id", typeof(long));
                table.Columns.Add("Content", typeof(string));

                s = Stopwatch.StartNew();

                for (int i = 0; i < 1000; i++)
                {
                    table.Rows.Add(i + 1, $"Content_{i}");
                }

                copy.WriteToServer(table);
            }

            s.Stop();

            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"select count(*) from BulkInsertTarget", conn))
            {
                conn.Open();
                Console.WriteLine($"件数：{cmd.ExecuteScalar()}");
            }

            Console.WriteLine($"{nameof(BULK_INSERT_DataTable)} - {s.ElapsedMilliseconds}ミリ秒");
        }
        static void BULK_INSERT_FastMember()
        {
            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"truncate table BulkInsertTarget", conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            Stopwatch s;
            using (var tran = new TransactionScope())
            using (var conn = new SqlConnection(connstr))
            using (var copy = new SqlBulkCopy(conn) { BatchSize = 1000, DestinationTableName = "BulkInsertTarget" })
            {
                conn.Open();
                var list = new List<Data>();
                s = Stopwatch.StartNew();

                for (int i = 0; i < 1000; i++)
                {
                    list.Add(new Data { Id = i + 1, Content = $"Content_{i}" });
                }
                using (var reader = ObjectReader.Create(list, "Id", "Content"))
                {
                    copy.WriteToServer(reader);
                }
            }

            s.Stop();

            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"select count(*) from BulkInsertTarget", conn))
            {
                conn.Open();
                Console.WriteLine($"件数：{cmd.ExecuteScalar()}");
            }

            Console.WriteLine($"{nameof(BULK_INSERT_FastMember)} - {s.ElapsedMilliseconds}ミリ秒");
        }
        class Data
        {
            public long Id { get; set; }
            public string Content { get; set; }
        }
    }
}
