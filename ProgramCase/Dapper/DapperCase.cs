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

namespace ProgramCase.Dapper
{
    [Main]
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

        public void Run()
        {
            ConnectionStringSettings setting =  ConfigurationManager.ConnectionStrings["conn"];
            DbProviderFactory factory = DbProviderFactories.GetFactory(setting.ProviderName);
            IEnumerable<int> homeworkIdArr = new int[] {  190 ,90};
            SqlMapper.AddTypeHandler(new MessageContentTypeHandler());
            using (IDbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = setting.ConnectionString;
                connection.Open();
                var sql = $"select * from ETeacher_Group_TipMessage";
                /*dynamic obj = new ExpandoObject();
                obj.ID = 1;
                obj.Name = "wgsd"
                
                connection.Execute("inser test(userid,username) value(@ID,@Name) where 用户 = @用户",(object)obj);*/
                var list = connection.Query<TipMessage>(sql, new { HomeworkID = homeworkIdArr.ToArray() }).AsList();
                
            }
        }
    }
}