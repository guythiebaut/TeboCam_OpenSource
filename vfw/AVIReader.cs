namespace Tiger.Video.VFW
{
	using System;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.Runtime.InteropServices;

	/// <summary>
	/// Reading AVI files using Video for Windows
	/// </summary>
	public class AVIReader : IDisposable
	{
		private IntPtr	file;
		private IntPtr	stream;
		private IntPtr	getFrame;

		private int		width;
		private int		height;
		private int		position;
		private int		start;
		private int		length;
		private float	rate;
		private string	codec;

		// Width property
		public int Width
		{
			get { return width; }
		}
		// Height property
		public int Height
		{
			get { return height; }
		}
		// FramesRate property
		public float FrameRate
		{
			get { return rate; }
		}
		// CurrentPosition property
		public int CurrentPosition
		{
			get { return position; }
			set
			{
				if ((value < start) || (value >= start + length))
					position = start;
				else
					position = value;
			}
		}
		// Length property
		public int Length
		{
			get { return length; }
		}
		// Codec property
		public string Codec
		{
			get { return codec; }
		}


		// Constructor
		public AVIReader()
		{
			Win32.AVIFileInit();
		}

		// Desctructor
		~AVIReader()
		{
			Dispose(false);
		}

		// Free all unmanaged resources
		public void Dispose()
		{
			Dispose(true);
			// Remove me from the Finalization queue 
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Dispose managed resources
				
				// there is nothing managed yet
			}
			Close();
			Win32.AVIFileExit();
		}

		// Open an AVI file
		public void Open(string fname)
		{
			// close previous file
			Close();

			// open file
			if (Win32.AVIFileOpen(out file, fname, Win32.OpenFileMode.ShareDenyWrite, IntPtr.Zero) != 0)
				throw new ApplicationException("Failed opening file");

			// get first video stream
			if (Win32.AVIFileGetStream(file, out stream, Win32.mmioFOURCC("vids"), 0) != 0)
				throw new ApplicationException("Failed getting video stream");

			// get stream info
			Win32.AVISTREAMINFO	info = new Win32.AVISTREAMINFO();
			Win32.AVIStreamInfo(stream, ref info, Marshal.SizeOf(info));

			width		= info.rcFrame.right;
			height		= info.rcFrame.bottom;
			position	= info.dwStart;
			start		= info.dwStart;
			length		= info.dwLength;
			rate		= (float) info.dwRate / (float) info.dwScale;
			codec		= Win32.decode_mmioFOURCC(info.fccHandler);

			// prepare decompressor
			Win32.BITMAPINFOHEADER bih = new Win32.BITMAPINFOHEADER();

			bih.biSize			= Marshal.SizeOf(bih.GetType());
			bih.biWidth			= width;
			bih.biHeight		= height;
			bih.biPlanes		= 1;
			bih.biBitCount		= 24;
			bih.biCompression	= 0; // Bi_RGB

			// get frame open object
			if ((getFrame = Win32.AVIStreamGetFrameOpen(stream, ref bih)) == IntPtr.Zero)
			{
				bih.biHeight = -height;

				if ((getFrame = Win32.AVIStreamGetFrameOpen(stream, ref bih)) == IntPtr.Zero)
					throw new ApplicationException("Failed initializing decompressor");
			}
		}

		// Close file
		public void Close()
		{
			// release frame open object
			if (getFrame != IntPtr.Zero)
			{
				Win32.AVIStreamGetFrameClose(getFrame);
				getFrame = IntPtr.Zero;
			}
			// release stream
			if (stream != IntPtr.Zero)
			{
				Win32.AVIStreamRelease(stream);
				stream = IntPtr.Zero;
			}
			// release file
			if (file != IntPtr.Zero)
			{
				Win32.AVIFileRelease(file);
				file = IntPtr.Zero;
			}
		}

		// Get next video frame
		public Bitmap GetNextFrame()
		{
			// get frame at specified position
			IntPtr pdib = Win32.AVIStreamGetFrame(getFrame, position);
			if (pdib == IntPtr.Zero)
				throw new ApplicationException("Failed getting frame");

			Win32.BITMAPINFOHEADER bih;

			// copy BITMAPINFOHEADER from unmanaged memory
			bih = (Win32.BITMAPINFOHEADER) Marshal.PtrToStructure(pdib, typeof(Win32.BITMAPINFOHEADER));

			// create new bitmap
			Bitmap	bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);

			// lock bitmap data
			BitmapData	bmData = bmp.LockBits(
				new Rectangle(0, 0, width, height),
				ImageLockMode.ReadWrite,
				PixelFormat.Format24bppRgb);

			// copy image data
			int srcStride = bmData.Stride;	// width * 3;
			int dstStride = bmData.Stride;

			// check image direction
			if (bih.biHeight > 0)
			{
				// it`s a bottom-top image
				int dst = bmData.Scan0.ToInt32() + dstStride * (height - 1);
				int src = pdib.ToInt32() + Marshal.SizeOf(typeof(Win32.BITMAPINFOHEADER));

				for (int y = 0; y < height; y++)
				{
					Win32.memcpy(dst, src, srcStride);
					dst -= dstStride;
					src += srcStride;
				}
			}
			else
			{
				// it`s a top bootom image
				int dst = bmData.Scan0.ToInt32();
				int src = pdib.ToInt32() + Marshal.SizeOf(typeof(Win32.BITMAPINFOHEADER));

				if (srcStride != dstStride)
				{
					// copy line by line
					for (int y = 0; y < height; y++)
					{
						Win32.memcpy(dst, src, srcStride);
						dst += dstStride;
						src += srcStride;
					}
				}
				else
				{
					// copy the whole image
					Win32.memcpy(dst, src, srcStride * height);
				}
			}

			// unlock bitmap data
			bmp.UnlockBits(bmData);

			position++;

			return bmp;
		}
	}
}
