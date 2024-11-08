using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 1 ||
            (args[0] != "-e" && args[0] != "--encrypt" && args[0] != "-d" && args[0] != "--decrypt"))
        {
            Console.WriteLine("Usage: dotnet encrypt_json.dll -e|--encrypt -d|--decrypt");
            return;
        }

        string personalKeyFilePath = "personal_key.json";
        if (!File.Exists(personalKeyFilePath))
        {
            Console.WriteLine($"Error: Personal key file '{personalKeyFilePath}' does not exist.");
            return;
        }

        string? personalKey;
        try
        {
            var jsonContent = File.ReadAllText(personalKeyFilePath);
            var jsonDocument = JsonDocument.Parse(jsonContent);
            var key = jsonDocument.RootElement.GetProperty("key");

            if (key.ValueKind != JsonValueKind.String)
            {
                Console.WriteLine("Error: 'key' property in personal key file is not a string.");
                return;
            }

            if (string.IsNullOrEmpty(key.GetString()))
            {
                Console.WriteLine("Error: 'key' property in personal key file is empty.");
                return;
            }

            personalKey = key.GetString();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading personal key: {ex.Message}");
            return;
        }

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDataProtection();
        var services = serviceCollection.BuildServiceProvider();

        var dataProtectionProvider = services.GetService<IDataProtectionProvider>();
        if (dataProtectionProvider == null)
        {
            Console.WriteLine("Error: IDataProtectionProvider service is not available.");
            return;
        }

        if (string.IsNullOrEmpty(personalKey))
        {
            Console.WriteLine("Error: Personal key is empty.");
            return;
        }

        var protector = dataProtectionProvider.CreateProtector(personalKey);

        string folderPath;
        if (args[0] == "-e" || args[0] == "--encrypt")
        {
            folderPath = "files_to_encrypt";
        }
        else
        {
            folderPath = "files_to_decrypt";
        }

        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine($"Error: Directory '{folderPath}' does not exist.");
            return;
        }

        var jsonFiles = Directory.GetFiles(folderPath, "*.json");
        if (jsonFiles.Length == 0)
        {
            Console.WriteLine($"No JSON files found in directory '{folderPath}'.");
            return;
        }

        foreach (var filePath in jsonFiles)
        {
            var jsonContent = File.ReadAllText(filePath);

            if (args[0] == "-e" || args[0] == "--encrypt")
            {
                var encryptedJson = protector.Protect(jsonContent);
                Console.WriteLine($"Encrypted JSON for '{filePath}': {encryptedJson}");
            }
            else if (args[0] == "-d" || args[0] == "--decrypt")
            {
                try
                {
                    var decryptedJson = protector.Unprotect(jsonContent);
                    Console.WriteLine($"Decrypted JSON for '{filePath}': {decryptedJson}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error decrypting file '{filePath}': {ex.Message}");
                }
            }
        }
    }
}
