using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataItemGeneratorClasses
{
    public class DataBaseStructure
    {
        private string _DataBaseName;

        public string DataBaseName
        {
            get { return _DataBaseName; }
        }

        public DataBaseStructure(string dataBaseName)
        {
            _DataBaseName = dataBaseName;
        }
    }
}
