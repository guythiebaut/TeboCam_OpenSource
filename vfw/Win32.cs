namespace Tiger.Video.VFW
{
	using System;
	using System.Runtime.InteropServices;

	/// <summary>
	/// Windows API functions and structures
	/// </summary>
	internal class Win32
	{
		// --- AVI Functions

		// Initialize the AVIFile library
		[DllImport("avifil32.dll")]
		public static extern void AVIFileInit();

		// Exit the AVIFile library 
		[DllImport("avifil32.dll")]
		public static extern void AVIFileExit();

		// Open an AVI file
		[DllImport("avifil32.dll", CharSet=CharSet.Unicode)]
		public static extern int AVIFileOpen(
			out IntPtr ppfile,
			String szFile,
			OpenFileMode mode,
			IntPtr pclsidHandler);

		// Release an open AVI stream
		[DllImport("avifil32.dll")]
		public static extern int AVIFileRelease(
			IntPtr pfile);

		// Get address of a stream interface that is associated
		// with a specified AVI file
		[DllImport("avifil32.dll")]
		public static extern int AVIFileGetStream(
			IntPtr pfile,
			out IntPtr ppavi,
			int fccType,
			int lParam);

		// Create a new stream in an existing file and creates an interface to the new stream
		[DllImport("avifil32.dll")]
		public static extern int AVIFileCreateStream(
			IntPtr pfile,
			out IntPtr ppavi, 
			ref AVISTREAMINFO psi);

		// Release an open AVI stream
		[DllImport("avifil32.dll")]
		public static extern int AVIStreamRelease(
			IntPtr pavi);

		// Set the format of a stream at the specified position
		[DllImport("avifil32.dll")]
		public static extern int AVIStreamSetFormat(
			IntPtr pavi,
			int lPos, 
			ref BITMAPINFOHEADER lpFormat,
			int cbFormat);

		// Get the starting sample number for the stream
		[DllImport("avifil32.dll")]
		public static extern int AVIStreamStart(
			IntPtr pavi);

		// Get the length of the stream
		[DllImport("avifil32.dll")]
		public static extern int AVIStreamLength(
			IntPtr pavi);

		// Obtain stream header information
		[DllImport("avifil32.dll", CharSet=CharSet.Unicode)]
		public static extern int AVIStreamInfo(
			IntPtr pavi,
			ref AVISTREAMINFO psi,
			int lSize);

		// Prepare to decompress video frames from the specified video stream
		[DllImport("avifil32.dll")]
		public static extern IntPtr AVIStreamGetFrameOpen(
			IntPtr pavi,
			ref BITMAPINFOHEADER lpbiWanted);
		[DllImport("avifil32.dll")]
		public static extern IntPtr AVIStreamGetFrameOpen(
			IntPtr pavi,
			int lpbiWanted);

		// Releases resources used to decompress video frames
		[DllImport("avifil32.dll")]
		public static extern int AVIStreamGetFrameClose(
			IntPtr pget);

		// Return the address of a decompressed video frame
		[DllImport("avifil32.dll")]
		public static extern IntPtr AVIStreamGetFrame(
			IntPtr pget,
			int lPos);

		// Write data to a stream
		[DllImport("avifil32.dll")]
		public static extern int AVIStreamWrite(
			IntPtr pavi,
			int lStart,
			int lSamples,
			IntPtr lpBuffer,
			int cbBuffer,
			int dwFlags, 
			IntPtr plSampWritten,
			IntPtr plBytesWritten);

		// Retrieve the save options for a file and returns them in a buffer
		[DllImport("avifil32.dll")]
		public static extern int AVISaveOptions(
			IntPtr hwnd,
			int flags,
			int streams,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] IntPtr [] ppavi,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] IntPtr [] plpOptions);

		// Free the resources allocated by the AVISaveOptions function
		[DllImport("avifil32.dll")]
		public static extern int AVISaveOptionsFree(
			int streams,
			[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] IntPtr [] plpOptions);

		// Create a compressed stream from an uncompressed stream and a
		// compression filter, and returns the address of a pointer to
		// the compressed stream
		[DllImport("avifil32.dll")]
		public static extern int AVIMakeCompressedStream(
			out IntPtr ppsCompressed,
			IntPtr psSource,
			ref AVICOMPRESSOPTIONS lpOptions,
			IntPtr pclsidHandler);

		// --- memory functions

		// memcpy - copy a block of memery
		[DllImport("ntdll.dll")]
		public static extern IntPtr memcpy(
			IntPtr dst,
			IntPtr src,
			int count);
		[DllImport("ntdll.dll")]
		public static extern int memcpy(
			int dst,
			int src,
			int count);

		// --- structures

		// Define the coordinates of the upper-left and
		// lower-right corners of a rectangle
		[StructLayout(LayoutKind.Sequential, Pack=1)]
		public struct RECT
		{
			[MarshalAs(UnmanagedType.I4)] public int left;
			[MarshalAs(UnmanagedType.I4)] public int top;
			[MarshalAs(UnmanagedType.I4)] public int right;
			[MarshalAs(UnmanagedType.I4)] public int bottom;
		}

		// Contains information for a single stream
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode, Pack=1)]
		public struct AVISTREAMINFO
		{
			[MarshalAs(UnmanagedType.I4)] public int fccType;
			[MarshalAs(UnmanagedType.I4)] public int fccHandler;
			[MarshalAs(UnmanagedType.I4)] public int dwFlags;
			[MarshalAs(UnmanagedType.I4)] public int dwCaps;
			[MarshalAs(UnmanagedType.I2)] public short wPriority;
			[MarshalAs(UnmanagedType.I2)] public short wLanguage;
			[MarshalAs(UnmanagedType.I4)] public int dwScale;
			[MarshalAs(UnmanagedType.I4)] public int dwRate;		// dwRate / dwScale == samples/second
			[MarshalAs(UnmanagedType.I4)] public int dwStart;
			[MarshalAs(UnmanagedType.I4)] public int dwLength;
			[MarshalAs(UnmanagedType.I4)] public int dwInitialFrames;
			[MarshalAs(UnmanagedType.I4)] public int dwSuggestedBufferSize;
			[MarshalAs(UnmanagedType.I4)] public int dwQuality;
			[MarshalAs(UnmanagedType.I4)] public int dwSampleSize;
			[MarshalAs(UnmanagedType.Struct, SizeConst=16)] public RECT rcFrame;
			[MarshalAs(UnmanagedType.I4)] public int dwEditCount;
			[MarshalAs(UnmanagedType.I4)] public int dwFormatChangeCount;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=64)] public string szName;
		}

		// Contains information about the dimensions and color format of a DIB
		[StructLayout(LayoutKind.Sequential, Pack=1)]
		public struct BITMAPINFOHEADER
		{
			[MarshalAs(UnmanagedType.I4)] public int biSize;
			[MarshalAs(UnmanagedType.I4)] public int biWidth;
			[MarshalAs(UnmanagedType.I4)] public int biHeight;
			[MarshalAs(UnmanagedType.I2)] public short biPlanes;
			[MarshalAs(UnmanagedType.I2)] public short biBitCount;
			[MarshalAs(UnmanagedType.I4)] public int biCompression;
			[MarshalAs(UnmanagedType.I4)] public int biSizeImage;
			[MarshalAs(UnmanagedType.I4)] public int biXPelsPerMeter;
			[MarshalAs(UnmanagedType.I4)] public int biYPelsPerMeter;
			[MarshalAs(UnmanagedType.I4)] public int biClrUsed;
			[MarshalAs(UnmanagedType.I4)] public int biClrImportant;
		}

		// Contains information about a stream and how it is compressed and saved
		[StructLayout(LayoutKind.Sequential, Pack=1)]
		public struct AVICOMPRESSOPTIONS
		{
			[MarshalAs(UnmanagedType.I4)] public int fccType;
			[MarshalAs(UnmanagedType.I4)] public int fccHandler;
			[MarshalAs(UnmanagedType.I4)] public int dwKeyFrameEvery;
			[MarshalAs(UnmanagedType.I4)] public int dwQuality;
			[MarshalAs(UnmanagedType.I4)] public int dwBytesPerSecond;
			[MarshalAs(UnmanagedType.I4)] public int dwFlags;
			[MarshalAs(UnmanagedType.I4)] public int lpFormat;
			[MarshalAs(UnmanagedType.I4)] public int cbFormat;
			[MarshalAs(UnmanagedType.I4)] public int lpParms;
			[MarshalAs(UnmanagedType.I4)] public int cbParms;
			[MarshalAs(UnmanagedType.I4)] public int dwInterleaveEvery;
		}

		// --- enumerations

		// File access modes
		[Flags]
		public enum OpenFileMode
		{
			Read            = 0x00000000,
			Write			= 0x00000001,
			ReadWrite		= 0x00000002,
			ShareCompat     = 0x00000000,
			ShareExclusive	= 0x00000010,
			ShareDenyWrite	= 0x00000020,
			ShareDenyRead	= 0x00000030,
			ShareDenyNone	= 0x00000040,
			Parse			= 0x00000100,
			Delete			= 0x00000200,
			Verify			= 0x00000400,
			Cancel			= 0x00000800,
			Create			= 0x00001000,
			Prompt			= 0x00002000,
			Exist			= 0x00004000,
			Reopen			= 0x00008000
		}

		// ---

		// Replacement of mmioFOURCC macros
		public static int mmioFOURCC(string str)
		{
			return (
				((int)(byte)(str[0])) |
				((int)(byte)(str[1]) << 8) |
				((int)(byte)(str[2]) << 16) |
				((int)(byte)(str[3]) << 24));
		}

		// Inverse of mmioFOURCC
		public static string decode_mmioFOURCC(int code)
		{
			char[]	chs = new char [4];

			for (int i = 0; i < 4; i++)
			{
				chs[i] = (char)(byte)((code >> (i << 3)) & 0xFF);
				if (!char.IsLetterOrDigit(chs[i]))
					chs[i] = ' ';
			}
			return new string(chs);
		}

		// --- public methods

		// Version of AVISaveOptions for one stream only
		//
		// I don't find a way to interop AVISaveOptions more likely, so the
		// usage of original version is not easy. The version makes it more
		// usefull.
		//
		public static int AVISaveOptions(IntPtr stream, ref AVICOMPRESSOPTIONS opts, IntPtr owner)
		{
			IntPtr[]	streams = new IntPtr[1];
			IntPtr[]	infPtrs = new IntPtr[1];
			IntPtr		mem;
			int			ret;

			// alloc unmanaged memory
			mem	= Marshal.AllocHGlobal(Marshal.SizeOf(typeof(AVICOMPRESSOPTIONS)));

			// copy from managed structure to unmanaged memory
			Marshal.StructureToPtr(opts, mem, false);

			streams[0] = stream;
			infPtrs[0] = mem;

			// show dialog with a list of available compresors and configuration
			ret = AVISaveOptions(IntPtr.Zero, 0, 1, streams, infPtrs);

			// copy from unmanaged memory to managed structure
			opts = (AVICOMPRESSOPTIONS) Marshal.PtrToStructure(mem, typeof(AVICOMPRESSOPTIONS));

			// free AVI compression options
			AVISaveOptionsFree(1, infPtrs);

			// clear it, because the information already freed by AVISaveOptionsFree
			opts.cbFormat	= 0;
			opts.cbParms	= 0;
			opts.lpFormat	= 0;
			opts.lpParms	= 0;

			// free unmanaged memory
			Marshal.FreeHGlobal(mem);

			return ret;
		}
	}
}
