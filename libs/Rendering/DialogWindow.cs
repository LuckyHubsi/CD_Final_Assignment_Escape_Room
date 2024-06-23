using System;
using System.Text;

public class DialogWindow
{
    private int width; // Width of the dialog window
    private int height; // Height of the dialog window
    private int x; // X-coordinate for the top-left corner of the dialog window
    private int y; // Y-coordinate for the top-left corner of the dialog window

    // Constructor to initialize the dialog window with given dimensions and position
    public DialogWindow(int width, int height, int x, int y)
    {
        this.width = width;
        this.height = height;
        this.x = x;
        this.y = y;
    }

    // Method to draw the dialog window with a title and message
    public void Draw(string title, string message)
    {
        // Draw the outer border of the dialog window
        DrawBorder();

        // Draw the title at the top of the dialog window
        DrawTitle(title);

        // Draw the message in the center of the dialog window
        DrawMessage(message);
    }

    // Private method to draw the border of the dialog window
    private void DrawBorder()
    {
        // Draw the top border
        Console.SetCursorPosition(x, y);
        Console.Write("┌" + new string('─', width - 2) + "┐");

        // Draw the side borders
        for (int i = 1; i < height - 1; i++)
        {
            Console.SetCursorPosition(x, y + i);
            Console.Write("│" + new string(' ', width - 2) + "│");
        }

        // Draw the bottom border
        Console.SetCursorPosition(x, y + height - 1);
        Console.Write("└" + new string('─', width - 2) + "┘");
    }

    // Private method to draw the title of the dialog window
    private void DrawTitle(string title)
    {
        int titleX = x + (width - title.Length) / 2; // Calculate the X-coordinate to center the title
        Console.SetCursorPosition(titleX, y);
        Console.Write(title); // Write the title at the calculated position
    }

    // Private method to draw the message in the dialog window
    private void DrawMessage(string message)
    {
        string[] lines = WrapText(message, width - 4).ToArray(); // Wrap the message text to fit within the dialog width
        int startY = y + (height - lines.Length) / 2; // Calculate the Y-coordinate to center the message

        // Write each line of the wrapped message
        for (int i = 0; i < lines.Length; i++)
        {
            Console.SetCursorPosition(x + 2, startY + i); // Set the cursor position with padding
            Console.Write(lines[i]);
        }
    }

    // Private method to wrap text into lines of a specified maximum length
    private IEnumerable<string> WrapText(string text, int maxLineLength)
    {
        StringBuilder line = new StringBuilder(); // Initialize a StringBuilder to build each line
        foreach (string word in text.Split(' ')) // Split the text into words
        {
            // Check if adding the next word would exceed the maximum line length
            if (line.Length + word.Length + 1 > maxLineLength)
            {
                yield return line.ToString(); // Return the current line
                line.Clear(); // Clear the StringBuilder for the next line
            }
            if (line.Length > 0)
            {
                line.Append(" "); // Add a space between words
            }
            line.Append(word); // Add the word to the current line
        }
        if (line.Length > 0)
        {
            yield return line.ToString(); // Return the last line if it exists
        }
    }
}
