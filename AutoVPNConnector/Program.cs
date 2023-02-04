using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutoVPNConnector {
    class Program {
        static async Task Main(string[] args) {

            if (!System.IO.File.Exists(GlobalSettings.SET_FILE)) {
                System.IO.File.Create(GlobalSettings.SET_FILE).Close();
                Initalize();
            }

            Console.WriteLine("VPN接続の開始");

            #region 各種情報取得
            var configure = new Configure();
            var connectionName = configure.Get(GlobalSettings.CONNECTION_NAME);
            var userId = configure.Get(GlobalSettings.USER_ID);
            var keyNum = configure.Get(GlobalSettings.KEY_NUMBER);
            var passBefore = configure.Get(GlobalSettings.PASS_BEFORE);
            var startIndex = int.Parse(configure.Get(GlobalSettings.START_INDEX)) - 1;
            var endIndex = int.Parse(configure.Get(GlobalSettings.END_INDEX)) - 1;

            var urlString = string.Format("https://matrix.iijgw.jp/passlogic/ui/keyreq.php?id={0}&kn={1}", userId, keyNum);

            // URLパース
            var document = default(IHtmlDocument);
            using (var client = new HttpClient()) {
                using (var stream = await client.GetStreamAsync(new Uri(urlString))) {
                    var parser = new HtmlParser();
                    document = await parser.ParseDocumentAsync(stream);
                }
            }
            #endregion

            #region パス取得
            var passlogicNumbers = new List<int>();

            var table = document.QuerySelector("#passlogic-matrix tbody");

            foreach (var matrixColumn in table.QuerySelectorAll("tr")) {
                foreach (var number in matrixColumn.QuerySelectorAll("td")) {
                    passlogicNumbers.Add(int.Parse(number.TextContent));
                }
            }
            var passphase = new List<string>();
            for (int i = startIndex; endIndex >= i; i++) {
                passphase.Add(passlogicNumbers[i].ToString());
            }
            #endregion

            #region ログイン
            var vpn = new VPNConnector(connectionName, userId, passBefore + String.Join("", passphase));
            var isConnected = vpn.Connect();

            if (isConnected) {
                Console.WriteLine("VPN接続に成功");
            }
            else {
                Console.WriteLine("VPN接続に失敗");
            }
            Console.WriteLine("press any key");
            Console.ReadKey();
            #endregion
        }

        /// <summary>
        /// 初期設定を行う
        /// </summary>
        private static void Initalize() {

            Console.WriteLine("初期設定モード");
            Console.WriteLine();

            Console.Write("接続先名(WindowsのVPN接続名に使用しているものと同一): ");
            var connectionName = Console.ReadLine();
            Console.Write("ユーザ名(PassLogicにログインする際のメールアドレス): ");
            var userId = Console.ReadLine();
            Console.Write("キー(PassLogicにログイン後のパラメタにある4桁の番号): ");
            var keyNumber = Console.ReadLine();
            Console.Write("パス固定値(自身が使用しているパス前半の固定値部分):　");
            var passBefore = Console.ReadLine();
            Console.WriteLine("パスフレーズ位置(左上を1, 右下を48とする連番で指定)");
            Console.Write("パスフレーズ開始地点(1-42): ");
            var startIndex = Console.ReadLine();
            Console.Write("パスフレーズ終了地点(6-48): ");
            var endIndex = Console.ReadLine();

            var configure = new Configure();
            configure.Set(GlobalSettings.CONNECTION_NAME, connectionName);
            configure.Set(GlobalSettings.USER_ID, userId);
            configure.Set(GlobalSettings.KEY_NUMBER, keyNumber);
            configure.Set(GlobalSettings.PASS_BEFORE, passBefore);
            configure.Set(GlobalSettings.START_INDEX, startIndex);
            configure.Set(GlobalSettings.END_INDEX, endIndex);

            configure.Save();

            Console.WriteLine();
            Console.WriteLine("初期設定の完了 次回起動時から設定を使用してログインします");
        }
    }
}
