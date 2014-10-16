using Microsoft.Build.BuildEngine;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Company.TargetsViewer
{
    [Guid(GuidList.guidTargetsListToolWindow)]
    class TargetListToolWindow : ToolWindowPane
    {
        public TargetListToolWindow()
        {
            this.Caption = "Build Targets";

            TargetListToolWindowControl control = new TargetListToolWindowControl();

            this.Content = control;
        }

        public void Open() {
            ((IVsWindowFrame)this.Frame).Show();
        }

        public void LoadTargets(TargetCollection targets)
        {
            TargetListToolWindowControl control = (TargetListToolWindowControl)this.Content;
            control.LoadTargets(targets);
        }
    }
}
