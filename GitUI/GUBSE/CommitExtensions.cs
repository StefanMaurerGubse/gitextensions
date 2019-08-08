using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitUI.GUBSE
{
    public class CommitExtensions
    {
        public const string PREFIX_COMMITID = "**COMMIT-ID:";
        public const string PREFIX_CODEREVIEW = "**CODEREVIEW:";

        public static string GetExtendedCommitMessage(string message, string commitID, string codereview)
        {
            var sb = new StringBuilder();

            sb.Append(message);
            sb.AppendLine();
            sb.AppendLine();
            sb.Append(PREFIX_COMMITID);
            sb.Append(commitID);
            sb.AppendLine();
            sb.Append(PREFIX_CODEREVIEW);
            sb.Append(codereview);

            return sb.ToString();
        }

        public static bool CheckCommitIDAndCodeReview(string commitID, string codeReview)
        {
            Match m = new Regex(@"^(TaskID|KB|RFC)-[0-9]+(/[0-9]+)?$").Match(commitID);

            if (!m.Success)
            {
                string message = "The commit ID does not comply with the GUBSE conventions (TaskID-nn/nn or KB-nn/nn or RFC-nn/nn). Continue the operation anyway?";
                string caption = "Invalid Commit-ID";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
                if (result != DialogResult.Yes)
                {
                    return false;
                }
            }

            if (codeReview.IsNullOrEmpty())
            {
                string message = "No code reviewer given. Continue the operation anyway?";
                string caption = "Codereview empty";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
                if (result != DialogResult.Yes)
                {
                    return false;
                }
            }

            return true;
        }

        public static void SplitExtendedCommitMessage(string extendedMessage, ref string message, ref string commitID, ref string codereview)
        {
            Match m = new Regex(@"(?<message>(.|\n)*)\*\*COMMIT-ID:(?<commitID>(.|\n)*)\*\*CODEREVIEW:(?<codeReview>(.|\n)*)").Match(extendedMessage);

            if (m.Success)
            {
                message = m.Groups["message"].Value.Trim();
                commitID = m.Groups["commitID"].Value.Trim();
                codereview = m.Groups["codeReview"].Value.Trim();
            }
            else
            {
                // Incorrect format
                commitID = "";
                codereview = "";
                message = extendedMessage;
                return;
            }
        }
    }
}