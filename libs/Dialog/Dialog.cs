namespace libs;

public class Dialog
{
    private DialogNode _currentNode;
    private DialogNode _startingNode;
    private DialogNode _endNode;

    public Dialog(DialogNode startingNode)
    {
        _startingNode = startingNode;
        _currentNode = startingNode;
        _endNode = new DialogNode("There is nothing left to say...");
    }

    public void Start()
    {
        DialogWindow dialogWindow = new DialogWindow(35, 10, 10, Math.Min(Console.BufferHeight - 10 - 1, 1)); // Adjust size and position as needed

        while (_currentNode != null)
        {
            dialogWindow.Draw(FileHandler.GetDialog("WitchHeader1"), _currentNode.Text);

            for (int i = 0; i < _currentNode.Responses.Count; i++)
            {
                Console.SetCursorPosition(2, 12 + i); // Adjust position as needed
                Console.WriteLine($"{i + 1}. {_currentNode.Responses[i].ResponseText}");
            }

            int choice;
            if (_currentNode.Responses.Count == 0)
                break;

            while (true)
            {
                Console.SetCursorPosition(2, 12 + _currentNode.Responses.Count + 1); // Adjust position as needed
                Console.Write("Choose an option: ");
                if (int.TryParse(Console.ReadLine(), out choice) && choice > 0 && choice <= _currentNode.Responses.Count)
                {
                    break;
                }
                Console.WriteLine("Invalid choice, please try again.");
            }

            _currentNode = _currentNode.Responses[choice - 1].NextNode;
        }

        _currentNode = _endNode;

        //dialogWindow.Draw("Dialog", "End of dialog.");
        Thread.Sleep(3000);
    }
}
