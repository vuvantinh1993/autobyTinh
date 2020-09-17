using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace autohana
{
    public class DungChung
    {

        public DungChung()
        {

        }

        public void ChangeCheckBox(DataGridViewCell dataGridViewCell, bool valueOld)
        {
            dataGridViewCell.Value = !valueOld;
        }
    }
}
