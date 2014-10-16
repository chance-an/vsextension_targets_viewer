namespace Company.TargetsViewer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Build.BuildEngine;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using EnvDTE;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio;

    class StatelessServiceSingleton
    {
        private static readonly StatelessServiceSingleton instance = new StatelessServiceSingleton();

        private StatelessServiceSingleton() { }

        public static StatelessServiceSingleton Instance
        {
            get
            {
                return instance;
            }
        }

        public TargetCollection ListTargets(string filename)
        {
            //Engine.GlobalEngine.BinPath = @"C:\Windows\Microsoft.NET\Framework\v2.0.NNNNN";
            Microsoft.Build.BuildEngine.Project project = new Microsoft.Build.BuildEngine.Project();
            project.Load(filename);
            return project.Targets;
        }

        public bool IsSupportedProjectType(string filename)
        {
            filename = System.IO.Path.GetFileName(filename);
            Regex regex = new Regex(@"proj$");
            return regex.IsMatch(filename);
        }

        public void OutputTargetsToOuputWindow(DTE dte, TargetCollection targetsCollection)
        {
            string myString =
              "Name is " + dte.Name + "\rVersion is " + dte.Version;

            Windows windows = dte.Windows;
            Window window =
            (Window)windows.Item("{34E76E81-EE4A-11D0-AE2E-00A0C90FFFC3}");
            window.Visible = true;

            OutputWindow outputWindow = (OutputWindow)window.Object;

            IEnumerable<OutputWindowPane> outputWindowPanes = outputWindow.OutputWindowPanes.Cast<OutputWindowPane>();

            OutputWindowPane outputWindowPane = outputWindowPanes.FirstOrDefault(pane => pane.Name == "Targets");
            if (outputWindowPane == null)
            {
                outputWindowPane = outputWindow.OutputWindowPanes.Add("Targets");
            }
            outputWindowPane.Activate();
            foreach (Target target in targetsCollection)
            {
                outputWindowPane.OutputString(target.Name + "\n");
            }
        }

        public TargetListToolWindow GetTargetListToolWindow(IServiceProvider sp) {
            IVsUIShell vsUIShell = (IVsUIShell)sp.GetService(typeof(SVsUIShell));
            IVsWindowFrame toolWindowFrame;

            Guid toolWindowGuid = GuidList.targetsListToolWindow;

            if (vsUIShell.FindToolWindow((uint)__VSFINDTOOLWIN.FTW_fForceCreate, ref toolWindowGuid, out toolWindowFrame) != VSConstants.S_OK)
            {
                throw new NotSupportedException("Getting ToolWindow frame failed.");
            };

            if (null == toolWindowFrame)
            {
                throw new NotSupportedException();
            }

            Object toolWindowPane;
            if (toolWindowFrame.GetProperty((int)__VSFPROPID.VSFPROPID_DocView, out toolWindowPane) != VSConstants.S_OK) {
                throw new NotSupportedException();
            }
            return toolWindowPane as TargetListToolWindow;
        }
    }
}
