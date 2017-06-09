using UnityEngine;
using System.Collections;
using System.IO;

public interface IRecordset
{
    BytesBuffer SaveBytesBuffer();
    void LoadBytesBuffer(BytesBuffer bb);

    void SaveData(BinaryWriter bw);
	void LoadData(BinaryReader br);
}