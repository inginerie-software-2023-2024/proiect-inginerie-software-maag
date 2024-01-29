# Petbook 2.0 :dog2:

Petbook is a unique social media platform specifically designed for pet owners to connect, share, and engage with fellow pet enthusiasts. With a focus on pets, this app provides a dedicated space where users can showcase their furry friends, exchange stories, seek advice, and build a vibrant community of pet lovers.

Project made by:

- Ciocan Alexandra-Diana
- Georgescu Miruna
- Panait Ana-Maria
- Teodorescu George Tiberiu

## 1. Documenting the existing application from MDS

📗 The documentation of the app from MDS can be found [here](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/Documentation_for_MDS_project.pdf).

## 2. Problem Statement or Product Vision

📗 The Product Vision for the Petbook app can be found [here](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/Product_vision.pdf).

## 3. Product features and functionalities

📗 Product features and functionalities can be found [here](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/Product_features_and_functionalities.pdf)

## 4. Non-functional requirements

📗 Non functional requirements can be found [here](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/NF-Requirements.pdf).

## 5. User Personas

📗 User Personas can be found [here](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/User_Personas.pdf).

## 6. Customer Journey Map

📗 Customer Journey Map can be found [here](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/User_Journey_Map.pdf).

## 7. UI Mockup

📗 UI Mockup can be found [here](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/Mockup_Chat_Page.pdf).

## 8. Activity Diagram

📗 The Activity Diagram can be found [here](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/Post%20Creation%20Activity%20Diagram.png).

## 9. A set of User Stories

📗 A set of User Stories can be found [here](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/Set%20of%20User%20Stories.pdf).

## 10. Prioritized Product Backlog

 <div align="center"> 
  <img width="480px" src="https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/Trello.png" alt="Photo">
 </div>
📗 The Trello board can be found here: https://trello.com/b/CsL3Aw4a/petbook-20.

</br>

**Final version of backlog:**

 <div align="center"> 
  <img width="480px" src="https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/Trello2.png" alt="Photo">
 </div>

## 11. Dev sprint reports:

- [Dev sprint report 1](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/Sprint%20Report%201.pdf)
- [Dev sprint report 2](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/Sprint%20Report%202.pdf)
- [Dev sprint report 3](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/Sprint%20Report%203.pdf)
- [Dev sprint report 4](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/Sprint%20Report%204.pdf)

## 12. Software Arhitecture Report

### ❔What the purpose of the software project is

#### a. Summary

Petbook is a unique social media platform specifically designed for pet owners to connect, share, and engage with fellow pet enthusiasts.

#### b. Fulfilled capabilities

At this point the project has the following capabilities:

✔️ share photos of your pets \
✔️ share text stories related to pets \
✔️ like and comment posts (photos or stories) \
✔️ follow users \
✔️ search user or pet by name \
✔️ chat in real-time with the users you follow

What is to be done until project fulfillment:

- the posibility to post a short video of you pet
- the posibility to send an image or a video in the chat

### ❔Guides on how to:

#### a. Run the project locally

    1. clone the repository
    2. open the solution in an IDE like Visual Studio
    3. build and run the project using the IDE

#### b. Build the project

    1. use the `dotnet` build command in the project directory to compile the project

#### c. Deploy the project on local IIS server

    1. publish the project using Visual Studio to a web server (IIS) and web deploy (without package)
    2. open `Windows Features` and enable the following features:
    -  Internet Information Services -> Web Management Tools -> IIS Management Console
    -  Internet Information Services -> World Wide Web Services -> Application Deveelopment Featrues -> .NET Extensibility 4.8, Application Initialization, ASP.NET 4.8, ISAPI Extension, ISAPI Filters
    -  Internet Information Services ->  World Wide Web Services -> Common HTTP Features -> Default Document, Static Content
    3. check the IIS server is up by visiting http://localhost
    4. open `Internet Information Services Manager` and in the connections section there should be a `Sites` folder. Right click on it and press on `Add Website` and complete the required information (at physical path you should put C:\inetpub\wwwroot; at IP address put *).
    5. Install Microsoft SQL Server Development edition from [here](https://www.microsoft.com/en-us/sql-server/sql-server-downloads), also install SQL Server Management Studio (SSMS)
    6. Create a database named Petbook and copy the content of the database from Visual Studio into it
    7. Create a user in the database named "IIS APPPOOL\Petbook". You can do this from SSMS by expanding the new database -> Security and right click on users and click on New User.
    8. Add user to roles: db_datareader and db_datawriter
    9. Restart the site from IIS Management Console
    10. Navigate to http://localhost in the browser and the app should be running

#### d. [Contribution guide](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/CONTRIBUTING.md)

### 🔧 Application entry points:

#### a. Data sources

-> We use `SQL Server` as the database management system, ensuring efficient storage and retrieval of user data.

#### b. Data inputs

-> We use user inputs through the UI.

#### c. Configuration files

-> We have a [configuration file](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/Petbook/Petbook/appsettings.json) for the app where we set the database connection string and some settings for logging.

### 🔧 High level diagrams of the arhitecture:

a. [User journey map](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/User_Journey_Map.pdf)

<div align="center"> 
<img width="480px" src="https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-maag/blob/main/UserJourneyMapPhoto.png" alt="Photo">
</div>

### 🔧 Deployment plan:

a. Where is the application deployed?

The application is deployed locally into an IIS Application server

b. How the CI/CD pipeline works:

Our Continuous Integration (CI) pipeline is designed to automate the process of integrating new code changes into the dev branch. It ensures that new code submissions are automatically built and tested. This helps in maintaining code quality and reducing manual errors.

We use [GitHub Actions]() for our CI pipeline.
The CI pipeline is triggered on every push and pull request event to the dev branch.

### 🔧 Description of the QA process:

Test suites - what do they test

-> There are two unit test suites, for the chats and pets controllers that test all the endpoints.

The rest of the services are mocked.

-> The End-To-End test suite:

- Test case: User logs and sends message
- Preconditions: None (the user is not logged in)
- Steps:

1. User logs in
2. User searches for another user
3. User follows the other user
4. User messages the new connection

We assert each step to ensure that the flow works as expcted.

### 🔧 External dependencies included in the project

- **Entity Framework** - we used [ASP.NET Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-8.0&tabs=visual-studio) for user authentication and authorization
- **[JQuery](https://jquery.com/)**
- **[Bootstrap](https://getbootstrap.com/)**
- **[SignalR 2](https://dotnet.microsoft.com/en-us/apps/aspnet/signalr)** - used for the real-time chat
- **[Summernote](https://summernote.org/)** - for text editing
- **[Croppie](https://foliotek.github.io/Croppie/)** - for image cropping

❔How vulnerable is the project to dependency attacks

- Most of the packages that our project depends on come from highly trusted and reliable sources, such as official Microsoft packages. Thus, the chanches that our project is vulnerable to dependecny attacks is low.
