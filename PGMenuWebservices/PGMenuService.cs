using PGMenuWebservices.Data;
using PGMenuWebservices.pgmenuwebservice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PGMenuWebservices
{
    public class PGMenuService
    {
        private CheckUserAuthentication CheckUserAuthenticationService(string username, [Optional]string password)
        {
            var userAuthorization = new pgmenuwebservice.UserAuthorization();
            var sysCode = Properties.Settings.Default["SysCode"].ToString();

            clsMenuAuthorization data;
            if (password != null)
                data = userAuthorization.checkUserAuthentication(sysCode, username, password, "", "");
            else
                data = userAuthorization.checkUserAuthenticationNoPassword(sysCode, username, "", "");

            if (data == null)
            {
                //throw new Exception("Authentication Fail");
                var _checkUserAuthentication = new CheckUserAuthentication()
                {
                    CostCenter = null,
                    Email = null,
                    PersonID = null,
                    user_login = null,
                    PGMenu = null,
                    IsAuthen = false
                };
                return _checkUserAuthentication;
            }

            var checkUserAuthentication = new CheckUserAuthentication()
            {
                CostCenter = data.CostCenter,
                Email = data.Email,
                PersonID = data.PersonID,
                user_login = data.user_login,
                PGMenu = null,
                IsAuthen = false
            };

            if (!data.AuthenStatus)
                return checkUserAuthentication;

            if (data.Gen_PGMenu == null)
                return checkUserAuthentication;

            checkUserAuthentication.IsAuthen = true;

            var menu = new List<Gen_PGMenu>();
            if (data.Gen_PGMenu.Count() > 0)
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(data.Gen_PGMenu);

                var xmlNodeList = xmlDocument.GetElementsByTagName("Table");


                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    menu.Add(new Gen_PGMenu()
                    {
                        sys_code = xmlNode.ChildNodes[0].InnerText,
                        parentmenu_code = xmlNode.ChildNodes[1].InnerText,
                        menu_code = xmlNode.ChildNodes[2].InnerText,
                        menu_name = xmlNode.ChildNodes[3].InnerText,
                        menu_desc = xmlNode.ChildNodes[4].InnerText,
                        menu_url = xmlNode.ChildNodes[5].InnerText,
                        menu_seq = xmlNode.ChildNodes[6].InnerText,
                        imagelist_id = xmlNode.ChildNodes[7].InnerText,
                        menu_status = xmlNode.ChildNodes[8].InnerText,
                        level = int.Parse(xmlNode.ChildNodes[9].InnerText),
                    });
                }
            }

            checkUserAuthentication.PGMenu = menu;

            return checkUserAuthentication;
        }

        public CheckUserAuthentication PGMenuCheckUserAuthenticationService(string user_name, string password)
        {
            var sysCode = Properties.Settings.Default["SysCode"].ToString();
            var data = this.CheckUserAuthenticationService(user_name, password);
            var group = this.PGGroupUserN(user_name);

            if (data == null)
                return null;

            if (!data.IsAuthen)
                return null;

            List<Gen_PGMenu> menuList = IsParent(data.PGMenu, sysCode);
            data.PGMenu = menuList;
            data.groupUserNList = group;
            return data;
        }

        public List<Gen_PGMenu> IsParent(List<Gen_PGMenu> list, string system)
        {
            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.parentmenu_code) && item.parentmenu_code.CompareTo(system) != 0)
                {
                    foreach (var parent in list)
                    {
                        if (parent.menu_code.CompareTo(item.parentmenu_code) == 0)
                        {
                            parent.isParent = true;
                        }
                    }
                }
            }
            return list;
        }

        public CheckUserAuthentication PGMenuCheckUserAuthenticationServiceNoPassword(string user_name)
        {
            var sysCode = Properties.Settings.Default["SysCode"].ToString();
            var data = this.CheckUserAuthenticationService(user_name);
            var group = this.PGGroupUserN(user_name);

            if (data == null)
                return null;

            if (!data.IsAuthen)
                return null;

            List<Gen_PGMenu> menuList = IsParent(data.PGMenu, sysCode);
            data.PGMenu = menuList;
            data.groupUserNList = group;
            return data;
        }

        public IEnumerable<GroupUserN> PGGroupUserN(string userLogin)
        {
            string sysCode = Properties.Settings.Default["SysCode"].ToString();
            var userGroupN = new List<GroupUserN>();

            var userAuthorization = new pgmenuwebservice.UserAuthorization();
            string strUserAuthorizeCtrl = userAuthorization.GroupUserN(sysCode, userLogin, "");

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(strUserAuthorizeCtrl);
            XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName("tbgroup");

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                userGroupN.Add(new GroupUserN()
                {
                    sys_code = xmlNode.ChildNodes[0].InnerText,
                    group_code = xmlNode.ChildNodes[1].InnerText,
                    user_login = xmlNode.ChildNodes[2].InnerText,
                    nameeng = xmlNode.ChildNodes[3].InnerText,
                    surnameeng = xmlNode.ChildNodes[4].InnerText,
                    department = xmlNode.ChildNodes[5].InnerText,
                    personid = xmlNode.ChildNodes[6].InnerText,
                    email = xmlNode.ChildNodes[7].InnerText,
                    user_status = xmlNode.ChildNodes[8].InnerText,
                });
            }

            return userGroupN;
        }

        public UserInformation GetUserInformation(string userName)
        {
            var userAuthorization = new pgmenuwebservice.UserAuthorization();
            DataSet data = userAuthorization.UserInformation(string.Format(@"@{0}", userName));

            if (data != null && data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0)
            {
                DataRow row = data.Tables[0].Rows[0];
                UserInformation userInformation = new UserInformation
                {
                    userId = row["userId"].ToString(),
                    nameeng = row["nameeng"].ToString(),
                    surnameeng = row["surnameeng"].ToString(),
                    positioneng = row["positioneng"].ToString(),
                    department = row["department"].ToString(),
                    PersonId = row["PersonId"].ToString(),
                    Email = row["Email"].ToString(),
                    CostCenter = row["CostCenter"].ToString(),
                    TitleThai = row["TitleThai"].ToString(),
                    NameThai = row["NameThai"].ToString(),
                    SurnameThai = row["SurnameThai"].ToString(),
                    TitleEng = row["TitleEng"].ToString(),
                    Location = row["Location"].ToString(),
                    CostCenterLocation = row["CostCenterLocation"].ToString(),
                    Sex = row["Sex"].ToString(),
                    DepartmentId = row["DepartmentId"].ToString(),
                    CostCenter7 = row["CostCenter7"].ToString(),
                };

                return userInformation;
            }

            return null;
        }

    }
}
