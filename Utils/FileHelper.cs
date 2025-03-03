public static class FileHelper
{
    public static List<string> ReadFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close(); // Create file if it doesn't exist
            return new List<string>();
        }

        return new List<string>(File.ReadAllLines(filePath));
    }

    public static void WriteToFile(string filePath, string data)
    {
        File.AppendAllText(filePath, data + Environment.NewLine);
    }

    public static void OverwriteFile(string filePath, List<string> data)
    {
        File.WriteAllLines(filePath, data);
    }
}