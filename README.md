<h1 align="center">Parking API</h1>

<p align="center">
  <a href="https://learn.microsoft.com/pt-br/dotnet/"><img alt="DotNet 8" src="https://img.shields.io/badge/.NET-5C2D91?logo=.net&logoColor=white&style=for-the-badge" /></a>
  <a href="https://learn.microsoft.com/pt-br/dotnet/csharp/programming-guide/"><img alt="C#" src="https://img.shields.io/badge/C%23-239120?logo=c-sharp&logoColor=white&style=for-the-badge" /></a>
</p>

## 💻 Project

This project simulates a small `parking` management system, this application was developed for academic purposes.

## 🚀 Technologies and Tools

This project was developed using the following technologies:

- **Backend:**  
  - `.NET 10`
  - `ASP.NET Core WebAPI`
  - `C#`
  - `Repository Pattern`
  - `Service Pattern`
  - `Microsoft Identity`
  - `JWT`
  - `Swagger`
  - `XUnit`
  - `Moq`
  - `Docker`
  - `Itext7`

## 💾 Clone the repository

```bash
git clone https://github.com/pauloalvesm/dotnet-parking-api.git
```
## ⬇️ How to Use

### Using Visual Studio Code:

- `Creating the Database`: after cloning the repository navigate to the `Parking.API` project using the terminal and, run the command `dotnet ef database update --context ApplicationDbContext` to restore the database with yours tables.
- `Restoring the IdentityDbContext`: after cloning the repository navigate to the `Parking.API` project using the terminal and, run the command `dotnet ef database update --context IdentityApplicationDbContext` to restore the Identity tables.
- `Using Docker`: navigate to the root folder of the project and run the `docker-compose up --build` command to create all the elements related to the Docker configuration.

### Using Visual Studio:

- `Creating the database`: after cloning the repository go to `Tools` and open the `Package Manager Console` selecting the `Parking.API` project and run the `Update-Database -Context ApplicationDbContext` command to restore the database with yours tables.
- `Restore the IdentityDbContext`: navigate to the `Parking.API` project using the terminal, then run the command `Update-Database -Context IdentityApplicationDbContext` to restore the Identity tables.
- `Using Docker`: the `Visual Studio` can restore the `Docker` settings automatically, if you prefer you can perform the process mentioned above. 

## 👤 Author

**[Paulo Alves](https://github.com/pauloalvesm)**
