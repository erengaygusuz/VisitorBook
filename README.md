# Visitor Book

![Alt text](/screenshots/11-visitor-book.png)

## Project Goal and Scope

* Most people like to travel. We may forget the knowledge of the places we visit after a certain period of time. For example, which day and time did we visit the place we visited?
* Let's assume that we need to save these places somewhere with time information.
* Let's even imagine that people in a certain region want to record the information of their visitors and want to see various statistical data about their visitors.
* Considering these needs, I developed a web-based application. Another reason why I developed the project was to gain new information and improve myself.

* You can try the live version of the project at the following address:
* Link: https://visitor-book.erengaygusuz.com.tr
  
* You can register the system as a Visitor, request for Visitor Recorder Account or try as a Admin with using information below
  
* Email: ``` admin@gmail.com ```
* Password: ``` 12345 ```

## User Types of the Project and the Transactions They Can Perform

* There are 4 user types available in this application. These are:

  - SuperAdmin
  - Admin
  - VisitorRecorder
  - Visitor

* The permissions of user types can be changed at run time.
* For example, while the Admin's powers are greater than those of the Visitor, their powers can be reduced by an edit.
* You can see the authorization table of user types in the system from the file below.

![Alt text](/screenshots/visitor-book-user-authorization-table.png)

## Project Architecture

* There are 5 layers in the application part of the project. These are:

  - VisitorBook.BL
  - VisitorBook.Core
  - VisitorBook.DAL
  - VisitorBook.Test
  - VisitorBook.UI
 
* Additionally, the general architectural diagram of the project is shown in the image below:

![Alt text](/screenshots/visitor-book.system-architecture.png)

## General Technical Features of the Project

* Layered Architecture with ASP.NET Core 7 MVC
* Generic Repository and Service Pattern
* UnitOfWork Pattern 
* Advanced Membership System with Identity
* Advanced Role and Permission Based Authentication and Authorization
* Sending Email with Html Email Template Files
* Multi-Language Support (Including All Plugins Used in the Application)
* Object Mapping with Automapper
* Fake Data Generation with Bogus
* Using DTOs in data transfer
* Using View Models in Views
* Logging Database Transactions with Audit Log
* Error Management with Exception Handling Middleware
* Logging of Errors
* Listing Datas with Datatables (Paging, Searching, Filtering)
* Datas Export in PDF Format
* Listing Datas with MVC Grid (Paging, Searching, Filtering)
* MS SQL Server Database
* Using Entity Framework Core 7 ORM
* Database Migration with Code First Approach
* Server Side Validation with Fluent Validation
* Client Side Validation with jQuery Validation Unobtrusive
* Using Extension Methods
* Minifying and Bundling Css and Js Files with Bundler Minifier
* Notification with Toastr and SweetAlert
* Docker and Docker Compose Support
* Unit Test xUnit (Not completed yet)
* Specflow BDD Test (Not completed yet)

## Tools and Technologies Used in the Project

The list of all packages and tools used in the project is provided below, along with their version and web address information.

* General Technologies
  
  - ASP.NET Core 7 MVC
  - jQuery
  - HTML5
  - CSS3
  - Javascript

* Css and Javascript Packages
  - Google Web Fonts
  - Font Awesome 5.15.4
  - iCheck Bootstrap 3.0.1
  - AdminLTE 3.2.0
  - BS Stepper 1.7.0
  - Fancy Select
  - Bootstrap Icons 1.11.3
  - Animate 3.5.2
  - Owl Carousel 2.2.1
  - Customized Bootstrap Stylesheet 5.0.0
  - Tempusdominus Bootstrap 4 5.39.0
  - Select2 4.0.13
  - Overlay Scrollbars 1.13.0
  - DataTables 1.11.4
  - Flag Icon
  - SweetAlert2 11.4.0
  - Toastr
  - Mvc Grid 7.2.0
  - jQuery 3.6.0
  - Bootstrap 4.6.1
  - Wow 1.3.0
  - Easing 1.4.1
  - Waypoints 4.0.1
  - Counterup 2.1.0
  - jQuery UI 1.11.4
  - jQuery Validation 1.19.3
  - Datetime Moment 1.7
  - InputMask 5.0.7
  - Pdf Make 0.2.4
 
* Nuget Packages

  - Auto Mapper 12.0.1
  - Fluent Validation 11.9.0
  - Bogus 35.3.0
  - GeoTimeZone 5.3.0
  - Microsoft.AspNetCore.Identity.EntityFrameworkCore 7.0.15
  - Microsoft.AspNetCore.Localization 2.2.0
  - Microsoft.AspNetCore.Mvc.ViewFeatures 2.2.0
  - Microsoft.EntityFrameworkCore 7.0.15
  - System.Device.Location.Portable 1.0.0
  - System.Linq.Dynamic.Core 1.3.7
  - TimeZoneConverter 6.1.0
  - Microsoft.EntityFrameworkCore.SqlServer 7.0.15
  - Microsoft.EntityFrameworkCore.Tools 7.0.15
  - Microsoft.NET.Test.Sdk 17.6.0
  - xunit 2.4.2
  - xunit.runner.visualstudio 2.4.5
  - coverlet.collector 3.2.0
  - AspNetCoreHero.ToastNotification 1.1.0
  - AutoMapper.Extensions.Microsoft.DependencyInjection 12.0.1
  - BuildBundlerMinifier 3.2.449
  - FluentValidation.AspNetCore 11.3.0
  - FluentValidation.DependencyInjectionExtensions 11.9.0
  - Microsoft.EntityFrameworkCore.Design 7.0.15
  - Microsoft.VisualStudio.Web.CodeGeneration.Design 7.0.11
  - NonFactors.Grid.Core.Mvc6 7.2.0
  - NonFactors.Grid.Mvc6 7.2.0
  - WebMarkupMin.AspNetCore7 2.15.2

