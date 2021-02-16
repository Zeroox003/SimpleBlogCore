A simple blog written in .NET Core MVC with mini social networking features

## Demo
An online demo that you can take a look is [here »](https://simpleblogcore.azurewebsites.net/)

The login credentials for the site can be found in the `SimpleBlogContextSeedInitializer.cs` file

## Stack used:
- .NET Core MVC
- Entity Framework Core
- SQL Server
- AspNetCore.Identity for User management
- X.PagedList.Mvc.Core for chopping data into pages and provides pager UI and settings
- Website template "Keep It Simple" designed by Styleshout development team
- JS / JQuery

## TODO list:
- [X] Pages Contact, About me
- [X] Possibility to login and register
- [X] User page
    - [X] Editing user info
    - [X] Edititng profile image
- [X] Add pager
- [X] Search by tag
- [X] Search by text
- [ ] Replace Guid with UrlSlug (posts, tags)
- [X] Post viewing page
  - [X] Post content
  - [X] List of comments
  - [X] Actions with comments
- [ ] Admin panel (managing)
  - [X] Posts
    - [X] Viewing
    - [X] Deleting
    - [X] Adding
    - [X] Editing
      - [X] Advanced content editor (rich text box)
  - [X] Tags
    - [X] Viewing
    - [X] Deleting
    - [X] Adding
    - [X] Editing
  - [ ] User management
    - [X] Viewing
    - [ ] Locking out (impossible to leave comments)
    - [X] Add the ability to change roles
- [ ] Sidebar in user profile
  - [ ] List of left comments + link to post
- [ ] Popup notifications (when login, user info editing, etc)
