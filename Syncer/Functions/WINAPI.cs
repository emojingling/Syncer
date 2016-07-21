using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Windows.Forms;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace chenz
{
    public class WINAPI
    {
        #region enums

        public enum GetLastErrorEnum
        {
            INVALID_HANDLE_VALUE = -1,
            ERROR_SUCCESS = 0,
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
            ERROR_JOURNAL_NOT_ACTIVE = 1179,
            ERROR_JOURNAL_ENTRY_DELETED = 1181,
            ERROR_INVALID_USER_BUFFER = 1784
        }

        public enum UsnJournalDeleteFlags
        {
            USN_DELETE_FLAG_DELETE = 1,
            USN_DELETE_FLAG_NOTIFY = 2
        }

        public enum FILE_INFORMATION_CLASS
        {
            FileDirectoryInformation = 1, // 1
            FileFullDirectoryInformation = 2, // 2
            FileBothDirectoryInformation = 3, // 3
            FileBasicInformation = 4, // 4
            FileStandardInformation = 5, // 5
            FileInternalInformation = 6, // 6
            FileEaInformation = 7, // 7
            FileAccessInformation = 8, // 8
            FileNameInformation = 9, // 9
            FileRenameInformation = 10, // 10
            FileLinkInformation = 11, // 11
            FileNamesInformation = 12, // 12
            FileDispositionInformation = 13, // 13
            FilePositionInformation = 14, // 14
            FileFullEaInformation = 15, // 15
            FileModeInformation = 16, // 16
            FileAlignmentInformation = 17, // 17
            FileAllInformation = 18, // 18
            FileAllocationInformation = 19, // 19
            FileEndOfFileInformation = 20, // 20
            FileAlternateNameInformation = 21, // 21
            FileStreamInformation = 22, // 22
            FilePipeInformation = 23, // 23
            FilePipeLocalInformation = 24, // 24
            FilePipeRemoteInformation = 25, // 25
            FileMailslotQueryInformation = 26, // 26
            FileMailslotSetInformation = 27, // 27
            FileCompressionInformation = 28, // 28
            FileObjectIdInformation = 29, // 29
            FileCompletionInformation = 30, // 30
            FileMoveClusterInformation = 31, // 31
            FileQuotaInformation = 32, // 32
            FileReparsePointInformation = 33, // 33
            FileNetworkOpenInformation = 34, // 34
            FileAttributeTagInformation = 35, // 35
            FileTrackingInformation = 36, // 36
            FileIdBothDirectoryInformation = 37, // 37
            FileIdFullDirectoryInformation = 38, // 38
            FileValidDataLengthInformation = 39, // 39
            FileShortNameInformation = 40, // 40
            FileHardLinkInformation = 46 // 46    
        }

        /// <summary>
        /// 定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举而直接使用数值）    
        /// </summary>
        [Flags()]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WindowsKey = 8
        }

        #endregion

        #region constants

        public const Int32 INVALID_HANDLE_VALUE = -1;

        public const UInt32 GENERIC_READ = 0x80000000;
        public const UInt32 GENERIC_WRITE = 0x40000000;
        public const UInt32 FILE_SHARE_READ = 0x00000001;
        public const UInt32 FILE_SHARE_WRITE = 0x00000002;
        public const UInt32 FILE_ATTRIBUTE_DIRECTORY = 0x00000010;

        public const UInt32 CREATE_NEW = 1;
        public const UInt32 CREATE_ALWAYS = 2;
        public const UInt32 OPEN_EXISTING = 3;
        public const UInt32 OPEN_ALWAYS = 4;
        public const UInt32 TRUNCATE_EXISTING = 5;

        public const UInt32 FILE_ATTRIBUTE_NORMAL = 0x80;
        public const UInt32 FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
        public const UInt32 FileNameInformationClass = 9;
        public const UInt32 FILE_OPEN_FOR_BACKUP_INTENT = 0x4000;
        public const UInt32 FILE_OPEN_BY_FILE_ID = 0x2000;
        public const UInt32 FILE_OPEN = 0x1;
        public const UInt32 OBJ_CASE_INSENSITIVE = 0x40;
        //public const OBJ_KERNEL_HANDLE = 0x200;

        // CTL_CODE( DeviceType, Function, Method, Access ) (((DeviceType) << 16) | ((Access) << 14) | ((Function) << 2) | (Method))
        private const UInt32 FILE_DEVICE_FILE_SYSTEM = 0x00000009;
        private const UInt32 METHOD_NEITHER = 3;
        private const UInt32 METHOD_BUFFERED = 0;
        private const UInt32 FILE_ANY_ACCESS = 0;
        private const UInt32 FILE_SPECIAL_ACCESS = 0;
        private const UInt32 FILE_READ_ACCESS = 1;
        private const UInt32 FILE_WRITE_ACCESS = 2;

        public const UInt32 USN_REASON_DATA_OVERWRITE = 0x00000001;
        public const UInt32 USN_REASON_DATA_EXTEND = 0x00000002;
        public const UInt32 USN_REASON_DATA_TRUNCATION = 0x00000004;
        public const UInt32 USN_REASON_NAMED_DATA_OVERWRITE = 0x00000010;
        public const UInt32 USN_REASON_NAMED_DATA_EXTEND = 0x00000020;
        public const UInt32 USN_REASON_NAMED_DATA_TRUNCATION = 0x00000040;
        public const UInt32 USN_REASON_FILE_CREATE = 0x00000100;
        public const UInt32 USN_REASON_FILE_DELETE = 0x00000200;
        public const UInt32 USN_REASON_EA_CHANGE = 0x00000400;
        public const UInt32 USN_REASON_SECURITY_CHANGE = 0x00000800;
        public const UInt32 USN_REASON_RENAME_OLD_NAME = 0x00001000;
        public const UInt32 USN_REASON_RENAME_NEW_NAME = 0x00002000;
        public const UInt32 USN_REASON_INDEXABLE_CHANGE = 0x00004000;
        public const UInt32 USN_REASON_BASIC_INFO_CHANGE = 0x00008000;
        public const UInt32 USN_REASON_HARD_LINK_CHANGE = 0x00010000;
        public const UInt32 USN_REASON_COMPRESSION_CHANGE = 0x00020000;
        public const UInt32 USN_REASON_ENCRYPTION_CHANGE = 0x00040000;
        public const UInt32 USN_REASON_OBJECT_ID_CHANGE = 0x00080000;
        public const UInt32 USN_REASON_REPARSE_POINT_CHANGE = 0x00100000;
        public const UInt32 USN_REASON_STREAM_CHANGE = 0x00200000;
        public const UInt32 USN_REASON_CLOSE = 0x80000000;

        public static Int32 GWL_EXSTYLE = -20;
        public static Int32 WS_EX_LAYERED = 0x00080000;
        public static Int32 WS_EX_TRANSPARENT = 0x00000020;

        public const UInt32 FSCTL_GET_OBJECT_ID = 0x9009c;

        // FSCTL_ENUM_USN_DATA = CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 44,  METHOD_NEITHER, FILE_ANY_ACCESS)
        public const UInt32 FSCTL_ENUM_USN_DATA =
            (FILE_DEVICE_FILE_SYSTEM << 16) | (FILE_ANY_ACCESS << 14) | (44 << 2) | METHOD_NEITHER;

        // FSCTL_READ_USN_JOURNAL = CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 46,  METHOD_NEITHER, FILE_ANY_ACCESS)
        public const UInt32 FSCTL_READ_USN_JOURNAL =
            (FILE_DEVICE_FILE_SYSTEM << 16) | (FILE_ANY_ACCESS << 14) | (46 << 2) | METHOD_NEITHER;

        //  FSCTL_CREATE_USN_JOURNAL        CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 57,  METHOD_NEITHER, FILE_ANY_ACCESS)
        public const UInt32 FSCTL_CREATE_USN_JOURNAL =
            (FILE_DEVICE_FILE_SYSTEM << 16) | (FILE_ANY_ACCESS << 14) | (57 << 2) | METHOD_NEITHER;

        //  FSCTL_QUERY_USN_JOURNAL         CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 61, METHOD_BUFFERED, FILE_ANY_ACCESS)
        public const UInt32 FSCTL_QUERY_USN_JOURNAL =
            (FILE_DEVICE_FILE_SYSTEM << 16) | (FILE_ANY_ACCESS << 14) | (61 << 2) | METHOD_BUFFERED;

        // FSCTL_DELETE_USN_JOURNAL        CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 62, METHOD_BUFFERED, FILE_ANY_ACCESS)
        public const UInt32 FSCTL_DELETE_USN_JOURNAL =
            (FILE_DEVICE_FILE_SYSTEM << 16) | (FILE_ANY_ACCESS << 14) | (62 << 2) | METHOD_BUFFERED;

        #endregion

        #region dll imports

        /// <summary>
        /// 可打开或创建以下对象，并返回可访问的句柄：控制台，通信资源，目录（只读打开），磁盘驱动器，文件，邮槽，管道。
        /// Creates the file specified by 'lpFileName' with desired access, share mode, security attributes,
        /// creation disposition, flags and attributes.
        /// </summary>
        /// <param name="lpFileName">Fully qualified path to a file</param>
        /// <param name="dwDesiredAccess">Requested access (write, read, read/write, none)</param>
        /// <param name="dwShareMode">Share mode (read, write, read/write, delete, all, none)</param>
        /// <param name="lpSecurityAttributes">IntPtr to a 'SECURITY_ATTRIBUTES' structure</param>
        /// <param name="dwCreationDisposition">Action to take on file or device specified by 'lpFileName' (CREATE_NEW,
        /// CREATE_ALWAYS, OPEN_ALWAYS, OPEN_EXISTING, TRUNCATE_EXISTING)</param>
        /// <param name="dwFlagsAndAttributes">File or device attributes and flags (typically FILE_ATTRIBUTE_NORMAL)</param>
        /// <param name="hTemplateFile">IntPtr to a valid handle to a template file with 'GENERIC_READ' access right</param>
        /// <returns>IntPtr handle to the 'lpFileName' file or device or 'INVALID_HANDLE_VALUE'</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr
            CreateFile(string lpFileName,
                uint dwDesiredAccess,
                uint dwShareMode,
                IntPtr lpSecurityAttributes,
                uint dwCreationDisposition,
                uint dwFlagsAndAttributes,
                IntPtr hTemplateFile);

        /// <summary>
        /// 关闭一个内核对象。其中包括文件、文件映射、进程、线程、安全和同步对象等。
        /// </summary>
        /// <param name="hObject">IntPtr handle to a file</param>
        /// <returns>'true' if successful, otherwise 'false'</returns>
        /// <remarks>只是关闭了一个线程句柄对象，即不对这个句柄对应的线程做任何干预了。并没有结束线程。</remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool
            CloseHandle(
            IntPtr hObject);

        /// <summary>
        /// 这个函数提供了获取文件信息的一种机制——在一个BY_HANDLE_FILE_INFORMATION结构中装载与文件有关的信息。
        /// </summary>
        /// <param name="hFile">Fully qualified name of a file</param>
        /// <param name="lpFileInformation">Out BY_HANDLE_FILE_INFORMATION argument</param>
        /// <returns>'true' if successful, otherwise 'false'</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool
            GetFileInformationByHandle(
            IntPtr hFile,
            out BY_HANDLE_FILE_INFORMATION lpFileInformation);

        /// <summary>
        /// 根据文件名删除文件。
        /// </summary>
        /// <param name="fileName">要删除文件的路径。Fully qualified path to the file to delete</param>
        /// <returns>调用成功,返回非0；调用不成功,返回为0。</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteFile(
            string fileName);

        /// <summary>
        /// 从文件指针指向的位置开始将数据读出到一个文件中， 且支持同步和异步操作。
        /// </summary>
        /// <param name="hFile">文件的句柄IntPtr handle to the file to read</param>
        /// <param name="lpBuffer">用于保存读入数据的一个缓冲区IntPtr to a buffer of bytes to receive the bytes read from 'hFile'</param>
        /// <param name="nNumberOfBytesToRead">要读入的字节数Number of bytes to read from 'hFile'</param>
        /// <param name="lpNumberOfBytesRead">指向实际读取字节数的指针Number of bytes read from 'hFile'</param>
        /// <param name="lpOverlapped">IntPtr to an 'OVERLAPPED' structure</param>
        /// <returns>调用成功,返回非0；调用不成功,返回为0。</returns>
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReadFile(
            IntPtr hFile,
            IntPtr lpBuffer,
            uint nNumberOfBytesToRead,
            out uint lpNumberOfBytesRead,
            IntPtr lpOverlapped);

        /// <summary>
        /// 从文件指针指向的位置开始将数据写入到一个文件中, 且支持同步和异步操作。
        /// </summary>
        /// <param name="hFile">需要写入数据的文件指针。IntPtr handle to the file to write</param>
        /// <param name="bytes">数据缓存区指针。IntPtr to a buffer of bytes to write to 'hFile'</param>
        /// <param name="nNumberOfBytesToWrite">要写入数据的字节数量。Number of bytes in 'lpBuffer' to write to 'hFile'</param>
        /// <param name="lpNumberOfBytesWritten">实际写入文件的字节数量。Number of bytes written to 'hFile'</param>
        /// <param name="overlapped">OVERLAPPED结构体指针。IntPtr to an 'OVERLAPPED' structure</param>
        /// <returns>调用成功,返回非0；调用不成功,返回为0。</returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WriteFile(
            IntPtr hFile,
            IntPtr bytes,
            uint nNumberOfBytesToWrite,
            out uint lpNumberOfBytesWritten,
            int overlapped);

        /// <summary>
        /// 从文件指针指向的位置开始将数据写入到一个文件中, 且支持同步和异步操作。
        /// </summary>
        /// <param name="hFile"> 需要写入数据的文件指针。</param>
        /// <param name="lpBuffer">数据缓存区字节数组。Buffer of bytes to write to file 'hFile'</param>
        /// <param name="nNumberOfBytesToWrite">要写入数据的字节数量。Number of bytes in 'lpBuffer' to write to 'hFile'</param>
        /// <param name="lpNumberOfBytesWritten">实际写入文件的字节数量。Number of bytes written to 'hFile'</param>
        /// <param name="overlapped">OVERLAPPED结构体指针。IntPtr to an 'OVERLAPPED' structure</param>
        /// <returns>调用成功,返回非0；调用不成功,返回为0。</returns>
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WriteFile(
            IntPtr hFile,
            byte[] lpBuffer,
            uint nNumberOfBytesToWrite,
            out uint lpNumberOfBytesWritten,
            int overlapped);

        /// <summary>
        /// 直接发送控制代码到指定的设备驱动程序，使相应设备执行相应的操作。
        /// Sends the 'dwIoControlCode' to the device specified by 'hDevice'.
        /// </summary>
        /// <param name="hDevice">IntPtr handle to the device to receive 'dwIoControlCode'</param>
        /// <param name="dwIoControlCode">Device IO Control Code to send</param>
        /// <param name="lpInBuffer">Input buffer if required</param>
        /// <param name="nInBufferSize">Size of input buffer</param>
        /// <param name="lpOutBuffer">Output buffer if required</param>
        /// <param name="nOutBufferSize">Size of output buffer</param>
        /// <param name="lpBytesReturned">Number of bytes returned in output buffer</param>
        /// <param name="lpOverlapped">IntPtr to an 'OVERLAPPED' structure</param>
        /// <returns>'true' if successful, otherwise 'false'</returns>
        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeviceIoControl(
            IntPtr hDevice,
            UInt32 dwIoControlCode,
            IntPtr lpInBuffer,
            Int32 nInBufferSize,
            out USN_JOURNAL_DATA lpOutBuffer,
            Int32 nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped);

        /// <summary>
        /// 直接发送控制代码到指定的设备驱动程序，使相应设备执行相应的操作。
        /// </summary>
        /// <param name="hDevice">设备句柄IntPtr handle to the device to receive 'dwIoControlCode</param>
        /// <param name="dwIoControlCode">应用程序调用驱动程序的控制命令，就是IOCTL_XXX IOCTLs。Device IO Control Code to send</param>
        /// <param name="lpInBuffer">应用程序传递给驱动程序的数据缓冲区地址。Input buffer if required</param>
        /// <param name="nInBufferSize">应用程序传递给驱动程序的数据缓冲区大小，字节数。Size of input buffer </param>
        /// <param name="lpOutBuffer">驱动程序返回给应用程序的数据缓冲区地址。Output buffer if required</param>
        /// <param name="nOutBufferSize">驱动程序返回给应用程序的数据缓冲区大小，字节数。Size of output buffer</param>
        /// <param name="lpBytesReturned">驱动程序实际返回给应用程序的数据字节数地址。Number of bytes returned</param>
        /// <param name="lpOverlapped">这个结构用于重叠操作。Pointer to an 'OVERLAPPED' struture</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeviceIoControl(
            IntPtr hDevice,
            UInt32 dwIoControlCode,
            IntPtr lpInBuffer,
            Int32 nInBufferSize,
            IntPtr lpOutBuffer,
            Int32 nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped);

        /// <summary>
        /// 用0来填充一块内存区域。
        /// </summary>
        /// <param name="ptr">指向一片内存区域</param>
        /// <param name="size">从起始位置开始需置零的内存位置大小</param>
        [DllImport("kernel32.dll")]
        public static extern void ZeroMemory(IntPtr ptr, int size);

        /// <summary>
        /// 检取光标的位置，以屏幕坐标表示。
        /// </summary>
        /// <param name="pt">POINT结构指针，该结构接收光标的屏幕坐标。</param>
        /// <returns>Returns nonzero if successful or zero otherwise. To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out POINT pt);

        /// <summary>
        /// 获得指定窗口的有关信息，函数也获得在额外窗口内存中指定偏移位地址的32位度整型值。
        /// </summary>
        /// <param name="hWnd">目标窗口的句柄。它可以是窗口句柄及间接给出的窗口所属的窗口类。</param>
        /// <param name="nIndex">需要获得的相关信息的类型。</param>
        /// <returns>
        /// 如果函数成功，返回值是所需的32位值；如果函数失败，返回值是0。若想获得更多错误信息请调用 GetLastError函数。
        ///</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Int32 GetWindowLong(IntPtr hWnd, Int32 nIndex);

        /// <summary>
        /// 改变指定窗口的属性．函数也将指定的一个32位值设置在窗口的额外存储空间的指定偏移位置。
        /// </summary>
        /// <param name="hWnd">窗口句柄及间接给出的窗口所属的类。</param>
        /// <param name="nIndex">指定将设定的大于等于0的偏移值。</param>
        /// <param name="newVal">新值。</param>
        /// <returns>
        /// 如果函数成功，返回值是指定的32位整数的原来的值。如果函数失败，返回值为0。若想获得更多错误信息，请调用GetLastError函数。
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Int32 SetWindowLong(IntPtr hWnd, Int32 nIndex, Int32 newVal);

        /// <summary>
        /// Creates a new file or directory, or opens an existing file, device, directory, or volume
        /// </summary>
        /// <param name="handle">A pointer to a variable that receives the file handle if the call is successful (out)</param>
        /// <param name="access">ACCESS_MASK value that expresses the type of access that the caller requires to the file or directory (in)</param>
        /// <param name="objectAttributes">A pointer to a structure already initialized with InitializeObjectAttributes (in)</param>
        /// <param name="ioStatus">A pointer to a variable that receives the final completion status and information about the requested operation (out)</param>
        /// <param name="allocSize">The initial allocation size in bytes for the file (in)(optional)</param>
        /// <param name="fileAttributes">file attributes (in)</param>
        /// <param name="share">type of share access that the caller would like to use in the file (in)</param>
        /// <param name="createDisposition">what to do, depending on whether the file already exists (in)</param>
        /// <param name="createOptions">options to be applied when creating or opening the file (in)</param>
        /// <param name="eaBuffer">Pointer to an EA buffer used to pass extended attributes (in)</param>
        /// <param name="eaLength">Length of the EA buffer</param>
        /// <returns>either STATUS_SUCCESS or an appropriate error status. If it returns an error status, the caller can find more information about the cause of the failure by checking the IoStatusBlock</returns>
        [DllImport("ntdll.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int NtCreateFile(
            ref IntPtr handle,
            FileAccess access,
            ref OBJECT_ATTRIBUTES objectAttributes,
            ref IO_STATUS_BLOCK ioStatus,
            ref long allocSize,
            uint fileAttributes,
            FileShare share,
            uint createDisposition,
            uint createOptions,
            IntPtr eaBuffer,
            uint eaLength);

        /// <summary>
        /// Routine returns various kinds of information about a file object.
        /// </summary>
        /// <param name="fileHandle"></param>
        /// <param name="IoStatusBlock"></param>
        /// <param name="pInfoBlock"></param>
        /// <param name="length"></param>
        /// <param name="fileInformation"></param>
        /// <returns>Returns STATUS_SUCCESS or an appropriate NTSTATUS error code.</returns>
        /// <remarks>See ZwQueryInformationFile.</remarks>
        [DllImport("ntdll.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int NtQueryInformationFile(
            IntPtr fileHandle,
            ref IO_STATUS_BLOCK IoStatusBlock,
            IntPtr pInfoBlock,
            uint length,
            FILE_INFORMATION_CLASS fileInformation);
   
        /// <summary>定义一个系统范围的热键。</summary>
        /// <param name="hWnd">与被定义的热键相关的窗口句柄。若热键不与窗口相关，则该参数为NULL</param>
        /// <param name="id">被定义的热键的标识符</param>
        /// <param name="fsModifiers">定义为了产生WM_HOTKEY消息而必须与由nVirtKey参数定义的键一起按下的键</param>
        /// <param name="vk">定义热键的虚拟键码</param>
        /// <returns>
        /// 如果函数执行成功，返回值不为0。
        /// 如果函数执行失败，返回值为0。要得到扩展错误信息，调用GetLastError。 
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(
            IntPtr hWnd,                //要定义热键的窗口的句柄
            int id,                     //定义热键ID（不能与其它ID重复）           
            KeyModifiers fsModifiers,   //标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效
            Keys vk                     //定义热键的内容
            );

        /// <summary>释放调用线程先前登记的热键。</summary>
        /// <param name="hWnd">与被释放的热键相关的窗口句柄。若热键不与窗口相关，则该参数为NULL</param>
        /// <param name="id">被释放的热键的标识符</param>
        /// <returns>
        /// 如果函数执行成功，返回值不为0。
        /// 如果函数执行失败，返回值为0。要得到扩展错误信息，调用GetLastError。 
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(
            IntPtr hWnd,                //要取消热键的窗口的句柄
            int id                      //要取消热键的ID
            );

        #endregion

        #region structures

        /// <summary>
        /// By Handle File Information structure, contains File Attributes(32bits), Creation Time(FILETIME),
        /// Last Access Time(FILETIME), Last Write Time(FILETIME), Volume Serial Number(32bits),
        /// File Size High(32bits), File Size Low(32bits), Number of Links(32bits), File Index High(32bits),
        /// File Index Low(32bits).
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BY_HANDLE_FILE_INFORMATION
        {
            public uint FileAttributes;
            public FILETIME CreationTime;
            public FILETIME LastAccessTime;
            public FILETIME LastWriteTime;
            public uint VolumeSerialNumber;
            public uint FileSizeHigh;
            public uint FileSizeLow;
            public uint NumberOfLinks;
            public uint FileIndexHigh;
            public uint FileIndexLow;
        }

        /// <summary>
        /// USN Journal Data structure, contains USN Journal ID(64bits), First USN(64bits), Next USN(64bits),
        /// Lowest Valid USN(64bits), Max USN(64bits), Maximum Size(64bits) and Allocation Delta(64bits).
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct USN_JOURNAL_DATA
        {
            public UInt64 UsnJournalID;
            public Int64 FirstUsn;
            public Int64 NextUsn;
            public Int64 LowestValidUsn;
            public Int64 MaxUsn;
            public UInt64 MaximumSize;
            public UInt64 AllocationDelta;
        }

        /// <summary>
        /// MFT Enum Data structure, contains Start File Reference Number(64bits), Low USN(64bits),
        /// High USN(64bits).
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MFT_ENUM_DATA
        {
            public UInt64 StartFileReferenceNumber;
            public Int64 LowUsn;
            public Int64 HighUsn;
        }

        /// <summary>
        /// Create USN Journal Data structure, contains Maximum Size(64bits) and Allocation Delta(64(bits).
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct CREATE_USN_JOURNAL_DATA
        {
            public UInt64 MaximumSize;
            public UInt64 AllocationDelta;
        }

        /// <summary>
        /// Create USN Journal Data structure, contains Maximum Size(64bits) and Allocation Delta(64(bits).
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct DELETE_USN_JOURNAL_DATA
        {
            public UInt64 UsnJournalID;
            public UInt32 DeleteFlags;
            public UInt32 Reserved;
        }

        /// <summary>
        /// Contains the USN Record Length(32bits), USN(64bits), File Reference Number(64bits), 
        /// Parent File Reference Number(64bits), Reason Code(32bits), File Attributes(32bits),
        /// File Name Length(32bits), the File Name Offset(32bits) and the File Name.
        /// </summary>
        public class UsnEntry : IComparable<UsnEntry>
        {
            private const int FR_OFFSET = 8;
            private const int PFR_OFFSET = 16;
            private const int USN_OFFSET = 24;
            private const int TS_OFFSET = 32;
            private const int REASON_OFFSET = 40;
            public const int FA_OFFSET = 52;
            private const int FNL_OFFSET = 56;
            private const int FN_OFFSET = 58;

            private UInt32 _recordLength;

            public UInt32 RecordLength
            {
                get { return _recordLength; }
            }

            private Int64 _ts;

            public DateTime TimeStamp
            {
                get { return DateTime.FromBinary(_ts); }
            }

            private Int64 _usn;

            public Int64 USN
            {
                get { return _usn; }
            }

            private UInt64 _frn;

            public UInt64 FileReferenceNumber
            {
                get { return _frn; }
            }

            private UInt64 _pfrn;

            public UInt64 ParentFileReferenceNumber
            {
                get { return _pfrn; }
            }

            private UInt32 _reason;

            public UInt32 Reason
            {
                get { return _reason; }
            }

            private string _name;

            public string Name
            {
                get { return _name; }
            }

            private string _oldName;

            public string OldName
            {
                get
                {
                    if (0 != (_fileAttributes & USN_REASON_RENAME_OLD_NAME))
                    {
                        return _oldName;
                    }
                    else
                    {
                        return null;
                    }
                }
                set { _oldName = value; }
            }

            private UInt32 _fileAttributes;

            public bool IsFolder
            {
                get
                {
                    bool bRtn = 0 != (_fileAttributes & FILE_ATTRIBUTE_DIRECTORY);
                    return bRtn;
                }
            }

            public bool IsFile
            {
                get
                {
                    bool bRtn = 0 == (_fileAttributes & FILE_ATTRIBUTE_DIRECTORY);
                    return bRtn;
                }
            }

            /// <summary>
            /// USN Record Constructor
            /// </summary>
            /// <param name="ptrToUsnRecord">Buffer pointer to first byte of the USN Record</param>
            public UsnEntry(IntPtr ptrToUsnRecord)
            {
                _recordLength = (UInt32) Marshal.ReadInt32(ptrToUsnRecord);
                _frn = (UInt64) Marshal.ReadInt64(ptrToUsnRecord, FR_OFFSET);
                _pfrn = (UInt64) Marshal.ReadInt64(ptrToUsnRecord, PFR_OFFSET);
                _ts = (Int64) Marshal.ReadInt64(ptrToUsnRecord, TS_OFFSET);
                _usn = (Int64) Marshal.ReadInt64(ptrToUsnRecord, USN_OFFSET);
                _reason = (UInt32) Marshal.ReadInt32(ptrToUsnRecord, REASON_OFFSET);
                _fileAttributes = (UInt32) Marshal.ReadInt32(ptrToUsnRecord, FA_OFFSET);
                short fileNameLength = Marshal.ReadInt16(ptrToUsnRecord, FNL_OFFSET);
                short fileNameOffset = Marshal.ReadInt16(ptrToUsnRecord, FN_OFFSET);
                _name = Marshal.PtrToStringUni(new IntPtr(ptrToUsnRecord.ToInt32() + fileNameOffset),
                    fileNameLength/sizeof (char));
            }



            #region IComparable<UsnEntry> Members

            public int CompareTo(UsnEntry other)
            {
                return string.Compare(this.Name, other.Name, true);
            }

            #endregion
        }

        /// <summary>
        /// Contains the Start USN(64bits), Reason Mask(32bits), Return Only on Close flag(32bits),
        /// Time Out(64bits), Bytes To Wait For(64bits), and USN Journal ID(64bits).
        /// </summary>
        /// <remarks> possible reason bits are from Win32Api
        /// USN_REASON_DATA_OVERWRITE
        /// USN_REASON_DATA_EXTEND
        /// USN_REASON_DATA_TRUNCATION
        /// USN_REASON_NAMED_DATA_OVERWRITE
        /// USN_REASON_NAMED_DATA_EXTEND
        /// USN_REASON_NAMED_DATA_TRUNCATION
        /// USN_REASON_FILE_CREATE
        /// USN_REASON_FILE_DELETE
        /// USN_REASON_EA_CHANGE
        /// USN_REASON_SECURITY_CHANGE
        /// USN_REASON_RENAME_OLD_NAME
        /// USN_REASON_RENAME_NEW_NAME
        /// USN_REASON_INDEXABLE_CHANGE
        /// USN_REASON_BASIC_INFO_CHANGE
        /// USN_REASON_HARD_LINK_CHANGE
        /// USN_REASON_COMPRESSION_CHANGE
        /// USN_REASON_ENCRYPTION_CHANGE
        /// USN_REASON_OBJECT_ID_CHANGE
        /// USN_REASON_REPARSE_POINT_CHANGE
        /// USN_REASON_STREAM_CHANGE
        /// USN_REASON_CLOSE
        /// </remarks>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct READ_USN_JOURNAL_DATA
        {
            public Int64 StartUsn;
            public UInt32 ReasonMask;
            public UInt32 ReturnOnlyOnClose;
            public UInt64 Timeout;
            public UInt64 bytesToWaitFor;
            public UInt64 UsnJournalId;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IO_STATUS_BLOCK
        {
            public uint status;
            public ulong information;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct OBJECT_ATTRIBUTES
        {
            public Int32 Length;
            public IntPtr RootDirectory;
            public IntPtr ObjectName;
            public Int32 Attributes;
            public Int32 SecurityDescriptor;
            public Int32 SecurityQualityOfService;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct UNICODE_STRING
        {
            public Int16 Length;
            public Int16 MaximumLength;
            public IntPtr Buffer;
        }

        #endregion

        #region functions

        /*
        public static string GetPathFromFileReference(IntPtr rootIntPtr, UInt64 frn)
        {
            string name = string.Empty;

            long allocSize = 0;
            UNICODE_STRING unicodeString;
            OBJECT_ATTRIBUTES objAttributes = new OBJECT_ATTRIBUTES();
            IO_STATUS_BLOCK ioStatusBlock = new IO_STATUS_BLOCK();
            IntPtr hFile;

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
            objAttributes.RootDirectory = rootIntPtr;
            objAttributes.Attributes = (int)OBJ_CASE_INSENSITIVE;

            int fOk = NtCreateFile(out hFile, 0, ref objAttributes, ref ioStatusBlock, ref allocSize, 0,
                FileShare.ReadWrite,
                FILE_OPEN, FILE_OPEN_BY_FILE_ID | FILE_OPEN_FOR_BACKUP_INTENT, IntPtr.Zero, 0);
            if (fOk.ToInt32() == 0)
            {
                fOk = NtQueryInformationFile(hFile, ref ioStatusBlock, buffer, 4096, FILE_INFORMATION_CLASS.FileNameInformation);
                if (fOk.ToInt32() == 0)
                {
                    //
                    // first 4 bytes are the name length
                    //
                    int nameLength = Marshal.ReadInt32(buffer, 0);
                    //
                    // next bytes are the name
                    //
                    name = Marshal.PtrToStringUni(new IntPtr(buffer.ToInt32() + 4), nameLength / 2);
                }
            }
            hFile.Close();
            Marshal.FreeHGlobal(buffer);
            Marshal.FreeHGlobal(objAttIntPtr);
            Marshal.FreeHGlobal(refPtr);
            return name;
        }
        */

        /// <summary>
        /// Writes the data in 'text' to the alternate stream ':Description' of the file 'currentFile.
        /// </summary>
        /// <param name="currentfile">Fully qualified path to a file</param>
        /// <param name="text">Data to write to the ':Description' stream</param>
        public static void WriteAlternateStream(string currentfile, string text)
        {
            string AltStreamDesc = currentfile + ":Description";
            IntPtr txtBuffer = IntPtr.Zero;
            IntPtr hFile = IntPtr.Zero;
            DeleteFile(AltStreamDesc);
            string descText = text.TrimEnd(' ');

            try
            {
                hFile = CreateFile(AltStreamDesc, GENERIC_WRITE, 0, IntPtr.Zero,
                    CREATE_ALWAYS, 0, IntPtr.Zero);
                if (-1 != hFile.ToInt32())
                {
                    txtBuffer = Marshal.StringToHGlobalUni(descText);
                    uint nBytes = (uint) descText.Length;
                    uint count;
                    bool bRtn = WriteFile(hFile, txtBuffer, sizeof (char)*nBytes, out count, 0);
                    if (!bRtn)
                    {
                        if ((sizeof (char)*nBytes) != count)
                        {
                            throw new Exception(string.Format("Bytes written {0} should be {1} for file {2}.",
                                count, sizeof (char)*nBytes, AltStreamDesc));
                        }
                        else
                        {
                            throw new Exception("WriteFile() returned false");
                        }
                    }
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            catch (Exception exception)
            {
                string msg = string.Format("Exception caught in WriteAlternateStream()\n  '{0}'\n  for file '{1}'.",
                    exception.Message, AltStreamDesc);
                Console.WriteLine(msg);
            }
            finally
            {
                CloseHandle(hFile);
                hFile = IntPtr.Zero;
                Marshal.FreeHGlobal(txtBuffer);
                GC.Collect();
            }
        }

        /// <summary>
        /// Adds the ':Description' alternate stream name to the argument 'currentFile'.
        /// </summary>
        /// <param name="currentfile">The file whose alternate stream is to be read</param>
        /// <returns>A string value representing the value of the alternate stream</returns>
        public static string ReadAlternateStream(string currentfile)
        {
            string AltStreamDesc = currentfile + ":Description";
            string returnstring = ReadAlternateStreamEx(AltStreamDesc);
            return returnstring;
        }

        /// <summary>
        /// Reads the stream represented by 'currentFile'.
        /// </summary>
        /// <param name="currentfile">Fully qualified path including stream</param>
        /// <returns>Value of the alternate stream as a string</returns>
        public static string ReadAlternateStreamEx(string currentfile)
        {
            string returnstring = string.Empty;
            IntPtr hFile = IntPtr.Zero;
            IntPtr buffer = IntPtr.Zero;
            try
            {
                hFile = CreateFile(currentfile, GENERIC_READ, 0, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
                if (-1 != hFile.ToInt32())
                {
                    buffer = Marshal.AllocHGlobal(1000*sizeof (char));
                    ZeroMemory(buffer, 1000*sizeof (char));
                    uint nBytes;
                    bool bRtn = ReadFile(hFile, buffer, 1000*sizeof (char), out nBytes, IntPtr.Zero);
                    if (bRtn)
                    {
                        if (nBytes > 0)
                        {
                            returnstring = Marshal.PtrToStringAuto(buffer);
                            //byte[] byteBuffer = new byte[nBytes];
                            //for (int i = 0; i < nBytes; i++)
                            //{
                            //    byteBuffer[i] = Marshal.ReadByte(buffer, i);
                            //}
                            //returnstring = Encoding.Unicode.GetString(byteBuffer, 0, (int)nBytes);
                        }
                        else
                        {
                            throw new Exception("ReadFile() returned true but read zero bytes");
                        }
                    }
                    else
                    {
                        if (nBytes <= 0)
                        {
                            throw new Exception("ReadFile() read zero bytes.");
                        }
                        else
                        {
                            throw new Exception("ReadFile() returned false");
                        }
                    }
                }
                else
                {
                    Exception excptn = new Win32Exception(Marshal.GetLastWin32Error());
                    if (!excptn.Message.Contains("cannot find the file"))
                    {
                        throw excptn;
                    }
                }
            }
            catch (Exception exception)
            {
                string msg = string.Format("Exception caught in ReadAlternateStream(), '{0}'\n  for file '{1}'.",
                    exception.Message, currentfile);
                Console.WriteLine(msg);
                Console.WriteLine(exception.Message);
            }
            finally
            {
                CloseHandle(hFile);
                hFile = IntPtr.Zero;
                if (buffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(buffer);
                }
                GC.Collect();
            }
            return returnstring;
        }

        /// <summary>
        /// Read the encrypted alternate stream specified by 'currentFile'.
        /// </summary>
        /// <param name="currentfile">Fully qualified path to encrypted alternate stream</param>
        /// <returns>The un-encrypted value of the alternate stream as a string</returns>
        public static string ReadAlternateStreamEncrypted(string currentfile)
        {
            string returnstring = string.Empty;
            IntPtr buffer = IntPtr.Zero;
            IntPtr hFile = IntPtr.Zero;
            try
            {
                hFile = CreateFile(currentfile, GENERIC_READ, 0, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
                if (-1 != hFile.ToInt32())
                {
                    buffer = Marshal.AllocHGlobal(1000*sizeof (char));
                    ZeroMemory(buffer, 1000*sizeof (char));
                    uint nBytes;
                    bool bRtn = ReadFile(hFile, buffer, 1000*sizeof (char), out nBytes, IntPtr.Zero);
                    if (0 != nBytes)
                    {
                        returnstring = DecryptLicenseString(buffer, nBytes);
                    }
                }
                else
                {
                    Exception excptn = new Win32Exception(Marshal.GetLastWin32Error());
                    if (!excptn.Message.Contains("cannot find the file"))
                    {
                        throw excptn;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception caught in ReadAlternateStreamEncrypted()");
                Console.WriteLine(exception.Message);
            }
            finally
            {
                CloseHandle(hFile);
                hFile = IntPtr.Zero;
                if (buffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(buffer);
                }
                GC.Collect();
            }
            return returnstring;
        }

        /// <summary>
        /// Writes the value of 'LicenseString' as an encrypted stream to the file:stream specified
        /// by 'currentFile'.
        /// </summary>
        /// <param name="currentFile">Fully qualified path to the alternate stream</param>
        /// <param name="LicenseString">The string value to encrypt and write to the alternate stream</param>
        public static void WriteAlternateStreamEncrypted(string currentFile, string LicenseString)
        {
            RC2CryptoServiceProvider rc2 = null;
            CryptoStream cs = null;
            MemoryStream ms = null;
            uint count = 0;
            IntPtr buffer = IntPtr.Zero;
            IntPtr hFile = IntPtr.Zero;
            try
            {
                Encoding enc = Encoding.Unicode;

                byte[] ba = enc.GetBytes(LicenseString);
                ms = new MemoryStream();

                rc2 = new RC2CryptoServiceProvider();
                rc2.Key = GetBytesFromHexString("7a6823a42a3a3ae27057c647db812d0");
                rc2.IV = GetBytesFromHexString("827d961224d99b2d");

                cs = new CryptoStream(ms, rc2.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(ba, 0, ba.Length);
                cs.FlushFinalBlock();

                buffer = Marshal.AllocHGlobal(1000*sizeof (char));
                ZeroMemory(buffer, 1000*sizeof (char));
                uint nBytes = (uint) ms.Length;
                Marshal.Copy(ms.GetBuffer(), 0, buffer, (int) nBytes);

                DeleteFile(currentFile);
                hFile = CreateFile(currentFile, GENERIC_WRITE, 0, IntPtr.Zero,
                    CREATE_ALWAYS, 0, IntPtr.Zero);
                if (-1 != hFile.ToInt32())
                {
                    bool bRtn = WriteFile(hFile, buffer, nBytes, out count, 0);
                }
                else
                {
                    Exception excptn = new Win32Exception(Marshal.GetLastWin32Error());
                    if (!excptn.Message.Contains("cannot find the file"))
                    {
                        throw excptn;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("WriteAlternateStreamEncrypted()");
                Console.WriteLine(exception.Message);
            }
            finally
            {
                CloseHandle(hFile);
                hFile = IntPtr.Zero;
                if (cs != null)
                {
                    cs.Close();
                    cs.Dispose();
                }

                rc2 = null;
                if (ms != null)
                {
                    ms.Close();
                    ms.Dispose();
                }
                if (buffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }
        }

        /// <summary>
        /// Encrypt the string 'LicenseString' argument and return as a MemoryStream.
        /// </summary>
        /// <param name="LicenseString">The string value to encrypt</param>
        /// <returns>A MemoryStream which contains the encrypted value of 'LicenseString'</returns>
        private static MemoryStream EncryptLicenseString(string LicenseString)
        {
            Encoding enc = Encoding.Unicode;

            byte[] ba = enc.GetBytes(LicenseString);
            MemoryStream ms = new MemoryStream();

            RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider
            {
                Key = GetBytesFromHexString("7a6823a42a3a3ae27057c647db812d0"),
                IV = GetBytesFromHexString("827d961224d99b2d")
            };

            CryptoStream cs = new CryptoStream(ms, rc2.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(ba, 0, ba.Length);

            cs.Close();
            cs.Dispose();
            rc2 = null;
            return ms;
        }

        /// <summary>
        /// Given an IntPtr to a bufer and the number of bytes, decrypt the buffer and return an 
        /// unencrypted text string.
        /// </summary>
        /// <param name="buffer">An IntPtr to the 'buffer' containing the encrypted string</param>
        /// <param name="nBytes">The number of bytes in 'buffer' to decrypt</param>
        /// <returns></returns>
        private static string DecryptLicenseString(IntPtr buffer, uint nBytes)
        {
            byte[] ba = new byte[nBytes];
            for (int i = 0; i < nBytes; i++)
            {
                ba[i] = Marshal.ReadByte(buffer, i);
            }
            MemoryStream ms = new MemoryStream(ba);

            RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider
            {
                Key = GetBytesFromHexString("7a6823a42a3a3ae27057c647db812d0"),
                IV = GetBytesFromHexString("827d961224d99b2d")
            };

            CryptoStream cs = new CryptoStream(ms, rc2.CreateDecryptor(), CryptoStreamMode.Read);
            string licenseString = string.Empty;
            byte[] ba1 = new byte[4096];
            int irtn = cs.Read(ba1, 0, 4096);
            Encoding enc = Encoding.Unicode;
            licenseString = enc.GetString(ba1, 0, irtn);

            cs.Close();
            cs.Dispose();
            ms.Close();
            ms.Dispose();
            rc2 = null;
            return licenseString;
        }

        /// <summary>
        /// Gets the byte array generated from the value of 'hexString'.
        /// </summary>
        /// <param name="hexString">Hexadecimal string</param>
        /// <returns>Array of bytes generated from 'hexString'.</returns>
        public static byte[] GetBytesFromHexString(string hexString)
        {
            int numHexChars = hexString.Length/2;
            byte[] ba = new byte[numHexChars];
            int j = 0;
            for (int i = 0; i < ba.Length; i++)
            {
                string hex = new string(new char[] {hexString[j], hexString[j + 1]});
                ba[i] = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                j = j + 2;
            }
            return ba;
        }

        #endregion
    }
}
