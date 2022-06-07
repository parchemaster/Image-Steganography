using HW04;
using System.Text;


string inputPath = Directory.GetCurrentDirectory().Replace("HW04/bin/Debug/net6.0", "Data/");
string outputPath = Directory.GetCurrentDirectory().Replace("/bin/Debug/net6.0", "/OutPutData/");


int textChoosen = KeyboardInput.readInt("1.Would you like to encode your own text from console" + "\n" +
                                        "2.Would you like to try prepared text", 2);
String text = textChoosen == 2 ? Samples.StringSample() : Console.ReadLine(); 
var stegoObject = StegoObject.LoadObject(text, (s) => Encoding.Default.GetBytes(s));

// Images to encode the stegoObject into
// Each image must be used to encode a part of the stegoObject
var imageNames = new[]
{
    "John_Martin_-_Belshazzar's_Feast.jpg",
    "John_Martin_-_Pandemonium.jpg",
    "John_Martin_-_Sodom_and_Gomorrah.jpg",
    "John_Martin_-_The_Great_Day_of_His_Wrath.jpg",
    "John_Martin_-_The_Last_Judgement.jpg",
    "John_Martin_-_The_Plains_of_Heaven.jpg"
};

// String newOutputDirectory = Directory.GetCurrentDirectory().Replace("/bin/Debug/net6.0", "/OutPutData/");
// String newInputDirectory = Directory.GetCurrentDirectory().Replace("HW04/bin/Debug/net6.0", "Data/");

// ask how many pictures user want to use for encoding
int numberOfPictures = KeyboardInput.readInt("Pleas write down number between 1 and 5 for encoding your message into pictures",5);

// Do the magic...
var stegoProgram = new StegoProgram(imageNames, stegoObject, inputPath, outputPath);
// Encode it
await stegoProgram.EncodeData(numberOfPictures);
// Decode it
await stegoProgram.DecodeData();