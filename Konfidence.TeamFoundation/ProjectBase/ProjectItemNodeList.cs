using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konfidence.TeamFoundation.ProjectBase
{
    class ProjectItemNodeList<T>: List<T> where T: ProjectItemNode
    {
        /// <summary>
        /// create and return a xxxDataItem derived from BaseDataItem
        /// </summary>
        /// <returns></returns>
        protected virtual T GetNewDataItem()
        {
            throw new NotImplementedException(); // NOP
        }

        public BaseDataItem GetDataItem()
        {
            T baseDataItem = GetNewDataItem();

            Add(baseDataItem);

            return baseDataItem;
        }
    }
}
