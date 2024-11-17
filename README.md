Chinook Admin Panel

A panel for modifying tracks in the Chinook public sample database in AvaloniaUI.

![image](https://github.com/user-attachments/assets/968edf6d-8f02-45f1-a041-ba7096267351)

The values for miliseconds and bytes are converted to "00:00" and "MB" formats.

![image](https://github.com/user-attachments/assets/3a173437-ec97-4e69-ac95-0b6a4df19e8f)
![image](https://github.com/user-attachments/assets/f07f8c9d-8653-43a2-a38c-3df6eafd005e)
![image](https://github.com/user-attachments/assets/225f74fd-ff9b-4ff4-b901-68590961a8fd)
![image](https://github.com/user-attachments/assets/09fe79dd-e409-4854-9a92-d6fb58670a8d)

User can create, update and delete entries in the Tracks table.
There is a dialog pop-up for every one of those buttons that uses Avalonia DialogHost.

I have created a layered and modular architecture that allows developers to simply and efficiently add new features.

![Diagram](https://github.com/user-attachments/assets/e7ece9bf-faa8-4e3d-af2e-4793c4806031)

DataAccess has the models and DB Context generated from the Chinook database.
Infrastructure contains the DTO and interfaces for interacting with the database.
Implementation Layer has classes implementing the from the previous layer.
The UI Layer is the AvaloniaUI project containing all the necessary tools for cross-platform development.
