using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Xml;
using Konfidence.TeamFoundation.Project;
using Konfidence.Base;
using System.IO;

namespace Konfidence.TeamFoundation
{
    public class TfsPermissions: BaseItem
    {
        private string _TfsServer = string.Empty;

        public string TfsServer
        {
            get { return _TfsServer; }
        }
        private TeamFoundationServer _Tfs = null;
        private VersionControlServer _VcServer = null;

        private List<string> _CheckOutList = new List<string>();

        public TfsPermissions(string tfsServer)
        {
            _TfsServer = tfsServer;
        }

        private void TfsInitialize()
        {
            if (!IsAssigned(_Tfs))
            {
                _Tfs = new TeamFoundationServer(_TfsServer, new UICredentialsProvider());

                _Tfs.EnsureAuthenticated();

                _VcServer = _Tfs.GetService(typeof(VersionControlServer)) as VersionControlServer;
            }
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

        private bool FindCheckOut(string fileName)
        {
            foreach (string file in _CheckOutList)
            {
                if (file.Equals(fileName))
                {
                    return true;
                }
            }

            return false;
        }

        private Workspace GetWorkSpace(string fileName)
        {
            string workSpacePath = Path.GetDirectoryName(fileName);

            return _VcServer.GetWorkspace(workSpacePath); 
        }

        public bool IsCheckedOut(string fileName)
        {
            TfsInitialize();

            Workspace ws = GetWorkSpace(fileName);

            PendingChange[] pendingChangeList = ws.GetPendingChanges(fileName);

            if (pendingChangeList.Length > 0)
            {
                return true;
            }

            return false;
        }

        public bool CheckOut(string fileName)
        {
            if (!IsCheckedOut(fileName))
            {
                TfsInitialize();

                Workspace ws = GetWorkSpace(fileName);

                int checkOutCount = ws.PendEdit(fileName);

                if (checkOutCount > 0)
                {
                    _CheckOutList.Add(fileName);

                    return true;
                }
            }

            return false;
        }

        public bool Undo(string fileName)
        {
            if (FindCheckOut(fileName))
            {
                TfsInitialize();

                Workspace ws = GetWorkSpace(fileName);

                ws.Undo(fileName);

                _CheckOutList.Remove(fileName);

                return true;
            }

            return false;
        }

        public bool CheckIn(string fileName)
        {
            if (FindCheckOut(fileName))
            {
                TfsInitialize();

                Workspace ws = GetWorkSpace(fileName);

                PendingChange[] pendingChangeList = ws.GetPendingChanges(fileName);

                ws.CheckIn(pendingChangeList, "dit is een test met de TF library");

                _CheckOutList.Remove(fileName);

                return true;
            }

            return false;
        }

    }
}
