using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace ProgramCase.Dapper
{
    public class MessageContentTypeHandler : TypeHandler<TipMessageContent>
    {
        private IDictionary<JSchema,Type> _schemas;

        public MessageContentTypeHandler()
        {
            _schemas = new Dictionary<JSchema, Type>();
            InitJsonSchema();
        }

        protected virtual void InitJsonSchema()
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            _schemas.Add(generator.Generate(typeof(JoinGroupMessageContent)),typeof(JoinGroupMessageContent));
            _schemas.Add(generator.Generate(typeof(HomeworkMessageContent)),typeof(HomeworkMessageContent));
        }

        protected virtual Type GetAppropriateType(string json)
        {
            JObject jsonObj = JObject.Parse(json);
            foreach(var item in _schemas)
            {
                if (jsonObj.IsValid(item.Key))
                    return item.Value;
            }

            return null;
        }

        public override TipMessageContent Parse(object value)
        {
            string content = value as string;
            if (string.IsNullOrWhiteSpace(content))
                return null;
            Type type = GetAppropriateType(content);
            if (type != null)
            {
                return JsonConvert.DeserializeObject(content, type) as TipMessageContent;
            }

            return null;
        }

        public override void SetValue(IDbDataParameter parameter, TipMessageContent value)
        {
            throw new NotImplementedException("MessageContentTypeHandler.SetValue");
        }
    }
}