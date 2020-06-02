using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FrameTruyenThong
{
    public class DataGridViewCustom : DataGridView
    {
        public delegate void CheckedHeaderDelegate(bool isChecked);
        public event CheckedHeaderDelegate OnHeaderCheckBoxClick;

        public delegate void CheckedCellDelegate(int rowIndex, bool isChecked);
        public event CheckedCellDelegate OnCellCheckBoxClick;

        public const string COLUMN_NAME_CHECKBOX = "colCheckBoxColumn";

        private CheckBox _headerCheckBox = new CheckBox();

        public DataGridViewCustom() : base () { }

        public void LoadCheckBox()
        {
            bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            if (designMode) return;

            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "";
            checkBoxColumn.Width = 30;
            checkBoxColumn.Name = COLUMN_NAME_CHECKBOX;
            Columns.Insert(0, checkBoxColumn);

            _headerCheckBox.BackColor = Color.White;
            _headerCheckBox.Size = new Size(18, 18);

            var headerCell = GetCellDisplayRectangle(0, -1, false);
            int x = Math.Abs((checkBoxColumn.Width / 2) - (_headerCheckBox.Width / 2)) + 4;
            int y = Math.Abs((headerCell.Height / 2) - (_headerCheckBox.Height / 2)) + 2;

            _headerCheckBox.Location = new Point(x, y);
            _headerCheckBox.Click += new EventHandler(HeaderCheckBox_Clicked);
            Controls.Add(_headerCheckBox);

            CellContentClick += new DataGridViewCellEventHandler(DataGridView_CellClick);
        }

        private void HeaderCheckBox_Clicked(object sender, EventArgs e)
        {
            EndEdit();

            foreach (DataGridViewRow row in Rows)
            {
                DataGridViewCheckBoxCell checkBox = (row.Cells[COLUMN_NAME_CHECKBOX] as DataGridViewCheckBoxCell);
                if (!checkBox.ReadOnly)
                {
                    checkBox.Value = _headerCheckBox.Checked;
                }
            }

            if (OnHeaderCheckBoxClick != null)
            {
                OnHeaderCheckBoxClick(_headerCheckBox.Checked);
            }    
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                CheckedHeaderGridView();

                if (OnCellCheckBoxClick != null)
                {
                    object obj = Rows[e.RowIndex].Cells[COLUMN_NAME_CHECKBOX].EditedFormattedValue;
                    bool isChecked = obj == null ? false : Convert.ToBoolean(obj);

                    OnCellCheckBoxClick(e.RowIndex, isChecked);
                }    
            }
        }

        public void CheckedHeaderGridView()
        {
            if (Rows.Count <= 0)
            {
                _headerCheckBox.Checked = false;
                return;
            }

            bool isChecked = true;
            foreach (DataGridViewRow row in Rows)
            {
                if (Convert.ToBoolean(row.Cells[COLUMN_NAME_CHECKBOX].EditedFormattedValue) == false)
                {
                    isChecked = false;
                    break;
                }
            }
            _headerCheckBox.Checked = isChecked;
        }
    }
}
