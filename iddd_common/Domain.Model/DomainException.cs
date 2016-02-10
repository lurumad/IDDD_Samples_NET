using System;

namespace SaaSOvation.Common.Domain.Model
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
            
        }
    }
}
