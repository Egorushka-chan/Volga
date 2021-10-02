using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace WordParser.Models
{
    class RAMCounter
    {
        private ulong _totalRAM = 99999;     //общая память ОЗУ
        private ulong _freeRAM = 2048;      //свободная память ОЗУ
        private int[] _acceptableRangeRAM; //какую память можно дать дать программе

        public RAMCounter()
        {
            ManagementObjectSearcher ramMonitor =    //запрос к WMI для получения памяти ПК
                    new ManagementObjectSearcher("SELECT TotalVisibleMemorySize,FreePhysicalMemory FROM Win32_OperatingSystem");
            foreach (ManagementObject objram in ramMonitor.Get())
            {
                _totalRAM = Convert.ToUInt64(objram["TotalVisibleMemorySize"]);
                _freeRAM = Convert.ToUInt64(objram["FreePhysicalMemory"]); 
            }

            ulong len = _freeRAM / 1024 / 1024;
            List<int> memList = new List<int>();

            int bit = OSBitChecker.Is64Bit()? 3 : 2; // необходимо учитывать максимум виртуальной памяти

            for(ulong i = 0; i < len & i < (ulong)bit; i++)
            {
                memList.Add(Convert.ToInt32(1000 * (i+1)));
            };

            _acceptableRangeRAM = memList.ToArray();
        }

        public ulong GetTotalRam()
        {
            return _totalRAM;
        }

        public ulong GetFreeRam()
        {
            return _freeRAM;
        }

        public int[] GetAcceptableRangeOfRAM()
        {
            return _acceptableRangeRAM;
        }
    }
}
