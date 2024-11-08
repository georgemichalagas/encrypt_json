# encrypt_json
Encrypt or Decrypt json files.

1.
Add the files that you want to encrypt in the "files_to_encrypt" folder.
Add the files that you want to decrypt in the "files_to_decrypt" folder.

2.
Type your personal secret key in the "personal_key.json" file.

3.
Add the below code in the .gitignore file

# Ignore all personal json files
encrypt_json/files_to_encrypt/*
encrypt_json/files_to_decrypt/*
encrypt_json/personal_key.json

4.
Run the below command if you want to encrypt the json files
dotnet run -e

Run the below command if you want to decrypt the json files
dotnet run -e


Happy encryptind friends...