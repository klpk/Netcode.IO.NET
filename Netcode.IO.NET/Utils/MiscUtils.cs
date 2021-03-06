﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using NetcodeIO.NET.Internal;

namespace NetcodeIO.NET.Utils
{
	internal class MiscUtils
	{
		public static bool AddressEqual(EndPoint lhs, EndPoint rhs)
		{
			if (lhs == null) return false;

			return lhs.Equals(rhs);
		}

		public static bool MatchChars(char[] chars, string match)
		{
			if (chars.Length != match.Length) return false;

			for (int i = 0; i < chars.Length; i++)
				if (chars[i] != match[i]) return false;

			return true;
		}
		
		public static bool ByteArraysEqual(byte[] lhs, byte[] rhs)
		{
			if (lhs.Length != rhs.Length) return false;
			for (int i = 0; i < lhs.Length; i++)
				if (lhs[i] != rhs[i]) return false;

			return true;
		}

		public static bool CompareByteArraysConstantTime(byte[] lhs, byte[] rhs)
		{
			int min = Math.Min(lhs.Length, rhs.Length);

			bool compare = true;

			for (int i = 0; i < min; i++)
				compare &= ((lhs[i] & rhs[i]) == lhs[i]);

			return compare;
		}

#if UNSAFE
		public unsafe static bool CompareHMACConstantTime(byte[] lhs, byte[] rhs)
		{
			if (lhs.Length != Defines.MAC_SIZE || rhs.Length != Defines.MAC_SIZE)
				throw new InvalidOperationException();

			// fast constant-time comparison for 16-byte MAC which gets away with just two compares (first 8 bytes, then second 8 bytes)
			bool compare = true;
			fixed (byte* lb = &lhs[0])
			{
				fixed (byte* rb = &rhs[0])
				{
					ulong* left = (ulong*)lb;
					ulong* right = (ulong*)rb;

					compare &= (*left == *right);

					left++;
					right++;

					compare &= (*left == *right);
				}
			}

			return compare;
		}
#else
		public static bool CompareHMACConstantTime(byte[] lhs, byte[] rhs)
		{
			if (lhs.Length != Defines.MAC_SIZE || rhs.Length != Defines.MAC_SIZE)
				throw new InvalidOperationException();

			bool compare = true;
			for( int i = 0; i < Defines.MAC_SIZE; i++ )
				compare &= (lhs[i] == rhs[i]);

			return compare;
		}
#endif
	}
}
