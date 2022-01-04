using System;
using System.IO;
using System.Linq;

namespace Demo
{
    public static class FileOperator
    {
        private static string GetPathRoot()
        {
            var path = Path.GetPathRoot(Environment.SystemDirectory);
            return path;
        }

        public static string GetPath()
        {
            var allDrives = GetDriveInfo();
            DriveInfo drive = null;
            if(allDrives.Length > 1)
            {
                drive = allDrives.FirstOrDefault(drive => !drive.Name.Equals(GetPathRoot()));
            }
            else if(allDrives.Length < 1 && allDrives.Length != 0)
            {
                drive = allDrives.FirstOrDefault(drive => drive.Name.Equals(GetPathRoot()));
            }
            return drive.Name;
        }

        public static DriveInfo[] GetDriveInfo()
        {
            DriveInfo[] driveInfos = DriveInfo.GetDrives();
            foreach (DriveInfo d in driveInfos)
            {
                Console.WriteLine("Drive {0}", d.Name);
                Console.WriteLine("  Drive type: {0}", d.DriveType);
                
                if (d.IsReady != true)
                    continue;
                
                Console.WriteLine("  Volume label: {0}", d.VolumeLabel);
                Console.WriteLine("  File system: {0}", d.DriveFormat);
                Console.WriteLine(
                    "  Available space to current user:{0, 15} bytes",
                    d.AvailableFreeSpace);

                Console.WriteLine(
                    "  Total available space:          {0, 15} bytes",
                    d.TotalFreeSpace);

                Console.WriteLine(
                    "  Total size of drive:            {0, 15} bytes ",
                    d.TotalSize);
            }

            return driveInfos;
        }
    }
}