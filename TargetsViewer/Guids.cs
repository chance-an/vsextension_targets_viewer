// Guids.cs
// MUST match guids.h
using System;

namespace Company.TargetsViewer
{
    static class GuidList
    {
        public const string guidTargetsViewerPkgString = "253b0af3-3072-4442-86e6-14dc1b8446da";
        public const string guidTargetsViewerCmdSetString = "009256f9-3903-4aaa-b9b4-92c88899eeed";
        public const string guidTargetsListToolWindow = "43A5E613-4C07-4B84-9B6F-0A30EFF07E8E";

        public static readonly Guid guidTargetsViewerCmdSet = new Guid(guidTargetsViewerCmdSetString);
        public static readonly Guid targetsListToolWindow = new Guid(guidTargetsListToolWindow);
    };
}