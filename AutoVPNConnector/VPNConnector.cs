using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoVPNConnector {
    /// <summary>
    /// VPN接続を管理するクラス
    /// </summary>
    class VPNConnector {

        /// <summary>
        /// 接続状態
        /// </summary>
        private bool _isConnected { get; set; }

        /// <summary>
        /// 接続名
        /// </summary>
        private string _connectionName { get; }

        /// <summary>
        /// ユーザ名
        /// </summary>
        private string _userId { get; }

        /// <summary>
        /// パスワード
        /// </summary>
        private string _password { get; }

        /// <summary>
        /// VPN接続を管理する
        /// </summary>
        /// <param name="connectionName">接続名</param>
        /// <param name="userId">ユーザ名</param>
        /// <param name="password">パスワード</param>
        public VPNConnector(string connectionName, string userId, string password) {
            this._isConnected = false;
            this._connectionName = connectionName;
            this._userId = userId;
            this._password = password;
        }

        /// <summary>
        /// 接続情報を元にVPN接続を行います。
        /// </summary>
        /// <param name="connectionName">接続先名</param>
        /// <param name="userId">ユーザID(メールアドレス)</param>
        /// <param name="password">パスワード</param>
        /// <returns>成功可否</returns>
        public bool Connect() {
            if (this._isConnected) {
                Console.WriteLine("already connected.");
                return true;
            }

            var arguments = String.Format("{0} {1} {2}", _connectionName, _userId, _password);

            var psi = new ProcessStartInfo() {
                FileName = "rasdial.exe",
                Arguments = arguments,
            };

            var p = Process.Start(psi);

            p.WaitForExit();

            if (p.ExitCode == 0) {
                _isConnected = true;
                return true;
            }

            return false;
        }


        /// <summary>
        /// 既存のVPN接続を切断します。
        /// </summary>
        /// <returns>成功可否</returns>
        public bool Disconnect() {
            if (!this._isConnected) {
                Console.WriteLine("already disconnected.");
                return true;
            }

            var arguments = String.Format("{0} /DISCONNECT", _connectionName);

            var psi = new ProcessStartInfo() {
                FileName = "rasdial.exe",
                Arguments = arguments,
            };

            var p = Process.Start(psi);

            p.WaitForExit();

            if (p.ExitCode == 0) {
                this._isConnected = false;
                return true;
            }

            return false;

        }
    }
}