## Project Usage

There are some options to use this project.

* If you want to run project on Visual Studio
  
  - Clone the project using this command: ``` https://github.com/erengaygusuz/VisitorBook.git ```
  - Open the project solution with Visual Studio 2022 Preview (You can also use VS Code or others)
  - Run the project with https port option.
  - You should see project that is running on your browser like ``` https://localhost:{HTTPS_PORT} ``` 

* If you would like to only run this project locally, you can do it with two options:

  - Downloading and running its release package on IIS Server in Windows Machine
    
    - First, go to release section and dowload latest release from there.
    - After that unzip the package. There are three ``` appsettings.json ``` files in this package.
    - You should type your secrets there for environments. For example, you should add your database server connection string to ``` ConnectionString ``` section.
    - You can use generic ``` appsettings.json ``` but if you want to use other environment please fill the releated one. For instance, for development use ``` appsettings.Development.json ```
      
  - (Suggested) Pulling its docker image from dockerhub and running it as a container with docker-compose (Windows or Linux Machine does not matter)
 
    - First download the ``` docker-compose.yml ``` file from repository.
    - After that you should override the paths for volumes.
    - For SSL certificate, type the certificate file path instead of this path: ``` C:\Users\gaygu\.aspnet\https ```
    - For Static Files, type static files path instead of this path: ``` E:\DownloadedFiles\StaticFiles ```
    - Now you should override environments values.
    - Choose and envrironment. Type Development or Production for ``` ASPNETCORE_ENVIRONMENT ```.
    - Type your SSL certificate password for ``` ASPNETCORE_Kestrel__Certificates__Default__Password ```.
    - Type your certificate filename with extension for ``` ASPNETCORE_Kestrel__Certificates__Default__Path ``` after /https/.
    - Type your connection string for ``` ConnectionString ```.
    - Type your credentils for ``` EmailSettings__Host ```, ``` EmailSettings__Port ```, ``` EmailSettings__Email ``` and ``` EmailSettings__Password ```.
    - Type true or false for ``` EmailSettings__SSLCertificate ``` if your mail server has SSL or not.
    - Now you are read to go. Open a terminal or commant prompt and run this command ``` docker-compose up ```
    - After that your database will be created and also you can see the project by opening your browser and typing ``` https://localhost:{HTTPS_PORT} ``` 

## Related Links

* Youtube:
* Docker Hub: https://hub.docker.com/r/erengaygusuz/visitor-book
* Live Version: https://visitor-book.erengaygusuz.com.tr

## License

The MIT License (MIT)

## Screenshots

![Alt text](/screenshots/01-visitor-book.png)

![Alt text](/screenshots/02-visitor-book.png)

![Alt text](/screenshots/03-visitor-book.png)

![Alt text](/screenshots/04-visitor-book.png)

![Alt text](/screenshots/05-visitor-book.png)

![Alt text](/screenshots/06-visitor-book.png)

![Alt text](/screenshots/07-visitor-book.png)

![Alt text](/screenshots/08-visitor-book.png)

![Alt text](/screenshots/09-visitor-book.png)

![Alt text](/screenshots/10-visitor-book.png)

![Alt text](/screenshots/11-visitor-book.png)

![Alt text](/screenshots/12-visitor-book.png)

![Alt text](/screenshots/13-visitor-book.png)

![Alt text](/screenshots/14-visitor-book.png)

![Alt text](/screenshots/15-visitor-book.png)

![Alt text](/screenshots/16-visitor-book.png)

![Alt text](/screenshots/17-visitor-book.png)

![Alt text](/screenshots/18-visitor-book.png)

![Alt text](/screenshots/19-visitor-book.png)

![Alt text](/screenshots/20-visitor-book.png)

![Alt text](/screenshots/21-visitor-book.png)

![Alt text](/screenshots/22-visitor-book.png)

![Alt text](/screenshots/23-visitor-book.png)

![Alt text](/screenshots/24-visitor-book.png)

![Alt text](/screenshots/25-visitor-book.png)

![Alt text](/screenshots/26-visitor-book.png)

![Alt text](/screenshots/27-visitor-book.png)

![Alt text](/screenshots/28-visitor-book.png)

![Alt text](/screenshots/29-visitor-book.png)

![Alt text](/screenshots/30-visitor-book.png)

![Alt text](/screenshots/31-visitor-book.png)

![Alt text](/screenshots/32-visitor-book.png)

![Alt text](/screenshots/33-visitor-book.png)
