using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace Konfidence.TeamFoundation
{
    public class Permissions
    {
        private string _TfsServer = string.Empty;
        private TeamFoundationServer _Tfs;

        public Permissions(string tfsServer)
        {
            _TfsServer = tfsServer;
        }

        public List<string> GetGlobalPermissions()
        {
            List<string> globalPermissions = new List<string>();

            Authenticate();

            VersionControlServer vcServer = _Tfs.GetService(typeof(VersionControlServer)) as VersionControlServer;

            globalPermissions.AddRange(vcServer.GetEffectiveGlobalPermissions(_Tfs.AuthenticatedUserName));

            return globalPermissions;
        }

        public List<string> GetItemPermissions(string sourceItem)
        {
            List<string> itemPermissions = new List<string>();

            Authenticate();

            VersionControlServer vcServer = _Tfs.GetService(typeof(VersionControlServer)) as VersionControlServer;

            itemPermissions.AddRange(vcServer.GetEffectivePermissions(_Tfs.AuthenticatedUserName, sourceItem));

            return itemPermissions;

        }

        private void Authenticate()
        {
            _Tfs = new TeamFoundationServer(_TfsServer, new UICredentialsProvider());

            _Tfs.EnsureAuthenticated();
        }

        private void CheckOut(string sourceItem)
        {

            Authenticate();

            VersionControlServer vcServer = _Tfs.GetService(typeof(VersionControlServer)) as VersionControlServer;

            //vcServer.

            Item SourceControlItem = vcServer.GetItem(sourceItem);

            SourceControlItem.DownloadFile("");


        }
    }
}
