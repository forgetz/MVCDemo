using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGMenuWebservices.Data
{
    public class CheckUserAuthentication
    {
        public string CostCenter { get; set; }
        public string user_login { get; set; }
        public string PersonID { get; set; }
        public string Email { get; set; }
        public List<Gen_PGMenu> PGMenu { get; set; }
        public bool IsAuthen { get; set; }
        public IEnumerable<GroupUserN> groupUserNList { get; set; }
    }

    public class Gen_PGMenu
    {
        public string sys_code { get; set; }
        public string parentmenu_code { get; set; }
        public string menu_code { get; set; }
        public string menu_name { get; set; }
        public string menu_desc { get; set; }
        public string menu_url { get; set; }
        public string menu_seq { get; set; }
        public string imagelist_id { get; set; }
        public string menu_status { get; set; }
        public int level { get; set; }
        public bool isParent { get; set; }
    }
}
