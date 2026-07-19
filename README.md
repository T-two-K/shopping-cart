# 🛒 Shopping cart
This web-application developed like a study project for improving my skills. The application is a simple shopping cart, where user can add, watch and change count of products in his cart, before this, complete the authorization. User can also log out of his account by clicking the appropriate button. Except of "user" there is another type of role in the system, called "admin". Persone with this role can edit product information (he has access to all CRUD operations related to a product).
## 🛠️ Technology stack
***
  - Asp NET Core
  - MySQL
  - Entity Framework Core
  - JavaScript
  - CSS
  - HTML
  - MVC with Razor Views
***
## ⏳ Development process 
1. In the beginning, it was necessary to create models that would represent database tables, set up connections between them, and write rules in the DbContext, and then performed the initialization migration.
2. After that, I developed authentication, authorization and logout functions and connected them to the corresponding windows.
3. Then I came up with a simple design for the application's main menu.
4. Next, I created repositories in which I wrote down the functions I needed and placed them in a separate DataManagerService class.
5. After creating the repositories, I refined the application logic (recalculating the quantity of products in the cart and in the warehouse, ), and also fixed minor bugs that arose during its development.
6. Afterwards, I connected my server-side component to the client-side component using AJAX requests. This ensured that the user's screen wouldn't flash when confirming changes to the cart.
7. In parallel with the previous step, I also created input validation, both on the server side and on the client side.
8. After validating the input, I designed the product information display window.
9. Finally, I started finalizing the admin functionality. First, I created product editing windows, then linked the previously created methods to them. Next, I added delete and edit buttons to the main menu, and slightly redesigned the product card.
10. In the end, I did some manual testing and fixed some bugs.
