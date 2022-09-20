using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Helper.Constant
{
    public static class MessageJapan
    {
        public static Dictionary<String, String> Japan = new Dictionary<string, string>()
        {
            // Information message
            { "I0002", "処理実行が成功しました。" },
            { "I0003", "マスタデータが最新版となっています。" },
            //
            { "I1000", "ユーザー情報更新が成功しました。" },
            { "I1002", "ユーザー情報の取得処理が成功しました。" },           

            // Error information message
            { "SMAL-040S", "パラメータが誤りました。" },
            { "SMAL-041D", "権限はありません。"},
            { "SMAL-020P", "タイムアウトしました。\n再度ログインしてください。" },
            { "SMAL-021C", "ユーザー名がありません。" },
            { "GCMF-020D", "ユーザー名またはパスワードが正しくありません。" },
            { "CEST-050S", "エラーが発生しました。" },
            { "CEST-051D", "エラーが発生しました。" },
            { "SMAI-023D", "エラーが発生しました。" },
            { "SMAI-013D", "エラーが発生しました。" },
            { "SMAI-014D", "エラーが発生しました。" },
            { "GCMF-100D", "勤務時間調整の該当データが存在しません。" },
            { "SMAI-010D", "残業申請の該当データが存在しません。" },
            { "SMAI-028D", "残業申請の該当データが既に存在しました。" },
            { "SMAI-001P", "ページ遷移エラーが発生しました。<br>ASNET/店頭商談NETの元の画面から本サービスを起動して下さい。" },
            { "SMAI-002S", "ページ遷移エラーが発生しました。<br>ASNET/店頭商談NETの元の画面から本サービスを起動して下さい。" },
            { "SICR-001S", "ページ遷移エラーが発生しました。<br>ASNET/店頭商談NETの元の画面から本サービスを起動して下さい。" },
            { "CEST-040D", "ページ遷移エラーが発生しました。<br>ASNET/店頭商談NETの元の画面から本サービスを起動して下さい。" },
        };
    }
}
