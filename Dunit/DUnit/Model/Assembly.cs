using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DUnit.Model
{
    public class Assembly
    {
        public string Path { get; private set; }

        public Assembly(string path)
        {
            this.Path = path;
        }
    }
}
