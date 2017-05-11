using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Dynamic;
using System.Threading;
using System.Data.SqlClient;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Net.Sockets;

namespace ProgramCase.Dapper
{
    //[Main]
    public class DapperCase : ICase
    {
        public class TeachInfo
        {
            public int ID { get; set; }

            public string Name { get; set; }
        }

        public Task[] Get()
        {
            Task[] taskArr = new Task[5];
            int[] idArr = new int[] { 5, 4,3,2,1 };
            for(int i = 0; i < taskArr.Length ;i++)
            {
                taskArr[i] = new Task((obj) =>
                {
                    int index = (int)obj;
                    Thread.Sleep(idArr[index] * 1000);
                    Console.WriteLine(idArr[index]);
                },i);
            }

            return taskArr;
        }

        public async Task Test()
        {
            Task[] taskArr = Get();
            foreach(Task task in taskArr)
            {
                task.Start();
                await task;
            }
        }

        public void GenerateJsonSchema()
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(typeof(JoinGroupMessageContent));

            Console.WriteLine(schema);
        }

        public void BatchAdd()
        {
            ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings["conn"];
            DbProviderFactory factory = DbProviderFactories.GetFactory(setting.ProviderName);
            using (IDbConnection connection = factory.CreateConnection())
            using (IDbTransaction trans = connection.BeginTransaction())
            {
                connection.ConnectionString = setting.ConnectionString;
                connection.Open();

                var sql = @"insert table(A1,A2) values(@A1,@A2)";
                var list = connection.Execute(new CommandDefinition(sql,new object[]{ new { A1 = "1", A2 = "用户1" },new { A1 = "2", A2 = "用户2" } } , trans,flags: CommandFlags.Buffered | CommandFlags.Pipelined));

                Console.WriteLine(JsonConvert.SerializeObject(list, Formatting.Indented));
            }
        }

        public class Temp : IDisposable
        {
            public void Dispose()
            {
                Console.WriteLine("释放");
            }
        }

        public void Run()
        {
            Console.WriteLine(Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork).ToString());
            return;
                ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings["conn"];
            DbProviderFactory factory = DbProviderFactories.GetFactory(setting.ProviderName);
            IEnumerable<int> homeworkIdArr = new int[] {  190 ,90};
            SqlMapper.AddTypeHandler(new MessageContentTypeHandler());
            using (IDbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = setting.ConnectionString;
                connection.Open();
                  
                var sql = @"select top 10 T1.*,T2.*,100 ReadStatus,'小兄弟，你怎么这么萌' Content,T3.* from ETeacher_Group_TipMessage T1 
                                             INNER JOIN ETeacher_Group_Student T2 ON T1.ReceiverID = T2.ID 
                                             INNER JOIN ETeacher_Group_StudentGroup T3 ON T2.ID = T3.StudentID ";
                var list = connection.Query<TipMessage, Student, StudentGroup,Tuple<TipMessage,Student, StudentGroup>>(sql,(t1,t2,t3) => new Tuple<TipMessage, Student, StudentGroup>(t1,t2,t3),splitOn:"id,id").AsList();

                Console.WriteLine(JsonConvert.SerializeObject(list,Formatting.Indented));
            }
        }
    }
}