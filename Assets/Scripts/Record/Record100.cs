using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class Record100 : IRecordset
{
    public static string mVersion = "100";

    public BytesBuffer SaveBytesBuffer()
    {
        BytesBuffer saveDataBuffer = new BytesBuffer(1024);
        saveDataBuffer.AddString(LocalSaveData.LoginUserName);
        saveDataBuffer.AddString(LocalSaveData.LoginPassword);
        return saveDataBuffer;
    }

    public void SaveData(BinaryWriter bw)
    {
        BytesBuffer bb = SaveBytesBuffer();
        int bbLength = bb.GetOffset();        
        bw.Write(bbLength);
        bw.Write(bb.GetBytes(), 0, bbLength);
    }

    public void LoadData(BinaryReader br)
    {
        int bufferLength = br.ReadInt32();
        byte[] bufferData = br.ReadBytes(bufferLength);
        BytesBuffer bb = new BytesBuffer(bufferData);
        LoadBytesBuffer(bb);
    }
    public void LoadBytesBuffer(BytesBuffer bb)
    {
        LocalSaveData.LoginUserName = bb.ReadString();
        LocalSaveData.LoginPassword = bb.ReadString();
    }
}