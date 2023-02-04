using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoVPNConnector {
    /// <summary>
    /// 共通で使う定数定義
    /// </summary>
    public static class GlobalSettings {
        /// <summary>
        /// 設定ファイルパス
        /// </summary>
        public const string SET_FILE = "set.ini";
        
        /// <summary>
        /// 接続先名
        /// </summary>
        public const string CONNECTION_NAME = "CONNECTION_NAME";

        /// <summary>
        /// ユーザID
        /// </summary>
        public const string USER_ID = "USER_ID";

        /// <summary>
        /// PASSLOGICで必要なKNパラメータ
        /// </summary>
        public const string KEY_NUMBER = "KEY_NUMBER";

        /// <summary>
        /// パスの前半部分
        /// </summary>
        public const string PASS_BEFORE = "PASS_BEFORE";

        /// <summary>
        /// 開始INDEX
        /// </summary>
        public const string START_INDEX = "START_INDEX";

        /// <summary>
        /// 終了INDEX
        /// </summary>
        public const string END_INDEX = "END_INDEX";
    }
}
