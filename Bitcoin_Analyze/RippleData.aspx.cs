using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebSocketSharp;

namespace Bitcoin_Analyze
{
    public partial class RippleData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            test();
        }

        public static void test()
        {
            using (var ws = new WebSocket("wss://s1.ripple.com:51233/"))
            {
                ws.OnMessage += (sender, e) =>
                  Console.WriteLine("Laputa says: " + e.Data);

                ws.Connect();


                string strtemp = "{\"id\": 2,\"command\": \"ping\"}";
                    //"{\"id\": 1,\"command\": \"book_offers\",\"taker\": \"r9cZA1mLK5R5Am25ArfXFmqgNwjZgnfk59\",\"taker_gets\": {\"value\": \"1\",\"currency\": \"EUR\",\"issuer\": \"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\"},\"taker_pays\": {\"value\": \"1\",\"currency\": \"USD\",\"issuer\":\"rvYAfWj5gh67oV6fW32ZzP3Aw4Eubs59B\"}\"}";
                ws.Send(strtemp);
                Console.ReadKey(true);
            }
        }
    }
}