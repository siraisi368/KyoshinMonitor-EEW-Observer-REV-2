using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KyoshinMonitor_EEW_Observer_REV_2
{
    internal class MonitorImageSelector
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns>(url, description)</returns>
        public static (string, string) GetUrlAndDescriptionFromCode(int code, string time1 = null, string time2 = null)
        {
            string url = string.Empty;
            string desc = string.Empty;

            switch (code)
            {
                case 0:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/jma_s/%TIME1%/%TIME2%.jma_s.gif";
                    desc = "地表震度";
                    break;

                case 1:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/acmap_s/%TIME1%/%TIME2%.acmap_s.gif";
                    desc = "地表加速度";
                    break;

                case 2:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/vcmap_s/%TIME1%/%TIME2%.vcmap_s.gif";
                    desc = "地表速度";
                    break;

                case 3:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/dcmap_s/%TIME1%/%TIME2%.dcmap_s.gif";
                    desc = "地表変位";
                    break;

                case 4:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp0125_s/%TIME1%/%TIME2%.rsp0125_s.gif";
                    desc = "地表0.125Hz応答";
                    break;

                case 5:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp0250_s/%TIME1%/%TIME2%.rsp0250_s.gif";
                    desc = "地表0.250Hz応答";
                    break;

                case 6:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp0500_s/%TIME1%/%TIME2%.rsp0500_s.gif";
                    desc = "地表0.500Hz応答";
                    break;

                case 7:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp1000_s/%TIME1%/%TIME2%.rsp1000_s.gif";
                    desc = "地表1Hz応答";
                    break;

                case 8:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp2000_s/%TIME1%/%TIME2%.rsp2000_s.gif";
                    desc = "地表2Hz応答";
                    break;

                case 9:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp4000_s/%TIME1%/%TIME2%.rsp4000_s.gif";
                    desc = "地表4Hz応答";
                    break;

                case 10:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/jma_b/%TIME1%/%TIME2%.jma_b.gif";
                    desc = "地中震度";
                    break;

                case 11:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/acmap_b/%TIME1%/%TIME2%.acmap_b.gif";
                    desc = "地中加速";
                    break;

                case 12:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/vcmap_b/%TIME1%/%TIME2%.vcmap_b.gif";
                    desc = "地中速度";
                    break;

                case 13:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/dcmap_b/%TIME1%/%TIME2%.dcmap_b.gif";
                    desc = "地中変位";
                    break;

                case 14:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp0125_b/%TIME1%/%TIME2%.rsp0125_b.gif";
                    desc = "地中0.125Hz応答";
                    break;

                case 15:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp0250_b/%TIME1%/%TIME2%.rsp0250_b.gif";
                    desc = "地中0.250Hz応答";
                    break;

                case 16:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp0500_b/%TIME1%/%TIME2%.rsp0500_b.gif";
                    desc = "地中0.500Hz応答";
                    break;

                case 17:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp1000_b/%TIME1%/%TIME2%.rsp1000_b.gif";
                    desc = "地中1Hz応答";
                    break;

                case 18:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp2000_b/%TIME1%/%TIME2%.rsp2000_b.gif";
                    desc = "地中2Hz応答";
                    break;

                case 19:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/RealTimeImg/rsp4000_b/%TIME1%/%TIME2%.rsp4000_b.gif";
                    desc = "地中4Hz応答";
                    break;

                case 20:
                    url = "http://www.kmoni.bosai.go.jp//data/map_img/EstShindoImg/eew/%TIME1%/%TIME2%.eew.gif";
                    desc = "EEW予測";
                    break;

                case 21:
                    url = "https://www.lmoni.bosai.go.jp/monitor/data/data/map_img/RealTimeImg/abrspmx_s/%TIME1%/%TIME2%.abrspmx_s.gif";
                    desc = "長周期地震動";
                    break;
            }

            if (time1 != null) url = url.Replace("%TIME1%", time1);
            if (time2 != null) url = url.Replace("%TIME2%", time2);

            return (url, desc);
        }
    }
}
