using System;
using System.Threading;
using System.Windows.Forms;

namespace FrameTruyenThong
{
    public class FrmBase : Form
    {
        protected void Exec(Action action)
        {
            Thread thread = new Thread(delegate ()
            {
                if (this.IsHandleCreated)
                {
                    this.BeginInvoke(action);
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }
    }
}