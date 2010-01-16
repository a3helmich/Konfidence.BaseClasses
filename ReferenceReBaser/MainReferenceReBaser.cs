using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Konfidence.TeamFoundation;

namespace Konfidence.ReferenceReBaserApp
{
    class MainReferenceReBaser
    {
        internal void Execute(string solutionFolder)
        {
            ReferenceReBaser reBaser = new ReferenceReBaser();

            reBaser.ReBaseProjects(solutionFolder);

            ChangesForm changesForm = new ChangesForm();

            changesForm.ShowList(reBaser.OverviewList);
        }
    }
}
