using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace OOBehave.Portal
{
    public interface IZip
    {

        byte[] Compress(string str);
        string Decompress(byte[] data);

    }


    public class Zip : IZip
    {
        public byte[] Compress(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) { return new byte[0]; }

            var bytes = Encoding.UTF8.GetBytes(data);
            using (MemoryStream msi = new MemoryStream(bytes))
            {
                using (MemoryStream mso = new MemoryStream())
                {
                    using (GZipStream zip = new GZipStream(mso, CompressionMode.Compress))
                    {
                        msi.CopyTo(zip);
                    }
                    return mso.ToArray();
                }
            }
        }

        public string Decompress(byte[] obj)
        {
            if (obj == null) { return null; }

            using (MemoryStream msi = new MemoryStream(obj))
            {
                using (MemoryStream mso = new MemoryStream())
                {
                    using (GZipStream zip = new GZipStream(msi, CompressionMode.Decompress))
                    {
                        zip.CopyTo(mso);
                    }

                    var resultArray = mso.ToArray();
                    return Encoding.UTF8.GetString(resultArray);
                }
            }
        }
    }
}
