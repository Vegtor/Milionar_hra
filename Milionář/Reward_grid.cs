using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Milionář
{
    internal class RewardGrid : Grid
    {
        public int NumberOfRewards
        {
            get 
            { 
                return this.RowDefinitions.Count;
            }
            set
            {
                this.RowDefinitions.Clear();
                for (int i = 0; i < value; i++)
                    RowDefinitions.Add(new RowDefinition());
            }
        }
    }
}
