using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServiceAPI.Utils
{
    public static class Extensions
    {
        public static byte[] AddBytes(this byte[] first, byte[] second)
        {
            int fl = first.Length;
            int sl = second.Length;

            var buffer = new byte[fl + sl];

            Buffer.BlockCopy(first, 0, buffer, 0, fl);
            Buffer.BlockCopy(second, 0, buffer, fl, sl);

            return buffer;
        }
    }
}
