using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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