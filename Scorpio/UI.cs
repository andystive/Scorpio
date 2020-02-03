using System.Windows.Forms;

/// <summary>
/// 显示对话框及按钮
/// </summary>
namespace Scorpio
{
    class UI
    {
        public static void Show(string msg)
        {
            MessageBox.Show(msg);
        }

        public static DialogResult ShowYesNo(string msg)
        {
            return MessageBox.Show(msg, "YesNo", MessageBoxButtons.YesNo);
        }
    }
}
