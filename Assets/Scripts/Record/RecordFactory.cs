using System;

public class RecordFactory
{
    public static IRecordset CreateRecord(string version)
    {
        IRecordset rec = null;

#if UNITY_EDITOR
        UnityEngine.Debug.Log("version = " + version);
#endif

        if (version.Equals(Record100.mVersion))
        {
            rec = new Record100();
        }

        return rec;
    }
}