using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Model.Response
{
    public class ResponseFileModel
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public string ContentType { get; set; }

        public ResponseFileModel(byte[] data)
        {
            Data = data;
        }

    }
    public class ResponseFileError
    {
        public string messageCode { get; set; }     
        public string messageContent { get; set; }
        public int ResultStatus { get; set; }      
        public object data { get; set; }

    }
}
