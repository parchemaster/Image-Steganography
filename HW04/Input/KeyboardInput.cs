namespace HW04;

public class KeyboardInput {
    public static int readInt(String consoleText, int diapason) {
        int n = 0;
        String s;
        
        try {	
            Console.WriteLine(consoleText);
            s = Console.ReadLine();
            n = Convert.ToInt32(s);
        } catch (Exception e) {
            Console.WriteLine("There was an error, try again");
            n = readInt(consoleText, diapason);
        }

        if (n < 1 || n > diapason + 1)
        {
            Console.WriteLine("Wrong diapason, try number from 1 to 5");
            n = readInt(consoleText, diapason);
        }

        return n;
    }

}