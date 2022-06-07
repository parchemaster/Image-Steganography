using System.Collections;

namespace HW04
{
    public class ByteSpliting
    {
        const int BitsInByte = 8;

        /// <summary>
        /// Splits the byte to chunks of given size.
        /// Mind the endianness! The least significant chunks are on lower index.
        /// </summary>
        /// <param name="byte">byte to split</param>
        /// <param name="size">bits in each chunk</param>
        /// <example>Split(207,2) => [3,3,0,3]</example>
        /// <returns>chunks</returns>
        public static IEnumerable<byte> Split(byte @byte, int size)
        {
            // convert byte to list of bits (1 == true, 0 == false)
            var bits = new BitArray(new byte[] {@byte});
            var result = new List<byte>();

            for (var i = 0; i < bits.Length; i += size)
            {
                var bit = new Char[size];
                for (var bitIndex = 0; bitIndex < size; bitIndex++)
                {
                    bit[bitIndex] = (char)(bits.Get(i + bitIndex) ? '1' : '0');
                }

                Array.Reverse(bit);
                var stringNumber = Convert.ToInt32(new string(bit), 2);
                result.Add(Convert.ToByte(stringNumber));
            }

            return result;
        }

        /// <summary>
        /// Reforms chunks to a byte.
        /// Mind the endianness! The least significant chunks are on lower index.
        /// </summary>
        /// <param name="parts">chunks to reform</param>
        /// <param name="size">bits in each chunk</param>
        /// <example>Split([3,3,0,3],2) => 207</example>
        /// <returns>byte</returns>
        public static byte Reform(IEnumerable<byte> parts, int size)
        {
            // converting each element to bit and adding to list of string
            var bitList = new List<string>();
            foreach (var bit in parts)
            {
                var num = Convert.ToInt32(bit.ToString());
                bitList.Add(Convert.ToString(num, 2));
            }

            // going throw each bit
            for (var i = 0; i < bitList.Count; i++)
            {
                var line = bitList[i];
                var newCharArray = new char[size];
                // if bit size smaller then expected size add additional zeros
                if (line.ToCharArray().Length < size)
                {
                    for (int originIndex = 0, newIndex = 0; newIndex < size; newIndex ++)
                    {
                        if (newIndex >= size - line.Length)
                        {
                            newCharArray[newIndex] = line.ToCharArray()[originIndex];
                            originIndex++;
                        }
                        else
                        {
                            newCharArray[newIndex] = '0';
                        }
                    }
                    bitList[i] = new string(newCharArray);
                }

            }
            bitList.Reverse();
            string binaryString = string.Join("", bitList);
            var resByte = Convert.ToByte(Convert.ToInt32(binaryString, 2).ToString());
            return resByte;
        }
    }
}