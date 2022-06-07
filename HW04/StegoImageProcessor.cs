using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace HW04
{
    public class StegoImageProcessor
    {
        const int BitsInByte = 8;

        // Use constructor for additional configuration

        public async Task<Image<Rgba32>> LoadImageAsync(string path) => await Image.LoadAsync<Rgba32>("Samples/stabbyCat.png");

        public Task SaveImageAsync(Image<Rgba32> image, string path) => image.SaveAsBmpAsync(path);

        public Task<Image<Rgba32>> EncodePayload(Image<Rgba32> image, byte[] payload) => Task.Run(() =>
        {
            // This can be CPU-intensive, so it can run in separate task

            var splitedBytesList = new List<List<byte>>();
            for (var i = 0; i < payload.Length; i++)
            {
                splitedBytesList.Add(ByteSpliting.Split(payload[i], 1).ToList());
            }
            
            image.ProcessPixelRows(accessor =>
            {
                int textBitIndex = 0;
                int splitByteListIndex = 0;
                bool isEndOfByteList = false;
                for (int y = 0; y < accessor.Height; y++)
                {
                    Span<Rgba32> pixelRow = accessor.GetRowSpan(y);

                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        if (splitByteListIndex == splitedBytesList.Count)
                        {
                            isEndOfByteList = true;
                            break;
                        }
                        
                        ref Rgba32 pixel = ref pixelRow[x];
                        var splitPixelR = ByteSpliting.Split(pixel.R, 1).ToArray();
                        splitPixelR[0] = splitedBytesList[splitByteListIndex].ElementAt(textBitIndex);
                        splitPixelR[1] = splitedBytesList[splitByteListIndex].ElementAt(textBitIndex+1);
                            
                        var newChanel = ByteSpliting.Reform(splitPixelR, 1);
                        pixel.R = newChanel;
                        
                        if (textBitIndex >= 6)
                        {
                            textBitIndex = 0;
                            splitByteListIndex++;
                        }
                            
                        else
                        {
                            textBitIndex+=2;
                        }

                    }
                    if (isEndOfByteList) break;
                }
            });
            return image;
        });
        
        
        public Task<byte[]> ExtractPayload(Image<Rgba32> image, int dataSize) => Task.Run(() =>
        {
            // This can be CPU-intensive, so it can run in separate task
            var hiddenText = new List<byte>();
            image.ProcessPixelRows(accessor =>
            {

                int textByteIndex = 0;
                int textBitIndex = 0;
                bool isEndOfProcess = false;
                byte[] textByte = new byte[8];
                for (int y = 0; y < accessor.Height; y++)
                {
                    Span<Rgba32> pixelRow = accessor.GetRowSpan(y);
                    
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        if (textByteIndex == dataSize)
                        {
                            isEndOfProcess = true;
                            break;
                        }
                        ref Rgba32 pixel = ref pixelRow[x];
                        
                        var splitPixelR = ByteSpliting.Split(pixel.R, 1).ToArray();
                        var pixelR1 = splitPixelR.ElementAt(0);
                        var pixelR2 = splitPixelR.ElementAt(1);

                        textByte[textBitIndex] = pixelR1;
                        textByte[textBitIndex + 1] = pixelR2;
               

                        if (textBitIndex >= 6)
                        {
                            textBitIndex = 0;
                            textByteIndex++;
                            byte newByte = ByteSpliting.Reform(textByte, 1);
                            hiddenText.Add(newByte);
                            textByte = new byte[8];
                        }
                        
                        else
                        {
                            textBitIndex += 2;
                        }
                        
                    }
                    if (isEndOfProcess) break;
                }
            });
            return hiddenText.ToArray();
        });
    }
}
