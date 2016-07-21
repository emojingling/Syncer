using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace chenz
{
    public class NtfsUsnJournal : IDisposable
    {
        #region enum(s)
        public enum UsnJournalReturnCode
        {
            INVALID_HANDLE_VALUE = -1,
            USN_JOURNAL_SUCCESS = 0,
            ERROR_INVALID_FUNCTION = 1,
            ERROR_FILE_NOT_FOUND = 2,
            ERROR_PATH_NOT_FOUND = 3,
            ERROR_TOO_MANY_OPEN_FILES = 4,
            ERROR_ACCESS_DENIED = 5,
            ERROR_INVALID_HANDLE = 6,
            ERROR_INVALID_DATA = 13,
            ERROR_HANDLE_EOF = 38,
            ERROR_NOT_SUPPORTED = 50,
            ERROR_INVALID_PARAMETER = 87,
            ERROR_JOURNAL_DELETE_IN_PROGRESS = 1178,
            USN_JOURNAL_NOT_ACTIVE = 1179,
            ERROR_JOURNAL_ENTRY_DELETED = 1181,
            ERROR_INVALID_USER_BUFFER = 1784,
            USN_JOURNAL_INVALID = 17001,
            VOLUME_NOT_NTFS = 17003,
            INVALID_FILE_REFERENCE_NUMBER = 17004,
            USN_JOURNAL_ERROR = 17005
        }

        public enum UsnReasonCode
        {
            USN_REASON_DATA_OVERWRITE = 0x00000001,
            USN_REASON_DATA_EXTEND = 0x00000002,
            USN_REASON_DATA_TRUNCATION = 0x00000004,
            USN_REASON_NAMED_DATA_OVERWRITE = 0x00000010,
            USN_REASON_NAMED_DATA_EXTEND = 0x00000020,
            USN_REASON_NAMED_DATA_TRUNCATION = 0x00000040,
            USN_REASON_FILE_CREATE = 0x00000100,
            USN_REASON_FILE_DELETE = 0x00000200,
            USN_REASON_EA_CHANGE = 0x00000400,
            USN_REASON_SECURITY_CHANGE = 0x00000800,
            USN_REASON_RENAME_OLD_NAME = 0x00001000,
            USN_REASON_RENAME_NEW_NAME = 0x00002000,
            USN_REASON_INDEXABLE_CHANGE = 0x00004000,
            USN_REASON_BASIC_INFO_CHANGE = 0x00008000,
            USN_REASON_HARD_LINK_CHANGE = 0x00010000,
            USN_REASON_COMPRESSION_CHANGE = 0x00020000,
            USN_REASON_ENCRYPTION_CHANGE = 0x00040000,
            USN_REASON_OBJECT_ID_CHANGE = 0x00080000,
            USN_REASON_REPARSE_POINT_CHANGE = 0x00100000,
            USN_REASON_STREAM_CHANGE = 0x00200000,
            USN_REASON_CLOSE = -1
        }

        #endregion

        #region private member variables

        private DriveInfo _driveInfo = null;
        private uint _volumeSerialNumber;
        private IntPtr _usnJournalRootHandle;

        private bool bNtfsVolume;

        private const ulong RootFileReferenceNumber = 1407374883553285;     //目前根目录的frn均固定为此数字

        #endregion

        #region properties

        private static TimeSpan _elapsedTime;
        public static TimeSpan ElapsedTime
        {
            get { return _elapsedTime; }
        }

        public string VolumeName
        {
            get { return _driveInfo.Name; }
        }

        public long AvailableFreeSpace
        {
            get { return _driveInfo.AvailableFreeSpace; }
        }

        public long TotalFreeSpace
        {
            get { return _driveInfo.TotalFreeSpace; }
        }

        public string Format
        {
            get { return _driveInfo.DriveFormat; }
        }

        public DirectoryInfo RootDirectory
        {
            get { return _driveInfo.RootDirectory; }
        }

        public long TotalSize
        {
            get { return _driveInfo.TotalSize; }
        }

        public string VolumeLabel
        {
            get { return _driveInfo.VolumeLabel; }
        }

        public uint VolumeSerialNumber
        {
            get { return _volumeSerialNumber; }
        }

        #endregion

        #region constructor(s)

        /// <summary>
        /// NtfsUsnJournal类的构造函数。如果未抛出异常，_usnJournalRootHandle和_volumeSerialNumber可被认为是好用的。
        /// 如果抛出了异常，本对象是不可用的。
        /// </summary>
        /// <param name="driveInfo">提供对有关驱动器的信息的访问。</param>
        /// <remarks> 
        /// 在下列情况下均会抛出异常：该卷不是“NTFS”卷；GetRootHandle() 或 GetVolumeSerialNumber()执行失败。
        /// 每个公共方法均检查该卷是否为“NTFS”卷以及_usnJournalRootHandle的有效性。
        /// 如果这两个条件不满足，公共函数会返回一个UsnJournalReturnCode类型的错误。
        /// </remarks>
        public NtfsUsnJournal(DriveInfo driveInfo)
        {
            DateTime start = DateTime.Now;
            _driveInfo = driveInfo;

            if (0 == string.Compare(_driveInfo.DriveFormat, "ntfs", true))
            {
                bNtfsVolume = true;

                IntPtr rootHandle = IntPtr.Zero;
                UsnJournalReturnCode usnRtnCode = GetRootHandle(out rootHandle);

                if (usnRtnCode == UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
                {
                    _usnJournalRootHandle = rootHandle;
                    usnRtnCode = GetVolumeSerialNumber(_driveInfo, out _volumeSerialNumber);
                    if (usnRtnCode != UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
                    {
                        _elapsedTime = DateTime.Now - start;
                        throw new Win32Exception((int)usnRtnCode);
                    }
                }
                else
                {
                    _elapsedTime = DateTime.Now - start;
                    throw new Win32Exception((int)usnRtnCode);
                }
            }
            else
            {
                _elapsedTime = DateTime.Now - start;
                throw new Exception(string.Format("{0} 不是“NTFS”卷。", _driveInfo.Name));
            }
            _elapsedTime = DateTime.Now - start;
        }

        #endregion

        #region public methods

        /// <summary>在卷上创建USN日志。如果USN日志已存在且需要更大的日志文件空间，函数将调整MaximumSize 和 AllocationDelta参数。</summary>
        /// <param name="maxSize">USN日志所需的最大空间</param>
        /// <param name="allocationDelta">当空间不足时，每次增加操作附加的空间大小</param>
        /// <returns>a UsnJournalReturnCode
        /// USN_JOURNAL_SUCCESS                 CreateUsnJournal() function succeeded. 
        /// VOLUME_NOT_NTFS                     volume is not an NTFS volume.
        /// INVALID_HANDLE_VALUE                NtfsUsnJournal object failed initialization.
        /// USN_JOURNAL_NOT_ACTIVE              usn journal is not active on volume.
        /// ERROR_ACCESS_DENIED                 accessing the usn journal requires admin rights, see remarks.
        /// ERROR_INVALID_FUNCTION              error generated by DeviceIoControl() call.
        /// ERROR_FILE_NOT_FOUND                error generated by DeviceIoControl() call.
        /// ERROR_PATH_NOT_FOUND                error generated by DeviceIoControl() call.
        /// ERROR_TOO_MANY_OPEN_FILES           error generated by DeviceIoControl() call.
        /// ERROR_INVALID_HANDLE                error generated by DeviceIoControl() call.
        /// ERROR_INVALID_DATA                  error generated by DeviceIoControl() call.
        /// ERROR_NOT_SUPPORTED                 error generated by DeviceIoControl() call.
        /// ERROR_INVALID_PARAMETER             error generated by DeviceIoControl() call.
        /// ERROR_JOURNAL_DELETE_IN_PROGRESS    usn journal delete is in progress.
        /// ERROR_INVALID_USER_BUFFER           error generated by DeviceIoControl() call.
        /// USN_JOURNAL_ERROR                   unspecified usn journal error.
        /// </returns>
        /// <remarks>
        /// 如果程序需要以管理员身份运行，函数返回ERROR_ACCESS_DENIED。
        /// ElapsedTime显示函数运行耗费的时间。
        /// </remarks>
        public UsnJournalReturnCode CreateUsnJournal(ulong maxSize, ulong allocationDelta)
        {
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.VOLUME_NOT_NTFS;
            DateTime startTime = DateTime.Now;

            if (bNtfsVolume)
            {
                if (_usnJournalRootHandle.ToInt32() != WINAPI.INVALID_HANDLE_VALUE)
                {
                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
                    UInt32 cb;

                    WINAPI.CREATE_USN_JOURNAL_DATA cujd = new WINAPI.CREATE_USN_JOURNAL_DATA
                    {
                        MaximumSize = maxSize,
                        AllocationDelta = allocationDelta
                    };

                    int sizeCujd = Marshal.SizeOf(cujd);
                    IntPtr cujdBuffer = Marshal.AllocHGlobal(sizeCujd);
                    WINAPI.ZeroMemory(cujdBuffer, sizeCujd);
                    Marshal.StructureToPtr(cujd, cujdBuffer, true);

                    bool fOk = WINAPI.DeviceIoControl(
                        _usnJournalRootHandle,
                        WINAPI.FSCTL_CREATE_USN_JOURNAL,
                        cujdBuffer,
                        sizeCujd,
                        IntPtr.Zero,
                        0,
                        out cb,
                        IntPtr.Zero);
                    if (!fOk)
                    {
                        usnRtnCode = ConvertWin32ErrorToUsnError((WINAPI.GetLastErrorEnum)Marshal.GetLastWin32Error());
                    }
                    Marshal.FreeHGlobal(cujdBuffer);
                }
                else
                {
                    usnRtnCode = UsnJournalReturnCode.INVALID_HANDLE_VALUE;
                }
            }

            _elapsedTime = DateTime.Now - startTime;
            return usnRtnCode;
        }

        /// <summary>删除卷中的USN日志。如果日志不存在，函数返回成功。</summary>
        /// <param name="journalState">该卷的 USN_JOURNAL_DATA 对象</param>
        /// <returns>a UsnJournalReturnCode
        /// USN_JOURNAL_SUCCESS                 DeleteUsnJournal() function succeeded. 
        /// VOLUME_NOT_NTFS                     volume is not an NTFS volume.
        /// INVALID_HANDLE_VALUE                NtfsUsnJournal object failed initialization.
        /// USN_JOURNAL_NOT_ACTIVE              usn journal is not active on volume.
        /// ERROR_ACCESS_DENIED                 accessing the usn journal requires admin rights, see remarks.
        /// ERROR_INVALID_FUNCTION              error generated by DeviceIoControl() call.
        /// ERROR_FILE_NOT_FOUND                error generated by DeviceIoControl() call.
        /// ERROR_PATH_NOT_FOUND                error generated by DeviceIoControl() call.
        /// ERROR_TOO_MANY_OPEN_FILES           error generated by DeviceIoControl() call.
        /// ERROR_INVALID_HANDLE                error generated by DeviceIoControl() call.
        /// ERROR_INVALID_DATA                  error generated by DeviceIoControl() call.
        /// ERROR_NOT_SUPPORTED                 error generated by DeviceIoControl() call.
        /// ERROR_INVALID_PARAMETER             error generated by DeviceIoControl() call.
        /// ERROR_JOURNAL_DELETE_IN_PROGRESS    usn journal delete is in progress.
        /// ERROR_INVALID_USER_BUFFER           error generated by DeviceIoControl() call.
        /// USN_JOURNAL_ERROR                   unspecified usn journal error.
        /// </returns>
        /// <remarks>
        /// 如果程序需要以管理员身份运行，函数返回ERROR_ACCESS_DENIED。
        /// ElapsedTime显示函数运行时间。
        /// </remarks>
        public UsnJournalReturnCode DeleteUsnJournal(WINAPI.USN_JOURNAL_DATA journalState)
        {
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.VOLUME_NOT_NTFS;
            DateTime startTime = DateTime.Now;

            if (bNtfsVolume)
            {
                if (_usnJournalRootHandle.ToInt32() != WINAPI.INVALID_HANDLE_VALUE)
                {
                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
                    UInt32 cb;

                    WINAPI.DELETE_USN_JOURNAL_DATA dujd = new WINAPI.DELETE_USN_JOURNAL_DATA
                    {
                        UsnJournalID = journalState.UsnJournalID,
                        DeleteFlags = (UInt32) WINAPI.UsnJournalDeleteFlags.USN_DELETE_FLAG_DELETE
                    };

                    int sizeDujd = Marshal.SizeOf(dujd);
                    IntPtr dujdBuffer = Marshal.AllocHGlobal(sizeDujd);
                    WINAPI.ZeroMemory(dujdBuffer, sizeDujd);
                    Marshal.StructureToPtr(dujd, dujdBuffer, true);

                    bool fOk = WINAPI.DeviceIoControl(
                        _usnJournalRootHandle,
                        WINAPI.FSCTL_DELETE_USN_JOURNAL,
                        dujdBuffer,
                        sizeDujd,
                        IntPtr.Zero,
                        0,
                        out cb,
                        IntPtr.Zero);

                    if (!fOk)
                    {
                        usnRtnCode = ConvertWin32ErrorToUsnError((WINAPI.GetLastErrorEnum)Marshal.GetLastWin32Error());
                    }
                    Marshal.FreeHGlobal(dujdBuffer);
                }
                else
                {
                    usnRtnCode = UsnJournalReturnCode.INVALID_HANDLE_VALUE;
                }
            }

            _elapsedTime = DateTime.Now - startTime;
            return usnRtnCode;
        }

        /// <summary>读取主文件表（MFT）并找到卷中全部的目录，存入List。</summary>
        /// <param name="folders">保存卷中目录的列表</param>
        /// <returns>
        /// USN_JOURNAL_SUCCESS                 GetNtfsVolumeFolders() function succeeded. 
        /// VOLUME_NOT_NTFS                     volume is not an NTFS volume.
        /// INVALID_HANDLE_VALUE                NtfsUsnJournal object failed initialization.
        /// USN_JOURNAL_NOT_ACTIVE              usn journal is not active on volume.
        /// ERROR_ACCESS_DENIED                 accessing the usn journal requires admin rights, see remarks.
        /// ERROR_INVALID_FUNCTION              error generated by DeviceIoControl() call.
        /// ERROR_FILE_NOT_FOUND                error generated by DeviceIoControl() call.
        /// ERROR_PATH_NOT_FOUND                error generated by DeviceIoControl() call.
        /// ERROR_TOO_MANY_OPEN_FILES           error generated by DeviceIoControl() call.
        /// ERROR_INVALID_HANDLE                error generated by DeviceIoControl() call.
        /// ERROR_INVALID_DATA                  error generated by DeviceIoControl() call.
        /// ERROR_NOT_SUPPORTED                 error generated by DeviceIoControl() call.
        /// ERROR_INVALID_PARAMETER             error generated by DeviceIoControl() call.
        /// ERROR_JOURNAL_DELETE_IN_PROGRESS    usn journal delete is in progress.
        /// ERROR_INVALID_USER_BUFFER           error generated by DeviceIoControl() call.
        /// USN_JOURNAL_ERROR                   unspecified usn journal error.
        /// </returns>
        /// <remarks>
        /// 如果程序需要以管理员身份运行，函数返回ERROR_ACCESS_DENIED。
        /// ElapsedTime显示函数运行时间。
        /// </remarks>
        public UsnJournalReturnCode GetNtfsVolumeFolders(out List<WINAPI.UsnEntry> folders)
        {
            DateTime startTime = DateTime.Now;
            folders = new List<WINAPI.UsnEntry>();
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.VOLUME_NOT_NTFS;

            if (bNtfsVolume)
            {
                if (_usnJournalRootHandle.ToInt32() != WINAPI.INVALID_HANDLE_VALUE)
                {
                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;

                    WINAPI.USN_JOURNAL_DATA usnState = new WINAPI.USN_JOURNAL_DATA();
                    usnRtnCode = QueryUsnJournal(ref usnState);

                    if (usnRtnCode == UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
                    {
                        //
                        // set up MFT_ENUM_DATA structure
                        //
                        WINAPI.MFT_ENUM_DATA med;
                        med.StartFileReferenceNumber = 0;
                        med.LowUsn = 0;
                        med.HighUsn = usnState.NextUsn;
                        Int32 sizeMftEnumData = Marshal.SizeOf(med);
                        IntPtr medBuffer = Marshal.AllocHGlobal(sizeMftEnumData);
                        WINAPI.ZeroMemory(medBuffer, sizeMftEnumData);
                        Marshal.StructureToPtr(med, medBuffer, true);

                        //
                        // set up the data buffer which receives the USN_RECORD data
                        //
                        int pDataSize = sizeof(UInt64) + 10000;
                        IntPtr pData = Marshal.AllocHGlobal(pDataSize);
                        WINAPI.ZeroMemory(pData, pDataSize);
                        uint outBytesReturned = 0;

                        //
                        // Gather up volume's directories
                        //
                        while (false != WINAPI.DeviceIoControl(
                            _usnJournalRootHandle,
                            WINAPI.FSCTL_ENUM_USN_DATA,
                            medBuffer,
                            sizeMftEnumData,
                            pData,
                            pDataSize,
                            out outBytesReturned,
                            IntPtr.Zero))
                        {
                            IntPtr pUsnRecord = new IntPtr(pData.ToInt32() + sizeof(Int64));
                            while (outBytesReturned > 60)
                            {
                                var usnEntry = new WINAPI.UsnEntry(pUsnRecord);
                                //
                                // check for directory entries
                                //
                                if (usnEntry.IsFolder)
                                {
                                    folders.Add(usnEntry);
                                }
                                pUsnRecord = new IntPtr(pUsnRecord.ToInt32() + usnEntry.RecordLength);
                                outBytesReturned -= usnEntry.RecordLength;
                            }
                            Marshal.WriteInt64(medBuffer, Marshal.ReadInt64(pData, 0));
                        }

                        Marshal.FreeHGlobal(pData);
                        usnRtnCode = ConvertWin32ErrorToUsnError((WINAPI.GetLastErrorEnum)Marshal.GetLastWin32Error());
                        if (usnRtnCode == UsnJournalReturnCode.ERROR_HANDLE_EOF)
                        {
                            usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
                        }
                    }
                }
                else
                {
                    usnRtnCode = UsnJournalReturnCode.INVALID_HANDLE_VALUE;
                }
            }
            folders.Sort();
            _elapsedTime = DateTime.Now - startTime;
            return usnRtnCode;
        }

        /// <summary>读取主文件表（MFT）并找到卷中全部的文件与目录，存入List。</summary>
        /// <param name="listEntry">保存卷中文件与目录的列表</param>
        /// <returns>
        /// USN_JOURNAL_SUCCESS                 GetNtfsVolumeFolders() function succeeded. 
        /// VOLUME_NOT_NTFS                     volume is not an NTFS volume.
        /// INVALID_HANDLE_VALUE                NtfsUsnJournal object failed initialization.
        /// USN_JOURNAL_NOT_ACTIVE              usn journal is not active on volume.
        /// ERROR_ACCESS_DENIED                 accessing the usn journal requires admin rights, see remarks.
        /// ERROR_INVALID_FUNCTION              error generated by DeviceIoControl() call.
        /// ERROR_FILE_NOT_FOUND                error generated by DeviceIoControl() call.
        /// ERROR_PATH_NOT_FOUND                error generated by DeviceIoControl() call.
        /// ERROR_TOO_MANY_OPEN_FILES           error generated by DeviceIoControl() call.
        /// ERROR_INVALID_HANDLE                error generated by DeviceIoControl() call.
        /// ERROR_INVALID_DATA                  error generated by DeviceIoControl() call.
        /// ERROR_NOT_SUPPORTED                 error generated by DeviceIoControl() call.
        /// ERROR_INVALID_PARAMETER             error generated by DeviceIoControl() call.
        /// ERROR_JOURNAL_DELETE_IN_PROGRESS    usn journal delete is in progress.
        /// ERROR_INVALID_USER_BUFFER           error generated by DeviceIoControl() call.
        /// USN_JOURNAL_ERROR                   unspecified usn journal error.
        /// </returns>
        /// <remarks>
        /// 如果程序需要以管理员身份运行，函数返回ERROR_ACCESS_DENIED。
        /// ElapsedTime显示函数运行时间。
        /// </remarks>
        public UsnJournalReturnCode GetNtfsVolumeFiles(out List<WINAPI.UsnEntry> listEntry)
        {
            DateTime startTime = DateTime.Now;
            listEntry = new List<WINAPI.UsnEntry>();
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.VOLUME_NOT_NTFS;

            if (bNtfsVolume)
            {
                if (_usnJournalRootHandle.ToInt32() != WINAPI.INVALID_HANDLE_VALUE)
                {
                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;

                    WINAPI.USN_JOURNAL_DATA usnState = new WINAPI.USN_JOURNAL_DATA();
                    usnRtnCode = QueryUsnJournal(ref usnState);

                    if (usnRtnCode == UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
                    {
                        //
                        // set up MFT_ENUM_DATA structure
                        //
                        WINAPI.MFT_ENUM_DATA med;
                        med.StartFileReferenceNumber = 0;
                        med.LowUsn = 0;
                        med.HighUsn = usnState.NextUsn;
                        Int32 sizeMftEnumData = Marshal.SizeOf(med);
                        IntPtr medBuffer = Marshal.AllocHGlobal(sizeMftEnumData);
                        WINAPI.ZeroMemory(medBuffer, sizeMftEnumData);
                        Marshal.StructureToPtr(med, medBuffer, true);

                        //
                        // set up the data buffer which receives the USN_RECORD data
                        //
                        int pDataSize = sizeof(UInt64) + 10000;
                        IntPtr pData = Marshal.AllocHGlobal(pDataSize);
                        WINAPI.ZeroMemory(pData, pDataSize);
                        uint outBytesReturned = 0;

                        //
                        // Gather up volume's directories
                        //
                        while (false != WINAPI.DeviceIoControl(
                            _usnJournalRootHandle,
                            WINAPI.FSCTL_ENUM_USN_DATA,
                            medBuffer,
                            sizeMftEnumData,
                            pData,
                            pDataSize,
                            out outBytesReturned,
                            IntPtr.Zero))
                        {
                            IntPtr pUsnRecord = new IntPtr(pData.ToInt32() + sizeof(Int64));
                            while (outBytesReturned > 60)
                            {
                                var usnEntry = new WINAPI.UsnEntry(pUsnRecord);
                                listEntry.Add(usnEntry);
                                pUsnRecord = new IntPtr(pUsnRecord.ToInt32() + usnEntry.RecordLength);
                                outBytesReturned -= usnEntry.RecordLength;
                            }
                            Marshal.WriteInt64(medBuffer, Marshal.ReadInt64(pData, 0));
                        }

                        Marshal.FreeHGlobal(pData);
                        usnRtnCode = ConvertWin32ErrorToUsnError((WINAPI.GetLastErrorEnum)Marshal.GetLastWin32Error());
                        if (usnRtnCode == UsnJournalReturnCode.ERROR_HANDLE_EOF)
                        {
                            usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
                        }
                    }
                }
                else
                {
                    usnRtnCode = UsnJournalReturnCode.INVALID_HANDLE_VALUE;
                }
            }
            listEntry.Sort();
            _elapsedTime = DateTime.Now - startTime;
            return usnRtnCode;
        }

        /// <summary>根据输入项寻找符合条件的文件信息列表</summary>
        /// <param name="filter">部分（或全部）文件名</param>
        /// <param name="files">输出参数，找到的符合项列表</param>
        /// <param name="filterIsExt">filter项是否为扩展名</param>
        /// <returns>
        /// USN_JOURNAL_SUCCESS                 GetNtfsVolumeFolders() function succeeded. 
        /// VOLUME_NOT_NTFS                     volume is not an NTFS volume.
        /// INVALID_HANDLE_VALUE                NtfsUsnJournal object failed initialization.
        /// USN_JOURNAL_NOT_ACTIVE              usn journal is not active on volume.
        /// ERROR_ACCESS_DENIED                 accessing the usn journal requires admin rights, see remarks.
        /// ERROR_INVALID_FUNCTION              error generated by DeviceIoControl() call.
        /// ERROR_FILE_NOT_FOUND                error generated by DeviceIoControl() call.
        /// ERROR_PATH_NOT_FOUND                error generated by DeviceIoControl() call.
        /// ERROR_TOO_MANY_OPEN_FILES           error generated by DeviceIoControl() call.
        /// ERROR_INVALID_HANDLE                error generated by DeviceIoControl() call.
        /// ERROR_INVALID_DATA                  error generated by DeviceIoControl() call.
        /// ERROR_NOT_SUPPORTED                 error generated by DeviceIoControl() call.
        /// ERROR_INVALID_PARAMETER             error generated by DeviceIoControl() call.
        /// ERROR_JOURNAL_DELETE_IN_PROGRESS    usn journal delete is in progress.
        /// ERROR_INVALID_USER_BUFFER           error generated by DeviceIoControl() call.
        /// USN_JOURNAL_ERROR                   unspecified usn journal error.
        /// </returns>
        public UsnJournalReturnCode GetFilesMatchingFilter(string filter, 
            out List<WINAPI.UsnEntry> files, bool filterIsExt = false)
        {
            DateTime startTime = DateTime.Now;
            filter = filter.ToLower();
            files = new List<WINAPI.UsnEntry>();
            string[] fileTypes = filter.Split(' ', ',', ';');
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.VOLUME_NOT_NTFS;

            if (bNtfsVolume)
            {
                if (_usnJournalRootHandle.ToInt32() != WINAPI.INVALID_HANDLE_VALUE)
                {
                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;

                    WINAPI.USN_JOURNAL_DATA usnState = new WINAPI.USN_JOURNAL_DATA();
                    usnRtnCode = QueryUsnJournal(ref usnState);

                    if (usnRtnCode == UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
                    {
                        //set up MFT_ENUM_DATA structure
                        WINAPI.MFT_ENUM_DATA med;
                        med.StartFileReferenceNumber = 0;
                        med.LowUsn = 0;
                        med.HighUsn = usnState.NextUsn;
                        Int32 sizeMftEnumData = Marshal.SizeOf(med);
                        IntPtr medBuffer = Marshal.AllocHGlobal(sizeMftEnumData);
                        WINAPI.ZeroMemory(medBuffer, sizeMftEnumData);
                        Marshal.StructureToPtr(med, medBuffer, true);

                        //set up the data buffer which receives the USN_RECORD data
                        int pDataSize = sizeof(UInt64) + 10000;
                        IntPtr pData = Marshal.AllocHGlobal(pDataSize);
                        WINAPI.ZeroMemory(pData, pDataSize);
                        uint outBytesReturned = 0;
                        WINAPI.UsnEntry usnEntry = null;

                        //Gather up volume's directories
                        while (false != WINAPI.DeviceIoControl(
                            _usnJournalRootHandle,
                            WINAPI.FSCTL_ENUM_USN_DATA,
                            medBuffer,
                            sizeMftEnumData,
                            pData,
                            pDataSize,
                            out outBytesReturned,
                            IntPtr.Zero))
                        {
                            IntPtr pUsnRecord = new IntPtr(pData.ToInt32() + sizeof(Int64));
                            while (outBytesReturned > 60)
                            {
                                usnEntry = new WINAPI.UsnEntry(pUsnRecord);
                                //check for directory entries
                                if (usnEntry.IsFile)
                                {
                                    if (filterIsExt)                                //仅搜索后缀
                                    {
                                        string extension = Path.GetExtension(usnEntry.Name).ToLower();
                                        if (0 == string.CompareOrdinal(filter, "*"))
                                        {
                                            files.Add(usnEntry);
                                        }
                                        else if (!string.IsNullOrEmpty(extension))  //搜索文件名+后缀
                                        {
                                            foreach (string fileType in fileTypes)
                                            {
                                                if (extension.Contains(fileType))
                                                {
                                                    files.Add(usnEntry);
                                                }
                                            }
                                        }
                                    }
                                    else if (usnEntry.Name.ToLower().Contains(filter))
                                    {
                                        files.Add(usnEntry);
                                    }
                                }
                                pUsnRecord = new IntPtr(pUsnRecord.ToInt32() + usnEntry.RecordLength);
                                outBytesReturned -= usnEntry.RecordLength;
                            }
                            Marshal.WriteInt64(medBuffer, Marshal.ReadInt64(pData, 0));
                        }

                        Marshal.FreeHGlobal(pData);
                        usnRtnCode = ConvertWin32ErrorToUsnError((WINAPI.GetLastErrorEnum)Marshal.GetLastWin32Error());
                        if (usnRtnCode == UsnJournalReturnCode.ERROR_HANDLE_EOF)
                        {
                            usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
                        }
                    }
                }
                else
                {
                    usnRtnCode = UsnJournalReturnCode.INVALID_HANDLE_VALUE;
                }
            }
            files.Sort();
            _elapsedTime = DateTime.Now - startTime;
            return usnRtnCode;
        }

        /// <summary>根据文件索引号（reference number）得到文件的完整路径。</summary>
        /// <param name="frn">一个64位文件索引号</param>
        /// <param name="path">待查找文件的完整路径</param>
        /// <returns>
        /// USN_JOURNAL_SUCCESS                 GetPathFromFileReference() function succeeded. 
        /// VOLUME_NOT_NTFS                     volume is not an NTFS volume.
        /// INVALID_HANDLE_VALUE                NtfsUsnJournal object failed initialization.
        /// ERROR_ACCESS_DENIED                 accessing the usn journal requires admin rights, see remarks.
        /// INVALID_FILE_REFERENCE_NUMBER       file reference number not found in Master File Table.
        /// ERROR_INVALID_FUNCTION              error generated by NtCreateFile() or NtQueryInformationFile() call.
        /// ERROR_FILE_NOT_FOUND                error generated by NtCreateFile() or NtQueryInformationFile() call.
        /// ERROR_PATH_NOT_FOUND                error generated by NtCreateFile() or NtQueryInformationFile() call.
        /// ERROR_TOO_MANY_OPEN_FILES           error generated by NtCreateFile() or NtQueryInformationFile() call.
        /// ERROR_INVALID_HANDLE                error generated by NtCreateFile() or NtQueryInformationFile() call.
        /// ERROR_INVALID_DATA                  error generated by NtCreateFile() or NtQueryInformationFile() call.
        /// ERROR_NOT_SUPPORTED                 error generated by NtCreateFile() or NtQueryInformationFile() call.
        /// ERROR_INVALID_PARAMETER             error generated by NtCreateFile() or NtQueryInformationFile() call.
        /// ERROR_INVALID_USER_BUFFER           error generated by NtCreateFile() or NtQueryInformationFile() call.
        /// USN_JOURNAL_ERROR                   unspecified usn journal error.
        /// </returns>
        /// <remarks>
        /// 如果程序需要以管理员身份运行，函数返回ERROR_ACCESS_DENIED。
        /// ElapsedTime显示函数运行时间。
        /// </remarks>
        public UsnJournalReturnCode GetPathFromFileReference(UInt64 frn, out string path)
        {
            DateTime startTime = DateTime.Now;
            path = "Unavailable"
                ;
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.VOLUME_NOT_NTFS;

            if (bNtfsVolume)
            {
                if (_usnJournalRootHandle.ToInt32() != WINAPI.INVALID_HANDLE_VALUE)
                {
                    if (frn != 0)
                    {
                        usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;

                        long allocSize = 0;
                        WINAPI.UNICODE_STRING unicodeString;
                        WINAPI.OBJECT_ATTRIBUTES objAttributes = new WINAPI.OBJECT_ATTRIBUTES();
                        WINAPI.IO_STATUS_BLOCK ioStatusBlock = new WINAPI.IO_STATUS_BLOCK();
                        IntPtr hFile = IntPtr.Zero;

                        IntPtr buffer = Marshal.AllocHGlobal(4096);
                        IntPtr refPtr = Marshal.AllocHGlobal(8);
                        IntPtr objAttIntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(objAttributes));

                        //
                        // pointer >> fileid
                        //
                        Marshal.WriteInt64(refPtr, (long)frn);

                        unicodeString.Length = 8;
                        unicodeString.MaximumLength = 8;
                        unicodeString.Buffer = refPtr;
                        //
                        // copy unicode structure to pointer
                        //
                        Marshal.StructureToPtr(unicodeString, objAttIntPtr, true);

                        //
                        //  InitializeObjectAttributes 
                        //
                        objAttributes.Length = Marshal.SizeOf(objAttributes);
                        objAttributes.ObjectName = objAttIntPtr;
                        objAttributes.RootDirectory = _usnJournalRootHandle;
                        objAttributes.Attributes = (int)WINAPI.OBJ_CASE_INSENSITIVE;

                        int fOk = WINAPI.NtCreateFile(
                            ref hFile,
                            FileAccess.Read,
                            ref objAttributes,
                            ref ioStatusBlock,
                            ref allocSize,
                            0,
                            FileShare.ReadWrite,
                            WINAPI.FILE_OPEN,
                            WINAPI.FILE_OPEN_BY_FILE_ID | WINAPI.FILE_OPEN_FOR_BACKUP_INTENT,
                            IntPtr.Zero, 0);
                        if (fOk == 0)
                        {
                            fOk = WINAPI.NtQueryInformationFile(
                                hFile,
                                ref ioStatusBlock,
                                buffer,
                                4096,
                                WINAPI.FILE_INFORMATION_CLASS.FileNameInformation);
                            if (fOk == 0)
                            {
                                //
                                // first 4 bytes are the name length
                                //
                                int nameLength = Marshal.ReadInt32(buffer, 0);
                                //
                                // next bytes are the name
                                //
                                path = Marshal.PtrToStringUni(new IntPtr(buffer.ToInt32() + 4), nameLength / 2);
                            }
                        }
                        WINAPI.CloseHandle(hFile);
                        Marshal.FreeHGlobal(buffer);
                        Marshal.FreeHGlobal(objAttIntPtr);
                        Marshal.FreeHGlobal(refPtr);
                    }
                }
            }
            _elapsedTime = DateTime.Now - startTime;
            return usnRtnCode;
        }

        /// <summary>如果USN日志是激活的，返回其当前状态。</summary>
        /// <param name="usnJournalState">记录USN日志状态的结构体</param>
        /// <returns>
        /// USN_JOURNAL_SUCCESS                 GetUsnJournalState() function succeeded. 
        /// VOLUME_NOT_NTFS                     volume is not an NTFS volume.
        /// INVALID_HANDLE_VALUE                NtfsUsnJournal object failed initialization.
        /// USN_JOURNAL_NOT_ACTIVE              usn journal is not active on volume.
        /// ERROR_ACCESS_DENIED                 accessing the usn journal requires admin rights, see remarks.
        /// ERROR_INVALID_FUNCTION              error generated by DeviceIoControl() call.
        /// ERROR_FILE_NOT_FOUND                error generated by DeviceIoControl() call.
        /// ERROR_PATH_NOT_FOUND                error generated by DeviceIoControl() call.
        /// ERROR_TOO_MANY_OPEN_FILES           error generated by DeviceIoControl() call.
        /// ERROR_INVALID_HANDLE                error generated by DeviceIoControl() call.
        /// ERROR_INVALID_DATA                  error generated by DeviceIoControl() call.
        /// ERROR_NOT_SUPPORTED                 error generated by DeviceIoControl() call.
        /// ERROR_INVALID_PARAMETER             error generated by DeviceIoControl() call.
        /// ERROR_JOURNAL_DELETE_IN_PROGRESS    usn journal delete is in progress.
        /// ERROR_INVALID_USER_BUFFER           error generated by DeviceIoControl() call.
        /// USN_JOURNAL_ERROR                   unspecified usn journal error.
        /// </returns>
        /// <remarks>
        /// 如果程序需要以管理员身份运行，函数返回ERROR_ACCESS_DENIED。
        /// ElapsedTime显示函数运行时间。
        /// </remarks>
        public UsnJournalReturnCode GetUsnJournalState(ref WINAPI.USN_JOURNAL_DATA usnJournalState)
        {
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.VOLUME_NOT_NTFS;
            DateTime startTime = DateTime.Now;

            if (bNtfsVolume)
            {
                usnRtnCode = _usnJournalRootHandle.ToInt32() != WINAPI.INVALID_HANDLE_VALUE
                    ? QueryUsnJournal(ref usnJournalState)
                    : UsnJournalReturnCode.INVALID_HANDLE_VALUE;
            }

            _elapsedTime = DateTime.Now - startTime;
            return usnRtnCode;
        }

        /// <summary>传入一个先前的USN日志状态，函数判断USN日志是否激活并且日志入口尚未丢失（换句话说，USN日志有效）。
        /// 随后函数读取USN日志入口集合。如果函数未返回USN_JOURNAL_SUCCESS，该列表为空。</summary>
        /// <param name="previousUsnState">The USN Journal state the last time volume changes were requested.</param>
        /// <param name="reasonMask"></param>
        /// <param name="usnEntries"></param>
        /// <param name="newUsnState">USN日志的当前状态</param>
        /// <returns>
        /// USN_JOURNAL_SUCCESS                 GetUsnJournalChanges() function succeeded. 
        /// VOLUME_NOT_NTFS                     volume is not an NTFS volume.
        /// INVALID_HANDLE_VALUE                NtfsUsnJournal object failed initialization.
        /// USN_JOURNAL_NOT_ACTIVE              usn journal is not active on volume.
        /// ERROR_ACCESS_DENIED                 accessing the usn journal requires admin rights, see remarks.
        /// ERROR_INVALID_FUNCTION              error generated by DeviceIoControl() call.
        /// ERROR_FILE_NOT_FOUND                error generated by DeviceIoControl() call.
        /// ERROR_PATH_NOT_FOUND                error generated by DeviceIoControl() call.
        /// ERROR_TOO_MANY_OPEN_FILES           error generated by DeviceIoControl() call.
        /// ERROR_INVALID_HANDLE                error generated by DeviceIoControl() call.
        /// ERROR_INVALID_DATA                  error generated by DeviceIoControl() call.
        /// ERROR_NOT_SUPPORTED                 error generated by DeviceIoControl() call.
        /// ERROR_INVALID_PARAMETER             error generated by DeviceIoControl() call.
        /// ERROR_JOURNAL_DELETE_IN_PROGRESS    usn journal delete is in progress.
        /// ERROR_INVALID_USER_BUFFER           error generated by DeviceIoControl() call.
        /// USN_JOURNAL_ERROR                   unspecified usn journal error.
        /// </returns>
        /// <remarks>
        /// 如果程序需要以管理员身份运行，函数返回ERROR_ACCESS_DENIED。
        /// ElapsedTime显示函数运行时间。
        /// </remarks>
        public UsnJournalReturnCode GetUsnJournalEntries(WINAPI.USN_JOURNAL_DATA previousUsnState,
            UInt32 reasonMask,
            out List<WINAPI.UsnEntry> usnEntries,
            out WINAPI.USN_JOURNAL_DATA newUsnState)
        {
            DateTime startTime = DateTime.Now;
            usnEntries = new List<WINAPI.UsnEntry>();
            newUsnState = new WINAPI.USN_JOURNAL_DATA();
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.VOLUME_NOT_NTFS;

            if (bNtfsVolume)
            {
                if (_usnJournalRootHandle.ToInt32() != WINAPI.INVALID_HANDLE_VALUE)
                {
                    //
                    // get current usn journal state
                    //
                    usnRtnCode = QueryUsnJournal(ref newUsnState);
                    if (usnRtnCode == UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
                    {
                        bool bReadMore = true;
                        //
                        // sequentially process the usn journal looking for image file entries
                        //
                        int pbDataSize = sizeof (UInt64) * 0x4000;
                        IntPtr pbData = Marshal.AllocHGlobal(pbDataSize);
                        WINAPI.ZeroMemory(pbData, pbDataSize);
                        uint outBytesReturned = 0;

                        WINAPI.READ_USN_JOURNAL_DATA rujd = new WINAPI.READ_USN_JOURNAL_DATA();
                        rujd.StartUsn = previousUsnState.NextUsn;
                        rujd.ReasonMask = reasonMask;
                        rujd.ReturnOnlyOnClose = 0;
                        rujd.Timeout = 0;
                        rujd.bytesToWaitFor = 0;
                        rujd.UsnJournalId = previousUsnState.UsnJournalID;
                        int sizeRujd = Marshal.SizeOf(rujd);

                        IntPtr rujdBuffer = Marshal.AllocHGlobal(sizeRujd);
                        WINAPI.ZeroMemory(rujdBuffer, sizeRujd);
                        Marshal.StructureToPtr(rujd, rujdBuffer, true);

                        WINAPI.UsnEntry usnEntry = null;

                        //
                        // read usn journal entries
                        //
                        while (bReadMore)
                        {
                            bool bRtn = WINAPI.DeviceIoControl(
                                _usnJournalRootHandle,
                                WINAPI.FSCTL_READ_USN_JOURNAL,
                                rujdBuffer,
                                sizeRujd,
                                pbData,
                                pbDataSize,
                                out outBytesReturned,
                                IntPtr.Zero);
                            if (bRtn)
                            {
                                IntPtr pUsnRecord = new IntPtr(pbData.ToInt32() + sizeof (UInt64));
                                while (outBytesReturned > 60) // while there are at least one entry in the usn journal
                                {
                                    usnEntry = new WINAPI.UsnEntry(pUsnRecord);
                                    if (usnEntry.USN >= newUsnState.NextUsn)
                                        // only read until the current usn points beyond the current state's usn
                                    {
                                        bReadMore = false;
                                        break;
                                    }
                                    usnEntries.Add(usnEntry);

                                    pUsnRecord = new IntPtr(pUsnRecord.ToInt32() + usnEntry.RecordLength);
                                    outBytesReturned -= usnEntry.RecordLength;

                                } // end while (outBytesReturned > 60) - closing bracket

                            } // if (bRtn)- closing bracket
                            else
                            {
                                WINAPI.GetLastErrorEnum lastWin32Error =
                                    (WINAPI.GetLastErrorEnum) Marshal.GetLastWin32Error();
                                usnRtnCode = lastWin32Error == WINAPI.GetLastErrorEnum.ERROR_HANDLE_EOF
                                    ? UsnJournalReturnCode.USN_JOURNAL_SUCCESS
                                    : ConvertWin32ErrorToUsnError(lastWin32Error);
                                break;
                            }

                            Int64 nextUsn = Marshal.ReadInt64(pbData, 0);
                            if (nextUsn >= newUsnState.NextUsn)
                            {
                                break;
                            }
                            Marshal.WriteInt64(rujdBuffer, nextUsn);

                        } // end while (bReadMore) - closing bracket

                        Marshal.FreeHGlobal(rujdBuffer);
                        Marshal.FreeHGlobal(pbData);

                    } // if (usnRtnCode == UsnJournalReturnCode.USN_JOURNAL_SUCCESS) - closing bracket

                } // if (_usnJournalRootHandle.ToInt32() != WINAPI.INVALID_HANDLE_VALUE)
                else
                {
                    usnRtnCode = UsnJournalReturnCode.INVALID_HANDLE_VALUE;
                }
            } // if (bNtfsVolume) - closing bracket

            _elapsedTime = DateTime.Now - startTime;
            return usnRtnCode;
        } // GetUsnJournalChanges() - closing bracket

        /// <summary>测试卷中的USN日志是否激活。</summary>
        /// <returns>如果USN日志已激活返回true；如果卷中没有USN日志返回false</returns>
        public bool IsUsnJournalActive()
        {
            DateTime start = DateTime.Now;
            bool bRtnCode = false;

            if (bNtfsVolume)
            {
                if (_usnJournalRootHandle.ToInt32() != WINAPI.INVALID_HANDLE_VALUE)
                {
                    WINAPI.USN_JOURNAL_DATA usnJournalCurrentState = new WINAPI.USN_JOURNAL_DATA();
                    UsnJournalReturnCode usnError = QueryUsnJournal(ref usnJournalCurrentState);
                    if (usnError == UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
                    {
                        bRtnCode = true;
                    }
                }
            }
            _elapsedTime = DateTime.Now - start;
            return bRtnCode;
        }

        /// <summary>测试本卷是否含有USN Journal，并且测试其是否有部分记录已丢失。</summary>
        /// <returns>true if the USN Journal is active and if the JournalId's are the same 
        /// and if all the usn journal entries expected by the previous state are available 
        /// from the current state.
        /// false if not</returns>
        public bool IsUsnJournalValid(WINAPI.USN_JOURNAL_DATA usnJournalPreviousState)
        {
            DateTime start = DateTime.Now;
            bool bRtnCode = false;

            if (bNtfsVolume)
            {
                if (_usnJournalRootHandle.ToInt32() != WINAPI.INVALID_HANDLE_VALUE)
                {
                    WINAPI.USN_JOURNAL_DATA usnJournalState = new WINAPI.USN_JOURNAL_DATA();
                    UsnJournalReturnCode usnError = QueryUsnJournal(ref usnJournalState);

                    if (usnError == UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
                    {
                        if (usnJournalPreviousState.UsnJournalID == usnJournalState.UsnJournalID)
                        {
                            if (usnJournalPreviousState.NextUsn >= usnJournalState.NextUsn)
                            {
                                bRtnCode = true;
                            }
                        }
                    }
                }
            }
            _elapsedTime = DateTime.Now - start;
            return bRtnCode;
        }

        /// <summary>得到卷中全部文件与目录的路径</summary>
        /// <param name="listFile">卷中全部文件与路径信息列表</param>
        /// <returns>路径字典。Key：frn；value：完整路径（文件包括自身文件名）</returns>
        public Dictionary<ulong, string> GetFilePaths(List<WINAPI.UsnEntry> listFile)
        {
            var dicPfrn = GetDicPfrn(listFile);
            Dictionary<ulong, string> dicPaths = new Dictionary<ulong, string>();                           //用于辅助递归
            ulong key = RootFileReferenceNumber;
            dicPaths.Add(key, _driveInfo.Name);
            BuildPath(key, dicPaths, dicPfrn);
            return dicPaths;
        }

        #endregion

        #region private member functions

        /// <summary>将一个Win32错误转化为UsnJournalReturnCode。</summary>
        /// <param name="win32LastError">The 'last' Win32 error.</param>
        /// <returns>
        /// INVALID_HANDLE_VALUE                error generated by Win32 Api calls.
        /// USN_JOURNAL_SUCCESS                 usn journal function succeeded. 
        /// ERROR_INVALID_FUNCTION              error generated by Win32 Api calls.
        /// ERROR_FILE_NOT_FOUND                error generated by Win32 Api calls.
        /// ERROR_PATH_NOT_FOUND                error generated by Win32 Api calls.
        /// ERROR_TOO_MANY_OPEN_FILES           error generated by Win32 Api calls.
        /// ERROR_ACCESS_DENIED                 accessing the usn journal requires admin rights.
        /// ERROR_INVALID_HANDLE                error generated by Win32 Api calls.
        /// ERROR_INVALID_DATA                  error generated by Win32 Api calls.
        /// ERROR_HANDLE_EOF                    error generated by Win32 Api calls.
        /// ERROR_NOT_SUPPORTED                 error generated by Win32 Api calls.
        /// ERROR_INVALID_PARAMETER             error generated by Win32 Api calls.
        /// ERROR_JOURNAL_DELETE_IN_PROGRESS    usn journal delete is in progress.
        /// ERROR_JOURNAL_ENTRY_DELETED         usn journal entry lost, no longer available.
        /// ERROR_INVALID_USER_BUFFER           error generated by Win32 Api calls.
        /// USN_JOURNAL_INVALID                 usn journal is invalid, id's don't match or required entries lost.
        /// USN_JOURNAL_NOT_ACTIVE              usn journal is not active on volume.
        /// VOLUME_NOT_NTFS                     volume is not an NTFS volume.
        /// INVALID_FILE_REFERENCE_NUMBER       bad file reference number - see remarks.
        /// USN_JOURNAL_ERROR                   unspecified usn journal error.
        /// </returns>
        private UsnJournalReturnCode ConvertWin32ErrorToUsnError(WINAPI.GetLastErrorEnum win32LastError)
        {
            UsnJournalReturnCode usnRtnCode;

            switch (win32LastError)
            {
                case WINAPI.GetLastErrorEnum.ERROR_JOURNAL_NOT_ACTIVE:
                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_NOT_ACTIVE;
                    break;
                case WINAPI.GetLastErrorEnum.ERROR_SUCCESS:
                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
                    break;
                case WINAPI.GetLastErrorEnum.ERROR_HANDLE_EOF:
                    usnRtnCode = UsnJournalReturnCode.ERROR_HANDLE_EOF;
                    break;
                default:
                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_ERROR;
                    break;
            }

            return usnRtnCode;
        }

        /// <summary>得到磁盘序列号。</summary>
        /// <param name="driveInfo">DriveInfo object representing the volume in question.</param>
        /// <param name="volumeSerialNumber">out parameter to hold the volume serial number.</param>
        /// <returns></returns>
        private UsnJournalReturnCode GetVolumeSerialNumber(DriveInfo driveInfo, out uint volumeSerialNumber)
        {
            Console.WriteLine("GetVolumeSerialNumber() function entered for drive '{0}'", driveInfo.Name);

            volumeSerialNumber = 0;
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
            string pathRoot = string.Concat("\\\\.\\", driveInfo.Name);

            IntPtr hRoot = WINAPI.CreateFile(pathRoot,
                0,
                WINAPI.FILE_SHARE_READ | WINAPI.FILE_SHARE_WRITE,
                IntPtr.Zero,
                WINAPI.OPEN_EXISTING,
                WINAPI.FILE_FLAG_BACKUP_SEMANTICS,
                IntPtr.Zero);

            if (hRoot.ToInt32() != WINAPI.INVALID_HANDLE_VALUE)
            {
                WINAPI.BY_HANDLE_FILE_INFORMATION fi = new WINAPI.BY_HANDLE_FILE_INFORMATION();
                bool bRtn = WINAPI.GetFileInformationByHandle(hRoot, out fi);

                if (bRtn)
                {
                    UInt64 fileIndexHigh = (UInt64)fi.FileIndexHigh;
                    UInt64 indexRoot = (fileIndexHigh << 32) | fi.FileIndexLow;
                    volumeSerialNumber = fi.VolumeSerialNumber;
                }
                else
                {
                    usnRtnCode = (UsnJournalReturnCode)Marshal.GetLastWin32Error();
                }

                WINAPI.CloseHandle(hRoot);
            }
            else
            {
                usnRtnCode = (UsnJournalReturnCode)Marshal.GetLastWin32Error();
            }

            return usnRtnCode;
        }

        private UsnJournalReturnCode GetRootHandle(out IntPtr rootHandle)
        {
            //
            // private functions don't need to check for an NTFS volume or
            // a valid _usnJournalRootHandle handle
            //
            UsnJournalReturnCode usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
            rootHandle = IntPtr.Zero;
            string vol = string.Concat("\\\\.\\", _driveInfo.Name.TrimEnd('\\'));

            rootHandle = WINAPI.CreateFile(vol,
                 WINAPI.GENERIC_READ | WINAPI.GENERIC_WRITE,
                 WINAPI.FILE_SHARE_READ | WINAPI.FILE_SHARE_WRITE,
                 IntPtr.Zero,
                 WINAPI.OPEN_EXISTING,
                 0,
                 IntPtr.Zero);

            if (rootHandle.ToInt32() == WINAPI.INVALID_HANDLE_VALUE)
            {
                usnRtnCode = (UsnJournalReturnCode)Marshal.GetLastWin32Error();
            }

            return usnRtnCode;
        }

        /// <summary>查询卷中的USN日志。</summary>
        /// <param name="usnJournalState">the USN_JOURNAL_DATA object that is associated with this volume</param>
        /// <returns></returns>
        private UsnJournalReturnCode QueryUsnJournal(ref WINAPI.USN_JOURNAL_DATA usnJournalState)
        {
            //
            // private functions don't need to check for an NTFS volume or
            // a valid _usnJournalRootHandle handle
            //
            UsnJournalReturnCode usnReturnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
            int sizeUsnJournalState = Marshal.SizeOf(usnJournalState);
            UInt32 cb;

            bool fOk = WINAPI.DeviceIoControl(
                _usnJournalRootHandle,
                WINAPI.FSCTL_QUERY_USN_JOURNAL,
                IntPtr.Zero,
                0,
                out usnJournalState,
                sizeUsnJournalState,
                out cb,
                IntPtr.Zero);

            if (!fOk)
            {
                int lastWin32Error = Marshal.GetLastWin32Error();
                usnReturnCode = ConvertWin32ErrorToUsnError((WINAPI.GetLastErrorEnum)Marshal.GetLastWin32Error());
            }

            return usnReturnCode;
        }

        /// <summary>得到子UsnEntry字典</summary>
        /// <param name="listFile">卷中Entry列表</param>
        /// <returns>Key: frn; Value: 该frn子目录与文件集合</returns>
        private Dictionary<ulong, List<WINAPI.UsnEntry>> GetDicPfrn(List<WINAPI.UsnEntry> listFile)
        {
            if (listFile == null || listFile.Count == 0)
                throw new NullReferenceException("文件信息表为空！");

            Dictionary<ulong, List<WINAPI.UsnEntry>> dic = new Dictionary<ulong, List<WINAPI.UsnEntry>>();  //根据pfrn作为key值对记录进行分类储存
            foreach (WINAPI.UsnEntry entry in listFile)
            {
                if (dic.ContainsKey(entry.ParentFileReferenceNumber))
                {
                    List<WINAPI.UsnEntry> list = dic[entry.ParentFileReferenceNumber];
                    list.Add(entry);
                }
                else
                {
                    List<WINAPI.UsnEntry> list = new List<WINAPI.UsnEntry>();
                    list.Add(entry);
                    dic.Add(entry.ParentFileReferenceNumber, list);
                }
            }
            return dic;
        }

        /// <summary>递归建立路径</summary>
        /// <param name="key">frn值</param>
        /// <param name="dicPaths">frn-路径字典</param>
        /// <param name="dicPfrn">prfn-entry字典</param>
        private void BuildPath(ulong key, Dictionary<ulong, string> dicPaths, Dictionary<ulong, List<WINAPI.UsnEntry>> dicPfrn)
        {
            // 获取到属于该key的记录
            if (!dicPfrn.ContainsKey(key)) return;
            List<WINAPI.UsnEntry> records = dicPfrn[key];
            if (records == null || records.Count <= 0) return;
            // 获取该key对应的路径
            string filePath = dicPaths[key];
            // 设置路径
            foreach (WINAPI.UsnEntry record in records)
            {
                if (record.IsFolder)
                {
                    if (!dicPaths.ContainsKey(record.FileReferenceNumber))
                        dicPaths.Add(record.FileReferenceNumber, filePath + record.Name + "\\");
                    BuildPath(record.FileReferenceNumber, dicPaths, dicPfrn);
                }
                else if (!dicPaths.ContainsKey(record.FileReferenceNumber))
                    dicPaths.Add(record.FileReferenceNumber, filePath + record.Name);
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>执行与释放或重置非托管资源相关的应用程序定义的任务。</summary>
        public void Dispose()
        {
            WINAPI.CloseHandle(_usnJournalRootHandle);
        }

        #endregion
    }
}