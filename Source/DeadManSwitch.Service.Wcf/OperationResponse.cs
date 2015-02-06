using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DeadManSwitch.Service.Wcf
{
    //Name the data contract because otherwise WCF appends "random" values to generic class names to make them unique
    [Serializable]
    [DataContract(Name = "ReturnObjectOfType{0}")]
    public class OperationResponse<T>
    {
        public OperationResponse(T result)
        {
            IsSuccessful = true;
            Message = string.Empty;
            this.Result = result;
        }

        public OperationResponse(string operationFailedMessage)
        {
            IsSuccessful = false;
            Message = operationFailedMessage;
            Result = default(T);
        }

        public OperationResponse(bool isSuccessful, string message, T result)
        {
            IsSuccessful = isSuccessful;
            Message = message;
            Result = result;
        }

        [DataMember]
        public bool IsSuccessful { get; private set; }
        [DataMember]
        public string Message { get; private set; }
        [DataMember]
        public T Result { get; private set; }
    }
}
