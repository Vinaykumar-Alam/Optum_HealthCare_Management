using System;
using System.Collections.Generic;
using System.Text;

namespace optum_healthcare_system.Models
{
    public static class MessageConstants
    {
        public const string NotFoundMessage= "No Matching records was found with given Id.";
        public const string MinPageNumberMessage= "Page number must be greater than 0.";
        public const string MinPageSizeMessage= "Page size must be between 1 and 50.";

    }
}
