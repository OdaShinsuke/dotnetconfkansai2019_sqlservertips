using System;
using System.Data.SqlClient;
using Dapper;

namespace _2_SqlParameter
{
    class Program
    {
        static string connstr = @"Data Source=MSI;Initial Catalog=dotnetconf2019;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        static void Main(string[] args)
        {
            // dbcc freeproccache
            // パラメータの型を指定せずと指定した();
            // パラメータの長さを指定せずに違う長さの文字列();
            // パラメータの長さを指定して違う長さの文字列();
            // 文字型に数値を渡す();
            // 数値型に文字列を渡す();
            // パラメータの型や長さを指定せず_Dapper();
            // パラメータの型や長さを指定する_Dapper();
            Console.WriteLine("終わり");
            /* https://qiita.com/itito/items/22fabe94d1fc0c881d1c
SELECT
    o.[name] AS [オブジェクト名]
    , p.[bucketid]
    , qs.[execution_count] AS [実行回数]
    , qs.[max_elapsed_time] AS [最大実行時間]
    , qs.[total_elapsed_time] AS [合計実行時間]
    , qs.[max_physical_reads] AS [最大物理読み込み]
    , qs.[max_logical_reads] AS [最大論理読み込み]
    , st.[text]  AS [クエリ]
    , qp.[query_plan] AS [実行計画]
FROM
    sys.dm_exec_cached_plans AS p
    INNER JOIN sys.dm_exec_query_stats AS qs
        ON p.[plan_handle] = qs.[plan_handle]
    CROSS APPLY sys.dm_exec_sql_text(p.[plan_handle]) AS st
    CROSS APPLY sys.dm_exec_query_plan(p.[plan_handle]) AS qp
    LEFT OUTER JOIN sys.objects AS o
        ON o.[object_id] = qp.[objectid]
ORDER BY
    qs.[max_elapsed_time] DESC
             */
        }

        static void パラメータの型を指定せずと指定した()
        {
            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"select * from [Hoge] where [var] = @var", conn))
            {
                cmd.Parameters.AddWithValue("var", "12");
                conn.Open();
                cmd.ExecuteReader();
            }
            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"select * from [Hoge] where [var] = @var", conn))
            {
                cmd.Parameters.Add(new SqlParameter("var", System.Data.SqlDbType.VarChar));
                cmd.Parameters[0].Value = "12";
                conn.Open();
                cmd.ExecuteReader();
            }
        }
        static void パラメータの長さを指定せずに違う長さの文字列()
        {
            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"select * from [Hoge] where [var] = @var", conn))
            {
                cmd.Parameters.Add(new SqlParameter("var", System.Data.SqlDbType.VarChar));
                cmd.Parameters[0].Value = "12";
                conn.Open();
                cmd.ExecuteReader();
            }
            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"select * from [Hoge] where [var] = @var", conn))
            {
                cmd.Parameters.Add(new SqlParameter("var", System.Data.SqlDbType.VarChar));
                cmd.Parameters[0].Value = "123";
                conn.Open();
                cmd.ExecuteReader();
            }
        }
        static void パラメータの長さを指定して違う長さの文字列()
        {
            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"select * from [Hoge] where [var] = @var", conn))
            {
                cmd.Parameters.Add(new SqlParameter("var", System.Data.SqlDbType.VarChar, 20));
                cmd.Parameters[0].Value = "12";
                conn.Open();
                cmd.ExecuteReader();
            }
            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"select * from [Hoge] where [var] = @var", conn))
            {
                cmd.Parameters.Add(new SqlParameter("var", System.Data.SqlDbType.VarChar, 20));
                cmd.Parameters[0].Value = "123";
                conn.Open();
                cmd.ExecuteReader();
            }
        }
        static void 文字型に数値を渡す()
        {
            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"select * from [Hoge] where [var] = @var", conn))
            {
                cmd.Parameters.AddWithValue("var", 12);
                conn.Open();
                cmd.ExecuteReader();
            }
        }
        static void 数値型に文字列を渡す()
        {
            using (var conn = new SqlConnection(connstr))
            using (var cmd = new SqlCommand(@"select * from [Hoge] where [intvar] = @intvar", conn))
            {
                cmd.Parameters.AddWithValue("intvar", "12");
                conn.Open();
                cmd.ExecuteReader();
            }
        }

        static void パラメータの型や長さを指定せず_Dapper()
        {
            using (var conn = new SqlConnection(connstr))
            {
                conn.Query(@"select * from [Hoge] where [var] = @var", new { var = "12" });
            }
        }
        static void パラメータの型や長さを指定する_Dapper()
        {
            using (var conn = new SqlConnection(connstr))
            {
                conn.Query(@"select * from [Hoge] where [var] = @var", new { var = new DbString { Value = "12", IsAnsi = true, IsFixedLength = false, Length = 20 } });
            }
        }
    }
}
