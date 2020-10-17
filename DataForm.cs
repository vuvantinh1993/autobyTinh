using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace autohana
{
    public class DataForm
    {
        DataGridView dgvAccounts;
        public DataForm(DataGridView dgvAccounts)
        {
            this.dgvAccounts = dgvAccounts;
        }



        public void ChangeCheckBox(DataGridViewCell dataGridViewCell, bool valueOld)
        {
            dataGridViewCell.Value = !valueOld;
        }
    }
}
