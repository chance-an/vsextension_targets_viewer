using Microsoft.Build.BuildEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Company.TargetsViewer
{
    /// <summary>
    /// Interaction logic for TargetListToolWindowControl.xaml
    /// </summary>
    public partial class TargetListToolWindowControl : UserControl
    {
        ObservableCollection<Target> targets;
        public ReadOnlyObservableCollection<Target> Targets
        {
            get {
                return new ReadOnlyObservableCollection<Target>(this.targets);
            }
        }
        public TargetListToolWindowControl()
        {
            targets = new ObservableCollection<Target>();

            InitializeComponent();
        }

        public void LoadTargets(TargetCollection targets)
        {
            this.targets.Clear();

            foreach (Target target in targets)
            {
                this.targets.Add(target);
            }
        }
    }
}
