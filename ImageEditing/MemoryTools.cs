using System;
using System.Runtime.InteropServices;

namespace ImageEditing
{
	public static class MemoryTools
	{
		// Win32 memory copy function
		[DllImport("ntdll.dll", CallingConvention = CallingConvention.Cdecl)]
		private static unsafe extern byte* memcpy(byte* dst, byte* src, int count);

		// Win32 memory set function
		[DllImport("ntdll.dll", CallingConvention = CallingConvention.Cdecl)]
		private static unsafe extern byte* memset(byte* dst, int filler, int count);

		public unsafe static IntPtr CopyUnmanagedMemory(IntPtr dst, IntPtr src, int count)
		{
			CopyUnmanagedMemory((byte*)dst.ToPointer(), (byte*)src.ToPointer(), count);
			return dst;
		}

		public static unsafe byte* CopyUnmanagedMemory(byte* dst, byte* src, int count)
		{
			return memcpy(dst, src, count);
		}

		public unsafe static IntPtr SetUnmanagedMemory(IntPtr dst, int filler, int count)
		{
			SetUnmanagedMemory((byte*)dst.ToPointer(), filler, count);
			return dst;
		}

		public static unsafe byte* SetUnmanagedMemory(byte* dst, int filler, int count)
		{
			return memset(dst, filler, count);
		}
	}
}
