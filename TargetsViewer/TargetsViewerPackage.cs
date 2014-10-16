using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using EnvDTE;

namespace Company.TargetsViewer
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [ProvideToolWindow(typeof(TargetListToolWindow), Style = Microsoft.VisualStudio.Shell.VsDockStyle.Tabbed, Window = "3ae79031-e1bc-11d0-8f78-00a0c9110057")]
    [Guid(GuidList.guidTargetsViewerPkgString)]
    public sealed class TargetsViewerPackage : Package, IVsRunningDocTableEvents
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public TargetsViewerPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }



        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();
            this.run();
        }
        #endregion

        private void run()
        {
            DTE dte = (DTE)GetService(typeof(DTE));

            IVsRunningDocumentTable runningDocumentTable = (IVsRunningDocumentTable)GetService(typeof(SVsRunningDocumentTable));

            uint pdwCookie;
            runningDocumentTable.AdviseRunningDocTableEvents(this, out pdwCookie);
        }


        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterSave(uint docCookie)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            IVsRunningDocumentTable runningDocumentTable = (IVsRunningDocumentTable)GetService(typeof(SVsRunningDocumentTable));

            uint pgrfRDTFlags, pdwReadLocks, pdwEditLocks, pitemid;
            string pbstrMkDocument;
            IVsHierarchy ppHier;
            IntPtr ppunkDocData;

            runningDocumentTable.GetDocumentInfo(docCookie,
                out  pgrfRDTFlags,
                out  pdwReadLocks,
                out  pdwEditLocks,
                out  pbstrMkDocument,
                out  ppHier,
                out  pitemid,
                out  ppunkDocData);

            Debug.WriteLine("--Document Opened");
            if (StatelessServiceSingleton.Instance.IsSupportedProjectType(pbstrMkDocument))
            {
                DTE dte = (DTE)GetService(typeof(DTE));
                Microsoft.Build.BuildEngine.TargetCollection targets = StatelessServiceSingleton.Instance.ListTargets(pbstrMkDocument);
                StatelessServiceSingleton.Instance.OutputTargetsToOuputWindow(dte, targets);

                TargetListToolWindow toolWindow = StatelessServiceSingleton.Instance.GetTargetListToolWindow(this);
                toolWindow.Open();
                toolWindow.LoadTargets(targets);
            }

            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }
    }
}
