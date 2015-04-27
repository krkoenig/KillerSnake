using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace scMessage
{
    public class conversionTools
    {
        public static Object convertBytesToObject(byte[] b)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(b, 0, b.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            object obj = (object)binForm.Deserialize(memStream);
            return obj;
        }

        public static byte[] convertObjectToBytes(object o)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            binForm.Serialize(memStream, o);
            return memStream.ToArray();
        }

        public static byte[] wrapMessage(byte[] mes)
        {
            byte[] lengPre = BitConverter.GetBytes(mes.Length);
            byte[] r = new byte[lengPre.Length + mes.Length];
            lengPre.CopyTo(r, 0);
            mes.CopyTo(r, lengPre.Length);
            return r;
        }
    }
}
