using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BytesBuffer
{
    protected static int DEFAULT_LEHGTH = 128;
    protected byte[] bytes;
    protected int offset = 0;

    public BytesBuffer()
    {
        bytes = new byte[DEFAULT_LEHGTH];
    }

    public BytesBuffer(int length)
    {
        if (length > 0)
            bytes = new byte[length];
    }

    public BytesBuffer(byte[] fromBytes)
    {
        bytes = fromBytes;
    }

    public void AddFloat(float f)
    {
        //Debug.Log(f);
        byte[] addArray = BitConverter.GetBytes(f);
        Array.Reverse(addArray);
        addArray.CopyTo(bytes, offset);
        offset += sizeof(float);
    }

    public void AddBool(bool b)
    {
        if (b)
        {
            AddByte(1);
        }
        else
        {
            AddByte(0);
        }
    }
    public void AddByte(byte b)
    {
        bytes[offset] = b;
        offset++;
    }

    public void AddShort(short s)
    {
        byte[] addArray = BitConverter.GetBytes(s);
        Array.Reverse(addArray);
        addArray.CopyTo(bytes, offset);
        offset += sizeof(short);
    }

    public static short ReverseShort(short s)
    {
        byte[] shortArray = BitConverter.GetBytes(s);
        Array.Reverse(shortArray);
        return BitConverter.ToInt16(shortArray, 0);
    }

    public void AddInt(int i)
    {
        byte[] addArray = BitConverter.GetBytes(i);
        Array.Reverse(addArray);
        addArray.CopyTo(bytes, offset);
        offset += sizeof(int);
    }
	
	public void AddLong(long i)
    {
        byte[] addArray = BitConverter.GetBytes(i);
        Array.Reverse(addArray);
        addArray.CopyTo(bytes, offset);
        offset += sizeof(long);
    }

    public void AddVector3(Vector3 v)
    {
        AddFloat(v.x);
        AddFloat(v.y);
        AddFloat(v.z);
    }

    public void AddBytes(byte[] b)
    {
        ////Debug.Log(bytes.Length + "," + offset+);
        b.CopyTo(bytes, offset);
        offset += b.Length;        
    }

    public void AddBytes(byte[] b, int count)
    {
        ////Debug.Log(bytes.Length + "," + offset+);
        Array.Copy(b, 0, bytes, offset, count);
        offset += count;
    }

    public void AddBytesBuffer(BytesBuffer bb)
    {
        int bbLength = bb.GetOffset();
        //Debug.Log("bbLength = " + bbLength);
        AddInt(bbLength);
        AddBytes(bb.GetBytes(), bbLength);
    }

    public void AddString(string s)
    {
        //Debug.Log("s = " + s);
        byte[] strBytes = Encoding.UTF8.GetBytes(s);
        AddByte((byte)strBytes.Length);
        AddBytes(strBytes);
    }

    public void AddStringShortLength(string s)
    {
        byte[] strBytes = Encoding.UTF8.GetBytes(s);
        AddShort((short)strBytes.Length);
        AddBytes(strBytes);
    }

    public static byte GetStringByteLength(string s)
    {
        byte[] strBytes = Encoding.UTF8.GetBytes(s);
        return (byte)(strBytes.Length + 1);
    }

    //2.40----
    public static byte GetIntByteLength(int s)
    {
        byte[] strBytes = System.BitConverter.GetBytes(s);
        return (byte)(strBytes.Length + 1);
    }
    //------

    public int GetOffset()
    {
        return offset;
    }

    public byte[] GetRequestBytes(short requestID)
    {
        byte[] encodeArray = new byte[offset + 4];
        encodeArray[0] = (byte)((requestID >> 8) & 0xff);
        encodeArray[1] = (byte)(requestID & 0xff);
        encodeArray[2] = (byte)((offset >> 8) & 0xff);
        encodeArray[3] = (byte)(offset & 0xff);

        if (bytes != null && bytes.Length > 0)
            Array.Copy(bytes, 0, encodeArray, 4, offset);

        //Debug.Log("bytes[0] = " + bytes[0]);
        //Debug.Log("bytes[1] = " + bytes[1]);
        //Debug.Log("bytes[2] = " + bytes[2]);
        //Debug.Log("bytes[3] = " + bytes[3]);
        //Debug.Log("bytes.Length = " + bytes.Length);

        return encodeArray;
    }

    public byte[] GetValidBytes()
    {
        byte[] validBytes = new byte[offset];
        if (bytes != null && bytes.Length > 0)
            Array.Copy(bytes, validBytes, offset);

        return validBytes;
    }

    public byte[] GetBytes()
    {
        return bytes;
    }

    public byte ReadByte()
    {
        byte b = bytes[offset];
        offset++;
        return b;
    }

    public byte[] ReadBytes(int count)
    {
        byte[] b = new byte[count];
        Array.Copy(bytes, offset, b, 0, count);
        offset += count;
        return b;
    }

    public bool ReadBool()
    {
        byte b = ReadByte();
        if (b == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public short ReadShort()
    {
        byte[] shortBytes = new byte[sizeof(short)];
        Array.Copy(bytes, offset, shortBytes, 0, sizeof(short));
        Array.Reverse(shortBytes);
        offset += sizeof(short);
        return BitConverter.ToInt16(shortBytes, 0);
    }

    public int ReadInt()
    {
        byte[] intBytes = new byte[sizeof(int)];
        Array.Copy(bytes, offset, intBytes, 0, sizeof(int));
        Array.Reverse(intBytes);
        offset += sizeof(int);
        return BitConverter.ToInt32(intBytes, 0);
    }
	
	public long ReadLong()
    {
        byte[] intBytes = new byte[sizeof(long)];
        Array.Copy(bytes, offset, intBytes, 0, sizeof(long));
        Array.Reverse(intBytes);
        offset += sizeof(long);
        return BitConverter.ToInt64(intBytes, 0);
    }
	
    public string ReadString()
    {
        byte length = ReadByte();
        if (length == 0)
        {
            return string.Empty;
        }

        byte[] strBytes = new byte[length];
        Array.Copy(bytes, offset, strBytes, 0, length);
        offset += length;
        return Encoding.UTF8.GetString(strBytes);
    }

    public string ReadStringShortLength()
    {
        short length = ReadShort();
        if (length == 0)
        {
            return string.Empty;
        }
        byte[] strBytes = new byte[length];
        Array.Copy(bytes, offset, strBytes, 0, length);
        offset += length;
        return Encoding.UTF8.GetString(strBytes);
    }

    public float ReadFloat()
    {
        byte[] intBytes = new byte[sizeof(float)];
        Array.Copy(bytes, offset, intBytes, 0, sizeof(float));
        Array.Reverse(intBytes);
        offset += sizeof(float);
        return BitConverter.ToSingle(intBytes, 0);
    }

    public Vector3 ReadVector3()
    {
        Vector3 result = Vector3.zero;
        result.x = ReadFloat();
        result.y = ReadFloat();
        result.z = ReadFloat();
        return result;
    }

    public int ReadIntNoOffset()
    {
        byte[] intBytes = new byte[sizeof(int)];
        Array.Copy(bytes, offset, intBytes, 0, sizeof(int));
        Array.Reverse(intBytes);
        return BitConverter.ToInt32(intBytes, 0);
    }

    public BytesBuffer ReadBytesBuffer()
    {
        int bbLength = ReadInt();
        byte[] data = ReadBytes(bbLength);
        return new BytesBuffer(data);
    }
}

