<h1 align="center">Parking API</h1>

<p align="center">
  <a href="https://learn.microsoft.com/pt-br/dotnet/"><img alt="DotNet 8" src="https://img.shields.io/badge/.NET-5C2D91?logo=.net&logoColor=white&style=for-the-badge" /></a>
  <a href="https://learn.microsoft.com/pt-br/dotnet/csharp/programming-guide/"><img alt="C#" src="https://img.shields.io/badge/C%23-239120?logo=c-sharp&logoColor=white&style=for-the-badge" /></a>
</p>

## 💻 Project

This project simulates a small `parking` management system, this application was developed for academic purposes.

## 📘 Business Rule

- `Customer registration`: on the first visit, the customer is asked to register by providing personal details. 

- `Entry to Parking Lot`: to store the vehicle in the parking lot, a `ticket` is issued with the vehicle's `permanence` data, informing the customer of the `entry date`, `license plate` and `references`.

- `Ticket Delivered to Customer`: most of the fields are filled in when the stay is opened, the fields for `date of exit` and `total value` are left open, while the `stay status` field receives the value `parked` indicating that the vehicle is in the parking lot.

- `Vehicle pickup`: the customer presents the `ticket` given at the opening of the stay for the `pickup` procedure, this value is changed to the `stay status` field, at this stage the `date of departure` and `total amount` fields are recorded with their values informing the `date and time` and the `total amount` of the vehicle's stay in the parking lot.

- The stay operation can be cancelled up to a maximum of 5 minutes after vehicle entry.

## 🔨 Features

`Operations`: for all entities in the application, you can perform basic operations such as `listing`, `searching all records`, `searching for individual records`, `creating`, `updating`, and `deleting`.

`Security`: new `Users` can be registered, and processes for `Authentication` and `Authorization` of these users are implemented.

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
  - `Mapster`
  - `Clean Architecture`
  - `Rich Domain Model`
  - `Swagger`
  - `XUnit`
  - `Moq`
  - `Docker`
  - `Itext7`

## 💾 Clone the repository

```bash
git clone https://github.com/pauloalvesm/dotnet-parking-api.git

# Navigate to the project folder
cd src/Parking.API

# Restore dependencies
dotnet restore

# Run the project
dotnet run
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

## ℹ️ Another option for restoring tables in the database

Run these scripts directly on the database created using Docker, so that the tables will be created based on their contexts:

[ApplicationDbContext.sql](https://github.com/pauloalvesm/dotnet-parking-api/blob/master/src/Parking.API/Resources/Scripts/ApplicationDbContext.sql)

[IdentityApplicationDbContext.sql](https://github.com/pauloalvesm/dotnet-parking-api/blob/master/src/Parking.API/Resources/Scripts/IdentityApplicationDbContext.sql)

## 👤 Author

**[Paulo Alves](https://github.com/pauloalvesm)**
