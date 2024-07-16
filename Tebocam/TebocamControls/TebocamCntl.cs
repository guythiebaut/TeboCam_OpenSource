using System.Windows.Forms;

namespace TeboCam.TebocamControls
{
    public partial class TebocamCntl : UserControl
    {
        public TebocamCntl()
        {
            InitializeComponent();
        }

        public virtual void AfterControlAdded() { }
    }
}
