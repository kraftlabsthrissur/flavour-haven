using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class OutofStockException : Exception
    {
        public OutofStockException(string message) : base(message)
        {
        }
    }

    public class InvalidReturnException : Exception
    {
        public InvalidReturnException(string message) : base(message)
        {
        }
    }

    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message)
        {
        }
    }

    public class AlreadyCancelledException : Exception
    {
        public AlreadyCancelledException(string message) : base(message)
        {
        }
    }

    public class NotEditableException : Exception
    {
        public NotEditableException(string message) : base(message)
        {
        }
    }

    public class CodeAlreadyExistsException : Exception
    {
        public CodeAlreadyExistsException(string message) : base(message)
        {
        }
    }
    public class DuplicateEntryException : Exception
    {
        public DuplicateEntryException(string message) : base(message)
        {
        }
    }
    public class QuantityExceededException : Exception
    {
        public QuantityExceededException(string message) : base(message)
        {
        }
    }
    public class AlreadyAdjustedException : Exception
    {
        public AlreadyAdjustedException(string message) : base(message)
        {
        }
    }
    public class LessProfitException : Exception
    {
        public LessProfitException(string message) : base(message)
        {
        }
    }

    public class CreditLimitExceededException : Exception
    {
        public CreditLimitExceededException(string message) : base(message)
        {
        }
    }

    public class MinimumBillingAmountException : Exception
    {
        public MinimumBillingAmountException(string message) : base(message)
        {
        }
    }
}


