using System.Runtime.Serialization;

namespace Test.AppService.Lightning.API.Exceptions
{
    [Serializable]
    internal class RowAndPartitionKeyNotImplementedException : Exception
    {
        public RowAndPartitionKeyNotImplementedException()
        {
        }

        public RowAndPartitionKeyNotImplementedException(string? message) : base(message)
        {
        }

        public RowAndPartitionKeyNotImplementedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RowAndPartitionKeyNotImplementedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}