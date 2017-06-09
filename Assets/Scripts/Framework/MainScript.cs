using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// 程序入口
/// 
/// 读取本地的数据文件
/// 其中的信息为加密的版本号，账号，密码 3个东西
/// 然后启动pureMVC框架
/// 设置帧率
/// </summary>
/// 
public class MainScript : MonoBehaviour
{
    private string mVersion = "100";
    private string mKey = "miaoBox";
    private string mReadVersion;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        //读取本地文件中的数据，其中包括版本号(默认100)，用户名，密码
        LocalLoad();

        //启动pureMVC框架
        AppFacade.getInstance.StartUp();

        Application.targetFrameRate = 60;
    }
    void OnApplicationQuit()
    {
        AppFacade.getInstance.QuitGameAndSave();
        LocalSave();
    }
    public void LocalSave()
    {
        string path = Application.persistentDataPath + "/Documents/";
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXEditor)
        {
            path = Application.dataPath + "/../Documents/";
        }

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        MemoryStream ms = new MemoryStream();
        BinaryWriter bw = new BinaryWriter(ms);

        bw.Write(mVersion);
        IRecordset rec = RecordFactory.CreateRecord(mVersion);
        if (rec != null)
        {
            rec.SaveData(bw);
        }
        byte[] buffer = ms.ToArray();
        ms.Close();
        bw.Close();

        buffer = CryptBuffer(buffer);
        Stream stream = File.Open(path + "MiaoBoxLocal", FileMode.Create);
        stream.Write(buffer, 0, buffer.Length);
        stream.Flush();
        stream.Close();
        Debug.Log("save");
    }

    public byte LocalLoad()
    {
        string path = Application.persistentDataPath + "/Documents/";
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXEditor)
        {
            path = Application.dataPath + "/../Documents/";
        }

        if (File.Exists(path + "MiaoBoxLocal"))
        {
            Stream stream = File.Open(path + "MiaoBoxLocal", FileMode.Open);
            byte[] original = GetBytesFromStream(stream);
            byte[] buffer = DecryptBuffer(original);
            stream.Close();
            MemoryStream ms = new MemoryStream(buffer);
            BinaryReader br = new BinaryReader(ms);
            string ver = string.Empty;
            try
            {
                Debug.Log("Load");

                //BinaryReader.ReadString(）方法如何确定从数据流中读多少内容
                //是这样的，BinaryReader.ReadString是和BinaryWriter.Write(string)配合使用的。
                //使用后者写入文件的时候，如果写入字符串，是会将字符串的长度也写在文件中的。你可以用BinaryWriter.Write(string)写入文件

                ver = br.ReadString();
                mReadVersion = ver;
                IRecordset rec = RecordFactory.CreateRecord(mReadVersion);
                if (rec != null)
                {
                    rec.LoadData(br);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Version: " + ver);
                Debug.LogWarning(e.StackTrace);

                if (!File.Exists(path + "MiaoBoxLocal" + "_" + ver))
                {
                    File.Copy(path + "MiaoBoxLocal", path + "MiaoBoxLocal" + "_" + ver);
                }
                return 1;
            }
            finally
            {
                br.Close();
                ms.Close();
            }

            if (!ver.Equals(mVersion))
            {
                LocalSave();
            }
            return 0;
        }
        else
        {
            return 2;
        }
    }

    /// <summary>
    /// 对字节数组进行加密
    /// 参考链接
    /// http://blog.163.com/black_ljz/blog/static/5102226220098181244620/
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public byte[] CryptBuffer(byte[] data)
    {
        byte[] keyBytes = System.Text.Encoding.ASCII.GetBytes(mKey);
        byte[] buffer = new byte[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            buffer[i] = (byte)(data[i] ^ keyBytes[i % keyBytes.Length]);
        }

        return buffer;
    }

    /// <summary>
    /// 对字节数组进行解密
    /// 参考链接
    /// http://blog.163.com/black_ljz/blog/static/5102226220098181244620/
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public byte[] DecryptBuffer(byte[] buffer)
    {
        byte[] keyByte = System.Text.Encoding.ASCII.GetBytes(mKey);

        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] ^= keyByte[i % keyByte.Length];
        }

        return buffer;
    }

    /// <summary>
    /// 获取文件流的字节数组
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public byte[] GetBytesFromStream(Stream stream)
    {
        byte[] buffer = new byte[stream.Length];
        int numBytesToRead = (int)stream.Length;
        int numBytesRead = 0;
        while (numBytesToRead > 0)
        {
            // Read may return anything from 0 to numBytesToRead.
            int n = stream.Read(buffer, numBytesRead, numBytesToRead);
            // The end of the file is reached.
            if (n == 0)
                break;
            numBytesRead += n;
            numBytesToRead -= n;
        }
        return buffer;
    }
}