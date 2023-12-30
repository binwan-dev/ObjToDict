using System.Linq;
using System;
using System.Collections.Generic;

namespace ObjToDict.CodeGeneration
{
    public class PersonConverter:Person,IDictConverter<Person>
    {
        public Dictionary<string, string> ToDict(object dataa)
        {
                        return new Dictionary<string, string>();

        }

    }
}
