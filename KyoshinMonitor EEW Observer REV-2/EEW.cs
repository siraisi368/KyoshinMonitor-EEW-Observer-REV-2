using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KyoshinMonitor_EEW_Observer_REV_2
{
    class EEW
    {
        public Result result { get; set; }
        public string report_time { get; set; }
        public string region_code { get; set; }
        public string request_time { get; set; }
        public string region_name { get; set; }
        public string longitude { get; set; }
        public String is_cancel { get; set; }
        public string depth { get; set; }
        public string calcintensity { get; set; }
        public String is_final { get; set; }
        public String is_training { get; set; }
        public string latitude { get; set; }
        public string origin_time { get; set; }
        public Security security { get; set; }
        public string magunitude { get; set; }
        public string report_num { get; set; }
        public string request_hypo_type { get; set; }
        public string report_id { get; set; }
        public string alertflg { get; set; }
    }
    public class Result
    {
        public string status { get; set; }
        public string message { get; set; }
        public String is_auth { get; set; }
    }

    public class Security
    {
        public string realm { get; set; }
        public string hash { get; set; }
    }
}
