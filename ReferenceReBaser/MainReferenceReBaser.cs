using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation;
using Konfidence.TeamFoundation.Exceptions;
using System.Windows.Forms;

namespace Konfidence.ReferenceReBaserApp
{
    class MainReferenceReBaser
    {
        internal void Execute(string solutionFolder)
        {
            try
            {
                ReferenceReBaser reBaser = new ReferenceReBaser();

                reBaser.ReBaseProjects(solutionFolder);

                ChangesForm changesForm = new ChangesForm();

                changesForm.ShowList(reBaser.RebasedProjectList);
            }
            catch (TfsServerException tfsEx)
            {
                MessageBox.Show(tfsEx.Message);
            }
        }
    }
}
