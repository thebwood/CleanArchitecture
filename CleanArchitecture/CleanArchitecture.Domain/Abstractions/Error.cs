
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Abstractions
{
    public record Error(string Code, string Name)
    {
        public static Error None = new(string.Empty, string.Empty);
        public static Error NotFound = new("Error.NotFound", "Not Found");
        public static Error Invalid = new("Error.Invalid", "Invalid");
        public static Error Unauthorized = new("Error.Unauthorized", "Unauthorized");
        public static Error NullValue = new("Error.NullValue", "Null value was provided");


    }
}
