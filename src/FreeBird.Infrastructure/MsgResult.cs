using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBird.Infrastructure
{
    public class MsgResult<T> where T : class
    {
        public MsgResult(T data) : this(true, data)
        {

        }

        public MsgResult(bool sucess, T data, string message = null)
        {
            Success = sucess;
            Message = message;
            Data = data;
        }

        public bool Success
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public T Data
        {
            get;
            set;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }

    public class MsgResultStr : MsgResult<string>
    {
        public MsgResultStr(string data) : base(data)
        {
        }

        public MsgResultStr(bool sucess, string data, string message = null) : base(sucess, data, message)
        {
        }

        public MsgResultStr(bool sucess, string message) : base(sucess, string.Empty, message)
        {
        }
    }
}
