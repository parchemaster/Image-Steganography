using System.Diagnostics;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace HW04;

public class StegoProgram
{
    private string[] _imageNames;
    private StegoObject _stegoObject;
    private string _inputPath;
    private string _outputPath;
    private StegoImageProcessor _stegoImageProcessor = new StegoImageProcessor();
    private int[] _dataChunkSizes;
    private int _dataCunkCount;
    
    public StegoProgram(String [] imageNames, StegoObject stegoObject, String inputPath, string outputPath)
    {
        _imageNames = imageNames;
        _stegoObject = stegoObject;
        _inputPath = inputPath;
        _outputPath = outputPath;
    }

    public async Task EncodeData(int numberOfPictures) 
    {
        var dataChunk = _stegoObject.GetDataChunks(numberOfPictures);
        _dataCunkCount = dataChunk.Count();
        _dataChunkSizes = new int[_dataCunkCount];
        var watch = System.Diagnostics.Stopwatch.StartNew();

        for (var i = 0; i < _dataCunkCount; i++)
        {
            _dataChunkSizes[i] = dataChunk.ElementAt(i).Length;
            var inputImage =  Image.LoadAsync<Rgba32>(_inputPath + _imageNames[i]);
            var encodeImage = _stegoImageProcessor.EncodePayload(inputImage.Result, dataChunk.ElementAt(i));
            await _stegoImageProcessor.SaveImageAsync(encodeImage.Result, _outputPath + _imageNames[i]);
        }
        
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        
        Console.WriteLine("Total execute time of encoding: " + elapsedMs);
    }

    public async Task DecodeData()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var byteResult = new List<byte[]>();

        for (var i = 0; i < _dataCunkCount; i++)
        {
            var inputImage = await Image.LoadAsync<Rgba32>(_outputPath + _imageNames[i]);
            var extractPayload = _stegoImageProcessor.ExtractPayload(inputImage, _dataChunkSizes[i]);
            byteResult.Add(extractPayload.Result);
        }
        
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        
        Console.WriteLine("Total execute time of extracting: " + elapsedMs);
        byteResult.ForEach(res => Console.Write(Encoding.Default.GetString(res)));
    }
}