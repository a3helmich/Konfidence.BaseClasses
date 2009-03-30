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
        private VersionControlServer _VcServer;

        public Permissions(string tfsServer)
        {
            _TfsServer = tfsServer;
        }

        private void TfsInitialize()
        {
            _Tfs = new TeamFoundationServer(_TfsServer, new UICredentialsProvider());

            _Tfs.EnsureAuthenticated();

            _VcServer = _Tfs.GetService(typeof(VersionControlServer)) as VersionControlServer;
        }

        public List<string> GetGlobalPermissions()
        {
            List<string> globalPermissions = new List<string>();

            TfsInitialize();

            globalPermissions.AddRange(_VcServer.GetEffectiveGlobalPermissions(_Tfs.AuthenticatedUserName));

            return globalPermissions;
        }

        public List<string> GetItemPermissions(string sourceItem)
        {
            List<string> itemPermissions = new List<string>();

            TfsInitialize();

            itemPermissions.AddRange(_VcServer.GetEffectivePermissions(_Tfs.AuthenticatedUserName, sourceItem));

            return itemPermissions;

        }

        public void CheckOut(string sourceItem)
        {
            string fileName = @"C:\Projects\Konfidence\BaseClasses\ReferenceReBaser\ReferenceReBaser.csproj"; 
            TfsInitialize();

            //Item SourceControlItem = _VcServer.GetItem(sourceItem);

            //SourceControlItem.DownloadFile("");

            //Workspace ws = _VcServer.GetWorkspace(@"c:\projects\konfidence\baseclasses");
            Workspace ws = _VcServer.GetWorkspace("XPBASEVS2008", "Administrator");

            //ws.SetLock(@"c:\projects\konfidence\baseclasses\Konfidence.TeamFoundation.csproj", LockLevel.None);
            int countertje = ws.PendEdit(fileName);
            //ws.Undo(@"c:\projects\konfidence\baseclasses\Konfidence.TeamFoundation.csproj");

            PendingChange[] pendingChangeList = ws.GetPendingChanges(fileName);

            ws.CheckIn(pendingChangeList, "dit is een test met de TF library");

        }
    }
}
