using System;
using System.IO;
using UnityEngine;

namespace GMEngine
{
    public interface ISaveDataSender
    {
        public void SendData(SaveData data);
    }

    public interface ISaveDataRecevier
    {
        public void ReceiveData(SaveData data);
    }

    public class SaveDataWriter : IDisposable
    {
        BinaryWriter writer;
        public SaveDataWriter(BinaryWriter writer) { this.writer = writer; }

        public void Dispose()
        {
            ((IDisposable)writer).Dispose();
        }

        public void Write(Quaternion value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
            writer.Write(value.z);
            writer.Write(value.w);
        }

        public void Write(Vector3 value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
            writer.Write(value.z);
        }

        public void Write(float value)
        {
            writer.Write(value);
        }

        public void Write(int value)
        {
            writer.Write(value);
        }

        public void Write(string name)
        {
            writer.Write(name);
        }
    }

    public class SaveDataReader : IDisposable
    {
        BinaryReader reader;

        public void Dispose()
        {
            ((IDisposable)reader).Dispose();
        }

        public SaveDataReader(BinaryReader reader) { this.reader = reader; }

        public float ReadFloat()
        {
            return reader.ReadSingle();
        }

        public int ReadInt()
        {
            return reader.ReadInt32();
        }

        public string ReadString()
        {
            return reader.ReadString();
        }

        public Quaternion ReadQuaternion()
        {
            Quaternion value;
            value.x = reader.ReadSingle();
            value.y = reader.ReadSingle();
            value.z = reader.ReadSingle();
            value.w = reader.ReadSingle();
            return value;
        }

        public Vector3 ReadVector3()
        {
            Vector3 value;
            value.x = reader.ReadSingle();
            value.y = reader.ReadSingle();
            value.z = reader.ReadSingle();
            return value;
        }
    }
}
