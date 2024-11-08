# encrypt_json
Encrypt or Decrypt multiple json files.

# 1. json files
Add the files that you want to encrypt in the `files_to_encrypt` folder.
Add the files that you want to decrypt in the `files_to_decrypt` folder.

# 2. secret key
Type your personal secret key in the `personal_key.json` file.

# 3. .gitignore files
Add the below code in the .gitignore file

	encrypt_json/files_to_encrypt/*
	encrypt_json/files_to_decrypt/*
	encrypt_json/personal_key.json

# 4. run commands
Run the command if you want to encrypt the json files
	`dotnet run -e`

Run the command if you want to decrypt the json files
	`dotnet run -d`


# Happy encryptind friends...